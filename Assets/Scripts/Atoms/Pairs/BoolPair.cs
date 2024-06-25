namespace Game.Atoms
{
    public struct BoolPair : IPair<bool>
    {
        /// <inheritdoc />
        public bool Item1 { get; set; }

        /// <inheritdoc />
        public bool Item2 { get; set; }
    }
}