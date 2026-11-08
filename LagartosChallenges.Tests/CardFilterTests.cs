using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the tribe/cost filter: OR inside a group, AND across groups.
/// </summary>
public class CardFilterTests
{
    /// <summary>Builds a filter from the given active challenges.</summary>
    private static CardFilter Filter(params ChallengeKey[] keys) => new CardFilter(keys);

    /// <summary>Card with the given tribes and blood cost.</summary>
    private static CardTraits Card(int blood, params FilterTribe[] tribes) =>
        new CardTraits(tribes, hasOtherTribe: false, blood: blood, bones: 0, energy: 0, gems: 0);

    private static readonly CardTraits Wolf2 = Card(2, FilterTribe.Canine);
    private static readonly CardTraits Mantis1 = Card(1, FilterTribe.Insect);
    private static readonly CardTraits Beehive2 = Card(2, FilterTribe.Insect);
    private static readonly CardTraits Raven2 = Card(2, FilterTribe.Bird);
    private static readonly CardTraits Grizzly3 = Card(3);

    [Fact]
    public void No_filter_passes_everything()
    {
        CardFilter filter = Filter(ChallengeKey.UnfairHand, ChallengeKey.NoCampfire);
        Assert.False(filter.IsActive);
        Assert.True(filter.Passes(Wolf2));
        Assert.True(filter.Passes(Grizzly3));
    }

    [Fact]
    public void Insect_only_passes_insects_only()
    {
        CardFilter filter = Filter(ChallengeKey.InsectOnly);
        Assert.True(filter.IsActive);
        Assert.True(filter.Passes(Mantis1));
        Assert.False(filter.Passes(Wolf2));
        Assert.False(filter.Passes(Grizzly3));
    }

    [Fact]
    public void Avian_and_canine_pass_either()
    {
        CardFilter filter = Filter(ChallengeKey.AvianOnly, ChallengeKey.CanineOnly);
        Assert.True(filter.Passes(Wolf2));
        Assert.True(filter.Passes(Raven2));
        Assert.False(filter.Passes(Mantis1));
    }

    [Fact]
    public void Insect_and_2_blood_need_both()
    {
        CardFilter filter = Filter(ChallengeKey.InsectOnly, ChallengeKey.Blood2Only);
        Assert.False(filter.Passes(Mantis1));
        Assert.False(filter.Passes(Wolf2));
        Assert.True(filter.Passes(Beehive2));
    }

    [Fact]
    public void Multi_tribe_card_passes_if_any_tribe_selected()
    {
        CardTraits hybrid = Card(2, FilterTribe.Canine, FilterTribe.Insect);
        Assert.True(Filter(ChallengeKey.InsectOnly).Passes(hybrid));
        Assert.False(Filter(ChallengeKey.ReptileOnly).Passes(hybrid));
    }

    [Fact]
    public void No_tribe_only_passes_tribeless_cards()
    {
        CardFilter filter = Filter(ChallengeKey.NoTribeOnly);
        Assert.True(filter.Passes(Grizzly3));
        Assert.False(filter.Passes(Wolf2));
    }

    [Fact]
    public void No_tribe_only_rejects_squirrel_tribe_cards()
    {
        var squirrelish = new CardTraits(Array.Empty<FilterTribe>(), hasOtherTribe: true, blood: 0, bones: 0, energy: 0, gems: 0);
        Assert.False(Filter(ChallengeKey.NoTribeOnly).Passes(squirrelish));
    }

    [Fact]
    public void Bones_only_needs_bone_cost()
    {
        CardFilter filter = Filter(ChallengeKey.BonesOnly);
        Assert.True(filter.Passes(new CardTraits(new[] { FilterTribe.Canine }, false, 0, 4, 0, 0)));
        Assert.False(filter.Passes(Wolf2));
    }

    [Fact]
    public void Free_only_rejects_energy_and_gems()
    {
        CardFilter filter = Filter(ChallengeKey.FreeOnly);
        Assert.True(filter.Passes(Card(0)));
        Assert.False(filter.Passes(new CardTraits(Array.Empty<FilterTribe>(), false, 0, 0, 1, 0)));
        Assert.False(filter.Passes(new CardTraits(Array.Empty<FilterTribe>(), false, 0, 0, 0, 1)));
        Assert.False(filter.Passes(Mantis1));
    }

    [Fact]
    public void Blood_challenges_pass_any_selected_cost()
    {
        CardFilter filter = Filter(ChallengeKey.Blood1Only, ChallengeKey.Blood3Only);
        Assert.True(filter.Passes(Mantis1));
        Assert.True(filter.Passes(Grizzly3));
        Assert.False(filter.Passes(Wolf2));
    }

    [Fact]
    public void Cost_choice_allowed_only_for_selected_costs()
    {
        CardFilter filter = Filter(ChallengeKey.Blood2Only);
        Assert.True(filter.AllowsCostChoice(2));
        Assert.False(filter.AllowsCostChoice(1));
        Assert.False(filter.AllowsCostChoice(null));
        Assert.True(Filter(ChallengeKey.BonesOnly).AllowsCostChoice(null));
        CardFilter none = Filter(ChallengeKey.InsectOnly);
        Assert.True(none.AllowsCostChoice(1));
        Assert.True(none.AllowsCostChoice(null));
    }

    [Fact]
    public void Tribe_choices_follow_selected_tribes()
    {
        CardFilter filter = Filter(ChallengeKey.ReptileOnly, ChallengeKey.CanineOnly, ChallengeKey.NoTribeOnly);
        Assert.True(filter.TribeGroupActive);
        Assert.Equal(new[] { FilterTribe.Canine, FilterTribe.Reptile }, filter.AllowedTribeChoices.OrderBy(t => t));
        Assert.False(Filter(ChallengeKey.Blood1Only).TribeGroupActive);
        Assert.Empty(Filter(ChallengeKey.NoTribeOnly).AllowedTribeChoices);
    }

    [Fact]
    public void Traits_from_mods_are_what_count()
    {
        // A death card: no tribe, cost comes from its mods (the glue passes the mod-aware costs).
        var deathCard = new CardTraits(Array.Empty<FilterTribe>(), false, 2, 0, 0, 0);
        Assert.True(Filter(ChallengeKey.Blood2Only, ChallengeKey.NoTribeOnly).Passes(deathCard));
        Assert.False(Filter(ChallengeKey.InsectOnly).Passes(deathCard));
    }
}
