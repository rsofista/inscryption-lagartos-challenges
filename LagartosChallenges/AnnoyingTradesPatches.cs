using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;

namespace LagartosChallenges
{
    /// <summary>
    /// Shared helpers for the Annoying Trades patches.
    /// </summary>
    internal static class AnnoyingCards
    {
        /// <summary>
        /// True for any pelt card: vanilla tags them with <see cref="Trait.Pelt"/>, and the Trader looks them up by
        /// <see cref="CardLoader.PeltNames"/> (which InscryptionAPI extends with modded pelts).
        /// </summary>
        /// <param name="card">Card to check.</param>
        /// <returns>Whether the card is a pelt.</returns>
        internal static bool IsPelt(CardInfo card) =>
            card.HasTrait(Trait.Pelt) || CardLoader.PeltNames.Contains(card.name);

        /// <summary>True when the card has the Annoying sigil (base or from a mod).</summary>
        /// <param name="card">Card to check.</param>
        /// <returns>Whether the card is Annoying.</returns>
        internal static bool IsAnnoying(CardInfo card) => card.HasAbility(Ability.BuffEnemy);

        /// <summary>
        /// A plain Annoying ability mod, exactly what vanilla Annoying Starters gives: the sigil is part of the card,
        /// so it shows like a normal sigil and can be moved with the sacrifice stone.
        /// </summary>
        /// <returns>A new mod carrying the Annoying sigil.</returns>
        internal static CardModificationInfo NewMod() => new CardModificationInfo(Ability.BuffEnemy);
    }

    /// <summary>
    /// Vanilla Annoying Starters (<see cref="AscensionChallenge.WeakStarterDeck"/>) skips pelts. With Annoying Trades
    /// also picked, the starter pelts get the Annoying sigil too.
    /// </summary>
    [HarmonyPatch(typeof(RunIntroSequencer), "TryModifyStarterCards")]
    internal static class StarterPeltsPatch
    {
        /// <summary>
        /// Harmony postfix: after vanilla made the non-pelt starters Annoying, make the pelts Annoying as well.
        /// </summary>
        [HarmonyPostfix]
        private static void Postfix()
        {
            if (!AnnoyingRules.StarterPeltsBecomeAnnoying(Plugin.AnnoyingStartersActive, Plugin.AnnoyingTradesActive))
            {
                return;
            }

            DeckInfo deck = RunState.Run.playerDeck;
            List<CardInfo> pelts = deck.Cards
                .Where(c => AnnoyingCards.IsPelt(c) && AnnoyingRules.NeedsAnnoying(AnnoyingCards.IsAnnoying(c)))
                .ToList();
            foreach (CardInfo pelt in pelts)
            {
                deck.ModifyCard(pelt, AnnoyingCards.NewMod());
            }

            Plugin.Log.LogInfo($"Annoying Starters + Annoying Trades: gave Annoying to {pelts.Count} starter pelts.");
            if (pelts.Count > 0)
            {
                ChallengeActivationUI.TryShowActivation(Plugin.AnnoyingTradesChallenge);
            }
        }
    }

    /// <summary>
    /// Every pelt trade event: if any pelt in the deck is Annoying when that trade starts (from Annoying Starters, or
    /// a sigil moved onto it later with the sacrifice stone), every card the Trader offers gets the Annoying sigil.
    /// </summary>
    [HarmonyPatch]
    internal static class TradeOffersPatch
    {
        /// <summary>
        /// Decided once per trade event, before any pelt is traded, so trading the Annoying pelts away mid-event
        /// does not turn the later tiers' offers back to normal.
        /// </summary>
        private static bool offersAnnoying;

        /// <summary>
        /// Harmony prefix on <c>TradePeltsSequencer.GetTradingTiers</c>, which each trade event calls once at its
        /// start, before any pelt is traded. Records whether this trade's offers are Annoying.
        /// (Not hooked on <c>TradePelts</c> itself: that iterator stub is tiny enough for Mono to inline into its
        /// caller, and an inlined call never reaches a Harmony patch.)
        /// </summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TradePeltsSequencer), "GetTradingTiers")]
        private static void DecideAtTradeStart()
        {
            List<CardInfo> pelts = RunState.DeckList.Where(AnnoyingCards.IsPelt).ToList();
            offersAnnoying = AnnoyingRules.TradeOffersBecomeAnnoying(
                Plugin.AnnoyingTradesActive,
                pelts.Select(AnnoyingCards.IsAnnoying));
            Plugin.Log.LogInfo($"Pelt trade: {pelts.Count(AnnoyingCards.IsAnnoying)}/{pelts.Count} pelts Annoying, " +
                               $"challenge {(Plugin.AnnoyingTradesActive ? "on" : "off")} -> offers Annoying: {offersAnnoying}.");
        }

        /// <summary>
        /// Harmony postfix on <c>TradePeltsSequencer.GetTradeCardInfos</c> (one call per pelt tier): adds the
        /// Annoying sigil to each offered card. The list holds fresh clones, so the card pool is untouched.
        /// </summary>
        /// <param name="__result">Cards the Trader offers for this tier.</param>
        [HarmonyPostfix]
        [HarmonyPatch(typeof(TradePeltsSequencer), "GetTradeCardInfos")]
        private static void MakeOffersAnnoying(List<CardInfo> __result)
        {
            if (!offersAnnoying || __result == null) return;

            foreach (CardInfo card in __result.Where(c => AnnoyingRules.NeedsAnnoying(AnnoyingCards.IsAnnoying(c))))
            {
                card.Mods.Add(AnnoyingCards.NewMod());
            }
            ChallengeActivationUI.TryShowActivation(Plugin.AnnoyingTradesChallenge);
        }
    }
}
