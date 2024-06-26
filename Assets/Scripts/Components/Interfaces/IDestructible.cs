namespace Game.Components
{
    public interface IDestructible : IObjectComponent
    {
        /// <summary>
        /// Applies damage to the object.
        /// </summary>
        /// <param name="value">The damage value.</param>
        void Damage(int value);
    }
}