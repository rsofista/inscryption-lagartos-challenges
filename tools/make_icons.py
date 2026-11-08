"""Generates the mod's pixel-art icons into PeltStarterHands/Assets (stdlib only, no PIL).

Run: python tools/make_icons.py

- deck_*.png (35x44): starter deck icons, one small pelt per card (2 on top, 1 below).
- thunderstore/icon.png (256x256): the assortment deck icon and the challenge icon side by side, upscaled.
- pixel_*.png (41x28): Act II style pixel portraits for the three pelts, which the base game lacks. The Kaycee's
  Mod starter deck screen draws cards in that style. Black silhouette with 50% black shading, like the game's own.
- challenge_icon*.png (49x49): Annoying Trades challenge, a pelt turning into an Annoying card.
  Same frame and palette as the Unfair Hand icons (2px transparent margin, dark/red background).
"""
import os
import struct
import zlib

ROOT = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..")
ASSETS = os.path.join(ROOT, "PeltStarterHands", "Assets")
THUNDERSTORE = os.path.join(ROOT, "thunderstore")

CLEAR = (0, 0, 0, 0)
BG = (40, 32, 26, 255)
BG_ACTIVE = (120, 20, 20, 255)
BEIGE = (222, 205, 170, 255)
RED = (190, 30, 30, 255)
BROWN = (140, 90, 50, 255)
OUTLINE = (20, 15, 12, 255)

# Pelt colours: (fill, shade).
RABBIT = ((222, 205, 170, 255), (170, 145, 110, 255))
WOLF = ((150, 150, 145, 255), (95, 95, 92, 255))
GOLDEN = ((235, 190, 60, 255), (180, 130, 30, 255))

# Hide stretched by its four corners, 9x11. f = fill, s = shade, o = outline.
PELT = [
    "oo.....oo",
    "ofooooofo",
    ".offfffo.",
    ".offsffo.",
    ".ofsssfo.",
    ".ofsssfo.",
    ".ofsssfo.",
    ".offsffo.",
    ".offfffo.",
    "ofooooofo",
    "oo.....oo",
]


def write_png(path, pixels):
    """Writes an RGBA image given as a list of rows of (r, g, b, a) tuples."""
    h, w = len(pixels), len(pixels[0])
    raw = b"".join(b"\x00" + b"".join(bytes(p) for p in row) for row in pixels)

    def chunk(kind, data):
        return struct.pack(">I", len(data)) + kind + data + struct.pack(">I", zlib.crc32(kind + data) & 0xFFFFFFFF)

    png = b"\x89PNG\r\n\x1a\n"
    png += chunk(b"IHDR", struct.pack(">IIBBBBB", w, h, 8, 6, 0, 0, 0))
    png += chunk(b"IDAT", zlib.compress(raw, 9))
    png += chunk(b"IEND", b"")
    with open(path, "wb") as f:
        f.write(png)


def canvas(w, h, colour):
    """New image filled with one colour."""
    return [[colour] * w for _ in range(h)]


def stamp(img, pattern, x0, y0, colours, scale=1):
    """Draws a char pattern at (x0, y0); chars missing from colours are transparent."""
    for y, row in enumerate(pattern):
        for x, ch in enumerate(row):
            if ch in colours:
                for dy in range(scale):
                    for dx in range(scale):
                        img[y0 + y * scale + dy][x0 + x * scale + dx] = colours[ch]


def rect(img, x0, y0, x1, y1, colour):
    """Fills the inclusive rectangle."""
    for y in range(y0, y1 + 1):
        for x in range(x0, x1 + 1):
            img[y][x] = colour


def deck_icon(name, pelts):
    """35x44 deck icon: dark card with a brown border and 3 pelts (2 on top, 1 below). Returns the image."""
    img = canvas(35, 44, BG)
    for x in range(35):
        img[0][x] = img[43][x] = BROWN
    for y in range(44):
        img[y][0] = img[y][34] = BROWN
    slots = [(7, 9), (19, 9), (13, 24)]
    for (fill, shade), (x, y) in zip(pelts, slots):
        stamp(img, PELT, x, y, {"f": fill, "s": shade, "o": OUTLINE})
    write_png(os.path.join(ASSETS, name), img)
    return img


def challenge_icon(name, background):
    """49x49 challenge icon: pelt -> arrow -> card bearing a red Annoying mark. Returns the image."""
    img = canvas(49, 49, CLEAR)
    rect(img, 2, 2, 46, 46, background)
    # Pelt, double size (18x22), left.
    stamp(img, PELT, 4, 13, {"f": BEIGE, "s": BROWN, "o": background}, scale=2)
    # Arrow.
    rect(img, 23, 23, 27, 25, BEIGE)
    stamp(img, ["x..", "xx.", "xxx", "xx.", "x.."], 28, 22, {"x": BEIGE})
    # Card with a red "!" (the Annoying sigil riles up the opponent).
    rect(img, 32, 13, 44, 35, BEIGE)
    rect(img, 34, 15, 42, 33, BROWN)
    rect(img, 37, 17, 39, 27, RED)
    rect(img, 37, 30, 39, 32, RED)
    write_png(os.path.join(ASSETS, name), img)
    return img


