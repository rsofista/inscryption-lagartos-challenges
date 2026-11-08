using System.Collections.Generic;
using DiskCardGame;
using InscryptionAPI.Ascension;
using InscryptionAPI.Helpers;

namespace LagartosChallenges
{
    /// <summary>
    /// Game-side view of <see cref="ChallengeCatalog"/>: registers each challenge with InscryptionAPI and answers
    /// "is this challenge on in the current run?".
    /// </summary>
    internal static class Challenges
    {
        /// <summary>Ids the API assigned, by key.</summary>
        private static readonly Dictionary<ChallengeKey, AscensionChallenge> ids = new Dictionary<ChallengeKey, AscensionChallenge>();

        /// <summary>
        /// Registers every catalog entry with InscryptionAPI, in catalog order.
        /// </summary>
        /// <param name="pluginGuid">Owner plugin GUID (part of each challenge id).</param>
        internal static void RegisterAll(string pluginGuid)
        {
            foreach (ChallengeDef def in ChallengeCatalog.All)
            {
                ids[def.Key] = ChallengeManager.Add(
                    pluginGuid,
                    def.Title,
                    def.Description,
                    def.Points,
                    TextureHelper.GetImageAsTexture(def.IconFile, typeof(Plugin).Assembly),
                    TextureHelper.GetImageAsTexture(def.ActiveIconFile, typeof(Plugin).Assembly)
                ).Challenge.challengeType;
            }
        }

        /// <summary>The id the API assigned to a challenge.</summary>
        /// <param name="key">Challenge key.</param>
        /// <returns>Its <see cref="AscensionChallenge"/> value.</returns>
        internal static AscensionChallenge Id(ChallengeKey key) => ids[key];

        /// <summary>
        /// True when the current run is a Kaycee's Mod run with the challenge selected.
        /// </summary>
        /// <param name="key">Challenge key.</param>
        /// <returns>Whether it is on.</returns>
        internal static bool IsActive(ChallengeKey key) => ids.ContainsKey(key) && Plugin.ChallengeActive(ids[key]);

        /// <summary>Flashes the challenge's icon at the top left, like vanilla challenges do when they act.</summary>
        /// <param name="key">Challenge key.</param>
        internal static void ShowActivation(ChallengeKey key)
        {
            if (ids.ContainsKey(key)) ChallengeActivationUI.TryShowActivation(ids[key]);
        }
    }
}
