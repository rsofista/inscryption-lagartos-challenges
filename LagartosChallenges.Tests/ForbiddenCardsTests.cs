using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for <see cref="ForbiddenCards"/>: which never-offered cards the Forbidden Cards challenge adds to each pool.
/// </summary>
public class ForbiddenCardsTests
{
    [Fact]
    public void Common_cards_are_the_boss_and_event_creatures()
    {
        Assert.Equal(new[]
        {
            "MoleSeaman", "SkeletonPirate", "SkeletonParrot", "TrapFrog", "Mule", "Smoke", "Smoke_Improved", "Starvation",
        }, ForbiddenCards.Common);
    }

    [Fact]
    public void Rare_cards_are_the_great_kraken()
    {
        Assert.Equal(new[] { "Kraken" }, ForbiddenCards.Rare);
    }

    [Fact]
    public void Choice_pool_gets_every_common_card_it_lacks()
    {
        var added = ForbiddenCards.MissingFrom(rarePool: false, new[] { "Stoat", "Smoke" });
        Assert.Equal(ForbiddenCards.Common.Where(n => n != "Smoke"), added);
    }

    [Fact]
    public void Rare_pool_gets_the_kraken()
    {
        Assert.Equal(new[] { "Kraken" }, ForbiddenCards.MissingFrom(rarePool: true, new[] { "Urayuli", "MantisGod" }));
    }

    [Fact]
    public void Pool_that_already_has_them_gets_nothing()
    {
        Assert.Empty(ForbiddenCards.MissingFrom(rarePool: true, new[] { "Kraken" }));
        Assert.Empty(ForbiddenCards.MissingFrom(rarePool: false, ForbiddenCards.Common));
    }
}
