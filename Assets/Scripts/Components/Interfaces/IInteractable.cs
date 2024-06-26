namespace Game.Components
{
    public interface IInteractable : IObjectComponent
    {
        /// <summary>
        /// Interacts with the object.
        /// </summary>
        void Interact();
    }
}