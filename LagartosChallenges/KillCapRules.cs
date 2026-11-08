using System;

namespace LagartosChallenges
{
    /// <summary>
    /// Damage math for No First Turn Kill / No Phase Turn Kill. The scale balance is opponent damage minus
    /// player damage: reaching +<see cref="Goal"/> wins, -<see cref="Goal"/> loses. Game-free, unit tested.
    /// </summary>
    public static class KillCapRules
    {
        /// <summary>Balance that ends the battle (vanilla <c>LifeManager.GOAL_BALANCE</c>).</summary>
        public const int Goal = 5;

        /// <summary>
        /// Caps damage so the scale stops one point short of a win or loss. Damage that doesn't reach it is
        /// returned unchanged.
        /// </summary>
        /// <param name="balance">Current balance (positive favours the player).</param>
        /// <param name="damage">Damage about to be added.</param>
        /// <param name="toPlayer">True when the player takes the damage.</param>
        /// <returns>Damage to actually add.</returns>
        public static int CapDamage(int balance, int damage, bool toPlayer)
        {
            int room = toPlayer ? balance + (Goal - 1) : (Goal - 1) - balance;
            return Math.Min(damage, Math.Max(0, room));
        }

        /// <summary>
        /// Caps a combat phase's total damage (<c>CombatPhaseManager.DamageDealtThisPhase</c>) before the game turns
        /// overkill into teeth, so a capped hit leaves no excess.
        /// </summary>
        /// <param name="balance">Current balance.</param>
        /// <param name="dealt">Damage dealt this combat phase.</param>
        /// <param name="playerIsAttacker">True during the player's combat.</param>
        /// <returns>Damage to keep.</returns>
        public static int CombatDamage(int balance, int dealt, bool playerIsAttacker) =>
            CapDamage(balance, dealt, toPlayer: !playerIsAttacker);
    }

    /// <summary>
    /// Tracks which round of a battle is "the first turn" for each kill-cap challenge: round 1 for
    /// No First Turn Kill, the round after the scales reset for No Phase Turn Kill. A round is the player's
    /// turn plus Leshy's following turn (the game's <c>TurnNumber</c>).
    /// </summary>
    public sealed class PhaseTracker
    {
        /// <summary>First round of the current boss phase (0 = still phase 1).</summary>
        private int phaseStartRound;

        /// <summary>A new battle began: back to phase 1.</summary>
        public void BattleStarted() => phaseStartRound = 0;

        /// <summary>The scales were reset after the opponent lost a life; the next round opens the new phase.</summary>
        /// <param name="currentRound">Round in which the reset happened.</param>
        public void PhaseStarted(int currentRound) => phaseStartRound = currentRound + 1;

        /// <summary>Whether damage in this round is capped.</summary>
        /// <param name="round">Current round (game <c>TurnNumber</c>).</param>
        /// <param name="firstTurnKillOn">No First Turn Kill is selected.</param>
        /// <param name="phaseTurnKillOn">No Phase Turn Kill is selected.</param>
        /// <returns>True when the cap applies.</returns>
        public bool IsCapRound(int round, bool firstTurnKillOn, bool phaseTurnKillOn) =>
            (firstTurnKillOn && round == 1 && phaseStartRound == 0) ||
            (phaseTurnKillOn && phaseStartRound > 0 && round == phaseStartRound);

        /// <summary>Which challenge a cap in this round belongs to (for its icon flash).</summary>
        /// <returns>True when the current cap round is a phase round.</returns>
        public bool InPhaseRound => phaseStartRound > 0;
    }
}
