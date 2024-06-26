using System;
using UnityEngine;

namespace Game.Components
{
    public class Trigger : MonoBehaviour, IInteractable
    {
        /// <summary>
        /// The state of this switch.
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// The target element.
        /// </summary>
        [field: SerializeField] public Openable Target { get; private set; }

        /// <summary>
        /// Handles the change in state.
        /// </summary>
        public event Action<bool> OnChange;

        // Registers object links
        private void OnEnable()
        {
            Target?.SetController(this);
        }

        /// <inheritdoc />
        public void Interact()
        {
            // Switch the state
            State = !State;
            Target.Interact(this);
            
            // Emit the value
            OnChange?.Invoke(State);
        }
    }
}