using System;
using System.Collections.Generic;
using System.Linq;

namespace LagartosChallenges
{
    /// <summary>
    /// How an offer is made to respect the filter: passing cards stay, failing ones are replaced from the filtered
    /// pool, and whatever can't be filled becomes the filler (an Annoying Rabbit Pelt in game). Generic and
    /// game-free so it can be unit tested.
    /// </summary>
    public static class OfferRules
    {
        /// <summary>
        /// Returns the offer with every failing slot replaced. Same length as <paramref name="offer"/>.
        /// </summary>
        /// <typeparam name="T">Card type.</typeparam>
        /// <param name="offer">What the game generated.</param>
        /// <param name="pool">Cards that could replace a failing one (only passing ones are used).</param>
        /// <param name="passes">Filter check.</param>
        /// <param name="nameOf">Card identity, used to keep the offer free of duplicates.</param>
        /// <param name="makeFiller">Creates one filler card.</param>
        /// <param name="seed">Random seed (same seed → same result).</param>
        /// <returns>The filtered offer.</returns>
        public static List<T> Fill<T>(IReadOnlyList<T> offer, IReadOnlyList<T> pool, Func<T, bool> passes,
            Func<T, string> nameOf, Func<T> makeFiller, int seed)
        {
            var random = new Random(seed);
            var used = new HashSet<string>(offer.Where(passes).Select(nameOf));
            var candidates = pool.Where(passes).Where(c => !used.Contains(nameOf(c))).ToList();
            var result = new List<T>(offer.Count);
            foreach (T card in offer)
            {
                if (passes(card))
                {
                    result.Add(card);
                    continue;
                }
                candidates.RemoveAll(c => used.Contains(nameOf(c)));
                if (candidates.Count == 0)
                {
                    result.Add(makeFiller());
                    continue;
                }
                T pick = candidates[random.Next(candidates.Count)];
                used.Add(nameOf(pick));
                result.Add(pick);
            }
            return result;
        }

        /// <summary>
        /// Picks a random passing card from the pool, or the filler when none passes.
        /// </summary>
        /// <typeparam name="T">Card type.</typeparam>
        /// <param name="pool">Cards to choose from.</param>
        /// <param name="passes">Filter check.</param>
        /// <param name="makeFiller">Creates the filler.</param>
        /// <param name="seed">Random seed.</param>
        /// <returns>A passing card or the filler; never default.</returns>
        public static T PickOrFiller<T>(IReadOnlyList<T> pool, Func<T, bool> passes, Func<T> makeFiller, int seed)
        {
            var candidates = pool.Where(passes).ToList();
            return candidates.Count == 0 ? makeFiller() : candidates[new Random(seed).Next(candidates.Count)];
        }
    }
}
