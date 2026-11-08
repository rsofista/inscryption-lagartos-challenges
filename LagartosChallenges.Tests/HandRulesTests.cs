using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the pure opening-hand rules of the Unfair Hand challenge.
/// </summary>
public class HandRulesTests
{
    [Fact]
    public void Opening_hand_draws_one_squirrel()
    {
        Assert.Equal(1, HandRules.SquirrelCount);
    }

    [Fact]
    public void Opening_hand_draws_three_deck_cards()
    {
        Assert.Equal(3, HandRules.DeckCardCount);
    }

    [Fact]
    public void With_no_fixed_cards_all_three_deck_cards_are_random()
    {
        Assert.Equal(3, HandRules.RandomDrawsNeeded(fixedCardsDrawn: 0, cardsLeftInDeck: 20));
    }

    [Fact]
    public void Fixed_cards_count_toward_the_three_deck_cards()
    {
        Assert.Equal(1, HandRules.RandomDrawsNeeded(fixedCardsDrawn: 2, cardsLeftInDeck: 20));
    }

    [Fact]
    public void More_fixed_cards_than_three_means_no_random_draws()
    {
        Assert.Equal(0, HandRules.RandomDrawsNeeded(fixedCardsDrawn: 4, cardsLeftInDeck: 20));
    }

    [Fact]
    public void Random_draws_never_exceed_cards_left_in_deck()
    {
        Assert.Equal(2, HandRules.RandomDrawsNeeded(fixedCardsDrawn: 0, cardsLeftInDeck: 2));
    }

    [Fact]
    public void Empty_deck_means_no_random_draws()
    {
        Assert.Equal(0, HandRules.RandomDrawsNeeded(fixedCardsDrawn: 0, cardsLeftInDeck: 0));
    }

    [Fact]
    public void Plain_card_is_worth_one_blood()
    {
        Assert.Equal(1, HandRules.SacrificeValue(hasTripleBlood: false, hasDoubleBlood: false));
    }

    [Fact]
    public void Double_blood_card_is_worth_two_blood()
    {
        Assert.Equal(2, HandRules.SacrificeValue(hasTripleBlood: false, hasDoubleBlood: true));
    }

    [Fact]
    public void Worthy_sacrifice_is_worth_three_blood()
    {
        Assert.Equal(3, HandRules.SacrificeValue(hasTripleBlood: true, hasDoubleBlood: false));
    }

    [Fact]
    public void Worthy_sacrifice_wins_over_double_blood()
    {
        Assert.Equal(3, HandRules.SacrificeValue(hasTripleBlood: true, hasDoubleBlood: true));
    }
}
