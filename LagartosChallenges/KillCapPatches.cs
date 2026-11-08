using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DiskCardGame;
using HarmonyLib;

namespace LagartosChallenges
{
    /// <summary>
    /// No First Turn Kill / No Phase Turn Kill: caps damage added to the scales during a capped round.
    /// </summary>
    [HarmonyPatch]
    internal static class KillCapPatches
    {
        /// <summary>Which round of the current battle is capped.</summary>
        internal static readonly PhaseTracker Tracker = new PhaseTracker();

        /// <summary>
        /// Harmony prefix on <c>LifeManager.ShowDamageSequence</c> (every source of scale damage goes through it).
        /// Its coroutine stub takes 7 arguments, too big for Mono to inline, so the prefix runs.
        /// </summary>
        /// <param name="__instance">The life manager.</param>
        /// <param name="damage">Damage about to be added (capped in place).</param>
        /// <param name="numWeights">Weights shown on the scale (kept ≤ damage).</param>
        /// <param name="toPlayer">True when the player takes the damage.</param>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LifeManager), nameof(LifeManager.ShowDamageSequence))]
        private static void CapDamage(LifeManager __instance, ref int damage, ref int numWeights, bool toPlayer)
        {
            bool firstOn = Challenges.IsActive(ChallengeKey.NoFirstTurnKill);
            bool phaseOn = Challenges.IsActive(ChallengeKey.NoPhaseTurnKill);
            if (!firstOn && !phaseOn) return;
            TurnManager turns = Singleton<TurnManager>.Instance;
            if (turns == null || !Tracker.IsCapRound(turns.TurnNumber, firstOn, phaseOn)) return;

            int capped = KillCapRules.CapDamage(__instance.Balance, damage, toPlayer);
            if (capped == damage) return;
            Plugin.Log.LogInfo($"Kill cap: round {turns.TurnNumber}, balance {__instance.Balance}, " +
                               $"{(toPlayer ? "player" : "opponent")} would take {damage}, capped to {capped}.");
            damage = capped;
            if (numWeights > capped) numWeights = capped;
            Challenges.ShowActivation(Tracker.InPhaseRound ? ChallengeKey.NoPhaseTurnKill : ChallengeKey.NoFirstTurnKill);
        }

        /// <summary>A battle's scales are set up: back to phase 1 (virtual method, never inlined).</summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LifeManager), nameof(LifeManager.Initialize))]
        private static void BattleStarted() => Tracker.BattleStarted();
    }

    /// <summary>
    /// Marks the start of a boss phase. Patches <c>Opponent.PostResetScalesSequence</c> and every override of it
    /// (the game calls it right after resetting the scales; virtual, so never inlined).
    /// </summary>
    [HarmonyPatch]
    internal static class PhaseStartPatch
    {
        /// <summary>Every declaration of <c>PostResetScalesSequence</c> on Opponent and its subclasses.</summary>
        /// <returns>The methods to patch.</returns>
        private static IEnumerable<MethodBase> TargetMethods() =>
            typeof(Opponent).Assembly.GetTypes()
                .Where(t => typeof(Opponent).IsAssignableFrom(t))
                .Select(t => AccessTools.DeclaredMethod(t, nameof(Opponent.PostResetScalesSequence)))
                .Where(m => m != null && !m.IsAbstract)
                .Cast<MethodBase>();

        /// <summary>The scales were just reset: the next round opens the new phase.</summary>
        private static void Prefix()
        {
            TurnManager turns = Singleton<TurnManager>.Instance;
            if (turns != null) KillCapPatches.Tracker.PhaseStarted(turns.TurnNumber);
        }
    }
}
