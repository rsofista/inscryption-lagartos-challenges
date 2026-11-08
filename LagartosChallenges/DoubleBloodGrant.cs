namespace LagartosChallenges
{
    /// <summary>
    /// One-shot flag for the Worthy Squirrel challenge: armed when the opening hand starts, consumed by the first
    /// side-pile draw (which then gets the Double Blood sigil), and disarmed when the opening hand ends so a later
    /// draw can never pick it up. Free of Unity/game types so it can be unit tested.
    /// </summary>
    public class DoubleBloodGrant
    {
        /// <summary>True while a side draw is still owed the sigil.</summary>
        private bool armed;

        /// <summary>Owes the sigil to the next side-pile draw.</summary>
        public void Arm() => armed = true;

        /// <summary>Drops the grant if nobody used it.</summary>
        public void Disarm() => armed = false;

        /// <summary>
        /// Called by a side-pile draw: returns true (and disarms) if this draw should get the sigil.
        /// </summary>
        /// <returns>True only for the first draw after <see cref="Arm"/>.</returns>
        public bool TryConsume()
        {
            bool grant = armed;
            armed = false;
            return grant;
        }
    }
}
