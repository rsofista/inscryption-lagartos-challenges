using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the Trapper under the filters and the One Squirrel side deck.
/// </summary>
public class TrapperRulesTests
{
    [Fact]
    public void Prices_unchanged_when_pelts_pass() =>
        Assert.Equal(new[] { 2, 5, 9 }, TrapperRules.Prices(new[] { 2, 5, 9 }, peltsPass: true));

    [Fact]
    public void All_slots_cost_rabbit_price_when_pelts_fail() =>
        Assert.Equal(new[] { 2, 2, 2 }, TrapperRules.Prices(new[] { 2, 5, 9 }, peltsPass: false));

    [Fact]
    public void Prices_do_not_touch_the_input()
    {
        int[] vanilla = { 2, 5, 9 };
        TrapperRules.Prices(vanilla, peltsPass: false);
        Assert.Equal(new[] { 2, 5, 9 }, vanilla);
    }

    [Fact]
    public void Side_deck_cut_to_two() =>
        Assert.Equal(2, TrapperRules.CutSideDeck(Enumerable.Repeat("Squirrel", 10).ToList(), openingSquirrels: 1).Count);

    [Fact]
    public void Side_deck_keeps_one_to_draw_after_two_opening_squirrels() =>
        Assert.Equal(3, TrapperRules.CutSideDeck(Enumerable.Repeat("Squirrel", 10).ToList(), openingSquirrels: 2).Count);

    [Fact]
    public void Short_side_deck_left_alone() =>
        Assert.Equal(new[] { "Squirrel" }, TrapperRules.CutSideDeck(new List<string> { "Squirrel" }, openingSquirrels: 1));
}
