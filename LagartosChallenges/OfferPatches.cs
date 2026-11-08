using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;

namespace LagartosChallenges
{
    /// <summary>
    /// Open while the game builds an offer from <c>CardLoader.GetDistinctCardsFromPool</c> (Deck Trial, Trader,
    /// boulders), so that call is filtered only for those offers and not for Leshy's cards.
    /// </summary>
    internal static class OfferScope
    {
        private static int depth;

        /// <summary>True while an offer is being built.</summary>
        internal static bool Active => depth > 0;

        /// <summary>Opens the scope (nests).</summary>
        internal static void Enter() => depth++;

        /// <summary>Closes the scope.</summary>
        internal static void Exit() => depth = Math.Max(0, depth - 1);

    }

    /// <summary>
    /// Applies the tribe/cost "X Only" challenges to every card the player is offered. All patches do nothing
    /// when no filter challenge is on.
    /// </summary>
    [HarmonyPatch]
    internal static class OfferPatches
    {
        /// <summary>Choice-node cards of Act 1, as fresh clones.</summary>
        private static List<CardInfo> Pool(CardMetaCategory category) =>
            CardLoader.GetUnlockedCards(category, CardTemple.Nature).Select(GameCards.Clone).ToList();

        /// <summary>
        /// Filters a list of direct card choices: matching cards stay, others are replaced from the pool, the rest
        /// become filler.
        /// </summary>
        /// <param name="choices">Choices the game generated.</param>
        /// <param name="pool">Replacement pool.</param>
        /// <param name="filter">The run's filter.</param>
        /// <param name="seed">Random seed.</param>
        /// <returns>Filtered choices.</returns>
        private static List<CardChoice> FillCardChoices(List<CardChoice> choices, List<CardInfo> pool, CardFilter filter, int seed)
        {
            var poolChoices = pool.Select(c => new CardChoice { CardInfo = c }).ToList();
            List<CardChoice> result = OfferRules.Fill(
                choices,
                poolChoices,
                c => GameCards.Matches(c.CardInfo, filter),
                c => c.CardInfo?.name ?? "",
                () => new CardChoice { CardInfo = GameCards.MakeFiller() },
                seed);
            int replaced = result.Count(c => !choices.Contains(c));
            if (replaced > 0)
            {
                Plugin.Log.LogInfo($"Card filter: replaced {replaced}/{choices.Count} offered cards " +
                                   $"({result.Count(c => GameCards.IsFiller(c.CardInfo))} filler).");
            }
            GameCards.ShowFilterActivation();
            return result;
        }

