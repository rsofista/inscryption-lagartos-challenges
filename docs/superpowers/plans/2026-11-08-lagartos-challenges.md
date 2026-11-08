# Lagarto's Challenges Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** One BepInEx plugin that adds 24 new Kaycee's Mod challenges and absorbs the Unfair Hand and Pelt Starter Hands mods (27 challenges + 6 starter decks total).

**Architecture:** Same shape as the sibling mods: game-free rule classes (compiled straight into an xUnit test project) do every decision; thin Harmony patches read game state, call the rules and write the result back. One `ChallengeCatalog` lists every challenge; `Plugin` registers them all and exposes `Challenges.IsActive(key)`.

**Tech Stack:** C# netstandard2.0, BepInEx 5, HarmonyX, InscryptionAPI 2.24+, xUnit (net8.0), Python 3 stdlib for icons, PowerShell for packaging.

**Spec:** `docs/superpowers/specs/2026-11-08-lagartos-challenges-design.md` (read it first; this plan argues from it).

## Global Constraints

- Project root `C:\dev\games\inscryption-mods\lagartos-challenges`, its own git repo (`git init` in Task 1).
- Plugin GUID `lucas.inscryption.lagartoschallenges`, name `Lagarto's Challenges`, version `1.0.0`.
- `[BepInIncompatibility("lucas.inscryption.unfairhand")]` and `[BepInIncompatibility("lucas.inscryption.peltstarterhands")]`.
- HardDependency `cyantist.inscryption.api`; Thunderstore deps `BepInEx-BepInExPack_Inscryption-5.4.2305`, `API_dev-API-2.24.1`.
- Every effect runs only in an Act 1 Kaycee's Mod run (`SaveFile.IsAscension`, `AscensionSaveData.Data.ChallengeIsActive`).
- Challenge titles are part of challenge ids: never change them after release.
- Every text in English + `fr, it, de, es, pt, tr, ru, ja, ko, zhcn, zhtw`.
- Every C# member gets `///` XML doc comments (user rule: JSDoc-style comments everywhere).
- TDD for all rule classes. Never `.Skip`, never comment out tests.
- Offers are always per-offer clones; never mutate `ScriptableObjectLoader<CardInfo>.AllData` entries.
- Filler card = clone of `PeltHare` + `new CardModificationInfo(Ability.BuffEnemy)`. Filler is exempt from filters.
- Icons 49x49, 2px transparent margin; active variant = base icon + yellow square frame `(240, 200, 40, 255)`, 2px wide, just inside the margin.
- Game code reference: a decompile of `Assembly-CSharp.dll` (`ilspycmd -p -o <dir> "<GameDir>\Inscryption_Data\Managed\Assembly-CSharp.dll"`; ilspycmd is installed). Method names below come from it.
- **Inlining trap** (learned in the pelt mod): Mono inlines tiny methods (≤ ~20 IL bytes) and Harmony patches on inlined callees never run. Never patch a coroutine stub that takes no arguments, an auto-property, or a one-line method. Prefer virtual methods, methods with real bodies, or iterator stubs with several arguments.

## Review Focus

1. **Empty filtered pools** (e.g. Reptile + Free, or No Tribe + face-down tribe choice): vanilla callers such as `CostChoiceChosen` crash on a `null` card. Expect filler, never null. Pinned in Task 4 (`Fill_offer_with_no_matches_is_all_filler`) and Task 5 (CardLoader postfix never returns null when a filter is on).
2. **Death cards** in choices: they have no tribe and costs come from mods. They must be judged by `CardInfo.BloodCost/BonesCost/tribes` (mod-aware), not the base card. Pinned in Task 4 (`Traits_from_mods_are_what_count` style test on `CardTraits`) and the Task 5 glue.
3. **Tipped Scales + kill cap**: balance starts at -1. Player cap must allow 5 damage, Leshy cap 3. Pinned in Task 6 tests.
4. **Skipped events** must leave the map usable (node consumed, no camera/lock left behind). Pinned by the in-game checklist in Task 9; the patch must transition with `GameFlowManager.TransitionToGameState(GameState.Map)` exactly like `CardRemoveSequencer` does.
5. **Filler + Annoying Trades**: a filler pelt in a Trader offer must not get a second Annoying sigil. Pinned in Task 4 (`AnnoyingRules.NeedsAnnoying(true) == false` already covers the rule; the Task 5 offer-scope glue must run *inside* `GetTradeCardInfos`, so the Annoying Trades postfix still checks `IsAnnoying` first).

