using System.Collections.Generic;
using System.Linq;

namespace LagartosChallenges
{
    /// <summary>
    /// The Forbidden Cards challenge's cards: Act 1 cards with full art that Kaycee's Mod never offers. Kept free of
    /// Unity/game types so it can be unit tested; <c>ForbiddenCardsPatch</c> adds them to the game's card pools.
    /// </summary>
    public static class ForbiddenCards
    {
        /// <summary>
        /// Boss and event creatures, added to the card choice pool: the Royal's crew (Mole Seaman, Skeleton Crew,
        /// Zombie Parrot), the Trapper's Strange Frog, the Prospector's Pack Mule, the Smoke, Greater Smoke and Starvation.
        /// </summary>
        public static readonly IReadOnlyList<string> Common = new[]
        {
            "MoleSeaman", "SkeletonPirate", "SkeletonParrot", "TrapFrog", "Mule", "Smoke", "Smoke_Improved", "Starvation",
        };

        /// <summary>
        /// Rares Kaycee's Mod never offers, added to the rare pool. The Great Kraken is tagged rare but locked; its
        /// only normal source is a starter deck.
        /// </summary>
        public static readonly IReadOnlyList<string> Rare = new[] { "Kraken" };

        /// <summary>The forbidden cards a pool does not hold yet, in list order.</summary>
        /// <param name="rarePool">True for the rare pool, false for the card choice pool.</param>
        /// <param name="poolNames">Card names (<c>CardInfo.name</c>) already in the pool.</param>
        /// <returns>The card names to add.</returns>
        public static IReadOnlyList<string> MissingFrom(bool rarePool, IEnumerable<string> poolNames)
        {
            var present = new HashSet<string>(poolNames);
            return (rarePool ? Rare : Common).Where(n => !present.Contains(n)).ToList();
        }
    }
}
