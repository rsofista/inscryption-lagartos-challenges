using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace LagartosChallenges
{
    /// <summary>
    /// "No X" challenges: the event's map node still exists, but entering it only flashes the challenge icon,
    /// Leshy says "...", and the player is sent back to the map.
    /// </summary>
    [HarmonyPatch(typeof(SpecialNodeHandler), nameof(SpecialNodeHandler.StartSpecialNodeSequence))]
    internal static class EventSkipPatch
    {
        /// <summary>Node data type → the event it starts.</summary>
        private static readonly Dictionary<Type, MapEvent> Events = new Dictionary<Type, MapEvent>
        {
            [typeof(CardRemoveNodeData)] = MapEvent.BoneAltar,
            [typeof(CardMergeNodeData)] = MapEvent.MysteriousStones,
            [typeof(DuplicateMergeNodeData)] = MapEvent.Mycologists,
            [typeof(DeckTrialNodeData)] = MapEvent.DeckTrial,
            [typeof(GainConsumablesNodeData)] = MapEvent.Consumables,
            [typeof(CardStatBoostNodeData)] = MapEvent.Campfire,
            [typeof(BuyPeltsNodeData)] = MapEvent.BuyPelts,
            [typeof(TradePeltsNodeData)] = MapEvent.TradePelts,
        };

        /// <summary>
        /// Harmony prefix: skips the event when its "No X" challenge is on.
        /// </summary>
        /// <param name="__instance">The node handler (runs the replacement coroutine).</param>
        /// <param name="nodeData">The node being entered.</param>
        /// <returns>False to skip the vanilla event.</returns>
        [HarmonyPrefix]
        private static bool Prefix(SpecialNodeHandler __instance, SpecialNodeData nodeData)
        {
            if (nodeData == null || !Events.TryGetValue(nodeData.GetType(), out MapEvent mapEvent)) return true;
            ChallengeKey key = EventRules.SkipChallengeFor(mapEvent);
            if (!Challenges.IsActive(key)) return true;

            Plugin.Log.LogInfo($"{key}: skipped {mapEvent}.");
            __instance.StartCoroutine(Skip(key));
            return false;
        }

        /// <summary>The replacement event: icon, "...", back to the map.</summary>
        /// <param name="key">Challenge that skipped the event.</param>
        /// <returns>The coroutine.</returns>
        private static IEnumerator Skip(ChallengeKey key)
        {
            yield return new WaitForSeconds(0.25f);
            Challenges.ShowActivation(key);
            yield return Singleton<TextDisplayer>.Instance.ShowUntilInput("...");
            Singleton<GameFlowManager>.Instance.TransitionToGameState(GameState.Map);
        }
    }

    /// <summary>
    /// Totem Kinship: the last totem head the woodcarver offers is a tribe from the player's deck.
    /// </summary>
    [HarmonyPatch(typeof(BuildTotemSequencer), "GenerateTotemChoices")]
    internal static class TotemKinshipPatch
    {
        /// <summary>Heads the woodcarver offers in vanilla.</summary>
        private static readonly Tribe[] VanillaHeads = { Tribe.Bird, Tribe.Canine, Tribe.Hooved, Tribe.Insect, Tribe.Reptile };

        /// <summary>
        /// Harmony postfix: rewrites the tribe of the last head in the offer (offers without heads stay vanilla).
        /// </summary>
        /// <param name="randomSeed">Seed the game used.</param>
        /// <param name="__result">The offered totem pieces.</param>
        [HarmonyPostfix]
        private static void Postfix(int randomSeed, List<ItemData> __result)
        {
            if (__result == null || !Challenges.IsActive(ChallengeKey.TotemKinship)) return;
            List<TotemTopData> heads = __result.OfType<TotemTopData>().ToList();
            if (heads.Count == 0) return;
            TotemTopData last = heads[heads.Count - 1];

            var eligible = VanillaHeads.ToList();
            if (StoryEventsData.EventCompleted(StoryEvent.SquirrelHeadDiscovered)) eligible.Add(Tribe.Squirrel);
            var shown = heads.Where(h => h != last).Select(h => h.prerequisites.tribe);
            var deckTribes = RunState.DeckList.SelectMany(c => c.tribes);

            if (!TotemRules.TryPickKinshipTribe(deckTribes, eligible, RunState.Run.totemTops, shown, randomSeed, out Tribe tribe)) return;
            Plugin.Log.LogInfo($"Totem Kinship: last head {last.prerequisites.tribe} -> {tribe}.");
            last.prerequisites.tribe = tribe;
            Challenges.ShowActivation(ChallengeKey.TotemKinship);
        }
    }
}
