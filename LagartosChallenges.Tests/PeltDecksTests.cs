using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the eight pelt starter decks.
/// </summary>
public class PeltDecksTests
{
    /// <summary>Counts how many of each pelt a deck holds, in (rabbit, wolf, golden) order.</summary>
    private static (int rabbit, int wolf, int golden) Count(PeltDeck deck) => (
        deck.CardNames.Count(n => n == PeltDecks.RabbitPelt),
        deck.CardNames.Count(n => n == PeltDecks.WolfPelt),
        deck.CardNames.Count(n => n == PeltDecks.GoldenPelt));

    [Fact]
    public void Pelt_card_names_match_the_game()
    {
        Assert.Equal("PeltHare", PeltDecks.RabbitPelt);
        Assert.Equal("PeltWolf", PeltDecks.WolfPelt);
        Assert.Equal("PeltGolden", PeltDecks.GoldenPelt);
    }

    [Fact]
    public void There_are_eight_decks()
    {
        Assert.Equal(8, PeltDecks.All.Count);
    }

    [Fact]
    public void Every_deck_has_three_pelts_and_nothing_else()
    {
        // Like the vanilla decks: 3 cards, and Kaycee's Mod adds its 2 bonus cards on top.
        foreach (PeltDeck deck in PeltDecks.All)
        {
            var (rabbit, wolf, golden) = Count(deck);
            Assert.Equal(3, deck.CardNames.Count);
            Assert.Equal(3, rabbit + wolf + golden);
        }
    }

    [Fact]
    public void Decks_have_the_requested_pelts_in_order()
    {
        Assert.Equal((3, 0, 0), Count(PeltDecks.All[0]));
        Assert.Equal((2, 1, 0), Count(PeltDecks.All[1]));
        Assert.Equal((2, 0, 1), Count(PeltDecks.All[2]));
        Assert.Equal((1, 2, 0), Count(PeltDecks.All[3]));
        Assert.Equal((1, 1, 1), Count(PeltDecks.All[4]));
        Assert.Equal((0, 2, 1), Count(PeltDecks.All[5]));
        Assert.Equal((0, 1, 2), Count(PeltDecks.All[6]));
        Assert.Equal((0, 0, 3), Count(PeltDecks.All[7]));
    }

    [Fact]
    public void Every_pelt_has_a_pixel_portrait()
    {
        Assert.Equal(3, PeltDecks.PixelPortraits.Count);
        Assert.Equal("pixel_rabbit.png", PeltDecks.PixelPortraits[PeltDecks.RabbitPelt]);
        Assert.Equal("pixel_wolf.png", PeltDecks.PixelPortraits[PeltDecks.WolfPelt]);
        Assert.Equal("pixel_golden.png", PeltDecks.PixelPortraits[PeltDecks.GoldenPelt]);
    }

    [Fact]
    public void Every_deck_has_a_distinct_title_and_icon()
    {
        Assert.Equal(8, PeltDecks.All.Select(d => d.Title).Distinct().Count());
        Assert.Equal(8, PeltDecks.All.Select(d => d.IconFile).Distinct().Count());
        Assert.All(PeltDecks.All, d => Assert.EndsWith(".png", d.IconFile));
    }

    [Fact]
    public void Every_deck_icon_ships_in_assets()
    {
        string assets = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "LagartosChallenges", "Assets"));
        Assert.All(PeltDecks.All, d => Assert.True(File.Exists(Path.Combine(assets, d.IconFile)), $"Missing {d.IconFile}"));
    }
}
