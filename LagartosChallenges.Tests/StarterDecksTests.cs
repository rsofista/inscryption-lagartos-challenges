using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the starter decks: eight pelt decks and the Royal's crew.
/// </summary>
public class StarterDecksTests
{
    /// <summary>Counts how many of each pelt a deck holds, in (rabbit, wolf, golden) order.</summary>
    private static (int rabbit, int wolf, int golden) Count(StarterDeck deck) => (
        deck.CardNames.Count(n => n == StarterDecks.RabbitPelt),
        deck.CardNames.Count(n => n == StarterDecks.WolfPelt),
        deck.CardNames.Count(n => n == StarterDecks.GoldenPelt));

    [Fact]
    public void Pelt_card_names_match_the_game()
    {
        Assert.Equal("PeltHare", StarterDecks.RabbitPelt);
        Assert.Equal("PeltWolf", StarterDecks.WolfPelt);
        Assert.Equal("PeltGolden", StarterDecks.GoldenPelt);
    }

    /// <summary>Assets folder, reached from the test binaries (bin/Debug/net8.0).</summary>
    private static readonly string AssetsDir =
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "LagartosChallenges", "Assets"));

    /// <summary>Reads a PNG's (width, height) from its IHDR chunk.</summary>
    private static (int width, int height) PngSize(string file)
    {
        byte[] png = File.ReadAllBytes(Path.Combine(AssetsDir, file));
        return ((png[16] << 24) | (png[17] << 16) | (png[18] << 8) | png[19],
                (png[20] << 24) | (png[21] << 16) | (png[22] << 8) | png[23]);
    }

    [Fact]
    public void There_are_eight_pelt_decks_and_nine_decks_in_all()
    {
        Assert.Equal(8, StarterDecks.Pelt.Count);
        Assert.Equal(9, StarterDecks.All.Count);
        Assert.Equal(StarterDecks.Pelt, StarterDecks.All.Take(8));
    }

    [Fact]
    public void Royal_card_names_match_the_game()
    {
        Assert.Equal("MoleSeaman", StarterDecks.MoleSeaman);
        Assert.Equal("SkeletonPirate", StarterDecks.SkeletonCrew);
        Assert.Equal("SkeletonParrot", StarterDecks.ZombieParrot);
    }

    [Fact]
    public void Royal_deck_is_last_and_holds_the_crew()
    {
        StarterDeck royal = StarterDecks.All[8];
        Assert.Same(StarterDecks.Royal, royal);
        Assert.Equal("Royal's Crew", royal.Title);
        Assert.Equal("deck_royal.png", royal.IconFile);
        Assert.Equal(new[] { StarterDecks.MoleSeaman, StarterDecks.SkeletonCrew, StarterDecks.ZombieParrot }, royal.CardNames);
    }

    [Fact]
    public void Every_deck_card_has_a_pixel_portrait_that_ships_and_is_41x28()
    {
        foreach (string card in StarterDecks.All.SelectMany(d => d.CardNames).Distinct())
        {
            Assert.True(StarterDecks.PixelPortraits.TryGetValue(card, out string? file), $"No portrait for {card}");
            Assert.Equal((41, 28), PngSize(file!));
        }
    }

    [Fact]
    public void Every_deck_icon_is_35x44()
    {
        Assert.All(StarterDecks.All, d => Assert.Equal((35, 44), PngSize(d.IconFile)));
    }

    [Fact]
    public void Every_deck_has_three_pelts_and_nothing_else()
    {
        // Like the vanilla decks: 3 cards, and Kaycee's Mod adds its 2 bonus cards on top.
        foreach (StarterDeck deck in StarterDecks.Pelt)
        {
            var (rabbit, wolf, golden) = Count(deck);
            Assert.Equal(3, deck.CardNames.Count);
            Assert.Equal(3, rabbit + wolf + golden);
        }
    }

    [Fact]
    public void Decks_have_the_requested_pelts_in_order()
    {
        Assert.Equal((3, 0, 0), Count(StarterDecks.All[0]));
        Assert.Equal((2, 1, 0), Count(StarterDecks.All[1]));
        Assert.Equal((2, 0, 1), Count(StarterDecks.All[2]));
        Assert.Equal((1, 2, 0), Count(StarterDecks.All[3]));
        Assert.Equal((1, 1, 1), Count(StarterDecks.All[4]));
        Assert.Equal((0, 2, 1), Count(StarterDecks.All[5]));
        Assert.Equal((0, 1, 2), Count(StarterDecks.All[6]));
        Assert.Equal((0, 0, 3), Count(StarterDecks.All[7]));
    }

    [Fact]
    public void Every_pelt_has_a_pixel_portrait()
    {
        Assert.Equal("pixel_rabbit.png", StarterDecks.PixelPortraits[StarterDecks.RabbitPelt]);
        Assert.Equal("pixel_wolf.png", StarterDecks.PixelPortraits[StarterDecks.WolfPelt]);
        Assert.Equal("pixel_golden.png", StarterDecks.PixelPortraits[StarterDecks.GoldenPelt]);
    }

    [Fact]
    public void Every_deck_has_a_distinct_title_and_icon()
    {
        Assert.Equal(9, StarterDecks.All.Select(d => d.Title).Distinct().Count());
        Assert.Equal(9, StarterDecks.All.Select(d => d.IconFile).Distinct().Count());
        Assert.All(StarterDecks.All, d => Assert.EndsWith(".png", d.IconFile));
    }

    [Fact]
    public void Every_deck_icon_ships_in_assets()
    {
        string assets = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "LagartosChallenges", "Assets"));
        Assert.All(StarterDecks.All, d => Assert.True(File.Exists(Path.Combine(assets, d.IconFile)), $"Missing {d.IconFile}"));
    }

    /// <summary>
    /// The deck screen draws the icon through a SpriteMask: only alpha counts and the shape is filled with one colour.
    /// So, like the vanilla icons, it must be a white silhouette on a transparent background (an opaque picture
    /// shows as a plain block, i.e. no icon).
    /// </summary>
    [Fact]
    public void Every_deck_icon_is_a_white_silhouette_on_transparent()
    {
        foreach (StarterDeck deck in StarterDecks.All)
        {
            byte[,][] px = IconAssetsTests.DecodeRgba(Path.Combine(AssetsDir, deck.IconFile));
            int opaque = 0, total = px.Length;
            foreach (byte[] p in px)
            {
                Assert.True(p[3] == 0 || (p[0] == 255 && p[1] == 255 && p[2] == 255 && p[3] == 255),
                    $"{deck.IconFile} has a pixel that is neither transparent nor opaque white");
                if (p[3] == 255) opaque++;
            }
            Assert.InRange(opaque, total / 6, total * 3 / 4);
        }
    }

    [Fact]
    public void Every_deck_icon_looks_different()
    {
        var pictures = StarterDecks.All.Select(d => Convert.ToBase64String(
            IconAssetsTests.DecodeRgba(Path.Combine(AssetsDir, d.IconFile)).Cast<byte[]>().SelectMany(p => p).ToArray()));
        Assert.Equal(StarterDecks.All.Count, pictures.Distinct().Count());
    }
}
