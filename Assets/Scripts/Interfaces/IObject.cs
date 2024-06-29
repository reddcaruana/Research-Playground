namespace Game.Interfaces
{
    public interface IObject
    {
        /// <summary>
        /// Returns a component of the specified type.
        /// </summary>
        T Get<T>() where T : IComponent;

        /// <summary>
        /// Gets the component of the specified type, if it exists.
        /// </summary>
        /// <param name="instance">The output argument that will contain the component.</param>
        bool TryGet<T>(out T instance) where T : IComponent;
    }
}