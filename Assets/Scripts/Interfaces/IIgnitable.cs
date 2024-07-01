namespace Game.Interfaces
{
    public interface IIgnitable : IComponent
    {
        /// <summary>
        /// Fires when an object is in contact with a fire source.
        /// </summary>
        void OnContact(IFireSource fireSource);

        /// <summary>
        /// Fires when an object is no longer in contact with a fire source.
        /// </summary>
        void OnSeparate(IFireSource fireSource);
    }
}