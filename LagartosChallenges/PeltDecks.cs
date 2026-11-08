using System.Collections.Generic;

namespace LagartosChallenges
{
    /// <summary>
    /// One pelt starter deck: its id title, the embedded icon file and the cards it starts with.
    /// </summary>
    public sealed class PeltDeck
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
        public PeltDeck(string title, string iconFile, params string[] cardNames)
        {
            Title = title;
            IconFile = iconFile;
            CardNames = cardNames;
        }
    }

    /// <summary>
    /// The six pelt starter decks. Like the vanilla decks they hold 3 cards; Kaycee's Mod adds its 2 bonus cards
    /// (Rabbit Pelts, or an Opossum/Ring Worm after giving up runs) on top.
    /// Kept free of Unity/game types so it can be unit tested.
    /// </summary>
    public static class PeltDecks
    {
        /// <summary>Game card name of the Rabbit Pelt.</summary>
        public const string RabbitPelt = "PeltHare";

        /// <summary>Game card name of the Wolf Pelt.</summary>
        public const string WolfPelt = "PeltWolf";

        /// <summary>Game card name of the Golden Pelt.</summary>
        public const string GoldenPelt = "PeltGolden";

        /// <summary>Every pelt starter deck, in the order they appear on the starter deck screen.</summary>
        public static readonly IReadOnlyList<PeltDeck> All = new[]
        {
            new PeltDeck("Rabbit Pelts", "deck_rabbit.png",
                RabbitPelt, RabbitPelt, RabbitPelt),
            new PeltDeck("Rabbit and Wolf Pelts", "deck_rabbit_wolf.png",
                RabbitPelt, RabbitPelt, WolfPelt),
            new PeltDeck("Rabbit and Golden Pelts", "deck_rabbit_golden.png",
                RabbitPelt, RabbitPelt, GoldenPelt),
            new PeltDeck("Wolf and Rabbit Pelts", "deck_wolf_rabbit.png",
                RabbitPelt, WolfPelt, WolfPelt),
            new PeltDeck("Pelt Assortment", "deck_assortment.png",
                RabbitPelt, WolfPelt, GoldenPelt),
            new PeltDeck("Wolf and Golden Pelts", "deck_wolf_golden.png",
                WolfPelt, WolfPelt, GoldenPelt),
        };

        /// <summary>
        /// Embedded Act II style pixel portrait for each pelt. The base game has none, and the Kaycee's Mod starter deck
        /// screen draws the deck's cards in that style, so without these the pelts show up blank.
        /// </summary>
        public static readonly IReadOnlyDictionary<string, string> PixelPortraits = new Dictionary<string, string>
        {
            [RabbitPelt] = "pixel_rabbit.png",
            [WolfPelt] = "pixel_wolf.png",
            [GoldenPelt] = "pixel_golden.png",
        };
    }
}
