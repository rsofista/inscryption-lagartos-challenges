using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for the one-shot grant that gives the Worthy Squirrel its Double Blood sigil.
/// </summary>
public class DoubleBloodGrantTests
{
    [Fact]
    public void Not_armed_grants_nothing()
    {
        var grant = new DoubleBloodGrant();
        Assert.False(grant.TryConsume());
    }

    [Fact]
    public void Armed_grants_the_next_side_draw()
    {
        var grant = new DoubleBloodGrant();
        grant.Arm();
        Assert.True(grant.TryConsume());
    }

    [Fact]
    public void Only_one_side_draw_gets_the_sigil()
    {
        var grant = new DoubleBloodGrant();
        grant.Arm();
        grant.TryConsume();
        Assert.False(grant.TryConsume());
    }

    [Fact]
    public void Disarm_drops_an_unused_grant()
    {
        var grant = new DoubleBloodGrant();
        grant.Arm();
        grant.Disarm();
        Assert.False(grant.TryConsume());
    }
}