---

### Task 1: Scaffold the repo and absorb the two sibling mods

**Files:**
- Create: `LagartosChallenges.sln`, `LagartosChallenges/LagartosChallenges.csproj`, `LagartosChallenges.Tests/LagartosChallenges.Tests.csproj`, `.gitignore`, `lib/` (copy the 3 DLLs from `unfair-hand/lib`).
- Copy + rename namespace `UnfairHand` / `PeltStarterHands` → `LagartosChallenges`:
  - from `unfair-hand/UnfairHand/`: `DoubleBlood.cs`, `DoubleBloodGrant.cs`, `HandRules.cs`, `OpeningHandPatch.cs`, `SacrificeValuePatch.cs`
  - from `inscryption-pelts-starter-hands/PeltStarterHands/`: `AnnoyingRules.cs`, `AnnoyingTradesPatches.cs`, `PeltDecks.cs`, `PeltPixelPortraits.cs`
  - tests: `DoubleBloodGrantTests.cs`, `HandRulesTests.cs`, `AnnoyingRulesTests.cs`, `PeltDecksTests.cs`
  - assets: all PNGs. Rename the colliding files: unfair-hand `challenge_icon*.png` → `unfair_hand_icon*.png`; pelt `challenge_icon*.png` → `annoying_trades_icon*.png`.
- Create: `LagartosChallenges/Plugin.cs` (merged), `LagartosChallenges/Translations.cs` (merged tables of both mods), `LagartosChallenges.Tests/TranslationsTests.cs` (merged text list).
- Copy `docs/` already present; it is committed in this task.

**Interfaces:**
- Produces: `Plugin.PluginGuid`, `Plugin.Log`; `Plugin.UnfairHandActive`, `Plugin.WorthySquirrelActive`, `Plugin.AnnoyingTradesActive`, `Plugin.AnnoyingStartersActive` (kept so the absorbed patches compile untouched; Task 2 reroutes them through the catalog).

- [ ] **Step 1:** Create the folder layout, csproj files and sln by copying the sibling csproj files (same `GameDir`/`ManagedDir`, references, `EmbeddedResource Include="Assets\*.png"`). Test csproj `<Compile Include>` links every pure file: `HandRules.cs`, `DoubleBloodGrant.cs`, `AnnoyingRules.cs`, `PeltDecks.cs`, `Translations.cs` (later tasks add theirs). `.gitignore` = sibling's.
- [ ] **Step 2:** Copy the files above, rename namespaces, update asset names used by the code.
- [ ] **Step 3:** Write the merged `Plugin.cs`: GUID/name/version from Global Constraints, both `BepInIncompatibility` attributes, registers the 6 decks, the pelt portraits, Annoying Trades, Unfair Hand, Worthy Squirrel, the Double Blood ability, translations, `PatchAll`. Points unchanged: Unfair Hand 5, Worthy Squirrel -5, Annoying Trades 5.
- [ ] **Step 4:** Merge both `Translations.All` tables and both translation test lists.
- [ ] **Step 5:** Run `dotnet test LagartosChallenges.sln` → all copied tests PASS. Run `dotnet build LagartosChallenges/LagartosChallenges.csproj -c Release` → succeeds.
- [ ] **Step 6:** `git init`, commit `chore: scaffold Lagarto's Challenges and absorb Unfair Hand and Pelt Starter Hands`.

### Task 2: Challenge catalog, registration and texts

**Files:**
- Create: `LagartosChallenges/ChallengeCatalog.cs` (pure), `LagartosChallenges/Challenges.cs` (game glue)
- Modify: `Plugin.cs`, `Translations.cs`
- Test: `LagartosChallenges.Tests/ChallengeCatalogTests.cs`, `TranslationsTests.cs`

