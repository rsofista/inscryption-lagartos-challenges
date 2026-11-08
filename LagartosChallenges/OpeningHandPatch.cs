using System.Collections;
using System.Collections.Generic;
using DiskCardGame;
using HarmonyLib;
using UnityEngine;

namespace LagartosChallenges
{
    /// <summary>
    /// Act 1 opening-hand changes for the two challenges, each usable without the other:
    /// <list type="bullet">
    /// <item><b>Unfair Hand</b>: replaces the draw with 1 squirrel + 3 plain random deck cards (no "fair hand").</item>
    /// <item><b>Two Squirrels</b>: a second squirrel is drawn after the opening hand.</item>
    /// <item><b>Worthy Squirrel</b>: the opening squirrel (whichever draw produces it) gets the Double Blood sigil.</item>
    /// </list>
    /// </summary>
    [HarmonyPatch(typeof(CardDrawPiles3D), nameof(CardDrawPiles3D.DrawOpeningHand))]
    internal static class OpeningHandPatch
    {
        /// <summary>Owes the Double Blood sigil to the opening hand's squirrel draw.</summary>
        internal static readonly DoubleBloodGrant SquirrelGrant = new DoubleBloodGrant();

        /// <summary>
        /// Harmony postfix on the opening-hand coroutine. With Unfair Hand the original enumerator is discarded
        /// (never iterated, so it never runs) and the unfair version runs instead. With Worthy Squirrel the chosen
        /// enumerator is wrapped so its squirrel draw gets the sigil.
        /// </summary>
        /// <param name="__instance">The draw piles drawing the hand.</param>
        /// <param name="fixedHand">Cards the battle forces into the opening hand (may be null).</param>
        /// <param name="__result">The coroutine the game will run.</param>
        [HarmonyPostfix]
        private static void Postfix(CardDrawPiles3D __instance, List<CardInfo> fixedHand, ref IEnumerator __result)
        {
            // Kaycee's Mod is Act 1 only; leave the other acts' piles alone.
            if (!(__instance is Part1CardDrawPiles)) return;

            if (Plugin.UnfairHandActive)
            {
                __result = DrawUnfairOpeningHand(__instance, fixedHand);
            }

            if (Challenges.IsActive(ChallengeKey.TwoSquirrels))
            {
                __result = WithExtraSquirrels(__instance, __result);
            }

            if (Plugin.WorthySquirrelActive)
            {
                __result = WithDoubleBloodSquirrel(__result);
            }
        }

        /// <summary>
        /// Two Squirrels: runs the opening hand, then draws side-deck cards until the hand got
        /// <see cref="HandRules.OpeningSquirrels"/> of them (the opening hand already drew one).
        /// </summary>
        /// <param name="piles">The draw piles drawing the hand.</param>
        /// <param name="openingHand">The opening-hand coroutine to run first.</param>
        /// <returns>Coroutine yielding the opening hand's steps, then the extra draws.</returns>
        private static IEnumerator WithExtraSquirrels(CardDrawPiles3D piles, IEnumerator openingHand)
        {
            while (openingHand.MoveNext())
            {
                yield return openingHand.Current;
            }

            Challenges.ShowActivation(ChallengeKey.TwoSquirrels);
            for (int i = 1; i < HandRules.OpeningSquirrels(twoSquirrels: true) && piles.SideDeck.CardsInDeck > 0; i++)
            {
                Plugin.Log.LogInfo("Two Squirrels: extra opening squirrel.");
                piles.SidePile.Draw();
                yield return piles.DrawFromSidePile();
            }
        }

        /// <summary>
        /// Runs an opening-hand coroutine with the Double Blood grant armed, so its first side-pile draw
        /// (see <see cref="SideDrawPatch"/>) becomes the Worthy Squirrel. The grant is dropped afterwards so
        /// squirrels drawn later in the battle stay normal.
        /// </summary>
        /// <param name="openingHand">The opening-hand coroutine to wrap.</param>
        /// <returns>Coroutine yielding exactly what <paramref name="openingHand"/> yields.</returns>
        private static IEnumerator WithDoubleBloodSquirrel(IEnumerator openingHand)
        {
            SquirrelGrant.Arm();
            try
            {
                while (openingHand.MoveNext())
                {
                    yield return openingHand.Current;
                }
            }
            finally
            {
                SquirrelGrant.Disarm();
            }
        }

