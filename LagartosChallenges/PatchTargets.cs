using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace LagartosChallenges
{
    /// <summary>
    /// Finds every concrete declaration of a virtual method in a class hierarchy, so a Harmony patch covers the
    /// subclass versions the game actually runs (e.g. Act 1's <c>CombatPhaseManager3D</c>). Pure reflection, unit tested.
    /// </summary>
    public static class PatchTargets
    {
        /// <summary>
        /// Non-abstract methods named <paramref name="methodName"/> declared directly on <paramref name="baseType"/>
        /// or one of its subclasses. Types that only inherit the method are skipped (no Harmony "not found" warnings).
        /// </summary>
        /// <param name="types">Candidate types (usually every type of the game assembly).</param>
        /// <param name="baseType">Root of the hierarchy.</param>
        /// <param name="methodName">Method name.</param>
        /// <returns>The declarations, in <paramref name="types"/> order.</returns>
        public static IEnumerable<MethodInfo> Declarations(IEnumerable<Type> types, Type baseType, string methodName) =>
            types.Where(t => t != null && baseType.IsAssignableFrom(t))
                .SelectMany(t => t.GetMethods(BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
                .Where(m => m.Name == methodName && !m.IsAbstract);

        /// <summary>
        /// Sets a property through the type that declares it, so a private setter works on a subclass instance
        /// (reflection from the subclass, which <c>Traverse.Create(instance)</c> does, cannot see it).
        /// </summary>
        /// <param name="target">Instance to update (may be a subclass of <paramref name="declaringType"/>).</param>
        /// <param name="declaringType">Type that declares the property.</param>
        /// <param name="name">Property name.</param>
        /// <param name="value">New value.</param>
        public static void SetProperty(object target, Type declaringType, string name, object value) =>
            declaringType.GetProperty(name, BindingFlags.DeclaredOnly | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .SetValue(target, value);
    }
}