**Interfaces:**
- Produces (pure):
  ```csharp
  public enum ChallengeKey { NoFirstTurnKill, NoPhaseTurnKill,
      CanineOnly, AvianOnly, InsectOnly, ReptileOnly, HoovedOnly, NoTribeOnly,
      Blood1Only, Blood2Only, Blood3Only, Blood4Only, BonesOnly, FreeOnly,
      OneSquirrel,
      NoBoneAltar, NoMysteriousStones, NoMycologists, NoDeckTrial, NoConsumables, NoCampfire, NoBuyPelts, NoTradePelts,
      TotemKinship,
      UnfairHand, WorthySquirrel, AnnoyingTrades }
  public sealed class ChallengeDef { ChallengeKey Key; string Title; string Description; int Points; string IconFile; string ActiveIconFile; }
  public static class ChallengeCatalog { public static IReadOnlyList<ChallengeDef> All; public static ChallengeDef Get(ChallengeKey key); }
  ```
- Produces (glue): `static class Challenges { static bool IsActive(ChallengeKey key); static AscensionChallenge Id(ChallengeKey key); static void ShowActivation(ChallengeKey key); }` (`ShowActivation` = `ChallengeActivationUI.TryShowActivation(Id(key))`).

Titles / points (titles are fixed copy; icon file = snake_case of key + `_icon.png`, active = `_icon_activated.png`; absorbed keep their Task 1 file names):

| Key | Title | Points |
| --- | --- | --- |
| NoFirstTurnKill | No First Turn Kill | 5 |
| NoPhaseTurnKill | No Phase Turn Kill | 5 |
| CanineOnly / AvianOnly / InsectOnly | Canine Only / Avian Only / Insect Only | 10 |
| ReptileOnly / HoovedOnly | Reptile Only / Hooved Only | 15 |
| NoTribeOnly | No Tribe Only | 10 |
| Blood1Only / Blood2Only | 1 Blood Only / 2 Blood Only | 10 |
| Blood3Only | 3 Blood Only | 15 |
| Blood4Only | 4 Blood Only | 20 |
| BonesOnly | Bones Only | 10 |
| FreeOnly | Free Only | 15 |
| OneSquirrel | One Squirrel | 15 |
| NoBoneAltar … NoBuyPelts | No Bone Altar, No Mysterious Stones, No Mycologists, No Deck Trial, No Consumables, No Campfire, No Buy Pelts | 5 |
| NoTradePelts | No Trade Pelts | 10 |
| TotemKinship | Totem Kinship | -5 |
| UnfairHand / WorthySquirrel / AnnoyingTrades | existing titles | 5 / -5 / 5 |

Descriptions: one short sentence each, written by the implementer from the spec (e.g. "Cards you are offered must be Insects. Tribe challenges stack with OR, cost challenges with AND."). The "..." line needs no translation.

- [ ] **Step 1: Write failing tests**
  ```csharp
  [Fact] public void Catalog_has_27_challenges() => Assert.Equal(27, ChallengeCatalog.All.Count);
  [Fact] public void Every_key_appears_once() => Assert.Equal(Enum.GetValues<ChallengeKey>().Length, ChallengeCatalog.All.Select(c => c.Key).Distinct().Count());
  [Fact] public void Titles_are_unique() => ...Distinct().Count() == 27
  [Theory, InlineData(ChallengeKey.Blood4Only, 20), InlineData(ChallengeKey.TotemKinship, -5), InlineData(ChallengeKey.NoTradePelts, 10), InlineData(ChallengeKey.ReptileOnly, 15), InlineData(ChallengeKey.WorthySquirrel, -5)]
  public void Points_match_spec(ChallengeKey key, int points)
  [Fact] public void Every_title_and_description_is_translated()  // loops the catalog through the existing 11-language check
  ```
