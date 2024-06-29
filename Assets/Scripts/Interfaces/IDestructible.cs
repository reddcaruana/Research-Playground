namespace Game.Interfaces
{
    public interface IDestructible : IComponent
    {
        /// <summary>
        /// The object's durability.
        /// </summary>
        int Integrity { get; }

        /// <summary>
        /// Applies durability damage.
        /// </summary>
        /// <param name="value">The damage value.</param>
        void Damage(int value);
    }
}