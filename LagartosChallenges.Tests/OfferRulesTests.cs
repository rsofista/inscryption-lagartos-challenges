using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for filling an offer: keep passing cards, replace failing ones from the pool, pad with filler.
/// Cards are plain strings here; "*" marks a passing card and "FILLER" is the filler.
/// </summary>
public class OfferRulesTests
{
    private static bool Passes(string card) => card.EndsWith("*") || card == "FILLER";

    private static string Filler() => "FILLER";

    private static List<string> Fill(IReadOnlyList<string> offer, IReadOnlyList<string> pool, int seed = 7) =>
        OfferRules.Fill(offer, pool, Passes, c => c, Filler, seed);

    [Fact]
    public void Fill_keeps_passing_cards_in_place()
    {
        var result = Fill(new[] { "a*", "b", "c*" }, new[] { "x*" });
        Assert.Equal("a*", result[0]);
        Assert.Equal("c*", result[2]);
        Assert.Equal("x*", result[1]);
    }

    [Fact]
    public void Fill_replaces_failing_with_distinct_pool_cards()
    {
        var result = Fill(new[] { "a", "b", "c" }, new[] { "x*", "y*", "z*", "q" });
        Assert.Equal(3, result.Count);
        Assert.All(result, c => Assert.EndsWith("*", c));
        Assert.Equal(3, result.Distinct().Count());
    }

    [Fact]
    public void Fill_never_duplicates_a_kept_card()
    {
        var result = Fill(new[] { "x*", "b", "c" }, new[] { "x*", "y*" });
        Assert.Equal(new[] { "FILLER", "x*", "y*" }, result.OrderBy(c => c));
    }

    [Fact]
    public void Fill_pads_with_filler_when_pool_runs_out()
    {
        var result = Fill(new[] { "a", "b", "c" }, new[] { "x*", "q" });
        Assert.Equal(3, result.Count);
        Assert.Single(result, "x*");
        Assert.Equal(2, result.Count(c => c == "FILLER"));
    }

    [Fact]
    public void Fill_offer_with_no_matches_is_all_filler()
    {
        var result = Fill(new[] { "a", "b", "c" }, new[] { "q", "r" });
        Assert.Equal(new[] { "FILLER", "FILLER", "FILLER" }, result);
    }

    [Fact]
    public void Fill_is_deterministic_for_a_seed()
    {
        var pool = Enumerable.Range(0, 20).Select(i => $"p{i}*").ToList();
        Assert.Equal(Fill(new[] { "a", "b", "c" }, pool, 42), Fill(new[] { "a", "b", "c" }, pool, 42));
    }

    [Fact]
    public void Pick_or_filler_returns_a_passing_pool_card()
    {
        Assert.Equal("y*", OfferRules.PickOrFiller(new[] { "a", "y*", "b" }, Passes, Filler, 3));
    }

    [Fact]
    public void Pick_or_filler_returns_filler_on_empty_pool()
    {
        Assert.Equal("FILLER", OfferRules.PickOrFiller(new[] { "a", "b" }, Passes, Filler, 3));
        Assert.Equal("FILLER", OfferRules.PickOrFiller(Array.Empty<string>(), Passes, Filler, 3));
    }
}
