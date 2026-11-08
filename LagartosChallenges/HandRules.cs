namespace LagartosChallenges
{
    /// <summary>
    /// Pure rules for the Unfair Hand opening hand. Kept free of Unity/game types so it can be unit tested.
    /// </summary>
    public static class HandRules
    {
        /// <summary>Number of side-deck cards (squirrels) drawn into the opening hand. It gets the Double Blood sigil.</summary>
        public const int SquirrelCount = 1;

        /// <summary>Side-deck squirrels the opening hand draws.</summary>
        /// <param name="twoSquirrels">The Two Squirrels challenge is on.</param>
        /// <returns>2 with Two Squirrels, otherwise 1.</returns>
        public static int OpeningSquirrels(bool twoSquirrels) => twoSquirrels ? 2 : 1;

        /// <summary>Number of main-deck cards drawn into the opening hand.</summary>
        public const int DeckCardCount = 3;

        /// <summary>
        /// How many random main-deck draws are still needed after the battle's fixed cards were drawn.
        /// </summary>
        /// <param name="fixedCardsDrawn">Main-deck cards already drawn because the battle forces them.</param>
        /// <param name="cardsLeftInDeck">Cards remaining in the main deck.</param>
        /// <returns>Number of random draws to make (never negative, never more than the deck holds).</returns>
        public static int RandomDrawsNeeded(int fixedCardsDrawn, int cardsLeftInDeck)
        {
            int wanted = DeckCardCount - fixedCardsDrawn;
            if (wanted < 0) wanted = 0;
            return wanted < cardsLeftInDeck ? wanted : cardsLeftInDeck;
        }

        /// <summary>
        /// Blood a single card is worth when sacrificed. Worthy Sacrifice (3) beats Double Blood (2); anything else is 1.
        /// </summary>
        /// <param name="hasTripleBlood">Card has the vanilla Worthy Sacrifice sigil.</param>
        /// <param name="hasDoubleBlood">Card has this mod's Double Blood sigil.</param>
        /// <returns>Blood value of the sacrifice.</returns>
        public static int SacrificeValue(bool hasTripleBlood, bool hasDoubleBlood)
        {
            if (hasTripleBlood) return 3;
            return hasDoubleBlood ? 2 : 1;
        }
    }
}
