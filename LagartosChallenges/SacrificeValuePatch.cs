using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace LagartosChallenges
{
    /// <summary>
    /// Makes cards with the <see cref="DoubleBlood"/> sigil worth 2 blood. The game's
    /// <c>BoardManager.GetValueOfSacrifices</c> counts every card as 1 (Worthy Sacrifice as 3); this postfix adds the
    /// difference. Every blood check (can-play, sacrifice loop, markers) goes through that method.
    /// </summary>
    [HarmonyPatch(typeof(BoardManager), "GetValueOfSacrifices")]
    internal static class SacrificeValuePatch
    {
        /// <summary>
        /// Harmony postfix: adds the extra blood from Double Blood cards to the vanilla total.
        /// </summary>
        /// <param name="sacrifices">Slots whose cards are being valued.</param>
        /// <param name="__result">Vanilla total, raised in place.</param>
        [HarmonyPostfix]
        private static void Postfix(List<CardSlot> sacrifices, ref int __result)
        {
            if (sacrifices == null) return;
            foreach (CardSlot slot in sacrifices)
            {
                if (slot == null || slot.Card == null || !slot.Card.HasAbility(DoubleBlood.ability)) continue;

                bool triple = slot.Card.HasAbility(Ability.TripleBlood);
                __result += HandRules.SacrificeValue(triple, hasDoubleBlood: true)
                          - HandRules.SacrificeValue(triple, hasDoubleBlood: false);
            }
        }
    }
}
