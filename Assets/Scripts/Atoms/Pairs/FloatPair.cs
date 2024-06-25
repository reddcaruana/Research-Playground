namespace Game.Atoms
{
    public struct FloatPair : IPair<float>
    {
        /// <inheritdoc />
        public float Item1 { get; set; }

        /// <inheritdoc />
        public float Item2 { get; set; }
    }
}