using System.Collections;
using System.Reflection;
using LagartosChallenges;

namespace LagartosChallenges.Tests;

/// <summary>
/// Tests for <see cref="PatchTargets"/>: every override of a virtual coroutine must be patched, because the game runs
/// the subclass version (Act 1 uses CombatPhaseManager3D, not the base CombatPhaseManager).
/// </summary>
public class PatchTargetsTests
{
    /// <summary>Stand-in for a game base class with a virtual no-op coroutine.</summary>
    public abstract class Manager
    {
        /// <summary>Base no-op.</summary>
        protected virtual IEnumerator Visualize(bool flag) { yield break; }

        /// <summary>Abstract coroutine (never patchable).</summary>
        protected abstract IEnumerator Other();
    }

    /// <summary>Overrides the coroutine (like CombatPhaseManager3D).</summary>
    public class Manager3D : Manager
    {
        /// <inheritdoc />
        protected override IEnumerator Visualize(bool flag) { yield break; }

        /// <inheritdoc />
        protected override IEnumerator Other() { yield break; }
    }

    /// <summary>Inherits without overriding (must not be listed, nor make Harmony warn).</summary>
    public class PlainManager : Manager3D { }

    /// <summary>Unrelated type with a same-named method.</summary>
    public class Unrelated
    {
        /// <summary>Same name, wrong hierarchy.</summary>
        protected IEnumerator Visualize(bool flag) { yield break; }
    }

    private static readonly Type[] AllTypes = { typeof(Manager), typeof(Manager3D), typeof(PlainManager), typeof(Unrelated) };

    [Fact]
    public void Lists_the_base_and_every_override()
    {
        IEnumerable<Type> owners = PatchTargets.Declarations(AllTypes, typeof(Manager), "Visualize").Select(m => m.DeclaringType!);
        Assert.Equal(new[] { typeof(Manager), typeof(Manager3D) }, owners);
    }

    [Fact]
    public void Skips_abstract_declarations() =>
        Assert.Equal(new[] { typeof(Manager3D) },
            PatchTargets.Declarations(AllTypes, typeof(Manager), "Other").Select(m => m.DeclaringType!));
}
