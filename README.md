# Lagarto's Challenges

Inscryption mod for **Kaycee's Mod** with 29 challenges and 9 starter decks.

Source code, issues and suggestions: https://github.com/rsofista/inscryption-lagartos-challenges

> **Unfair Hand and Pelt Starter Hands are now part of this mod.** Everything they did (Unfair Hand, Worthy Squirrel,
> Annoying Trades and the pelt starter decks) is included here, and those mods will no longer be updated.
> Uninstall them before installing Lagarto's Challenges: BepInEx refuses to load them next to this one.

Selected challenges show their icon with a yellow frame.

## Challenges

| Challenge | Points | Effect |
| --- | --- | --- |
| No First Turn Kill | +5 | No battle can be won or lost on anyone's first turn: that damage stops one short (scale at ±4; 5 damage with Tipped Scales). Boss phase 1 only. |
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
| Forbidden Cards | -5 | Cards you normally can't get join the offers: the Great Kraken joins the rares; Mole Seaman, Skeleton Crew, Zombie Parrot, Strange Frog, Pack Mule, The Smoke, Greater Smoke and Starvation join the card choices. |
| Unfair Hand | +5 | Your opening hand is no longer guaranteed to be playable. |
| Worthy Squirrel | -5 | Your opening squirrel has the Double Blood sigil. |
| Annoying Trades | +5 | Annoying Starters also hits pelts; Trader offers are Annoying when any of your pelts is. |

### How the card filters combine

- Tribe challenges combine with **OR**: Avian Only + Canine Only offers birds and canines.
- Cost challenges combine with **OR**, and tribes and costs combine with **AND**: Insect Only + 2 Blood Only offers
  only insects that cost 2 blood.
- Filters apply to card choices, rare choices, face-down cost/tribe choices, the Trader, Deck Trial rewards and
  boulders. Your starter deck and the Trapper's pelts are untouched.
- When too few cards match, the empty slots become a **Rabbit Pelt with the Annoying sigil**.

## Starter decks

Rabbit Pelts, Rabbit and Wolf Pelts, Rabbit and Golden Pelts, Wolf and Rabbit Pelts, Pelt Assortment, Wolf and Golden
Pelts, Golden and Wolf Pelts (1 Wolf, 2 Golden), Golden Pelts (3 Golden) (3 pelts each), and **Royal's Crew** (Mole
Seaman, Skeleton Crew, Zombie Parrot). Every deck card gets an Act II style pixel portrait. Pelt deck icons stack their 3
pelts (Rabbit plain, Wolf striped, Golden gridded); Royal's Crew shows a skull with an eye patch.

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

## Build

Needs the .NET 8 SDK, Python 3 and the game installed on Steam.

```sh
dotnet test                                                          # unit tests
dotnet build LagartosChallenges/LagartosChallenges.csproj -c Release # -> LagartosChallenges/bin/Release/netstandard2.0/LagartosChallenges.dll
./package.ps1                                                        # -> dist/Lagartos_Challenges-<version>.zip (Thunderstore)
python tools/make_icons.py                                           # regenerate icons, portraits, thunderstore icon
python tools/new_texts.py                                            # regenerate the new challenges' translations
```

Game not in the default Steam folder? Add `-p:GameDir="D:\Path\To\Inscryption"`.

## How it works

Game-free rule classes (`CardFilter`, `OfferRules`, `KillCapRules`/`PhaseTracker`, `TrapperRules`, `EventRules`,
`TotemRules`, `ForbiddenCards`, `ChallengeCatalog`, plus the absorbed `HandRules`, `DoubleBloodGrant`, `AnnoyingRules`, `StarterDecks`) are
unit tested; Harmony patches only read game state and apply their answers. Every effect logs to
`BepInEx/LogOutput.log`.

- `Challenges` registers every `ChallengeCatalog` entry with `ChallengeManager.Add`.
- `OfferPatches`: postfixes on `Part1CardChoiceGenerator.GenerateChoices` (normal, cost, tribe, death card),
  `Part1RareChoiceGenerator.GenerateChoices`, `CardLoader.GetRandomChoosableCardWithCost/OfTribe/BonesCard` and
  `Part1CardChoiceGenerator.RollRandomDeathCardOfCost`. Deck Trial (`GenerateRewardChoices`), Trader
  (`GetTradeCardInfos`) and boulders (the `BoulderChoiceSequence` state machine's `MoveNext`) open an `OfferScope` in
  which `CardLoader.GetDistinctCardsFromPool` only draws matching cards and pads with filler.
- `KillCapPatches`: prefix on `LifeManager.ShowDamageSequence` caps damage; `LifeManager.Initialize` starts a battle;
  `Opponent.PostResetScalesSequence` (and every override) starts a boss phase.
- `OneSquirrelPatch`: `Part1CardDrawPiles.SideDeckData` getter postfix.
- Two Squirrels: `OpeningHandPatch` appends a side-pile draw to the opening-hand coroutine.
- `EventSkipPatch`: prefix on `SpecialNodeHandler.StartSpecialNodeSequence`.
- `TotemKinshipPatch`: postfix on `BuildTotemSequencer.GenerateTotemChoices`.
- `ForbiddenCardsPatch`: postfix on `CardLoader.GetUnlockedCards` (ChoiceNode and Rare, Nature) adds `ForbiddenCards`.
- Absorbed: `OpeningHandPatch`, `SacrificeValuePatch`, `AnnoyingTradesPatches`, `PixelPortraits`.

Patches avoid tiny methods (argument-less coroutine stubs, auto-properties): Mono inlines those and Harmony patches on
inlined methods never run.

The DLLs in `lib/` are compile-time references only (from the BepInEx pack and InscryptionAPI on Thunderstore).
