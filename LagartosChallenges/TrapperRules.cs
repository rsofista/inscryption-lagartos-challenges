using System.Collections.Generic;
using System.Linq;

namespace LagartosChallenges
{
    /// <summary>
    /// Pure rules for the Trapper under the card filters and for the One Squirrel side deck. Unit tested.
    /// </summary>
    public static class TrapperRules
    {
        /// <summary>Side deck size with One Squirrel: one for the opening hand, one to draw.</summary>
        public const int OneSquirrelSideDeckSize = 2;

        /// <summary>
        /// Trapper prices. When pelts fail the filter every slot sells an Annoying Rabbit Pelt, so every slot costs
        /// the Rabbit Pelt price (slot 0).
        /// </summary>
        /// <param name="vanillaPrices">Prices per slot (Rabbit, Wolf, Golden).</param>
        /// <param name="peltsPass">Pelts match the run's filter.</param>
        /// <returns>New price array (the input is not changed).</returns>
        public static int[] Prices(int[] vanillaPrices, bool peltsPass) =>
            peltsPass ? vanillaPrices.ToArray() : vanillaPrices.Select(_ => vanillaPrices[0]).ToArray();

        /// <summary>Cuts a side deck to <see cref="OneSquirrelSideDeckSize"/> cards (shorter decks stay as they are).</summary>
        /// <typeparam name="T">Card type.</typeparam>
        /// <param name="deck">The side deck.</param>
        /// <returns>The cut deck.</returns>
        public static List<T> CutSideDeck<T>(List<T> deck) => deck.Take(OneSquirrelSideDeckSize).ToList();
    }
}
