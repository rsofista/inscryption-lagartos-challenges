using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the No X Event mapping and the Totem Kinship head pick.
/// </summary>
public class EventAndTotemRulesTests
{
    [Theory]
    [InlineData(MapEvent.BoneAltar, ChallengeKey.NoBoneAltar)]
    [InlineData(MapEvent.MysteriousStones, ChallengeKey.NoMysteriousStones)]
    [InlineData(MapEvent.Mycologists, ChallengeKey.NoMycologists)]
    [InlineData(MapEvent.DeckTrial, ChallengeKey.NoDeckTrial)]
    [InlineData(MapEvent.Consumables, ChallengeKey.NoConsumables)]
    [InlineData(MapEvent.Campfire, ChallengeKey.NoCampfire)]
    [InlineData(MapEvent.BuyPelts, ChallengeKey.NoBuyPelts)]
    [InlineData(MapEvent.TradePelts, ChallengeKey.NoTradePelts)]
    public void Each_event_maps_to_its_challenge(MapEvent e, ChallengeKey key) => Assert.Equal(key, EventRules.SkipChallengeFor(e));

    private static readonly string[] Eligible = { "Bird", "Canine", "Hooved", "Insect", "Reptile" };

    private static string? Pick(string[] deck, string[] owned, string[] shown, int seed = 1) =>
        TotemRules.TryPickKinshipTribe(deck, Eligible, owned, shown, seed, out string tribe) ? tribe : null;

    [Fact]
    public void Picks_a_deck_tribe()
    {
        string? pick = Pick(new[] { "Canine", "Insect", "Bird" }, Array.Empty<string>(), Array.Empty<string>());
        Assert.Contains(pick, new[] { "Canine", "Insect", "Bird" });
    }

    [Fact]
    public void Never_picks_owned_or_shown_heads()
    {
        for (int seed = 0; seed < 50; seed++)
        {
            Assert.Equal("Bird", Pick(new[] { "Canine", "Insect", "Bird" }, new[] { "Canine" }, new[] { "Insect" }, seed));
        }
    }

    [Fact]
    public void Never_picks_a_tribe_that_is_not_eligible() =>
        Assert.Null(Pick(new[] { "Squirrel" }, Array.Empty<string>(), Array.Empty<string>()));

    [Fact]
    public void Null_when_deck_has_no_tribes() => Assert.Null(Pick(Array.Empty<string>(), Array.Empty<string>(), Array.Empty<string>()));

    [Fact]
    public void Null_when_all_deck_tribes_taken() =>
        Assert.Null(Pick(new[] { "Canine", "Insect" }, new[] { "Canine" }, new[] { "Insect" }));

    [Fact]
    public void Deterministic_for_seed()
    {
        string[] deck = { "Canine", "Insect", "Bird", "Reptile", "Hooved" };
        Assert.Equal(Pick(deck, Array.Empty<string>(), Array.Empty<string>(), 9), Pick(deck, Array.Empty<string>(), Array.Empty<string>(), 9));
    }
}
