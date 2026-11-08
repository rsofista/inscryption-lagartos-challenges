# Lagarto's Challenges: design

An Inscryption mod that adds 24 new **Kaycee's Mod** (Act 1 ascension) challenges, each picked on its own on the
challenge select screen with fixed points. It is also a **compilation** (decided by the user): it absorbs the two
sibling mods, so one DLL ships everything.

## Why

Kaycee's Mod runs get samey. These challenges force runs built around one tribe or cost tier, cut map events you
lean on, stop cheap turn-one wins in both directions, and add a couple of odd twists (one squirrel, a guaranteed
kinship totem). The point is replay variety you build yourself by mixing challenges.

## Goals

- Absorb the sibling mods unchanged in behavior:
  - `unfair-hand`: **Unfair Hand** (+5) and **Worthy Squirrel** (-5).
  - `inscryption-pelts-starter-hands`: the six pelt **starter decks** (with their pixel portraits) and
    **Annoying Trades** (+5).
  Their code, tests, translations, assets and icon generators move into this repo; titles, descriptions, points and
  icons stay as they are.

- 24 challenges registered with InscryptionAPI's `ChallengeManager`, each with its own pixel-art icon and an
  "active" icon (same art with a yellow frame; vanilla lights up eyes, a frame is enough here).
- Each challenge works alone and in any combination with the others and with vanilla challenges.
- Same build, test and packaging setup as the sibling mods in `C:\dev\games\inscryption-mods`
  (`unfair-hand`, `inscryption-pelts-starter-hands`).

## Non-goals

- Acts 2 and 3, and the Act 1 story mode. Every effect checks it is in an Act 1 Kaycee's Mod run.
- Filtering Leshy's cards. Filters only touch what the **player** is offered.
- Filtering the starter deck, Kaycee's bonus starter cards, or the side deck (except One Squirrel, below).

## Anti-goals (would be a failure even if it "works")

- A card choice that softlocks or shows an empty slot. Thin pools are filled, never left empty.
- A filter that leaks: any offered card that breaks the active filters (other than the filler pelt).
- Skipped events that crash, hang, or leave the map in a broken state.
- Changing the shared card pool permanently (offers must be clones or per-offer lists, like the pelt mod does).

## The challenges

### 1. No First Turn Kill (two challenges, decided by the user)

| Challenge | Caps | Points |
| --- | --- | --- |
| No First Turn Kill | first turn of each side at the start of every battle (bosses: phase 1 only) | 5 |
| No Phase Turn Kill | first turn of each side after a boss phase change (phases 2 and 3) | 5 |

Picking both caps the first turns of every phase. Reading the user's "one caps the second and third phase, one caps
only the first" literally, so the two stack instead of overlapping (my call). Points are my call. The cap works the
same in both:

- **Player's first turn:** if the damage the player deals during their first turn would win the battle, it is capped
  so the scale stops **one point short of the win**. Normal scale: player needs 5, cap leaves them at 4. With
  vanilla **Tipped Scales** (scale starts 1 against the player) the player needs 6, so the cap is 5 damage and the
  scale stops at 4 in their favor.
- **Leshy's first turn:** mirror image. If Leshy's damage on his first turn would kill the player, it is capped so
  the scale stops one point short of the loss (player at -4).
