using System;
using Game.Interfaces;

namespace Game.Components
{
    public abstract class Pickable : BaseComponent, IPickable
    {
        /// <summary>
        /// Handles the pickup action.
        /// </summary>
        public event Action<IPickable> OnPickedUp;

        /// <summary>
        /// Handles the drop action.
        /// </summary>
        public event Action<IPickable> OnDropped;

        // Determines if the object was picked up
        private bool grounded;

        /// <inheritdoc />
        public virtual void Drop()
        {
            // The object is already grounded
            if (grounded)
            {
                return;
            }
            
            // Set the grounded flag
            grounded = true;
            
            // Invoke the dropped event
            OnDropped?.Invoke(this);
        }

        /// <inheritdoc />
        public virtual void PickUp()
        {
            // The object is not on the ground
            if (!grounded)
            {
                return;
            }

            // Remove the grounded flag
            grounded = false;
            
            // Invoke the pickup event
            OnPickedUp?.Invoke(this);
        }
    }
}