        /// <summary>
        /// Harmony postfix on Act 1 card choice generation (normal, face-down cost, face-down tribe, death card).
        /// </summary>
        /// <param name="data">Node data (may be null).</param>
        /// <param name="randomSeed">Seed the game used.</param>
        /// <param name="__result">Choices the game generated.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Part1CardChoiceGenerator), nameof(Part1CardChoiceGenerator.GenerateChoices),
            new[] { typeof(CardChoicesNodeData), typeof(int) })]
        private static void FilterCardChoices(CardChoicesNodeData data, int randomSeed, ref List<CardChoice> __result)
        {
            CardFilter filter = GameCards.CurrentFilter();
            if (!filter.IsActive || __result == null) return;

            switch (data?.choicesType)
            {
                case CardChoicesType.Cost:
                    __result = FilterCostChoices(__result, filter);
                    GameCards.ShowFilterActivation();
                    break;
                case CardChoicesType.Tribe:
                    if (filter.TribeGroupActive) __result = FilterTribeChoices(__result, filter);
                    GameCards.ShowFilterActivation();
                    break;
                case CardChoicesType.Deathcard:
                    // Death card nodes only offer death cards; failing ones become filler, not regular cards.
                    __result = FillCardChoices(__result, new List<CardInfo>(), filter, randomSeed);
                    break;
                default:
                    __result = FillCardChoices(__result, Pool(CardMetaCategory.ChoiceNode), filter, randomSeed);
                    break;
            }
        }

        /// <summary>
        /// Face-down cost choices: keep allowed options, add missing allowed ones (Blood 1-3, Bones) up to 3,
        /// pad with filler.
        /// </summary>
        private static List<CardChoice> FilterCostChoices(List<CardChoice> choices, CardFilter filter)
        {
            bool Allowed(CardChoice c) =>
                c.CardInfo == null && filter.AllowsCostChoice(c.resourceType == ResourceType.Blood ? c.resourceAmount : (int?)null);
            bool Same(CardChoice a, CardChoice b) => a.resourceType == b.resourceType && a.resourceAmount == b.resourceAmount;

            var result = choices.Where(Allowed).ToList();
            var all = new[] { 1, 2, 3 }.Select(n => new CardChoice { resourceType = ResourceType.Blood, resourceAmount = n })
                .Concat(new[] { new CardChoice { resourceType = ResourceType.Bone } });
            foreach (CardChoice option in all.Where(Allowed))
            {
                if (result.Count >= 3) break;
                if (!result.Any(r => Same(r, option))) result.Add(option);
            }
            while (result.Count < 3) result.Add(new CardChoice { CardInfo = GameCards.MakeFiller() });
            return result;
        }

        /// <summary>
        /// Face-down tribe choices: only selected tribes (vanilla's picks first), max 3, pad with filler.
        /// </summary>
        private static List<CardChoice> FilterTribeChoices(List<CardChoice> choices, CardFilter filter)
        {
            var allowed = filter.AllowedTribeChoices.Select(GameCards.ToGame).ToList();
            var result = choices.Where(c => c.CardInfo == null && allowed.Contains(c.tribe)).ToList();
            foreach (Tribe tribe in allowed)
            {
                if (result.Count >= 3) break;
                if (!result.Any(r => r.tribe == tribe)) result.Add(new CardChoice { tribe = tribe });
            }
            while (result.Count < 3) result.Add(new CardChoice { CardInfo = GameCards.MakeFiller() });
            return result;
        }

        /// <summary>Harmony postfix on Act 1 rare card choices.</summary>
        /// <param name="randomSeed">Seed the game used.</param>
        /// <param name="__result">Rare choices.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Part1RareChoiceGenerator), nameof(Part1RareChoiceGenerator.GenerateChoices),
            new[] { typeof(CardChoicesNodeData), typeof(int) })]
        private static void FilterRareChoices(int randomSeed, ref List<CardChoice> __result)
        {
            CardFilter filter = GameCards.CurrentFilter();
            if (!filter.IsActive || __result == null) return;
            __result = FillCardChoices(__result, Pool(CardMetaCategory.Rare), filter, randomSeed);
        }

        /// <summary>
        /// Shared body of the three <c>CardLoader.GetRandomChoosable*</c> postfixes: a non-matching (or missing)
        /// card is replaced by a matching one with the same criteria, or the filler. Never null while filtering.
        /// </summary>
        private static void PickMatching(ref CardInfo result, Predicate<CardInfo> criteria, int seed)
        {
            CardFilter filter = GameCards.CurrentFilter();
            if (!filter.IsActive || GameCards.Matches(result, filter)) return;
            List<CardInfo> pool = CardLoader.GetUnlockedCards(CardMetaCategory.ChoiceNode, CardTemple.Nature).FindAll(criteria);
            CardInfo pick = OfferRules.PickOrFiller(pool, c => GameCards.Matches(c, filter), () => null, seed);
            result = pick == null ? GameCards.MakeFiller() : GameCards.Clone(pick);
        }

        /// <summary>Postfix: face-down cost choice result (and Pack Mule).</summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardLoader), nameof(CardLoader.GetRandomChoosableCardWithCost))]
        private static void FilterCardWithCost(int randomSeed, int bloodCost, ref CardInfo __result) =>
            PickMatching(ref __result, c => c.BloodCost == bloodCost, randomSeed);

        /// <summary>Postfix: face-down tribe choice result (and the region's tribal card).</summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardLoader), nameof(CardLoader.GetRandomChoosableCardOfTribe))]
        private static void FilterCardOfTribe(int randomSeed, Tribe tribe, ref CardInfo __result) =>
            PickMatching(ref __result, c => c.IsOfTribe(tribe), randomSeed);

        /// <summary>Postfix: face-down Bones choice result (and Pack Mule).</summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardLoader), nameof(CardLoader.GetRandomChoosableBonesCard))]
        private static void FilterBonesCard(int randomSeed, ref CardInfo __result) =>
            PickMatching(ref __result, c => c.BonesCost > 0, randomSeed);

        /// <summary>Postfix: a death card rolled for a face-down cost choice is dropped when it doesn't match.</summary>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Part1CardChoiceGenerator), nameof(Part1CardChoiceGenerator.RollRandomDeathCardOfCost))]
        private static void FilterDeathCardOfCost(ref CardInfo __result)
        {
            CardFilter filter = GameCards.CurrentFilter();
            if (filter.IsActive && __result != null && !GameCards.Matches(__result, filter)) __result = null;
        }

        /// <summary>Opens the offer scope for the Deck Trial rewards.</summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(DeckTrialSequencer), "GenerateRewardChoices")]
        private static void DeckTrialEnter() => OfferScope.Enter();

        /// <summary>Closes the offer scope after the Deck Trial rewards (even on exceptions).</summary>
        [HarmonyFinalizer]
        [HarmonyPatch(typeof(DeckTrialSequencer), "GenerateRewardChoices")]
        private static void DeckTrialExit() => OfferScope.Exit();

        /// <summary>Opens the offer scope for a Trader tier.</summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TradePeltsSequencer), "GetTradeCardInfos")]
        private static void TraderEnter() => OfferScope.Enter();

        /// <summary>Closes the offer scope after a Trader tier (even on exceptions).</summary>
        [HarmonyFinalizer]
        [HarmonyPatch(typeof(TradePeltsSequencer), "GetTradeCardInfos")]
        private static void TraderExit() => OfferScope.Exit();

        /// <summary>Inside an offer scope, only matching cards can be drawn from the pool.</summary>
        /// <param name="cardPool">Pool the game draws from (replaced by a filtered copy).</param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CardLoader), nameof(CardLoader.GetDistinctCardsFromPool))]
        private static void FilterPool(ref List<CardInfo> cardPool)
        {
            if (!OfferScope.Active || cardPool == null) return;
            CardFilter filter = GameCards.CurrentFilter();
            if (!filter.IsActive) return;
            cardPool = cardPool.FindAll(c => GameCards.Matches(c, filter));
        }

        /// <summary>Inside an offer scope, a short draw is padded with filler.</summary>
        /// <param name="numCards">How many cards the game asked for.</param>
        /// <param name="__result">Cards drawn.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardLoader), nameof(CardLoader.GetDistinctCardsFromPool))]
        private static void PadPool(int numCards, List<CardInfo> __result)
        {
            if (!OfferScope.Active || __result == null || !GameCards.CurrentFilter().IsActive) return;
            int missing = numCards - __result.Count;
            for (int i = 0; i < missing; i++) __result.Add(GameCards.MakeFiller());
            if (missing > 0) Plugin.Log.LogInfo($"Card filter: offer padded with {missing} filler.");
            GameCards.ShowFilterActivation();
        }
    }
}

namespace LagartosChallenges
{
    /// <summary>
    /// Holds the offer scope open while each step of the boulder event runs (its card comes from
    /// <c>GetDistinctCardsFromPool</c>). Patches the coroutine's <c>MoveNext</c>, not the
    /// <c>BoulderChoiceSequence()</c> stub: a stub taking only <c>this</c> is small enough for Mono to inline,
    /// and patches on inlined methods never run.
    /// </summary>
    [HarmonyPatch]
    internal static class BoulderOfferScopePatch
    {
        /// <summary>The boulder coroutine's state machine step.</summary>
        /// <returns>Its <c>MoveNext</c> method.</returns>
        private static MethodBase TargetMethod() =>
            AccessTools.EnumeratorMoveNext(AccessTools.Method(typeof(BoulderChoiceSequencer), nameof(BoulderChoiceSequencer.BoulderChoiceSequence)));

        /// <summary>Opens the scope before the step.</summary>
        private static void Prefix() => OfferScope.Enter();

        /// <summary>Closes the scope after the step (even on exceptions).</summary>
        private static void Finalizer() => OfferScope.Exit();
    }
}
