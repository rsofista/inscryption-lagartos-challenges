using System.Collections.Generic;
using System.Linq;

namespace LagartosChallenges
{
    /// <summary>
    /// Pure rules for the "Annoying Trades" challenge. Kept free of Unity/game types so it can be unit tested.
    /// </summary>
    public static class AnnoyingRules
    {
        /// <summary>
        /// Whether the starter pelts get the Annoying sigil. Vanilla "Annoying Starters" skips pelts, so they only
        /// get it when "Annoying Trades" is picked too.
        /// </summary>
        /// <param name="annoyingStartersActive">Vanilla Annoying Starters challenge is on.</param>
        /// <param name="annoyingTradesActive">This mod's Annoying Trades challenge is on.</param>
        /// <returns>True when both challenges are on.</returns>
        public static bool StarterPeltsBecomeAnnoying(bool annoyingStartersActive, bool annoyingTradesActive) =>
            annoyingStartersActive && annoyingTradesActive;

        /// <summary>
        /// Whether every card the Trader offers in this pelt trade gets the Annoying sigil.
        /// </summary>
        /// <param name="annoyingTradesActive">This mod's Annoying Trades challenge is on.</param>
        /// <param name="peltsAnnoying">For each pelt in the deck when the trade starts: does it have Annoying.</param>
        /// <returns>True when the challenge is on and at least one pelt is Annoying.</returns>
        public static bool TradeOffersBecomeAnnoying(bool annoyingTradesActive, IEnumerable<bool> peltsAnnoying) =>
            annoyingTradesActive && peltsAnnoying.Any(annoying => annoying);

        /// <summary>
        /// Whether a card should be given the Annoying sigil (never stacks a second copy).
        /// </summary>
        /// <param name="alreadyAnnoying">The card already has Annoying.</param>
        /// <returns>True when the card does not have it yet.</returns>
        public static bool NeedsAnnoying(bool alreadyAnnoying) => !alreadyAnnoying;
    }
}
