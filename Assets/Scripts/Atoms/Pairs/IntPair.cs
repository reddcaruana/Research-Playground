namespace Game.Atoms
{
    public struct IntPair : IPair<int>
    {
        /// <inheritdoc />
        public int Item1 { get; set; }

        /// <inheritdoc />
        public int Item2 { get; set; }
    }
}