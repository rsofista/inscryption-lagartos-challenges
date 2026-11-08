# Lagarto's Challenges

Inscryption mod for **Kaycee's Mod** with 28 challenges and 8 pelt starter decks.

> **Unfair Hand and Pelt Starter Hands are now part of this mod.** Everything they did (Unfair Hand, Worthy Squirrel,
> Annoying Trades and the pelt starter decks) is included here, and those mods will no longer be updated.
> Uninstall them before installing Lagarto's Challenges: BepInEx refuses to load them next to this one.

Selected challenges show their icon with a yellow frame.

## Challenges

| Challenge | Points | Effect |
| --- | --- | --- |
| No First Turn Kill | +5 | Nobody can win or lose a battle on their first turn: that damage stops one short (scale at ±4; 5 damage with Tipped Scales). Boss phase 1 only. |
| No Phase Turn Kill | +5 | Same cap on each side's first turn after a boss's scales reset (phases 2 and 3). Pick both to cap every phase. |
| Canine / Avian / Insect Only | +10 each | Every card you are offered is of that tribe. |
| Reptile / Hooved Only | +15 each | Same. |
| No Tribe Only | +10 | Only tribeless cards (Grizzly, Great White...). |
| 1 / 2 Blood Only | +10 each | Only cards with that blood cost. |
| 3 Blood Only | +15 | Same. |
| 4 Blood Only | +20 | Same. |
| Bones Only | +10 | Only cards costing Bones. |
| Free Only | +15 | Only cards with no cost. |
| One Squirrel | +15 | After your opening hand, only one squirrel is left to draw. |
| Two Squirrels | -5 | Your opening hand gets a second squirrel (5 cards). |
| No Bone Altar / Mysterious Stones / Mycologists / Deck Trial / Consumables / Campfire / Buy Pelts | +5 each | The node is still on the map; entering it flashes the icon, Leshy says "...", and you go back. |
| No Trade Pelts | +10 | Same, for the Trader. |
| Totem Kinship | -5 | The last totem head the woodcarver offers is a tribe from your deck (if it has any). |
| Unfair Hand | +5 | Your opening hand is no longer guaranteed to be playable. |
| Worthy Squirrel | -5 | Your opening squirrel has the Double Blood sigil. |
| Annoying Trades | +5 | Annoying Starters also hits pelts; Trader offers are Annoying when any of your pelts is. |

### How the card filters combine

- Tribe challenges combine with **OR**: Avian Only + Canine Only offers birds and canines.
- Cost challenges combine with **OR**, and tribes and costs combine with **AND**: Insect Only + 2 Blood Only offers
  only insects that cost 2 blood.
- Filters apply to card choices, rare choices, face-down cost/tribe choices, the Trader, the Trapper, Deck Trial
  rewards and boulders. Your starter deck is untouched.
- When too few cards match, the empty slots become a **Rabbit Pelt with the Annoying sigil**. Pelts have no tribe
  and no cost, so at the Trapper they only stay real pelts when No Tribe / Free allow them; otherwise every pelt for
  sale is an Annoying Rabbit Pelt at the Rabbit Pelt price.

## Starter decks

Rabbit Pelts, Rabbit and Wolf Pelts, Rabbit and Golden Pelts, Wolf and Rabbit Pelts, Pelt Assortment, Wolf and Golden
Pelts, Golden and Wolf Pelts (1 Wolf, 2 Golden), Golden Pelts (3 Golden) (3 pelts each, with Act II style pixel portraits).

## Requirements

- [BepInExPack_Inscryption](https://thunderstore.io/c/inscryption/p/BepInEx/BepInExPack_Inscryption/)
- [InscryptionAPI](https://thunderstore.io/c/inscryption/p/API_dev/API/) (2.24+)

## Install (with r2modman)

1. Install **r2modman**, pick **Inscryption**, create a profile.
2. In **Online**, download **API** (by API_dev). r2modman pulls BepInEx automatically.
3. Click **Start modded** once and close the game, so the BepInEx folders exist.
4. Settings → **Browse profile folder**. Copy `LagartosChallenges.dll` into `BepInEx\plugins\` (remove
   `UnfairHand.dll` / `PeltStarterHands.dll` if present).
5. Launch with **Start modded**.
