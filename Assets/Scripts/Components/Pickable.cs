using System;
using UnityEngine;

namespace Game.Components
{
    public class Pickable : MonoBehaviour
    {
        /// <summary>
        /// Handles the change in state.
        /// </summary>
        public event Action<bool> OnStateChanged;
        
        // Determines if the object was picked up
        private bool _isPickedUp;
        
        /// <inheritdoc />
        public void Drop()
        {
            // The object is not in hand
            if (!_isPickedUp)
            {
                return;
            }

            _isPickedUp = false;
            transform.SetParent(null);
            
            OnStateChanged?.Invoke(_isPickedUp);
        }

        /// <inheritdoc />
        public void PickUp(Transform holder)
        {
            // The object is in hand
            if (_isPickedUp)
            {
                return;
            }

            _isPickedUp = true;
            transform.SetParent(holder);
            
            OnStateChanged?.Invoke(_isPickedUp);
        }
    }
}