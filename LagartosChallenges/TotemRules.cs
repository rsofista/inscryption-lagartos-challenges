using System;
using System.Collections.Generic;
using System.Linq;

namespace LagartosChallenges
{
    /// <summary>Totem Kinship: which tribe the last totem head offered becomes. Pure, unit tested.</summary>
    public static class TotemRules
    {
        /// <summary>
        /// Picks a random tribe from the deck that the woodcarver may offer, the player doesn't own as a head yet,
        /// and isn't already shown in another slot.
        /// </summary>
        /// <typeparam name="T">Tribe type.</typeparam>
        /// <param name="deckTribes">Tribes of the cards in the deck (duplicates allowed).</param>
        /// <param name="eligibleHeads">Heads the woodcarver can offer at all.</param>
        /// <param name="ownedHeads">Heads the player already has.</param>
        /// <param name="shownHeads">Heads in the other slots of this offer.</param>
        /// <param name="seed">Random seed.</param>
        /// <param name="tribe">The pick, when there is one.</param>
        /// <returns>False when no tribe qualifies (the offer stays vanilla).</returns>
        public static bool TryPickKinshipTribe<T>(IEnumerable<T> deckTribes, IEnumerable<T> eligibleHeads,
            IEnumerable<T> ownedHeads, IEnumerable<T> shownHeads, int seed, out T tribe)
        {
            var excluded = new HashSet<T>(ownedHeads.Concat(shownHeads));
            var eligible = new HashSet<T>(eligibleHeads);
            var candidates = deckTribes.Distinct().Where(t => eligible.Contains(t) && !excluded.Contains(t)).ToList();
            if (candidates.Count == 0)
            {
                tribe = default;
                return false;
            }
            tribe = candidates[new Random(seed).Next(candidates.Count)];
            return true;
        }
    }
}
