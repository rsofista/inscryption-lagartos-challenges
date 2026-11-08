using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for when pelts and the Trader's cards get the Annoying sigil.
/// </summary>
public class AnnoyingRulesTests
{
    [Fact]
    public void Starter_pelts_become_annoying_with_both_challenges()
    {
        Assert.True(AnnoyingRules.StarterPeltsBecomeAnnoying(annoyingStartersActive: true, annoyingTradesActive: true));
    }

    [Fact]
    public void Starter_pelts_stay_plain_with_only_annoying_starters()
    {
        // Vanilla Annoying Starters skips pelts; only this mod's challenge extends it to them.
        Assert.False(AnnoyingRules.StarterPeltsBecomeAnnoying(annoyingStartersActive: true, annoyingTradesActive: false));
    }

    [Fact]
    public void Starter_pelts_stay_plain_with_only_annoying_trades()
    {
        Assert.False(AnnoyingRules.StarterPeltsBecomeAnnoying(annoyingStartersActive: false, annoyingTradesActive: true));
    }

    [Fact]
    public void One_annoying_pelt_makes_the_offers_annoying()
    {
        Assert.True(AnnoyingRules.TradeOffersBecomeAnnoying(annoyingTradesActive: true, new[] { false, true, false }));
    }

    [Fact]
    public void Plain_pelts_keep_the_offers_plain()
    {
        Assert.False(AnnoyingRules.TradeOffersBecomeAnnoying(annoyingTradesActive: true, new[] { false, false }));
    }

    [Fact]
    public void No_pelts_means_plain_offers()
    {
        Assert.False(AnnoyingRules.TradeOffersBecomeAnnoying(annoyingTradesActive: true, Array.Empty<bool>()));
    }

    [Fact]
    public void Annoying_pelts_do_nothing_without_the_challenge()
    {
        Assert.False(AnnoyingRules.TradeOffersBecomeAnnoying(annoyingTradesActive: false, new[] { true }));
    }

    [Fact]
    public void A_card_that_is_already_annoying_gets_no_second_copy()
    {
        Assert.False(AnnoyingRules.NeedsAnnoying(alreadyAnnoying: true));
        Assert.True(AnnoyingRules.NeedsAnnoying(alreadyAnnoying: false));
    }
}
