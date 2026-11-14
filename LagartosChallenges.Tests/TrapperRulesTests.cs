using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the One Squirrel side deck.
/// </summary>
public class TrapperRulesTests
{
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
