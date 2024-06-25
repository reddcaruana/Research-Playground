using UnityEngine;

namespace Game.World
{
    public class BooleanState : BaseState
    {
        /// <summary>
        /// The toggle callback.
        /// </summary>
        public BooleanCallback OnChange;
        
        /// <summary>
        /// The value of this state.
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// Sets the boolean value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        public void SetValue(bool newValue)
        {
            // Don't do anything if the value is the same
            if (Value == newValue)
            {
                return;
            }
            
            Value = newValue;
            OnChange?.Invoke(newValue);
        }
        
        /// <summary>
        /// Toggles the state value.
        /// </summary>
        public void Toggle()
        {
            SetValue(!Value);
        }
    }
}