using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace LagartosChallenges
{
    /// <summary>
    /// The Trapper follows the card filters: pelts have no tribe and no cost, so unless the filters allow
    /// No Tribe / Free, every pelt for sale becomes an Annoying Rabbit Pelt at the Rabbit Pelt price.
    /// </summary>
    [HarmonyPatch(typeof(BuyPeltsSequencer))]
    internal static class TrapperPatches
    {
        /// <summary>Whether the filters are on and reject pelts (all three pelts share the same traits).</summary>
        /// <returns>True when the Trapper must sell filler.</returns>
        private static bool PeltsRejected()
        {
            CardFilter filter = GameCards.CurrentFilter();
            if (!filter.IsActive) return false;
            CardInfo rabbit = ScriptableObjectLoader<CardInfo>.AllData.Find(c => c.name == GameCards.FillerCardName);
            return rabbit != null && !filter.Passes(GameCards.Traits(rabbit));
        }

        /// <summary>Postfix on the price getter: every slot costs the Rabbit Pelt price when pelts are rejected.</summary>
        /// <param name="__result">Prices per slot.</param>
        [HarmonyPostfix]
        [HarmonyPatch("PeltPrices", MethodType.Getter)]
        private static void FilterPrices(ref int[] __result)
        {
            if (__result != null && PeltsRejected()) __result = TrapperRules.Prices(__result, peltsPass: false);
        }

        /// <summary>
        /// Postfix on <c>CreatePelt</c>: swaps the pelt just put up for sale with the filler. Also covers the free
        /// pelt the Trapper hands out (slot 0).
        /// </summary>
        /// <param name="__instance">The Trapper sequence.</param>
        /// <param name="index">Slot just created (0 Rabbit, 1 Wolf, 2 Golden).</param>
        [HarmonyPostfix]
        [HarmonyPatch("CreatePelt")]
        private static void SellFiller(BuyPeltsSequencer __instance, int index)
        {
            if (!PeltsRejected()) return;
            var forSale = Traverse.Create(__instance).Field("peltsForSale").GetValue<List<SelectableCard>>();
            if (forSale == null || index < 0 || index >= forSale.Count || forSale[index] == null) return;
            forSale[index].SetInfo(GameCards.MakeFiller());
            GameCards.ShowFilterActivation();
        }
    }

    /// <summary>
    /// One Squirrel: the Act 1 side deck holds only the opening hand's squirrels plus one to draw.
    /// </summary>
    [HarmonyPatch]
    internal static class OneSquirrelPatch
    {
        /// <summary>Postfix on the side deck builder (whatever it is made of: squirrels, bees, aqua squirrels).</summary>
        /// <param name="__result">The side deck's cards.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Part1CardDrawPiles), "SideDeckData", MethodType.Getter)]
        private static void CutSideDeck(ref List<CardInfo> __result)
        {
            if (__result == null || !Challenges.IsActive(ChallengeKey.OneSquirrel)) return;
            __result = TrapperRules.CutSideDeck(__result, HandRules.OpeningSquirrels(Challenges.IsActive(ChallengeKey.TwoSquirrels)));
        }

        /// <summary>Flashes the challenge icon when the opening hand is drawn.</summary>
        /// <param name="__instance">The draw piles.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CardDrawPiles3D), nameof(CardDrawPiles3D.DrawOpeningHand))]
        private static void ShowIcon(CardDrawPiles3D __instance)
        {
            if (__instance is Part1CardDrawPiles) Challenges.ShowActivation(ChallengeKey.OneSquirrel);
        }
    }
}
