using System;
using UnityEngine;

namespace Game.Components
{
    public class Openable : MonoBehaviour
    {
        /// <summary>
        /// The object controlling this property.
        /// </summary>
        // public IInteractable Controller { get; private set; }

        /// <summary>
        /// Identifies if the property is being modified by another object.
        /// </summary>
        // public bool IsControlled => Controller != null;
        
        /// <summary>
        /// Identifies if the object was opened.
        /// </summary>
        public bool IsOpen { get; private set; }

        /// <summary>
        /// Handles the change in state.
        /// </summary>
        public event Action<bool> OnChange;

        /// <summary>
        /// Sets the controller for this property.
        /// </summary>
        /// <param name="controller">The interactable property.</param>
        // public void SetController(IInteractable controller)
        // {
        //     Controller = controller;
        // }
        
        /// <inheritdoc />
        public void Interact()
        {
            // // The object is controlled
            // if (IsControlled)
            // {
            //     return;
            // }
            //
            // Interact(null);
        }

        /// <summary>
        /// Applies an interaction through a controller.
        /// </summary>
        /// <param name="controller">The controller value.</param>
        // internal void Interact(IInteractable controller)
        // {
        //     // This is not the same controller
        //     if (Controller != controller)
        //     {
        //         return;
        //     }
        //     
        //     // Switch the flag
        //     IsOpen = !IsOpen;
        //     
        //     // Emit the value
        //     OnChange?.Invoke(IsOpen);
        // }
    }
}