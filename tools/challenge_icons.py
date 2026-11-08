"""Pixel art for the 24 new Lagarto's Challenges icons (49x49, 2px transparent margin, dark background).

make_all() writes <key>_icon.png and <key>_icon_activated.png for each challenge and returns the base images by
key. Patterns are text art: each char maps to a colour in PALETTE, '.' is background.
"""
import os

CLEAR = (0, 0, 0, 0)
BG = (40, 32, 26, 255)
PALETTE = {
    "W": (222, 205, 170, 255),  # beige / bone
    "R": (190, 30, 30, 255),    # blood red
    "B": (140, 90, 50, 255),    # brown
    "D": (20, 15, 12, 255),     # ink
    "G": (130, 130, 125, 255),  # stone grey
    "Y": (240, 200, 40, 255),   # flame yellow
    "O": (220, 110, 30, 255),   # flame orange
}

PAW = ["...WW...WW...", "...WW...WW...", ".............", ".WW.......WW.", ".WW..WWW..WW.", "....WWWWW....",
       "...WWWWWWW...", "...WWWWWWW...", "....WWWWW...."]
BIRD = [".....WW......", "....WWWWR....", "....WWWW.....", ".W..WWW......", ".WWWWWWW.....", "..WWWWWWW....",
        "...WWWWWW....", "....WWWW.....", ".....W.W.....", "....WW.WW...."]
INSECT = ["...W.....W...", "....W...W....", ".....WWW.....", "....WWWWW....", ".W..WWWWW..W.", "..WWWWWWWWW..",
          "....WWWWW....", "..WWWWWWWWW..", ".W..WWWWW..W.", "....WWWWW....", ".....WWW....."]
SNAKE = ["........WWW..", ".......WWRWW.", ".......WWWWW.", "......WWW....", ".....WWW.....", "....WWW......",
         "...WWW.......", "..WWWWWWWW...", ".WW......WW..", ".WWWWWWWWW..."]
ANTLERS = ["W.W.......W.W", "WWW.......WWW", ".W.W.....W.W.", "..WWW...WWW..", "....WWWWW....", "....WDWDW....",
           "....WWWWW....", ".....WWW.....", ".....WWW.....", "......W......"]
NO_TRIBE = ["....WWWWW....", "..WW.....WW..", ".W.........W.", ".W.......R.W.", "W.......R...W", "W......R....W",
            "W.....R.....W", "W....R......W", "W...R.......W", ".W.R.......W.", ".W.........W.", "..WW.....WW..",
            "....WWWWW...."]
DROP = ["..R..", "..R..", ".RRR.", ".RRR.", "RRRRR", "RRRRR", "RRRRR", ".RRR."]
BONE = ["WW.......WW", "WWW.....WWW", ".WWWWWWWWW.", "WWW.....WWW", "WW.......WW"]
ZERO = [".WWWWW.", "WW...WW", "WW...WW", "WW...WW", "WW...WW", "WW...WW", ".WWWWW."]
SCALES = ["......W......", ".WWWWWWWWWWW.", ".W....W....W.", "W.W...W...W.W", "W.W...W...W.W", "WWW...W...WWW",
          "......W......", "......W......", "....WWWWW...."]
DIGITS = {"1": [".W.", "WW.", ".W.", ".W.", "WWW"], "2": ["WW.", "..W", ".W.", "W..", "WWW"]}
SQUIRREL = [".......WWW...", "......WWWWW..", ".WW..WWWWWW..", "WWWW.WWWWW...", "WWWWWWWWW....", ".WWWWWWWW....",
            "..WWWWWWW....", "...WWWWWW....", "...WWWWWWW...", "....WW..WW..."]
ALTAR = [".WW.....WW.", ".WWWWWWWWW.", ".WW.....WW.", "...........", "BBBBBBBBBBB", ".BBBBBBBBB.", ".BB.....BB.",
         ".BB.....BB."]
STONES = ["....GGG......", "...GGGGG.....", "..GGGGGGG....", "..GGGGGGG.GG.", "...GGGGG.GGGG", ".........GGGG",
          "..........GG."]
MUSHROOM = ["....RRRRR....", "..RRWRRRWRR..", ".RRRRRRRRRRR.", "RRWRRRRRWRRRR", "RRRRRRRRRRRRR", "....WWWWW....",
            "....WWWWW....", "....WWWWW....", "...WWWWWWW..."]
CARDS = ["....WWWWWWW", "....WBBBBBW", "..WWWWWWWBW", "..WBBBBBWBW", "WWWWWWWBWBW", "WBBBBBWBWBW", "WBBBBBWBWWW",
         "WBBBBBWBW..", "WBBBBBWWW..", "WBBBBBW....", "WWWWWWW...."]
PACK = ["....BBBBB....", "...B.....B...", "..BBBBBBBBB..", "..BBBBBBBBB..", "..BBYYYYYBB..", "..BBBBBBBBB..",
        "..BBBBBBBBB..", "..BBBBBBBBB.."]
FIRE = ["......O......", ".....OYO.....", "....OYYO.....", "....OYYYO....", "...OYYWYYO...", "...OYWWWYO...",
        "....OYYYO....", ".BB.......BB.", "...BBBBBBB...", ".BB.......BB."]