- [ ] **Step 2:** `dotnet test` → FAIL (types missing).
- [ ] **Step 3:** Implement `ChallengeCatalog`, add all titles/descriptions + 11 translations to `Translations` (follow the existing tone; reuse the game's own words for tribes, Blood, Bones, Trapper, Trader, Mycologists, campfire, etc.).
- [ ] **Step 4:** Implement `Challenges` and make `Plugin.Awake` register every catalog entry in order with `ChallengeManager.Add(PluginGuid, Title, Description, Points, icon, activeIcon)`, storing ids in `Challenges`. Point the four `Plugin.*Active` properties at `Challenges.IsActive`. Until Task 3 lands, a missing icon file may reuse `unfair_hand_icon*.png` so the build runs.
- [ ] **Step 5:** `dotnet test` PASS; Release build succeeds.
- [ ] **Step 6:** Commit `feat: register all 27 challenges from a catalog`.

### Task 3: Icons

**Files:**
- Create: `tools/make_icons.py` (merges the pelt mod's generator; stdlib only), `thunderstore/icon.png`
- Test: `LagartosChallenges.Tests/IconAssetsTests.cs`

**Interfaces:**
- Consumes: `ChallengeCatalog.All[*].IconFile / ActiveIconFile`.

- [ ] **Step 1: Failing test.** `Every_catalog_icon_exists_and_is_49x49` reads each PNG under `../../../../LagartosChallenges/Assets/` (path relative to the test bin), checks the file exists and the IHDR width/height are 49.
- [ ] **Step 2:** Run → FAIL.
- [ ] **Step 3:** Write the generator. Keep the pelt mod's deck icons, pixel portraits and Annoying Trades art. Add a minimal PNG reader (8-bit RGBA, non-interlaced) so the hand-made Unfair Hand / Worthy Squirrel base icons are loaded and their active variants rebuilt with the yellow frame. One `frame_active(img)` helper makes every active variant. New icon motifs per the spec "Icons" section; exact art is the implementer's call. `thunderstore/icon.png` 256x256: a 3x3 grid of representative challenge icons on the dark background.
- [ ] **Step 4:** `python tools/make_icons.py`; `dotnet test` PASS. Look at a few PNGs with the Read tool to sanity-check.
- [ ] **Step 5:** Remove the Task 2 icon fallback in `Plugin`. Commit `feat: pixel-art icons with yellow active frame for all challenges`.

### Task 4: Card filter and offer rules (pure)

**Files:**
- Create: `LagartosChallenges/CardFilter.cs`, `LagartosChallenges/OfferRules.cs`
- Test: `LagartosChallenges.Tests/CardFilterTests.cs`, `OfferRulesTests.cs`

**Interfaces:**
- Produces:
  ```csharp
  public enum FilterTribe { Canine, Bird, Insect, Reptile, Hooved }
  public readonly struct CardTraits { CardTraits(IReadOnlyCollection<FilterTribe> tribes, bool hasOtherTribe, int blood, int bones, int energy, int gems); }
  public sealed class CardFilter {
      CardFilter(IReadOnlyCollection<ChallengeKey> active);   // ignores non-filter keys
      bool IsActive { get; }                                    // any tribe or cost key selected
      bool Passes(CardTraits card);
      bool AllowsCostChoice(int? blood /* null = bones option */);
      IReadOnlyList<FilterTribe> AllowedTribeChoices { get; }   // selected tribes, empty if tribe group inactive
      bool TribeGroupActive { get; }
  }
  public static class OfferRules {
      List<T> Fill<T>(IReadOnlyList<T> offer, IReadOnlyList<T> pool, Func<T,bool> passes, Func<T,string> nameOf, Func<T> makeFiller, int seed);
      T PickOrFiller<T>(IReadOnlyList<T> pool, Func<T,bool> passes, Func<T> makeFiller, int seed);
  }
  ```
- Rules: tribe group passes if no tribe key selected, or card has any selected tribe, or `NoTribeOnly` selected and the card has no tribe at all (`tribes` empty and `!hasOtherTribe`; `hasOtherTribe` = Squirrel or any modded tribe). Cost group passes if none selected or any selected matches: `BloodN` ⇔ `blood == N`; `Bones` ⇔ `bones > 0`; `Free` ⇔ all four costs 0. Card passes ⇔ both groups pass.
- `Fill`: keeps passing items in their slots; each failing slot gets a random (`new System.Random(seed)`) passing pool item whose `nameOf` is not already in the result; when none is left, `makeFiller()`. Result has the same count as `offer`.

- [ ] **Step 1: Failing tests** (names and assertions):
  - `No_filter_passes_everything`
  - `Insect_only_passes_insects_only`
  - `Avian_and_canine_pass_either` (OR within group)
  - `Insect_and_2_blood_need_both` (insect 1-blood fails, wolf 2-blood fails, insect 2-blood passes)
  - `Multi_tribe_card_passes_if_any_tribe_selected`
  - `No_tribe_only_rejects_squirrel_tribe_cards` (`hasOtherTribe: true` fails)
  - `Bones_only_needs_bone_cost`, `Free_only_rejects_energy_and_gems`
  - `Cost_choice_allowed_only_for_selected_costs` (`Blood2Only` → `AllowsCostChoice(2)` true, `(1)` false, `(null)` false; no cost key → all true)
  - `Fill_keeps_passing_cards_in_place`
  - `Fill_replaces_failing_with_distinct_pool_cards`
  - `Fill_pads_with_filler_when_pool_runs_out` (1 match, 3 slots → [match, filler, filler] in some order, count 3)
  - `Fill_offer_with_no_matches_is_all_filler`
  - `Fill_is_deterministic_for_a_seed`
  - `Pick_or_filler_returns_filler_on_empty_pool`
- [ ] **Step 2:** Run → FAIL. **Step 3:** Implement. **Step 4:** Run → PASS. Add both files to the test csproj.
- [ ] **Step 5:** Commit `feat: tribe/cost card filter and offer fill rules`.

### Task 5: Filter glue for every card offer

**Files:**
- Create: `LagartosChallenges/GameCards.cs`, `LagartosChallenges/OfferPatches.cs`

**Interfaces:**
- Consumes: `CardFilter`, `OfferRules`, `Challenges`.
- Produces:
  ```csharp
  internal static class GameCards {
      static CardTraits Traits(CardInfo card);         // from card.tribes, BloodCost, BonesCost, EnergyCost, GemsCost (mod-aware)
      static CardFilter CurrentFilter();               // from Challenges.IsActive over the 12 filter keys; inactive outside Kaycee's Mod
      static bool Passes(CardInfo card);               // filler always passes
      static CardInfo MakeFiller();                    // clone of PeltHare + BuffEnemy mod, built from ScriptableObjectLoader<CardInfo>.AllData (not GetCardByName, to avoid re-entering Task 7's patch)
      static bool IsFiller(CardInfo card);
  }
  internal static class OfferScope { static bool Active { get; } static void Enter(); static void Exit(); }  // counter
  ```

Patch points (all no-ops when `CurrentFilter().IsActive` is false; log each replacement to `Plugin.Log`):

| Target | Kind | Does |
| --- | --- | --- |
| `Part1CardChoiceGenerator.GenerateChoices(CardChoicesNodeData, int)` | postfix | Direct: `OfferRules.Fill` on the `CardInfo` choices, pool = `GetUnlockedCards(ChoiceNode, Nature)`. Cost: keep options where `AllowsCostChoice`, add missing allowed options (Blood 1-3, Bones) up to 3, pad with filler `CardChoice{CardInfo=filler}`. Tribe: if `TribeGroupActive`, options = allowed tribes in vanilla order (vanilla's picks first), max 3, pad with filler. Death-card choices: `Fill` too. Seed = the method's `randomSeed`. Flash every active filter challenge's icon once. |
| `Part1RareChoiceGenerator.GenerateChoices` | postfix | `Fill`, pool = `GetUnlockedCards(Rare, Nature)`. |
| `CardLoader.GetRandomChoosableCardWithCost`, `GetRandomChoosableCardOfTribe`, `GetRandomChoosableBonesCard` | postfix | If the result is null or fails: `PickOrFiller` over the same criteria ∩ filter. Never returns null while a filter is on. Covers face-down choice results, Pack Mule, oil painting. |
| `Part1CardChoiceGenerator.RollRandomDeathCardOfCost` | postfix | Set result to null if it fails (caller then uses the CardLoader card). |
| `DeckTrialSequencer.GenerateRewardChoices`, `TradePeltsSequencer.GetTradeCardInfos` | prefix `OfferScope.Enter`, finalizer `OfferScope.Exit` | Opens the scope. |
| `BoulderChoiceSequencer.BoulderChoiceSequence` | postfix wrapping `__result` | Enumerator wrapper that holds `OfferScope` open during each `MoveNext` (pattern: `OpeningHandPatch.WithDoubleBloodSquirrel`). The golden pelt reward stays vanilla (my call, it is not a card choice). |
| `CardLoader.GetDistinctCardsFromPool` | prefix (ref `cardPool`) + postfix | Only while `OfferScope.Active`: prefix swaps in a filtered copy; postfix pads `__result` to `numCards` with filler. Ability mods on real cards stay vanilla. |

Annoying Trades' `GetTradeCardInfos` postfix runs after the filler is in the list and already skips cards that are Annoying (`AnnoyingCards.IsAnnoying`), so no double sigil.

- [ ] **Step 1:** Add a test in `CardFilterTests`: `Traits_from_mods_are_what_count`, which builds `CardTraits` the way `GameCards.Traits` will for a death card (empty tribes, blood 2) and asserts `Blood2Only` + `NoTribeOnly` passes while `InsectOnly` fails. Run → PASS. (This pins the rule the glue relies on. Glue itself is game-bound and checked in-game, Task 9.)
- [ ] **Step 2:** Implement `GameCards`, `OfferScope`, `OfferPatches`.
- [ ] **Step 3:** `dotnet build -c Release` succeeds; `dotnet test` PASS.
- [ ] **Step 4:** Commit `feat: apply tribe and cost filters to every card offer`.

### Task 6: No First Turn Kill / No Phase Turn Kill

**Files:**
- Create: `LagartosChallenges/KillCapRules.cs` (pure), `LagartosChallenges/KillCapPatches.cs`
- Test: `LagartosChallenges.Tests/KillCapRulesTests.cs`

**Interfaces:**
- Produces:
  ```csharp
  public static class KillCapRules {
      const int Goal = 5;
      static int CapDamage(int balance, int damage, bool toPlayer);   // balance = LifeManager.Balance (OpponentDamage - PlayerDamage)
  }
  public sealed class PhaseTracker {
      void BattleStarted();
      void PhaseStarted(int currentRound);          // next round is the phase's first
      bool IsCapRound(int round, bool firstTurnKillOn, bool phaseTurnKillOn);
  }
  ```
- `CapDamage`: toward opponent (`toPlayer=false`) the scale may end at most at +4: returns `min(damage, max(0, 4 - balance))`. Toward player: at least -4: `min(damage, max(0, balance + 4))`. Damage that doesn't reach ±5 comes back unchanged.
- `IsCapRound`: round 1 caps when `firstTurnKillOn`; round `== phaseStart` (set by `PhaseStarted(r)` → `r + 1`) caps when `phaseTurnKillOn`. `BattleStarted` clears the phase start.

- [ ] **Step 1: Failing tests:**
  - `Player_lethal_first_turn_capped_to_4` (`CapDamage(0, 7, false) == 4`)
  - `Tipped_scales_player_cap_is_5` (`CapDamage(-1, 9, false) == 5`)
  - `Leshy_lethal_first_turn_capped_to_4` (`CapDamage(0, 6, true) == 4`)
  - `Tipped_scales_leshy_cap_is_3` (`CapDamage(-1, 5, true) == 3`)
  - `Non_lethal_damage_untouched` (`CapDamage(0, 3, false) == 3`, `CapDamage(2, 2, false) == 2`)
  - `Second_hit_in_same_round_is_capped_by_running_balance` (`CapDamage(3, 4, false) == 1`)
  - `Round_1_caps_only_with_first_turn_challenge`
  - `Phase_round_caps_only_with_phase_challenge` (`PhaseStarted(4)` → round 5 caps, round 6 doesn't)
  - `Battle_start_clears_phase`
- [ ] **Step 2:** Run → FAIL. **Step 3:** Implement. **Step 4:** Run → PASS.
- [ ] **Step 5: Glue** in `KillCapPatches`:
  - prefix `LifeManager.ShowDamageSequence(ref int damage, ref int numWeights, bool toPlayer)` (7-arg iterator stub, too big to inline): if Act 1 Kaycee and `IsCapRound(TurnManager.Instance.TurnNumber, …)`, set `damage = CapDamage(Balance, damage, toPlayer)`, `numWeights = min(numWeights, damage)`, flash the challenge icon that caused it when the value changed.
  - prefix `LifeManager.Initialize(bool)` (virtual) → `BattleStarted()`.
  - prefix on `Opponent.PostResetScalesSequence` **and every override** (use `TargetMethods()` that yields the method on `Opponent` and each subclass that declares it; virtual, not inlined) → `PhaseStarted(TurnNumber)`. Idempotent if an override calls base.
- [ ] **Step 6:** Build + test PASS. Commit `feat: no first turn kill and no phase turn kill`.

### Task 7: Trapper filter and One Squirrel

**Files:**
- Create: `LagartosChallenges/TrapperRules.cs` (pure), `LagartosChallenges/TrapperPatches.cs`, `LagartosChallenges/OneSquirrelPatch.cs`
- Test: `LagartosChallenges.Tests/TrapperRulesTests.cs`

**Interfaces:**
- Produces: `static class TrapperRules { static int[] Prices(int[] vanillaPrices, bool peltsPass); const int OneSquirrelSideDeckSize = 2; static List<T> CutSideDeck<T>(List<T> deck); }`; glue `internal static class TrapperScope` with the same shape as `OfferScope`.
- `Prices`: when pelts don't pass, every slot costs `vanillaPrices[0]` (Rabbit price); otherwise unchanged. Pelts are judged with `GameCards.Passes` on the `PeltHare` traits (no tribe, free), so all three slots pass or fail together.

- [ ] **Step 1: Failing tests:** `Prices_unchanged_when_pelts_pass`, `All_slots_cost_rabbit_price_when_pelts_fail` (`[2,5,9]` → `[2,2,2]`), `Side_deck_cut_to_two` (10 → 2), `Short_side_deck_left_alone` (1 → 1).
- [ ] **Step 2:** Run → FAIL. **Step 3:** Implement. **Step 4:** Run → PASS.
- [ ] **Step 5: Glue:**
  - `BuyPeltsSequencer.PeltPrices` getter postfix → `TrapperRules.Prices`.
  - `BuyPeltsSequencer.CreatePelt` prefix `TrapperScope.Enter` / finalizer `Exit`; `CardLoader.GetCardByName` postfix: inside the scope, if the pelt fails the filter, `__result = GameCards.MakeFiller()`. The free pelt (`GiveFreePeltSequence` takes slot 0) becomes a filler too, which is intended.
  - `Part1CardDrawPiles.SideDeckData` getter postfix (body builds a 10-card list, not inlined) → `CutSideDeck` when `OneSquirrel` is on; flash its icon in a postfix on `CardDrawPiles3D.DrawOpeningHand` (already patched by `OpeningHandPatch`; add a second postfix class, don't edit the absorbed one).
- [ ] **Step 6:** Build + test PASS. Commit `feat: trapper follows filters; one squirrel side deck`.

### Task 8: No X Event and Totem Kinship

**Files:**
- Create: `LagartosChallenges/EventRules.cs` (pure), `LagartosChallenges/EventSkipPatch.cs`, `LagartosChallenges/TotemRules.cs` (pure), `LagartosChallenges/TotemKinshipPatch.cs`
- Test: `LagartosChallenges.Tests/EventRulesTests.cs`, `TotemRulesTests.cs`

**Interfaces:**
- Produces:
  ```csharp
  public enum MapEvent { BoneAltar, MysteriousStones, Mycologists, DeckTrial, Consumables, Campfire, BuyPelts, TradePelts }
  public static class EventRules { static ChallengeKey SkipChallengeFor(MapEvent e); }
  public static class TotemRules {
      static T? PickKinshipTribe<T>(IReadOnlyCollection<T> deckTribes, IReadOnlyCollection<T> eligibleHeads, IReadOnlyCollection<T> ownedHeads, IReadOnlyCollection<T> shownHeads, int seed) where T : struct;
  }
  ```
- Node → event map (glue): `CardRemoveNodeData`→BoneAltar, `CardMergeNodeData`→MysteriousStones, `DuplicateMergeNodeData`→Mycologists, `DeckTrialNodeData`→DeckTrial, `GainConsumablesNodeData`→Consumables, `CardStatBoostNodeData`→Campfire, `BuyPeltsNodeData`→BuyPelts, `TradePeltsNodeData`→TradePelts.
- `PickKinshipTribe`: candidates = deck tribes ∩ eligible − owned − shown; random (`System.Random(seed)`) or null if empty.

- [ ] **Step 1: Failing tests:** `Each_event_maps_to_its_challenge` (8 cases), `Picks_a_deck_tribe`, `Never_picks_owned_or_shown_heads`, `Null_when_deck_has_no_tribes`, `Null_when_all_deck_tribes_taken`, `Deterministic_for_seed`.
- [ ] **Step 2:** Run → FAIL. **Step 3:** Implement. **Step 4:** Run → PASS.
- [ ] **Step 5: Glue:**
  - prefix `SpecialNodeHandler.StartSpecialNodeSequence(SpecialNodeData nodeData)`: when the node maps to an active skip challenge, start a coroutine on the handler and return `false`. Coroutine: `Challenges.ShowActivation(key)`; `yield return TextDisplayer.Instance.ShowUntilInput("...")`; `GameFlowManager.Instance.TransitionToGameState(GameState.Map)`. Must not touch view locks; check in-game that the node is consumed.
  - postfix `BuildTotemSequencer.GenerateTotemChoices(BuildTotemNodeData, int randomSeed)` → `List<ItemData>`: find the last `TotemTopData`; if none, leave it. Eligible = Bird, Canine, Hooved, Insect, Reptile (+ Squirrel if `StoryEventsData.EventCompleted(StoryEvent.SquirrelHeadDiscovered)`); owned = `RunState.Run.totemTops`; shown = the other tops' tribes; deck tribes = `RunState.DeckList` tribes. Non-null pick → set `prerequisites.tribe`, flash icon.
- [ ] **Step 6:** Build + test PASS. Commit `feat: no-event challenges and totem kinship`.

### Task 9: Packaging, docs and in-game check

**Files:**
- Create: `package.ps1` (sibling's, names swapped to `LagartosChallenges`), `thunderstore/manifest.json` (`name: Lagartos_Challenges`, `version_number: 1.0.0`, deps from Global Constraints), `thunderstore/README.md`, `thunderstore/CHANGELOG.md`, `README.md` (challenge table with points, "replaces Unfair Hand and Pelt Starter Hands", build/install like siblings, "How it works" listing each patch point).

- [ ] **Step 1:** `./package.ps1` → tests pass, `dist/Lagartos_Challenges-1.0.0.zip` created with `manifest.json, icon.png, README.md, CHANGELOG.md, plugins/LagartosChallenges.dll`.
- [ ] **Step 2: In-game checklist** (the user runs these with r2modman; report results back, fix failures with superpowers:systematic-debugging):
  1. All 27 challenges show with icons; active ones show the yellow frame.
  2. Insect Only + 2 Blood Only: card choice, rare, cost and tribe face-down choices, Trader, Deck Trial, boulders only offer 2-blood insects or Annoying Rabbit Pelts.
  3. Trapper under Canine Only: three Annoying Rabbit Pelts at Rabbit price; free pelt is one too.
  4. No First Turn Kill: lethal turn-1 attack stops at 4 (5 with Tipped Scales); Leshy's lethal turn-1 attack stops at -4.
  5. No Phase Turn Kill: boss phase 2 first-turn lethal capped; phase 1 not.
  6. Each No X event: icon flashes, "...", back on the map, node consumed, map still navigable.
  7. Totem Kinship: with a canine+insect deck, last head offered is Canine or Insect.
  8. One Squirrel: after the opening hand, side pile has exactly 1 card.
  9. Unfair Hand, Worthy Squirrel, Annoying Trades and the 6 decks behave as in the old mods.
  10. Old DLL installed alongside → BepInEx log shows the incompatibility, nothing doubled.
- [ ] **Step 3:** Commit `chore: packaging and docs`.
