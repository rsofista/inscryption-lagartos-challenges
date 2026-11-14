using System.IO.Compression;
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
    /// <summary>Active icon files only.</summary>
    public static TheoryData<string> ActiveIconFiles
    {
        get
        {
            var files = new TheoryData<string>();
            foreach (ChallengeDef def in ChallengeCatalog.All) files.Add(def.ActiveIconFile);
            return files;
        }
    }

    /// <summary>
    /// The game draws the active sprite behind the base icon, whose 2px outer margin is the only transparent area,
    /// so the yellow frame must fill that margin to be seen.
    /// </summary>
    [Theory]
    [MemberData(nameof(ActiveIconFiles))]
    public void Active_icon_frame_fills_the_transparent_margin(string file)
    {
        byte[,][] px = DecodeRgba(Path.Combine(AssetsDir, file));
        for (int i = 0; i < 49; i++)
        for (int t = 0; t < 2; t++)
        {
            foreach ((int x, int y) in new[] { (i, t), (i, 48 - t), (t, i), (48 - t, i) })
                Assert.Equal(new byte[] { 240, 200, 40, 255 }, px[y, x]);
        }
    }

    /// <summary>Decodes the PNGs tools/make_icons.py writes (8-bit RGBA, one IDAT, filter 0 on every row).</summary>
    /// <param name="path">PNG file.</param>
    /// <returns>Pixels as [y, x] → {r, g, b, a}.</returns>
    internal static byte[,][] DecodeRgba(string path)
    {
        byte[] png = File.ReadAllBytes(path);
        int width = (png[16] << 24) | (png[17] << 16) | (png[18] << 8) | png[19];
        int height = (png[20] << 24) | (png[21] << 16) | (png[22] << 8) | png[23];
        using var idat = new MemoryStream();
        for (int pos = 8; pos < png.Length;)
        {
            int len = (png[pos] << 24) | (png[pos + 1] << 16) | (png[pos + 2] << 8) | png[pos + 3];
            string kind = System.Text.Encoding.ASCII.GetString(png, pos + 4, 4);
            if (kind == "IDAT") idat.Write(png, pos + 8, len);
            pos += 12 + len;
        }
        idat.Position = 0;
        using var z = new ZLibStream(idat, CompressionMode.Decompress);
        using var raw = new MemoryStream();
        z.CopyTo(raw);
        byte[] data = raw.ToArray();
        var px = new byte[height, width][];
        int stride = 1 + width * 4;
        for (int y = 0; y < height; y++)
        {
            Assert.Equal(0, data[y * stride]);
            for (int x = 0; x < width; x++) px[y, x] = data.AsSpan(y * stride + 1 + x * 4, 4).ToArray();
        }
        return px;
    }
}
