namespace Game.Interfaces
{
    public interface IPickable : IComponent
    {
        /// <summary>
        /// Drops the item.
        /// </summary>
        void Drop();

        /// <summary>
        /// Collects the item.
        /// </summary>
        void PickUp();
    }
}