using System;
using Game.Interfaces;
using UnityEngine;

namespace Game.Components
{
    public class Triggerable : MonoBehaviour, IInteractable
    {
        /// <summary>
        /// The state of this switch.
        /// </summary>
        public bool State { get; private set; }

        /// <summary>
        /// Handles the change in state.
        /// </summary>
        public event Action<bool> OnChange;
        
        /// <inheritdoc />
        public void Interact()
        {
            // Switch the state
            State = !State;
            
            // Emit the value
            OnChange?.Invoke(State);
        }
    }
}