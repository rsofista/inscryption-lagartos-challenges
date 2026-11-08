using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the mod's translation table: every text the player sees must exist in every game language.
/// </summary>
public class TranslationsTests
{
    /// <summary>Codes of the 11 non-English languages the base game ships (InscryptionAPI language codes).</summary>
    private static readonly string[] GameLanguages = { "fr", "it", "de", "es", "pt", "tr", "ru", "ja", "ko", "zhcn", "zhtw" };

    /// <summary>Every English text the mod shows in game.</summary>
    public static TheoryData<string> AllTexts
    {
        get
        {
            var texts = new TheoryData<string> { Translations.DoubleBloodName, Translations.DoubleBloodDescription };
            foreach (ChallengeDef def in ChallengeCatalog.All)
            {
                texts.Add(def.Title);
                texts.Add(def.Description);
            }
            return texts;
        }
    }

    [Theory]
    [MemberData(nameof(AllTexts))]
    public void Every_text_is_translated_to_every_game_language(string english)
    {
        Assert.True(Translations.All.ContainsKey(english), $"No translations for \"{english}\"");
        foreach (string language in GameLanguages)
        {
            Assert.True(Translations.All[english].TryGetValue(language, out string? text) && !string.IsNullOrWhiteSpace(text),
                $"\"{english}\" is missing {language}");
        }
    }

    [Fact]
    public void Table_has_no_texts_the_mod_does_not_show()
    {
        Assert.Equal(AllTexts.Count(), Translations.All.Count);
    }

    [Fact]
    public void Sigil_description_keeps_the_creature_placeholder_in_every_language()
    {
        Assert.Contains("[creature]", Translations.DoubleBloodDescription);
        foreach (string language in GameLanguages)
        {
            Assert.Contains("[creature]", Translations.All[Translations.DoubleBloodDescription][language]);
        }
    }

    [Fact]
    public void Sigil_name_in_the_challenge_description_matches_the_sigil_name()
    {
        Assert.Contains(Translations.DoubleBloodName, Translations.WorthySquirrelDescription);
        foreach (string language in GameLanguages)
        {
            Assert.Contains(Translations.All[Translations.DoubleBloodName][language],
                Translations.All[Translations.WorthySquirrelDescription][language], StringComparison.OrdinalIgnoreCase);
        }
    }
}
