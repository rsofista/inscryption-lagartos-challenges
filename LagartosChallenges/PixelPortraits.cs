using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Ascension;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using UnityEngine;

namespace LagartosChallenges
{
    /// <summary>
    /// Gives the three pelts Act II style pixel portraits. The base game has none, and the Kaycee's Mod starter deck
    /// screen draws a deck's cards in that style, so the pelt decks' cards would show up blank.
    /// </summary>
    internal static class PixelPortraits
    {
        /// <summary>Card name → portrait sprite, built once from the embedded PNGs.</summary>
        private static readonly Dictionary<string, Sprite> Sprites = new Dictionary<string, Sprite>();

        /// <summary>
        /// Loads the sprites and hooks InscryptionAPI's card and starter deck lists. Must run before the decks are
        /// added, so the deck list built by that call already gets the portraits.
        /// </summary>
        internal static void Register()
        {
            foreach (var entry in StarterDecks.PixelPortraits)
            {
                Texture2D texture = TextureHelper.GetImageAsTexture(entry.Value, typeof(Plugin).Assembly);
                Sprites[entry.Key] = texture.ConvertTexture(TextureHelper.SpriteType.PixelPortrait, FilterMode.Point);
            }

            // Every card list the API builds (what the game loads cards from).
            CardManager.ModifyCardList += cards =>
            {
                Apply(cards);
                return cards;
            };

            // The starter decks hold their own card clones, made when the deck list is synced.
            StarterDeckManager.ModifyDeckList += decks =>
            {
                foreach (StarterDeckManager.FullStarterDeck deck in decks)
                {
                    if (deck.Info.cards != null) Apply(deck.Info.cards);
                }
                return decks;
            };
        }

        /// <summary>Sets the pixel portrait on every pelt in the list that does not have one yet.</summary>
        /// <param name="cards">Cards to update in place.</param>
        private static void Apply(IEnumerable<CardInfo> cards)
        {
            foreach (CardInfo card in cards)
            {
                if (card != null && card.pixelPortrait == null && Sprites.TryGetValue(card.name, out Sprite sprite))
                {
                    card.pixelPortrait = sprite;
                }
            }
        }
    }
}
