using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for No First Turn Kill / No Phase Turn Kill: damage math and which rounds are capped.
/// Balance = opponent damage - player damage; +5 wins, -5 loses.
/// </summary>
public class KillCapRulesTests
{
    [Fact]
    public void Player_lethal_first_turn_capped_to_4() => Assert.Equal(4, KillCapRules.CapDamage(0, 7, toPlayer: false));

    [Fact]
    public void Tipped_scales_player_cap_is_5() => Assert.Equal(5, KillCapRules.CapDamage(-1, 9, toPlayer: false));

    [Fact]
    public void Leshy_lethal_first_turn_capped_to_4() => Assert.Equal(4, KillCapRules.CapDamage(0, 6, toPlayer: true));

    [Fact]
    public void Tipped_scales_leshy_cap_is_3() => Assert.Equal(3, KillCapRules.CapDamage(-1, 5, toPlayer: true));

    [Fact]
    public void Non_lethal_damage_untouched()
    {
        Assert.Equal(3, KillCapRules.CapDamage(0, 3, toPlayer: false));
        Assert.Equal(2, KillCapRules.CapDamage(2, 2, toPlayer: false));
        Assert.Equal(4, KillCapRules.CapDamage(0, 4, toPlayer: true));
    }

    [Fact]
    public void Second_hit_in_same_round_is_capped_by_running_balance() =>
        Assert.Equal(1, KillCapRules.CapDamage(3, 4, toPlayer: false));

    [Fact]
    public void Already_at_the_edge_deals_nothing() => Assert.Equal(0, KillCapRules.CapDamage(4, 2, toPlayer: false));

    [Fact]
    public void Round_1_caps_only_with_first_turn_challenge()
    {
        var tracker = new PhaseTracker();
        tracker.BattleStarted();
        Assert.True(tracker.IsCapRound(1, firstTurnKillOn: true, phaseTurnKillOn: false));
        Assert.False(tracker.IsCapRound(1, firstTurnKillOn: false, phaseTurnKillOn: true));
        Assert.False(tracker.IsCapRound(2, firstTurnKillOn: true, phaseTurnKillOn: true));
    }

    [Fact]
    public void Phase_round_caps_only_with_phase_challenge()
    {
        var tracker = new PhaseTracker();
        tracker.BattleStarted();
        tracker.PhaseStarted(4);
        Assert.True(tracker.IsCapRound(5, firstTurnKillOn: false, phaseTurnKillOn: true));
        Assert.False(tracker.IsCapRound(5, firstTurnKillOn: true, phaseTurnKillOn: false));
        Assert.False(tracker.IsCapRound(6, firstTurnKillOn: true, phaseTurnKillOn: true));
    }

    [Fact]
    public void Phase_started_twice_in_same_round_is_idempotent()
    {
        var tracker = new PhaseTracker();
        tracker.PhaseStarted(4);
        tracker.PhaseStarted(4);
        Assert.True(tracker.IsCapRound(5, firstTurnKillOn: false, phaseTurnKillOn: true));
    }

    [Fact]
    public void Battle_start_clears_phase()
    {
        var tracker = new PhaseTracker();
        tracker.PhaseStarted(4);
        tracker.BattleStarted();
        Assert.False(tracker.IsCapRound(5, firstTurnKillOn: true, phaseTurnKillOn: true));
    }
}
