using System.Collections.Generic;
using System.Linq;
using DiskCardGame;

namespace LagartosChallenges
{
    /// <summary>
    /// Bridges game cards to the pure filter rules: reads a card's traits, builds the run's filter and makes the
    /// filler card (an Annoying Rabbit Pelt).
    /// </summary>
    internal static class GameCards
    {
        /// <summary>Name of the Rabbit Pelt card.</summary>
        internal const string FillerCardName = "PeltHare";

        /// <summary>Game tribe → filter tribe, for the five tribes a challenge can select.</summary>
        private static readonly Dictionary<Tribe, FilterTribe> TribeMap = new Dictionary<Tribe, FilterTribe>
        {
            [Tribe.Canine] = FilterTribe.Canine,
            [Tribe.Bird] = FilterTribe.Bird,
            [Tribe.Insect] = FilterTribe.Insect,
            [Tribe.Reptile] = FilterTribe.Reptile,
            [Tribe.Hooved] = FilterTribe.Hooved,
        };

        /// <summary>Filter tribe → game tribe.</summary>
        /// <param name="tribe">Filter tribe.</param>
        /// <returns>The game's tribe.</returns>
        internal static Tribe ToGame(FilterTribe tribe) => TribeMap.First(t => t.Value == tribe).Key;

        /// <summary>
        /// Reads what the filter needs from a card. Costs come from the mod-aware properties, so death cards and
        /// modded costs count as they are shown.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <returns>Its traits.</returns>
        internal static CardTraits Traits(CardInfo card)
        {
            var tribes = card.tribes.Where(TribeMap.ContainsKey).Select(t => TribeMap[t]).Distinct().ToList();
            bool other = card.tribes.Any(t => t != Tribe.None && !TribeMap.ContainsKey(t));
            return new CardTraits(tribes, other, card.BloodCost, card.BonesCost, card.EnergyCost, card.GemsCost?.Count ?? 0);
        }

        /// <summary>The filter for the current run; inactive outside Kaycee's Mod or when no filter challenge is on.</summary>
        /// <returns>The run's filter.</returns>
        internal static CardFilter CurrentFilter() =>
            new CardFilter(CardFilter.FilterKeys.Where(Challenges.IsActive));

        /// <summary>
        /// Whether a real card matches the run's filter. The filler does not count as a match, so offers replace it
        /// with a real card whenever one is available.
        /// </summary>
        /// <param name="card">The card.</param>
        /// <param name="filter">The run's filter.</param>
        /// <returns>True when the card may be offered as a real card.</returns>
        internal static bool Matches(CardInfo card, CardFilter filter) =>
            card != null && !IsFiller(card) && filter.Passes(Traits(card));

        /// <summary>Whether a card is the filler (a Rabbit Pelt carrying Annoying).</summary>
        /// <param name="card">The card.</param>
        /// <returns>True for the filler.</returns>
        internal static bool IsFiller(CardInfo card) =>
            card != null && card.name == FillerCardName && card.HasAbility(Ability.BuffEnemy);

        /// <summary>
        /// Makes a fresh filler: a clone of the Rabbit Pelt with the Annoying sigil as a mod (like Annoying Starters).
        /// Read straight from the card data so it never goes through <c>CardLoader.GetCardByName</c> (patched for
        /// the Trapper).
        /// </summary>
        /// <returns>The filler card.</returns>
        internal static CardInfo MakeFiller()
        {
            CardInfo filler = Clone(ScriptableObjectLoader<CardInfo>.AllData.Find(c => c.name == FillerCardName));
            filler.Mods.Add(new CardModificationInfo(Ability.BuffEnemy));
            return filler;
        }

        /// <summary>Clones a card the way <c>CardLoader</c> does (fresh, mod-free copy).</summary>
        /// <param name="card">The card.</param>
        /// <returns>The clone.</returns>
        internal static CardInfo Clone(CardInfo card) => (CardInfo)card.Clone();

        /// <summary>Flashes the icon of every filter challenge that is on.</summary>
        internal static void ShowFilterActivation()
        {
            foreach (ChallengeKey key in CardFilter.FilterKeys.Where(Challenges.IsActive))
            {
                Challenges.ShowActivation(key);
            }
        }
    }
}
