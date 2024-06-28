namespace Game.Interfaces
{
    public interface IObject
    {
        /// <summary>
        /// Returns a component of the specified type.
        /// </summary>
        T Get<T>() where T : IComponent;
    }
}