        /// <summary>
        /// Draws the unfair opening hand: the squirrel first, then the battle's fixed cards, then plain random
        /// deck draws (the game's normal seeded draw) until the hand holds 3 main-deck cards.
        /// </summary>
        /// <param name="piles">The draw piles to draw from.</param>
        /// <param name="fixedHand">Cards the battle forces into the opening hand (may be null).</param>
        /// <returns>Coroutine that performs the draws.</returns>
        private static IEnumerator DrawUnfairOpeningHand(CardDrawPiles3D piles, List<CardInfo> fixedHand)
        {
            // Keep the vanilla flash for the "Squirrel Fish" challenge, then flash ours.
            ChallengeActivationUI.TryShowActivation(AscensionChallenge.SubmergeSquirrels);
            ChallengeActivationUI.TryShowActivation(Plugin.UnfairHandChallenge);

            for (int i = 0; i < HandRules.SquirrelCount && piles.SideDeck.CardsInDeck > 0; i++)
            {
                piles.SidePile.Draw();
                yield return piles.DrawFromSidePile();
            }

            int fixedCardsDrawn = 0;
            if (fixedHand != null)
            {
                foreach (CardInfo card in fixedHand)
                {
                    if (card != null && piles.Deck.HasCard(card))
                    {
                        piles.Pile.Draw();
                        yield return piles.DrawCardFromDeck(card);
                        fixedCardsDrawn++;
                    }
                }
            }

            int randomDraws = HandRules.RandomDrawsNeeded(fixedCardsDrawn, piles.Deck.CardsInDeck);
            for (int i = 0; i < randomDraws; i++)
            {
                piles.Pile.Draw();
                yield return piles.DrawCardFromDeck();
            }
        }
    }

    /// <summary>
    /// Turns the side-pile draw that consumes the <see cref="OpeningHandPatch.SquirrelGrant"/> into the Worthy
    /// Squirrel. Every other side draw is left untouched.
    /// </summary>
    [HarmonyPatch(typeof(CardDrawPiles3D), nameof(CardDrawPiles3D.DrawFromSidePile))]
    internal static class SideDrawPatch
    {
        /// <summary>
        /// Harmony postfix: swaps in the Double Blood draw when the grant is armed.
        /// </summary>
        /// <param name="__instance">The draw piles drawing the card.</param>
        /// <param name="__result">The coroutine the game will run.</param>
        [HarmonyPostfix]
        private static void Postfix(CardDrawPiles3D __instance, ref IEnumerator __result)
        {
            if (OpeningHandPatch.SquirrelGrant.TryConsume())
            {
                __result = DrawDoubleBloodSquirrel(__instance);
            }
        }

        /// <summary>
        /// Same as <c>CardDrawPiles3D.DrawFromSidePile</c>, but the drawn card gets the Double Blood sigil as a
        /// temporary mod, so only this card in this battle has it (the side deck itself is untouched).
        /// </summary>
        /// <param name="piles">The draw piles to draw from.</param>
        /// <returns>Coroutine that spawns the card into the hand.</returns>
        private static IEnumerator DrawDoubleBloodSquirrel(CardDrawPiles3D piles)
        {
            ChallengeActivationUI.TryShowActivation(Plugin.WorthySquirrelChallenge);
            CardInfo info = piles.SideDeck.Draw();
            Vector2 offset = Traverse.Create(piles).Field("drawFromPilesSpawnOffset").GetValue<Vector2>();
            var mods = new List<CardModificationInfo> { new CardModificationInfo(DoubleBlood.ability) };
            yield return Singleton<CardSpawner>.Instance.SpawnCardToHand(info, mods, offset, 0.15f);
        }
    }
}
