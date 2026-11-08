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
            if (!InCapRound(out TurnManager turns)) return;

            int capped = KillCapRules.CapDamage(__instance.Balance, damage, toPlayer);
            if (capped == damage) return;
            Plugin.Log.LogInfo($"Kill cap: round {turns.TurnNumber}, balance {__instance.Balance}, " +
                               $"{(toPlayer ? "player" : "opponent")} would take {damage}, capped to {capped}.");
            damage = capped;
            if (numWeights > capped) numWeights = capped;
            ShowCapActivation();
        }

        /// <summary>Whether the current round is capped (also false outside Kaycee's Mod or without the challenges).</summary>
        /// <param name="turns">The turn manager, when it exists.</param>
        /// <returns>True when damage this round is capped.</returns>
        internal static bool InCapRound(out TurnManager turns)
        {
            turns = Singleton<TurnManager>.Instance;
            bool firstOn = Challenges.IsActive(ChallengeKey.NoFirstTurnKill);
            bool phaseOn = Challenges.IsActive(ChallengeKey.NoPhaseTurnKill);
            return (firstOn || phaseOn) && turns != null && Tracker.IsCapRound(turns.TurnNumber, firstOn, phaseOn);
        }

        /// <summary>Flashes the icon of the challenge that capped this round.</summary>
        internal static void ShowCapActivation() =>
            Challenges.ShowActivation(Tracker.InPhaseRound ? ChallengeKey.NoPhaseTurnKill : ChallengeKey.NoFirstTurnKill);

        /// <summary>A battle's scales are set up: back to phase 1 (virtual method, never inlined).</summary>
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LifeManager), nameof(LifeManager.Initialize))]
        private static void BattleStarted() => Tracker.BattleStarted();
    }

    /// <summary>
    /// Caps combat damage before <c>CombatPhaseManager.DoCombatPhase</c> turns overkill into teeth: it computes the
    /// excess from <c>DamageDealtThisPhase</c> right after <c>VisualizeDamageMovingToScales</c>, so the total is
    /// clamped on that coroutine's first step (it also shows the capped weights). Patches the state machine's
    /// <c>MoveNext</c> of every declaration: Act 1 runs <c>CombatPhaseManager3D</c>'s override, never the base no-op.
    /// </summary>
    [HarmonyPatch]
    internal static class CombatDamageCapPatch
    {
        /// <summary>The damage-to-scales coroutine's state machine step, for the base class and every override.</summary>
        /// <returns>Their <c>MoveNext</c> methods.</returns>
        private static IEnumerable<MethodBase> TargetMethods() =>
            PatchTargets.Declarations(AccessTools.GetTypesFromAssembly(typeof(CombatPhaseManager).Assembly),
                    typeof(CombatPhaseManager), "VisualizeDamageMovingToScales")
                .Select(m => (MethodBase)AccessTools.EnumeratorMoveNext(m))
                .Where(m => m != null);

        /// <summary>Clamps <c>DamageDealtThisPhase</c> in a capped round (idempotent, so every step may run it).</summary>
        private static void Prefix()
        {
            if (!KillCapPatches.InCapRound(out TurnManager turns)) return;
            CombatPhaseManager combat = turns.CombatPhaseManager;
            LifeManager life = Singleton<LifeManager>.Instance;
            if (combat == null || life == null) return;
            int dealt = combat.DamageDealtThisPhase;
            int capped = KillCapRules.CombatDamage(life.Balance, dealt, playerIsAttacker: turns.IsPlayerTurn);
            if (capped == dealt) return;
            Plugin.Log.LogInfo($"Kill cap: combat damage {dealt} capped to {capped} (no overkill teeth).");
            Traverse.Create(combat).Property(nameof(CombatPhaseManager.DamageDealtThisPhase)).SetValue(capped);
            KillCapPatches.ShowCapActivation();
        }
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
            PatchTargets.Declarations(AccessTools.GetTypesFromAssembly(typeof(Opponent).Assembly),
                    typeof(Opponent), nameof(Opponent.PostResetScalesSequence))
                .Cast<MethodBase>();

        /// <summary>The scales were just reset: the next round opens the new phase.</summary>
        private static void Prefix()
        {
            TurnManager turns = Singleton<TurnManager>.Instance;
            if (turns != null) KillCapPatches.Tracker.PhaseStarted(turns.TurnNumber);
        }
    }
}
