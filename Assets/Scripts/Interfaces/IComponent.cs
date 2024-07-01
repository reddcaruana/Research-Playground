namespace Game.Interfaces
{
    public interface IComponent
    {
        /// <summary>
        /// The owning object.
        /// </summary>
        IObject Owner { get; }
        
        /// <summary>
        /// Sets the component owner.
        /// </summary>
        /// <param name="owner">The component owner.</param>
        void SetOwner(IObject owner);
    }
}