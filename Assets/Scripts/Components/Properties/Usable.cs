using Game.Atoms;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(BoolVariable))]
    public class Usable : BaseInteractable
    {
        // Atomic components
        private BoolVariable _variable;

#region Unity Events

        // Component caching
        private void Awake()
        {
            _variable = GetComponent<BoolVariable>();
        }

#endregion

#region Methods
        
        /// <summary>
        /// Changes the variable value.
        /// </summary>
        public void Use()
        {
            _variable.Value = !_variable.Value;
        }

#endregion
    }
}