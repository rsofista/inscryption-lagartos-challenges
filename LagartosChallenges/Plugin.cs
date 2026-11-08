using System.Linq;
using BepInEx;
using BepInEx.Logging;
using DiskCardGame;
using HarmonyLib;
using InscryptionAPI.Ascension;
using InscryptionAPI.Card;
using InscryptionAPI.Helpers;
using InscryptionAPI.Localizing;

namespace LagartosChallenges
{
    /// <summary>
    /// BepInEx entry point. Registers every challenge, the pelt starter decks and the Double Blood sigil,
    /// then applies the Harmony patches. Replaces the Unfair Hand and Pelt Starter Hands mods, so it refuses to
    /// load next to them.
    /// </summary>
    [BepInPlugin(PluginGuid, PluginName, PluginVersion)]
    [BepInDependency("cyantist.inscryption.api", BepInDependency.DependencyFlags.HardDependency)]
    [BepInIncompatibility("lucas.inscryption.unfairhand")]
    [BepInIncompatibility("lucas.inscryption.peltstarterhands")]
    public class Plugin : BaseUnityPlugin
    {
        /// <summary>Unique plugin id.</summary>
        public const string PluginGuid = "lucas.inscryption.lagartoschallenges";

        /// <summary>Display name.</summary>
        public const string PluginName = "Lagarto's Challenges";

        /// <summary>Plugin version.</summary>
        public const string PluginVersion = "1.0.0";

        /// <summary>Plugin logger, shared with the patches.</summary>
        internal static ManualLogSource Log;

        /// <summary>Id the API assigned to the Unfair Hand challenge.</summary>
        internal static AscensionChallenge UnfairHandChallenge;

        /// <summary>Id the API assigned to the Worthy Squirrel challenge.</summary>
        internal static AscensionChallenge WorthySquirrelChallenge;

        /// <summary>Id the API assigned to the Annoying Trades challenge.</summary>
        internal static AscensionChallenge AnnoyingTradesChallenge;

        /// <summary>
        /// Unity entry point: registers decks, challenges and the sigil, then patches the game.
        /// </summary>
        private void Awake()
        {
            Log = Logger;
            RegisterTranslations();
            PeltPixelPortraits.Register();

            foreach (PeltDeck deck in PeltDecks.All)
            {
                StarterDeckManager.New(
                    PluginGuid,
                    deck.Title,
                    TextureHelper.GetImageAsTexture(deck.IconFile, typeof(Plugin).Assembly),
                    deck.CardNames.ToArray());
            }

            UnfairHandChallenge = AddChallenge(Translations.UnfairHandTitle, Translations.UnfairHandDescription, 5, "unfair_hand_icon");
            WorthySquirrelChallenge = AddChallenge(Translations.WorthySquirrelTitle, Translations.WorthySquirrelDescription, -5, "worthy_squirrel_icon");
            AnnoyingTradesChallenge = AddChallenge(Translations.AnnoyingTradesTitle, Translations.AnnoyingTradesDescription, 5, "annoying_trades_icon");

            // Rulebook entry so the sigil can be inspected; no other meta categories, so it never shows up
            // in totems, the sigil altar or random cards.
            DoubleBlood.ability = AbilityManager.New(
                PluginGuid,
                Translations.DoubleBloodName,
                Translations.DoubleBloodDescription,
                typeof(DoubleBlood),
                TextureHelper.GetImageAsTexture("ability_doubleblood.png", typeof(Plugin).Assembly)
            ).AddMetaCategories(AbilityMetaCategory.Part1Rulebook).ability;

            new Harmony(PluginGuid).PatchAll(typeof(Plugin).Assembly);
            Log.LogInfo($"{PluginName} loaded.");
        }

        /// <summary>
        /// Registers one Kaycee's Mod challenge with InscryptionAPI.
        /// </summary>
        /// <param name="title">English title (also part of the challenge id).</param>
        /// <param name="description">English description.</param>
        /// <param name="points">Points the challenge adds (negative for assists).</param>
        /// <param name="iconBase">Embedded icon name without extension; the active icon adds "_activated".</param>
        /// <returns>The id the API assigned.</returns>
        private static AscensionChallenge AddChallenge(string title, string description, int points, string iconBase) =>
            ChallengeManager.Add(
                PluginGuid,
                title,
                description,
                points,
                TextureHelper.GetImageAsTexture(iconBase + ".png", typeof(Plugin).Assembly),
                TextureHelper.GetImageAsTexture(iconBase + "_activated.png", typeof(Plugin).Assembly)
            ).Challenge.challengeType;

        /// <summary>
        /// Registers every <see cref="Translations"/> entry with InscryptionAPI, so the game's
        /// <c>Localization.Translate</c> (which looks texts up by their English version) finds them in any language.
        /// </summary>
        private static void RegisterTranslations()
        {
            foreach (var text in Translations.All)
            {
                foreach (var translation in text.Value)
                {
                    LocalizationManager.Translate(
                        PluginGuid,
                        $"{PluginGuid}.{text.Key}",
                        text.Key,
                        translation.Value,
                        LocalizationManager.CodeToLanguage(translation.Key));
                }
            }
        }

        /// <summary>True when the current run is a Kaycee's Mod run with Unfair Hand selected.</summary>
        internal static bool UnfairHandActive => ChallengeActive(UnfairHandChallenge);

        /// <summary>True when the current run is a Kaycee's Mod run with Worthy Squirrel selected.</summary>
        internal static bool WorthySquirrelActive => ChallengeActive(WorthySquirrelChallenge);

        /// <summary>True when the current run is a Kaycee's Mod run with Annoying Trades selected.</summary>
        internal static bool AnnoyingTradesActive => ChallengeActive(AnnoyingTradesChallenge);

        /// <summary>True when the current run is a Kaycee's Mod run with vanilla Annoying Starters selected.</summary>
        internal static bool AnnoyingStartersActive => ChallengeActive(AscensionChallenge.WeakStarterDeck);

        /// <summary>
        /// True when the current run is a Kaycee's Mod run with the given challenge selected.
        /// </summary>
        /// <param name="challenge">Challenge id to check.</param>
        /// <returns>Whether the challenge is active.</returns>
        internal static bool ChallengeActive(AscensionChallenge challenge) =>
            SaveFile.IsAscension && AscensionSaveData.Data != null &&
            AscensionSaveData.Data.ChallengeIsActive(challenge);
    }
}
