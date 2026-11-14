using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;

namespace LagartosChallenges
{
    /// <summary>
    /// Forbidden Cards: adds the <see cref="ForbiddenCards"/> to the Act 1 card choice and rare pools. Every offer
    /// (card choices, cost/tribe choices, rare choices, boulders, Deck Trial, the Trader's rare tier) reads its pool
    /// from <c>CardLoader.GetUnlockedCards</c>, so one postfix covers them all; the run's tribe/cost filters still
    /// apply on top.
    /// </summary>
    [HarmonyPatch(typeof(CardLoader), nameof(CardLoader.GetUnlockedCards))]
    internal static class ForbiddenCardsPatch
    {
        /// <summary>
        /// Appends the forbidden cards the pool lacks (as the shared card data, like the rest of the pool; callers
        /// clone what they pick).
        /// </summary>
        /// <param name="category">Pool category (only ChoiceNode and Rare are extended).</param>
        /// <param name="temple">Pool temple (only Nature, i.e. Act 1).</param>
        /// <param name="__result">The pool, extended in place.</param>
        [HarmonyPostfix]
        private static void Postfix(CardMetaCategory category, CardTemple temple, List<CardInfo> __result)
        {
            if (__result == null || temple != CardTemple.Nature) return;
            if (category != CardMetaCategory.ChoiceNode && category != CardMetaCategory.Rare) return;
            if (!Challenges.IsActive(ChallengeKey.ForbiddenCards)) return;

            IReadOnlyList<string> missing = ForbiddenCards.MissingFrom(category == CardMetaCategory.Rare,
                __result.Where(c => c != null).Select(c => c.name));
            foreach (string name in missing)
            {
                CardInfo card = ScriptableObjectLoader<CardInfo>.AllData.Find(c => c.name == name);
                if (card != null) __result.Add(card);
                else Plugin.Log.LogWarning($"Forbidden Cards: no card named {name}.");
            }
        }
    }
}
