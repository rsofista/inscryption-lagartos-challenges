using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the challenge catalog: the one list <c>Plugin</c> registers with InscryptionAPI.
/// </summary>
public class ChallengeCatalogTests
{
    /// <summary>Codes of the 11 non-English languages the base game ships (InscryptionAPI language codes).</summary>
    private static readonly string[] GameLanguages = { "fr", "it", "de", "es", "pt", "tr", "ru", "ja", "ko", "zhcn", "zhtw" };

    [Fact]
    public void Catalog_has_29_challenges() => Assert.Equal(29, ChallengeCatalog.All.Count);

    [Fact]
    public void Every_key_appears_once() =>
        Assert.Equal(Enum.GetValues<ChallengeKey>().OrderBy(k => k), ChallengeCatalog.All.Select(c => c.Key).OrderBy(k => k));

    [Fact]
    public void Titles_are_unique() => Assert.Equal(29, ChallengeCatalog.All.Select(c => c.Title).Distinct().Count());

    [Theory]
    [InlineData(ChallengeKey.NoFirstTurnKill, 5)]
    [InlineData(ChallengeKey.NoPhaseTurnKill, 5)]
    [InlineData(ChallengeKey.CanineOnly, 10)]
    [InlineData(ChallengeKey.ReptileOnly, 15)]
    [InlineData(ChallengeKey.HoovedOnly, 15)]
    [InlineData(ChallengeKey.NoTribeOnly, 10)]
    [InlineData(ChallengeKey.Blood3Only, 15)]
    [InlineData(ChallengeKey.Blood4Only, 20)]
    [InlineData(ChallengeKey.FreeOnly, 15)]
    [InlineData(ChallengeKey.OneSquirrel, 15)]
    [InlineData(ChallengeKey.TwoSquirrels, -5)]
    [InlineData(ChallengeKey.NoCampfire, 5)]
    [InlineData(ChallengeKey.NoTradePelts, 10)]
    [InlineData(ChallengeKey.TotemKinship, -5)]
    [InlineData(ChallengeKey.ForbiddenCards, -5)]
    [InlineData(ChallengeKey.UnfairHand, 5)]
    [InlineData(ChallengeKey.WorthySquirrel, -5)]
    [InlineData(ChallengeKey.AnnoyingTrades, 5)]
    public void Points_match_spec(ChallengeKey key, int points) => Assert.Equal(points, ChallengeCatalog.Get(key).Points);

    [Theory]
    [InlineData(ChallengeKey.UnfairHand, "Unfair Hand", "unfair_hand_icon.png")]
    [InlineData(ChallengeKey.AnnoyingTrades, "Annoying Trades", "annoying_trades_icon.png")]
    [InlineData(ChallengeKey.Blood1Only, "1 Blood Only", "blood1_only_icon.png")]
    [InlineData(ChallengeKey.NoTribeOnly, "No Tribe Only", "no_tribe_only_icon.png")]
    public void Absorbed_titles_and_icon_names_are_kept(ChallengeKey key, string title, string icon)
    {
        ChallengeDef def = ChallengeCatalog.Get(key);
        Assert.Equal(title, def.Title);
        Assert.Equal(icon, def.IconFile);
        Assert.Equal(icon.Replace(".png", "_activated.png"), def.ActiveIconFile);
    }

    [Fact]
    public void Forbidden_cards_title_and_icon()
    {
        ChallengeDef def = ChallengeCatalog.Get(ChallengeKey.ForbiddenCards);
        Assert.Equal("Forbidden Cards", def.Title);
        Assert.Equal("forbidden_cards_icon.png", def.IconFile);
    }

    [Fact]
    public void Every_title_and_description_is_translated()
    {
        foreach (ChallengeDef def in ChallengeCatalog.All)
        {
            foreach (string english in new[] { def.Title, def.Description })
            {
                Assert.True(Translations.All.ContainsKey(english), $"No translations for \"{english}\"");
                foreach (string language in GameLanguages)
                {
                    Assert.True(Translations.All[english].TryGetValue(language, out string? text) && !string.IsNullOrWhiteSpace(text),
                        $"\"{english}\" is missing {language}");
                }
            }
        }
    }
}
