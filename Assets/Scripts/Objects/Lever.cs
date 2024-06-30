using Game.Components;
using Game.Interfaces;
using UnityEngine;

namespace Game.Objects
{
    public class Lever : BaseObject
    {
        // Possible components
        private Triggerable triggerable;

#region Unity Events

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            // Get the possible components
            triggerable = Get<Triggerable>();
        }

        // Unregister all actions
        private void OnDisable()
        {
            // Trigger
            if (triggerable)
            {
                triggerable.OnChange -= OnTriggerableStateChanged;
            }
        }

        // Register all actions
        private void OnEnable()
        {
            // Trigger
            if (triggerable)
            {
                triggerable.OnChange += OnTriggerableStateChanged;
            }
        }

#endregion

#region Event Handlers

        /// <summary>
        /// Handles the change in trigger state.
        /// </summary>
        /// <param name="state">The state value.</param>
        private void OnTriggerableStateChanged(bool state)
        {
            Debug.Log("Lever state changed!");
        }

#endregion
    }
}