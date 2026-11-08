using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LagartosChallenges
{
    /// <summary>Every challenge this mod adds to Kaycee's Mod.</summary>
    public enum ChallengeKey
    {
        NoFirstTurnKill, NoPhaseTurnKill,
        CanineOnly, AvianOnly, InsectOnly, ReptileOnly, HoovedOnly, NoTribeOnly,
        Blood1Only, Blood2Only, Blood3Only, Blood4Only, BonesOnly, FreeOnly,
        OneSquirrel, TwoSquirrels,
        NoBoneAltar, NoMysteriousStones, NoMycologists, NoDeckTrial, NoConsumables, NoCampfire, NoBuyPelts, NoTradePelts,
        TotemKinship,
        UnfairHand, WorthySquirrel, AnnoyingTrades,
    }

    /// <summary>One challenge's registration data.</summary>
    public sealed class ChallengeDef
    {
        /// <summary>Which challenge this is.</summary>
        public ChallengeKey Key { get; }

        /// <summary>English title; part of the challenge id, so never changed after release.</summary>
        public string Title { get; }

        /// <summary>English description.</summary>
        public string Description { get; }

        /// <summary>Points the challenge adds to the run (negative for assists).</summary>
        public int Points { get; }

        /// <summary>Embedded icon file shown when the challenge is not selected.</summary>
        public string IconFile { get; }

        /// <summary>Embedded icon file shown when the challenge is selected (yellow frame).</summary>
        public string ActiveIconFile { get; }

        /// <summary>
        /// Creates a definition. Icon files are named after the key in snake_case
        /// (<c>Blood1Only</c> → <c>blood1_only_icon.png</c> / <c>blood1_only_icon_activated.png</c>).
        /// </summary>
        /// <param name="key">Which challenge.</param>
        /// <param name="title">English title.</param>
        /// <param name="description">English description.</param>
        /// <param name="points">Points.</param>
        public ChallengeDef(ChallengeKey key, string title, string description, int points)
        {
            Key = key;
            Title = title;
            Description = description;
            Points = points;
            IconFile = SnakeCase(key.ToString()) + "_icon.png";
            ActiveIconFile = SnakeCase(key.ToString()) + "_icon_activated.png";
        }

        /// <summary>Converts PascalCase to snake_case (digits stay attached to the word before them).</summary>
        /// <param name="name">PascalCase name.</param>
        /// <returns>snake_case name.</returns>
        private static string SnakeCase(string name)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]) && i > 0) sb.Append('_');
                sb.Append(char.ToLowerInvariant(name[i]));
            }
            return sb.ToString();
        }
    }

    /// <summary>
    /// The list <c>Plugin</c> registers, in challenge-screen order. Pure data, unit tested.
    /// </summary>
    public static class ChallengeCatalog
    {
        /// <summary>All challenges, in registration order.</summary>
        public static readonly IReadOnlyList<ChallengeDef> All = new List<ChallengeDef>
        {
            new ChallengeDef(ChallengeKey.NoFirstTurnKill, Translations.NoFirstTurnKillTitle, Translations.NoFirstTurnKillDescription, 5),
            new ChallengeDef(ChallengeKey.NoPhaseTurnKill, Translations.NoPhaseTurnKillTitle, Translations.NoPhaseTurnKillDescription, 5),
            new ChallengeDef(ChallengeKey.CanineOnly, Translations.CanineOnlyTitle, Translations.CanineOnlyDescription, 10),
            new ChallengeDef(ChallengeKey.AvianOnly, Translations.AvianOnlyTitle, Translations.AvianOnlyDescription, 10),
            new ChallengeDef(ChallengeKey.InsectOnly, Translations.InsectOnlyTitle, Translations.InsectOnlyDescription, 10),
            new ChallengeDef(ChallengeKey.ReptileOnly, Translations.ReptileOnlyTitle, Translations.ReptileOnlyDescription, 15),
            new ChallengeDef(ChallengeKey.HoovedOnly, Translations.HoovedOnlyTitle, Translations.HoovedOnlyDescription, 15),
            new ChallengeDef(ChallengeKey.NoTribeOnly, Translations.NoTribeOnlyTitle, Translations.NoTribeOnlyDescription, 10),
            new ChallengeDef(ChallengeKey.Blood1Only, Translations.Blood1OnlyTitle, Translations.Blood1OnlyDescription, 10),
            new ChallengeDef(ChallengeKey.Blood2Only, Translations.Blood2OnlyTitle, Translations.Blood2OnlyDescription, 10),
            new ChallengeDef(ChallengeKey.Blood3Only, Translations.Blood3OnlyTitle, Translations.Blood3OnlyDescription, 15),
            new ChallengeDef(ChallengeKey.Blood4Only, Translations.Blood4OnlyTitle, Translations.Blood4OnlyDescription, 20),
            new ChallengeDef(ChallengeKey.BonesOnly, Translations.BonesOnlyTitle, Translations.BonesOnlyDescription, 10),
            new ChallengeDef(ChallengeKey.FreeOnly, Translations.FreeOnlyTitle, Translations.FreeOnlyDescription, 15),
            new ChallengeDef(ChallengeKey.OneSquirrel, Translations.OneSquirrelTitle, Translations.OneSquirrelDescription, 15),
            new ChallengeDef(ChallengeKey.TwoSquirrels, Translations.TwoSquirrelsTitle, Translations.TwoSquirrelsDescription, -5),
            new ChallengeDef(ChallengeKey.NoBoneAltar, Translations.NoBoneAltarTitle, Translations.NoBoneAltarDescription, 5),
            new ChallengeDef(ChallengeKey.NoMysteriousStones, Translations.NoMysteriousStonesTitle, Translations.NoMysteriousStonesDescription, 5),
            new ChallengeDef(ChallengeKey.NoMycologists, Translations.NoMycologistsTitle, Translations.NoMycologistsDescription, 5),
            new ChallengeDef(ChallengeKey.NoDeckTrial, Translations.NoDeckTrialTitle, Translations.NoDeckTrialDescription, 5),
            new ChallengeDef(ChallengeKey.NoConsumables, Translations.NoConsumablesTitle, Translations.NoConsumablesDescription, 5),
            new ChallengeDef(ChallengeKey.NoCampfire, Translations.NoCampfireTitle, Translations.NoCampfireDescription, 5),
            new ChallengeDef(ChallengeKey.NoBuyPelts, Translations.NoBuyPeltsTitle, Translations.NoBuyPeltsDescription, 5),
            new ChallengeDef(ChallengeKey.NoTradePelts, Translations.NoTradePeltsTitle, Translations.NoTradePeltsDescription, 10),
            new ChallengeDef(ChallengeKey.TotemKinship, Translations.TotemKinshipTitle, Translations.TotemKinshipDescription, -5),
            new ChallengeDef(ChallengeKey.UnfairHand, Translations.UnfairHandTitle, Translations.UnfairHandDescription, 5),
            new ChallengeDef(ChallengeKey.WorthySquirrel, Translations.WorthySquirrelTitle, Translations.WorthySquirrelDescription, -5),
            new ChallengeDef(ChallengeKey.AnnoyingTrades, Translations.AnnoyingTradesTitle, Translations.AnnoyingTradesDescription, 5),
        };

        /// <summary>Looks a challenge up by key.</summary>
        /// <param name="key">Challenge key.</param>
        /// <returns>Its definition.</returns>
        public static ChallengeDef Get(ChallengeKey key) => All.First(c => c.Key == key);
    }
}
