using DiskCardGame;

namespace LagartosChallenges
{
    /// <summary>
    /// "Double Blood" sigil: the card counts as 2 blood when sacrificed. It has no triggers of its own;
    /// the extra blood is added by <see cref="SacrificeValuePatch"/>. Only given to the Unfair Hand opening squirrel.
    /// </summary>
    public class DoubleBlood : AbilityBehaviour
    {
        /// <summary>Id the API assigned to this sigil (set in <see cref="Plugin"/>.Awake).</summary>
        public static Ability ability;

        /// <summary>The ability this behaviour implements.</summary>
        public override Ability Ability => ability;
    }
}