PELT = ["DD.....DD", "DWDDDDDWD", ".DWWWWWD.", ".DWWBWWD.", ".DWBBBWD.", ".DWBBBWD.", ".DWBBBWD.", ".DWWBWWD.",
        ".DWWWWWD.", "DWDDDDDWD", "DD.....DD"]
TOOTH = ["WWW", "WWW", "W.W"]
ARROW = ["W....", "WW...", "WWWWW", "WW...", "W...."]
TOTEM = ["..BBBBBBBBB..", ".BBWBBBBBWBB.", ".BBBBBBBBBBB.", "..BBDDBDDBB..", "..BBBBBBBBB..", "...BBDDDBB...",
         "....BBBBB....", ".....GGG.....", ".....GGG.....", "....GGGGG...."]

# Shown on thunderstore/icon.png.
STORE_ICONS = ["no_first_turn_kill", "insect_only", "blood2_only", "one_squirrel", "no_campfire",
               "totem_kinship", "canine_only", "free_only", "no_trade_pelts"]


def new_icon():
    """Empty 49x49 icon: transparent 2px margin around the dark background."""
    img = [[CLEAR] * 49 for _ in range(49)]
    for y in range(2, 47):
        for x in range(2, 47):
            img[y][x] = BG
    return img


def draw(img, pattern, x0, y0, scale):
    """Draws a text-art pattern at (x0, y0), each char a scale x scale block."""
    for y, row in enumerate(pattern):
        for x, ch in enumerate(row):
            if ch in PALETTE:
                for dy in range(scale):
                    for dx in range(scale):
                        img[y0 + y * scale + dy][x0 + x * scale + dx] = PALETTE[ch]


def centered(img, pattern, scale, dx=0, dy=0):
    """Draws a pattern centred in the icon (optionally nudged)."""
    w, h = max(len(r) for r in pattern) * scale, len(pattern) * scale
    draw(img, pattern, (49 - w) // 2 + dx, (49 - h) // 2 + dy, scale)


def cross_out(img):
    """Red X over the icon (the event is disabled)."""
    for i in range(9, 40):
        for t in (-1, 0, 1):
            img[i][i + t] = PALETTE["R"]
            img[i][48 - i + t] = PALETTE["R"]


def single(pattern, scale):
    """Icon with one centred pattern."""
    img = new_icon()
    centered(img, pattern, scale)
    return img


def blood(n):
    """Icon with n blood drops."""
    img = new_icon()
    layouts = {1: [(17, 12, 3)], 2: [(9, 12, 3), (25, 12, 3)], 3: [(19, 6, 2), (11, 25, 2), (27, 25, 2)],
               4: [(11, 6, 2), (27, 6, 2), (11, 26, 2), (27, 26, 2)]}
    for x, y, s in layouts[n]:
        draw(img, DROP, x, y, s)
    return img


def with_digit(pattern, digit):
    """Pattern on top, red digit underneath on the right."""
    img = new_icon()
    centered(img, pattern, 3, dy=-5)
    draw(img, [r.replace("W", "R") for r in DIGITS[digit]], 36, 34, 2)
    return img


def event(pattern, scale):
    """Event motif crossed out."""
    img = single(pattern, scale)
    cross_out(img)
    return img


def buy_pelts():
    """Pelt with a tooth (the Trapper's currency), crossed out."""
    img = new_icon()
    draw(img, PELT, 7, 8, 3)
    draw(img, TOOTH, 35, 14, 3)
    cross_out(img)
    return img


def trade_pelts():
    """Pelt -> card, crossed out."""
    img = new_icon()
    draw(img, PELT, 5, 13, 2)
    draw(img, ARROW, 24, 20, 2)
    for y in range(13, 36):
        for x in range(34, 45):
            img[y][x] = PALETTE["W"] if x in (34, 44) or y in (13, 35) else PALETTE["B"]
    cross_out(img)
    return img


def make_all(write_png, assets, frame_active):
    """Writes every new icon and its active variant; returns {snake_key: base image}."""
    icons = {
        "no_first_turn_kill": with_digit(SCALES, "1"),
        "no_phase_turn_kill": with_digit(SCALES, "2"),
        "canine_only": single(PAW, 3),
        "avian_only": single(BIRD, 3),
        "insect_only": single(INSECT, 3),
        "reptile_only": single(SNAKE, 3),
        "hooved_only": single(ANTLERS, 3),
        "no_tribe_only": single(NO_TRIBE, 3),
        "blood1_only": blood(1),
        "blood2_only": blood(2),
        "blood3_only": blood(3),
        "blood4_only": blood(4),
        "bones_only": single(BONE, 3),
        "free_only": single(ZERO, 4),
        "one_squirrel": with_digit(SQUIRREL, "1"),
        "no_bone_altar": event(ALTAR, 3),
        "no_mysterious_stones": event(STONES, 3),
        "no_mycologists": event(MUSHROOM, 3),
        "no_deck_trial": event(CARDS, 3),
        "no_consumables": event(PACK, 3),
        "no_campfire": event(FIRE, 3),
        "no_buy_pelts": buy_pelts(),
        "no_trade_pelts": trade_pelts(),
        "totem_kinship": single(TOTEM, 3),
    }
    for key, img in icons.items():
        write_png(os.path.join(assets, key + "_icon.png"), img)
        write_png(os.path.join(assets, key + "_icon_activated.png"), frame_active(img))
    return icons
