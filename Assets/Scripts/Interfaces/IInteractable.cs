namespace Game.Interfaces
{
    public interface IInteractable : IComponent
    {
        /// <summary>
        /// Triggers an interaction with the object.
        /// </summary>
        void Interact();
    }
}