- General rule (the user's examples, generalised; builder may verify against the game): during the first turn of
  each side, damage dealt toward that side's win can't push the scale beyond 4 toward that side. Damage that does
  not reach a win is untouched.
- Damage from anything the side does during its turn counts (attacks, sigils that fire that turn).
- When the cap kicks in, flash the challenge icon (vanilla `ChallengeActivationUI` style).

### 2. Tribe Only (6 challenges)

| Challenge | Matches cards whose tribes include | Points |
| --- | --- | --- |
| Canine Only | Canine | 10 |
| Avian Only | Bird | 10 |
| Insect Only | Insect | 10 |
| Reptile Only | Reptile | 15 |
| Hooved Only | Hooved | 15 |
| No Tribe Only | (no tribe at all, e.g. Grizzly, Great White) | 10 |

No Squirrel Only: the only Squirrel-tribe card is the Squirrel, which is never offered. Points are my call.

### 3. Cost Only (6 challenges)

| Challenge | Matches | Points |
| --- | --- | --- |
| 1 Blood Only | blood cost 1 | 10 |
| 2 Blood Only | blood cost 2 | 10 |
| 3 Blood Only | blood cost 3 | 15 |
| 4 Blood Only | blood cost 4 | 20 |
| Bones Only | bone cost > 0 | 10 |
| Free Only | no blood, bone, energy or gem cost | 15 |

Points are my call.

### How filters combine (decided by the user)

- Inside a group (tribe or cost) the selected challenges are **OR**: Avian Only + Canine Only offers birds and canines.
- Across groups it's **AND**: Insect Only + 2 Blood Only offers only insects that cost 2 blood.
- A group with nothing selected doesn't filter.
- A card with several tribes passes if any of its tribes is selected.

### Where filters apply

Every card the player picks during the run (my reading of "every card the player can choose, on every event"):

- normal card choices, rare card choices, the face-down cost/tribe choices, Trader offers (Trade Pelts), and
  Deck Trial rewards.
- The **Trapper** (Buy Pelts) too (decided by the user). Pelts have no tribe and no cost, so they only pass when the
  tribe group allows No Tribe (or is empty) and the cost group allows Free (or is empty). Every pelt type that fails
  is sold as an Annoying Rabbit Pelt at the Rabbit Pelt price instead (my call on the price).
- Face-down cost/tribe choices: only show cost/tribe options the filters allow (my call). If that leaves too few
  options, fill them as below.

### Thin pools (decided by the user)

When the filtered pool can't fill all the slots of an offer, the empty slots become a **Rabbit Pelt with the
Annoying sigil** (added as a card mod on a clone, like Annoying Starters does). Example: Hooved + 1 Blood with only
one matching card offers that card plus two Annoying Rabbit Pelts. With no matches at all, every slot is a pelt.
The filler pelt is a normal Rabbit Pelt otherwise, so it can be traded later (my call).
It is exempt from the filters wherever it appears (Trapper included).

### 4. One Squirrel (+15)

The side deck holds 2 Squirrels instead of 10. One goes to the opening hand as usual, so exactly one Squirrel is left
to draw for the rest of the battle. Points are my call. Must play nicely with vanilla side-deck changes; at minimum,
whatever the side deck is made of, it is cut to that size.

### 5. No X Event (8 challenges, one per event)

| Challenge | Event (map node) | Points |
| --- | --- | --- |
| No Bone Altar | sacrifice altar where you give up a card for a boon (my reading of "bone altar") | 5 |
| No Mysterious Stones | sigil transfer stones | 5 |
| No Mycologists | duplicate-merge | 5 |
| No Deck Trial | deck trial | 5 |
| No Consumables | gain consumables (items) | 5 |
| No Campfire | stat boost campfire | 5 |
| No Buy Pelts | Trapper | 5 |
| No Trade Pelts | Trader | 10 |

Points are my call.

Behavior: the node still shows on the map and the player can still walk to it. Entering it flashes the challenge's
icon top left (like every vanilla challenge), Leshy says **"..."**, and the event ends with nothing happening; the
player returns to the map as if the event were done.

### 6. Totem Kinship (-5, an assist)

At a totem event, the **last tribe-head option** offered is guaranteed to be a tribe the player has in their deck,
picked at random among those tribes. Example: deck holds dogs, insects and birds, so the last head is Canine, Insect
or Avian. If the player has no tribe cards, the challenge does nothing. Negative points like the sibling mod's
Worthy Squirrel assist; the value is my call.

My calls (the builder may refine): pick among tribes not already shown in the other head slots; if every deck tribe
is already shown or excluded by vanilla rules (e.g. a head the player already owns), leave the options vanilla.
Only visits that offer heads are touched; a visit offering bottoms (sigils) stays vanilla.

## Icons

- One pixel-art icon per challenge, same 49x49 size, frame and palette family as the sibling mods' challenge icons,
  generated by a stdlib-only `tools/make_icons.py`.
- Active variant = same icon with a yellow square frame.
- Visual language (my call): tribe challenges show the tribe's vanilla tribe symbol style; cost challenges show the
  blood drop count / bone / an empty cost; No X events show the event's motif crossed out; No First Turn Kill shows a
  scale with a stop mark; One Squirrel shows a single squirrel; Totem Kinship shows a totem head.
- A `thunderstore/icon.png` (256x256).

## Constraints

- C# netstandard2.0 BepInEx 5 plugin, HardDependency on InscryptionAPI (2.24+); Harmony patches.
- Pure rules (filter matching, damage cap math, fill-with-pelts, totem tribe pick, translations) live in Unity-free
  files compiled into an xUnit net8.0 test project, TDD, like the sibling mods.
- Game assemblies referenced from the Steam install via `GameDir`; `lib/` holds BepInEx, Harmony, InscryptionAPI.
- All texts in English plus the 11 base-game languages via a `Translations.cs` (titles and descriptions, and the
  "..." line).
- Every C# function gets JSDoc-style `///` XML doc comments.
- Challenge titles are part of the challenge ids, so they never change after release.
- Project folder `C:\dev\games\inscryption-mods\lagartos-challenges`, plugin GUID
  `lucas.inscryption.lagartoschallenges`, display name "Lagarto's Challenges", own git repo, `package.ps1` and
  `thunderstore/` like the siblings.

## Compilation details (my calls)

- Plugin GUID changes to the new one, so the absorbed challenges get new ids. Old saves with those challenges
  selected won't carry them over; acceptable for a new release.
- The plugin declares `[BepInIncompatibility]` on `lucas.inscryption.unfairhand` and
  `lucas.inscryption.peltstarterhands`, so BepInEx won't load them alongside it and nothing appears twice.
- The old repos stay as they are; README says the compilation replaces them.
- Absorbed challenges keep their existing base icons, but their active icons are redone with the same yellow frame
  as the new ones (decided by the user), so all 27 challenges share one active style.

## Left to the builder

- Which game methods to patch for each effect (find them in `Assembly-CSharp`), as long as the behavior matches.
- Exact icon art.
- How the One Squirrel cut is applied (side deck build vs. draw pile).
- Log lines in `BepInEx/LogOutput.log` for each effect firing (recommended, the siblings do it).
