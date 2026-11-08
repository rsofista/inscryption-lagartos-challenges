using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Every challenge in the catalog must ship both icons (49x49 PNGs embedded from LagartosChallenges/Assets).
/// </summary>
public class IconAssetsTests
{
    /// <summary>Assets folder, reached from the test binaries (bin/Debug/net8.0).</summary>
    private static readonly string AssetsDir =
        Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "LagartosChallenges", "Assets"));

    /// <summary>Both icon files of every catalog entry.</summary>
    public static TheoryData<string> IconFiles
    {
        get
        {
            var files = new TheoryData<string>();
            foreach (ChallengeDef def in ChallengeCatalog.All)
            {
                files.Add(def.IconFile);
                files.Add(def.ActiveIconFile);
            }
            return files;
        }
    }

    [Theory]
    [MemberData(nameof(IconFiles))]
    public void Every_catalog_icon_exists_and_is_49x49(string file)
    {
        string path = Path.Combine(AssetsDir, file);
        Assert.True(File.Exists(path), $"Missing {path}");
        byte[] png = File.ReadAllBytes(path);
        // IHDR: width at bytes 16..19, height at 20..23 (big endian).
        int width = (png[16] << 24) | (png[17] << 16) | (png[18] << 8) | png[19];
        int height = (png[20] << 24) | (png[21] << 16) | (png[22] << 8) | png[23];
        Assert.Equal((49, 49), (width, height));
    }
}
