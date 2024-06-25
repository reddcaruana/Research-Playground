namespace Game.Atoms
{
    public struct StringPair : IPair<string>
    {
        /// <inheritdoc />
        public string Item1 { get; set; }

        /// <inheritdoc />
        public string Item2 { get; set; }
    }
}