using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;

namespace LagartosChallenges
{
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
