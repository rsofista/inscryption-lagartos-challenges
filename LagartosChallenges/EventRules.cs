using System.Collections.Generic;

namespace LagartosChallenges
{
    /// <summary>Map events a "No X" challenge can switch off.</summary>
    public enum MapEvent { BoneAltar, MysteriousStones, Mycologists, DeckTrial, Consumables, Campfire, BuyPelts, TradePelts }

    /// <summary>Which challenge switches off which map event. Pure, unit tested.</summary>
    public static class EventRules
    {
        /// <summary>Event → its "No X" challenge.</summary>
        private static readonly Dictionary<MapEvent, ChallengeKey> Skips = new Dictionary<MapEvent, ChallengeKey>
        {
            [MapEvent.BoneAltar] = ChallengeKey.NoBoneAltar,
            [MapEvent.MysteriousStones] = ChallengeKey.NoMysteriousStones,
            [MapEvent.Mycologists] = ChallengeKey.NoMycologists,
            [MapEvent.DeckTrial] = ChallengeKey.NoDeckTrial,
            [MapEvent.Consumables] = ChallengeKey.NoConsumables,
            [MapEvent.Campfire] = ChallengeKey.NoCampfire,
            [MapEvent.BuyPelts] = ChallengeKey.NoBuyPelts,
            [MapEvent.TradePelts] = ChallengeKey.NoTradePelts,
        };

        /// <summary>The challenge that switches an event off.</summary>
        /// <param name="mapEvent">The event.</param>
        /// <returns>Its "No X" challenge.</returns>
        public static ChallengeKey SkipChallengeFor(MapEvent mapEvent) => Skips[mapEvent];
    }
}
