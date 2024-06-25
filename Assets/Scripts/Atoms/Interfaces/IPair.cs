namespace Game.Atoms
{
    public interface IPair<T>
    {
        /// <summary>
        /// The first value.
        /// </summary>
        T Item1 { get; set; }
        
        /// <summary>
        /// The second value.
        /// </summary>
        T Item2 { get; set; }
    }
}