using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// The challenge screen shows the description on one line and cuts what does not fit, so no description may be
/// longer than the longest vanilla challenge description in the same language (measured from the game's data).
/// </summary>
public class DescriptionLengthTests
{
    /// <summary>Longest vanilla challenge description per language ("en" plus InscryptionAPI language codes).</summary>
    private static readonly IReadOnlyDictionary<string, int> VanillaMax = new Dictionary<string, int>
    {
        ["en"] = 59, ["fr"] = 75, ["it"] = 67, ["de"] = 65, ["es"] = 74, ["pt"] = 64,
        ["tr"] = 74, ["ru"] = 62, ["ja"] = 32, ["ko"] = 29, ["zhcn"] = 24, ["zhtw"] = 21,
    };

    /// <summary>Every challenge description in every language.</summary>
    public static TheoryData<string, string, string> Descriptions
    {
        get
        {
            var data = new TheoryData<string, string, string>();
            foreach (ChallengeDef def in ChallengeCatalog.All)
            {
                data.Add(def.Title, "en", def.Description);
                foreach (KeyValuePair<string, string> t in Translations.All[def.Description])
                    data.Add(def.Title, t.Key, t.Value);
            }
            return data;
        }
    }

    [Theory]
    [MemberData(nameof(Descriptions))]
    public void Description_fits_the_challenge_screen(string title, string language, string text) =>
        Assert.True(text.Length <= VanillaMax[language],
            $"{title} [{language}] has {text.Length} chars (max {VanillaMax[language]}): {text}");
}
