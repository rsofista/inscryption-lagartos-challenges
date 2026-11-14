using System.Collections.Generic;
using System.Linq;

namespace LagartosChallenges
{
    /// <summary>
    /// One starter deck: its id title, the embedded icon file and the cards it starts with.
    /// </summary>
    public sealed class StarterDeck
    {
        /// <summary>
        /// Deck title. The game never shows it, but InscryptionAPI builds the deck id from it, so never change it.
        /// </summary>
        public string Title { get; }

        /// <summary>Embedded 35x44 icon shown on the starter deck screen.</summary>
        public string IconFile { get; }

        /// <summary>Game card names (<c>CardInfo.name</c>) the deck starts with.</summary>
        public IReadOnlyList<string> CardNames { get; }

        /// <summary>Creates a deck definition.</summary>
        /// <param name="title">Deck id title.</param>
        /// <param name="iconFile">Embedded icon file name.</param>
        /// <param name="cardNames">Cards the deck starts with.</param>
        public StarterDeck(string title, string iconFile, params string[] cardNames)
        {
            Title = title;
            IconFile = iconFile;
            CardNames = cardNames;
        }
    }

    /// <summary>
    /// The mod's starter decks: eight pelt decks and the Royal's crew. Like the vanilla decks they hold 3 cards;
    /// Kaycee's Mod adds its 2 bonus cards (Rabbit Pelts, or an Opossum/Ring Worm after giving up runs) on top.
    /// Kept free of Unity/game types so it can be unit tested.
    /// </summary>
    public static class StarterDecks
    {
        /// <summary>Game card name of the Rabbit Pelt.</summary>
        public const string RabbitPelt = "PeltHare";

        /// <summary>Game card name of the Wolf Pelt.</summary>
        public const string WolfPelt = "PeltWolf";

        /// <summary>Game card name of the Golden Pelt.</summary>
        public const string GoldenPelt = "PeltGolden";

        /// <summary>Game card name of the Mole Seaman (Royal's crew, normally only seen in his boss fight).</summary>
        public const string MoleSeaman = "MoleSeaman";

        /// <summary>Game card name of the Skeleton Crew.</summary>
        public const string SkeletonCrew = "SkeletonPirate";

        /// <summary>Game card name of the Royal's skeleton parrot (shown in game as "Zombie Parrot").</summary>
        public const string ZombieParrot = "SkeletonParrot";

        /// <summary>The eight pelt starter decks, in the order they appear on the starter deck screen.</summary>
        public static readonly IReadOnlyList<StarterDeck> Pelt = new[]
        {
            new StarterDeck("Rabbit Pelts", "deck_rabbit.png",
                RabbitPelt, RabbitPelt, RabbitPelt),
            new StarterDeck("Rabbit and Wolf Pelts", "deck_rabbit_wolf.png",
                RabbitPelt, RabbitPelt, WolfPelt),
            new StarterDeck("Rabbit and Golden Pelts", "deck_rabbit_golden.png",
                RabbitPelt, RabbitPelt, GoldenPelt),
            new StarterDeck("Wolf and Rabbit Pelts", "deck_wolf_rabbit.png",
                RabbitPelt, WolfPelt, WolfPelt),
            new StarterDeck("Pelt Assortment", "deck_assortment.png",
                RabbitPelt, WolfPelt, GoldenPelt),
            new StarterDeck("Wolf and Golden Pelts", "deck_wolf_golden.png",
                WolfPelt, WolfPelt, GoldenPelt),
            new StarterDeck("Golden and Wolf Pelts", "deck_golden_wolf.png",
                WolfPelt, GoldenPelt, GoldenPelt),
            new StarterDeck("Golden Pelts", "deck_golden.png",
                GoldenPelt, GoldenPelt, GoldenPelt),
        };

        /// <summary>The Royal's crew: Mole Seaman, Skeleton Crew and the skeleton parrot.</summary>
        public static readonly StarterDeck Royal = new StarterDeck("Royal's Crew", "deck_royal.png",
            MoleSeaman, SkeletonCrew, ZombieParrot);

        /// <summary>Every starter deck, in the order they appear on the starter deck screen (pelt decks, then Royal's).</summary>
        public static readonly IReadOnlyList<StarterDeck> All = Pelt.Concat(new[] { Royal }).ToList();

        /// <summary>
        /// Embedded Act II style pixel portrait for every card in the decks. The base game has none for them, and the
        /// Kaycee's Mod starter deck screen draws the deck's cards in that style, so without these they show up blank.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> PixelPortraits = new Dictionary<string, string>
        {
            [RabbitPelt] = "pixel_rabbit.png",
            [WolfPelt] = "pixel_wolf.png",
            [GoldenPelt] = "pixel_golden.png",
            [MoleSeaman] = "pixel_mole_seaman.png",
            [SkeletonCrew] = "pixel_skeleton_crew.png",
            [ZombieParrot] = "pixel_zombie_parrot.png",
        };
    }
}
