using System.Collections.Generic;
using System.Linq;

namespace LagartosChallenges
{
    /// <summary>The vanilla tribes a "X Only" challenge can select.</summary>
    public enum FilterTribe { Canine, Bird, Insect, Reptile, Hooved }

    /// <summary>
    /// What the filter needs to know about a card, already mod-aware (death cards and modded costs included).
    /// Kept free of game types so the rules can be unit tested.
    /// </summary>
    public readonly struct CardTraits
    {
        /// <summary>The card's vanilla filterable tribes.</summary>
        public IReadOnlyCollection<FilterTribe> Tribes { get; }

        /// <summary>The card has a tribe outside <see cref="FilterTribe"/> (Squirrel or a modded tribe).</summary>
        public bool HasOtherTribe { get; }

        /// <summary>Blood cost.</summary>
        public int Blood { get; }

        /// <summary>Bone cost.</summary>
        public int Bones { get; }

        /// <summary>Energy cost.</summary>
        public int Energy { get; }

        /// <summary>Number of gem costs.</summary>
        public int Gems { get; }

        /// <summary>Creates the traits.</summary>
        /// <param name="tribes">Vanilla filterable tribes.</param>
        /// <param name="hasOtherTribe">Has Squirrel or a modded tribe.</param>
        /// <param name="blood">Blood cost.</param>
        /// <param name="bones">Bone cost.</param>
        /// <param name="energy">Energy cost.</param>
        /// <param name="gems">Number of gem costs.</param>
        public CardTraits(IReadOnlyCollection<FilterTribe> tribes, bool hasOtherTribe, int blood, int bones, int energy, int gems)
        {
            Tribes = tribes;
            HasOtherTribe = hasOtherTribe;
            Blood = blood;
            Bones = bones;
            Energy = energy;
            Gems = gems;
        }

        /// <summary>True when the card has no tribe at all.</summary>
        public bool Tribeless => Tribes.Count == 0 && !HasOtherTribe;

        /// <summary>True when the card costs nothing of any kind.</summary>
        public bool Free => Blood == 0 && Bones == 0 && Energy == 0 && Gems == 0;
    }

    /// <summary>
    /// The tribe and cost "X Only" challenges combined: a card passes when it matches any selected tribe
    /// (or the tribe group is empty) AND any selected cost (or the cost group is empty).
    /// </summary>
    public sealed class CardFilter
    {
        /// <summary>Tribe challenge → tribe it allows.</summary>
        private static readonly Dictionary<ChallengeKey, FilterTribe> TribeKeys = new Dictionary<ChallengeKey, FilterTribe>
        {
            [ChallengeKey.CanineOnly] = FilterTribe.Canine,
            [ChallengeKey.AvianOnly] = FilterTribe.Bird,
            [ChallengeKey.InsectOnly] = FilterTribe.Insect,
            [ChallengeKey.ReptileOnly] = FilterTribe.Reptile,
            [ChallengeKey.HoovedOnly] = FilterTribe.Hooved,
        };

        /// <summary>Blood challenge → blood cost it allows.</summary>
        private static readonly Dictionary<ChallengeKey, int> BloodKeys = new Dictionary<ChallengeKey, int>
        {
            [ChallengeKey.Blood1Only] = 1,
            [ChallengeKey.Blood2Only] = 2,
            [ChallengeKey.Blood3Only] = 3,
            [ChallengeKey.Blood4Only] = 4,
        };

        /// <summary>Every challenge that belongs to one of the two filter groups.</summary>
        public static readonly IReadOnlyList<ChallengeKey> FilterKeys = TribeKeys.Keys
            .Concat(new[] { ChallengeKey.NoTribeOnly })
            .Concat(BloodKeys.Keys)
            .Concat(new[] { ChallengeKey.BonesOnly, ChallengeKey.FreeOnly })
            .ToList();

        private readonly HashSet<FilterTribe> tribes;
        private readonly bool noTribe;
        private readonly HashSet<int> bloods;
        private readonly bool bones;
        private readonly bool free;

        /// <summary>Builds the filter from the run's active challenges (non-filter keys are ignored).</summary>
        /// <param name="active">Active challenge keys.</param>
        public CardFilter(IEnumerable<ChallengeKey> active)
        {
            var set = new HashSet<ChallengeKey>(active);
            tribes = new HashSet<FilterTribe>(TribeKeys.Where(k => set.Contains(k.Key)).Select(k => k.Value));
            noTribe = set.Contains(ChallengeKey.NoTribeOnly);
            bloods = new HashSet<int>(BloodKeys.Where(k => set.Contains(k.Key)).Select(k => k.Value));
            bones = set.Contains(ChallengeKey.BonesOnly);
            free = set.Contains(ChallengeKey.FreeOnly);
        }

        /// <summary>True when any tribe challenge (No Tribe included) is on.</summary>
        public bool TribeGroupActive => tribes.Count > 0 || noTribe;

        /// <summary>True when any cost challenge is on.</summary>
        public bool CostGroupActive => bloods.Count > 0 || bones || free;

        /// <summary>True when the filter restricts anything.</summary>
        public bool IsActive => TribeGroupActive || CostGroupActive;

        /// <summary>Tribes the face-down tribe choice may show (empty when the tribe group is off or only No Tribe is on).</summary>
        public IReadOnlyList<FilterTribe> AllowedTribeChoices => tribes.ToList();

        /// <summary>Whether a card passes both groups.</summary>
        /// <param name="card">The card's traits.</param>
        /// <returns>True when it may be offered.</returns>
        public bool Passes(CardTraits card) => PassesTribe(card) && PassesCost(card);

        /// <summary>
        /// Whether the face-down cost choice may show an option.
        /// </summary>
        /// <param name="blood">The option's blood cost, or null for the Bones option.</param>
        /// <returns>True when the cost group is off or allows that option.</returns>
        public bool AllowsCostChoice(int? blood) =>
            !CostGroupActive || (blood.HasValue ? bloods.Contains(blood.Value) : bones);

        /// <summary>Tribe group check.</summary>
        private bool PassesTribe(CardTraits card) =>
            !TribeGroupActive || card.Tribes.Any(tribes.Contains) || (noTribe && card.Tribeless);

        /// <summary>Cost group check.</summary>
        private bool PassesCost(CardTraits card) =>
            !CostGroupActive || bloods.Contains(card.Blood) || (bones && card.Bones > 0) || (free && card.Free);
    }
}
