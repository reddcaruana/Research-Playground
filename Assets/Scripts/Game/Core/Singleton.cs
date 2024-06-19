namespace Game
{
    public abstract class Singleton<T>
        where T : new()
    {
        /// <summary>
        /// The current instance for this class.
        /// </summary>
        public static T Current { get; protected set; }
    }
}