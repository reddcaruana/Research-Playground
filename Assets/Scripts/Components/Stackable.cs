using System;
using Game.Interfaces;

namespace Game.Components
{
    public class Stackable : BaseComponent, IComponent
    {
        /// <summary>
        /// The object that was stacked onto this one.
        /// </summary>
        public IObject Child { get; private set; }

        /// <summary>
        /// Invoked when an object is added.
        /// </summary>
        public event Action<IObject> OnStacked;

        /// <summary>
        /// Invoked when an object is removed.
        /// </summary>
        public event Action<IObject> OnUnstacked;

        /// <summary>
        /// Returns the result of a successful stacking.
        /// </summary>
        /// <param name="value">The object value.</param>
        public bool Stack(IObject value)
        {
            // There is already a child
            if (Child != null)
            {
                // The child cannot be stacked onto
                var stackable = Child.Get<Stackable>();
                if (!stackable)
                {
                    return false;
                }

                // Return the outcome of the child object
                return stackable.Stack(value);
            }

            // Stack the child
            Child = value;
            OnStacked?.Invoke(Child);
            
            return true;
        }

        /// <summary>
        /// Returns the stacked object and clears it from this instance.
        /// </summary>
        public IObject Unstack()
        {
            // Get the child object
            var child = Child;

            // Clear the property
            Child = null;
            
            // Call the event
            OnUnstacked?.Invoke(child);
            return child;
        }
    }
}