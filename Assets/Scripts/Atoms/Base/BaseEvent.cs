using System;

namespace Game.Atoms
{
    public abstract class BaseEvent<T> : BaseAtom
    {
        /// <summary>
        /// The change event.
        /// </summary>
        protected event Action<T> ChangeEvent;

        /// <summary>
        /// Invoke the change event.
        /// </summary>
        /// <param name="value">The value associated with this event.</param>
        public void Emit(T value)
        {
            ChangeEvent?.Invoke(value);
        }
        
        /// <summary>
        /// Registers a callback to the event.
        /// </summary>
        /// <param name="action">The event handler.</param>
        public void Register(Action<T> action)
        {
            ChangeEvent += action;
        }

        /// <summary>
        /// Unregisters a callback from the event.
        /// </summary>
        /// <param name="action">The event handler.</param>
        public void Unregister(Action<T> action)
        {
            ChangeEvent -= action;
        }
    }
}