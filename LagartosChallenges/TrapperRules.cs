using System.Collections.Generic;
using System.Linq;

namespace LagartosChallenges
{
    /// <summary>
    /// Pure rules for the One Squirrel side deck. Unit tested.
    /// </summary>
    public static class TrapperRules
    {
        /// <summary>
        /// One Squirrel: cuts a side deck to the opening hand's squirrels plus one to draw (shorter decks stay as
        /// they are).
        /// </summary>
        /// <typeparam name="T">Card type.</typeparam>
        /// <param name="deck">The side deck.</param>
        /// <param name="openingSquirrels">Squirrels the opening hand draws (<see cref="HandRules.OpeningSquirrels"/>).</param>
        /// <returns>The cut deck.</returns>
        public static List<T> CutSideDeck<T>(List<T> deck, int openingSquirrels) => deck.Take(openingSquirrels + 1).ToList();
    }
}