def store_icon(deck, challenge):
    """256x256 Thunderstore icon: deck icon and challenge icon, both x3, side by side on the dark background."""
    img = canvas(256, 256, BG)
    for src, scale, x0 in ((deck, 3, 6), (challenge, 3, 108)):
        h, w = len(src), len(src[0])
        y0 = (256 - h * scale) // 2
        for y in range(h * scale):
            for x in range(w * scale):
                p = src[y // scale][x // scale]
                if p[3]:
                    img[y0 + y][x0 + x] = p
    write_png(os.path.join(THUNDERSTORE, "icon.png"), img)


INK = (0, 0, 0, 255)
HALF = (0, 0, 0, 128)


def ellipse(mask, cx, cy, rx, ry):
    """Adds a filled ellipse to a set of (x, y) pixels."""
    for y in range(28):
        for x in range(41):
            if ((x - cx) / rx) ** 2 + ((y - cy) / ry) ** 2 <= 1:
                mask.add((x, y))


def line(mask, x0, y0, x1, y1, thick):
    """Adds a thick line (a run of filled circles) to a set of pixels."""
    steps = max(abs(x1 - x0), abs(y1 - y0)) * 2 or 1
    for i in range(steps + 1):
        t = i / steps
        ellipse(mask, x0 + (x1 - x0) * t, y0 + (y1 - y0) * t, thick / 2, thick / 2)


def pixel_portrait(name, mask, ink=(), holes=()):
    """41x28 portrait: mask edge in black, inside at 50% black, plus extra black pixels and see-through holes."""
    img = canvas(41, 28, CLEAR)
    for (x, y) in mask:
        edge = any((x + dx, y + dy) not in mask for dx, dy in ((1, 0), (-1, 0), (0, 1), (0, -1)))
        img[y][x] = INK if edge else HALF
    for (x, y) in ink:
        img[y][x] = INK
    for (x, y) in holes:
        img[y][x] = CLEAR
    write_png(os.path.join(ASSETS, name), img)


def rabbit_portrait():
    """Rabbit Pelt: small flat hide, long ears swept back, short legs, puff tail."""
    m = set()
    ellipse(m, 22, 14, 9, 6)
    ellipse(m, 10, 14, 4, 3.5)
    line(m, 8, 12, 3, 6, 3)
    line(m, 8, 16, 3, 22, 3)
    for x0, x1 in ((16, 13), (28, 31)):
        line(m, x0, 10, x1, 5, 3)
        line(m, x0, 18, x1, 23, 3)
    ellipse(m, 32, 14, 2.5, 2.5)
    pixel_portrait("pixel_rabbit.png", m, holes=[(10, 12), (10, 16)])


def wolf_portrait():
    """Wolf Pelt: big flat hide, pointed ears, snout, clawed legs, bushy tail, dark spine."""
    m = set()
    ellipse(m, 21, 14, 11, 7)
    ellipse(m, 7, 14, 4, 4)
    ellipse(m, 3, 14, 2, 2)
    line(m, 6, 11, 5, 8, 2)
    line(m, 6, 17, 5, 20, 2)
    for x0, x1 in ((14, 10), (28, 32)):
        line(m, x0, 9, x1, 2, 3)
        line(m, x0, 19, x1, 26, 3)
    line(m, 31, 14, 37, 14, 3)
    ellipse(m, 36, 14, 3.5, 2.5)
    spine = [(x, 14) for x in range(13, 31) if x % 3 != 0]
    claws = [(9, 1), (11, 1), (31, 1), (33, 1), (9, 27), (11, 27), (31, 27), (33, 27)]
    pixel_portrait("pixel_wolf.png", m, ink=spine + [c for c in claws if 0 <= c[1] < 28],
                   holes=[(7, 12), (7, 16)])


def golden_portrait():
    """Golden Pelt (the game's golden fleece): curly-edged fleece, floppy ears, hooves, sparkles."""
    import math
    m = set()
    ellipse(m, 21, 14, 10, 6)
    for i in range(14):
        a = 2 * math.pi * i / 14
        ellipse(m, 21 + 10 * math.cos(a), 14 + 6 * math.sin(a), 2.3, 2.3)
    ellipse(m, 7, 14, 3, 3)
    line(m, 7, 11, 4, 8, 2)
    line(m, 7, 17, 4, 20, 2)
    for x0, x1 in ((14, 11), (28, 31)):
        line(m, x0, 8, x1, 3, 2)
        line(m, x0, 20, x1, 25, 2)
    curls = [(x, y) for (x, y) in ((16, 12), (17, 11), (22, 11), (23, 12), (27, 13), (28, 14),
                                   (15, 16), (16, 17), (20, 15), (21, 16), (25, 17), (26, 16))]
    hooves = [(10, 2), (11, 2), (31, 2), (32, 2), (10, 26), (11, 26), (31, 26), (32, 26)]
    sparkles = []
    for cx, cy in ((2, 2), (38, 3), (38, 24), (2, 25)):
        sparkles += [(cx, cy), (cx - 1, cy), (cx + 1, cy), (cx, cy - 1), (cx, cy + 1)]
    pixel_portrait("pixel_golden.png", m, ink=curls + hooves + sparkles, holes=[(7, 13), (7, 15)])


def main():
    os.makedirs(ASSETS, exist_ok=True)
    deck_icon("deck_rabbit.png", [RABBIT] * 3)
    deck_icon("deck_rabbit_wolf.png", [RABBIT] * 2 + [WOLF])
    deck_icon("deck_rabbit_golden.png", [RABBIT] * 2 + [GOLDEN])
    deck_icon("deck_wolf_rabbit.png", [RABBIT] + [WOLF] * 2)
    assortment = deck_icon("deck_assortment.png", [RABBIT, WOLF, GOLDEN])
    deck_icon("deck_wolf_golden.png", [WOLF] * 2 + [GOLDEN])
    rabbit_portrait()
    wolf_portrait()
    golden_portrait()
    challenge = challenge_icon("challenge_icon.png", BG)
    challenge_icon("challenge_icon_activated.png", BG_ACTIVE)
    os.makedirs(THUNDERSTORE, exist_ok=True)
    store_icon(assortment, challenge)


if __name__ == "__main__":
    main()
