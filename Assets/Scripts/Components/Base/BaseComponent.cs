using Game.Interfaces;
using UnityEngine;

namespace Game.Components
{
    public abstract class BaseComponent : MonoBehaviour, IComponent
    {
        /// <summary>
        /// True if the component is being destroyed.
        /// </summary>
        protected bool IsBeingDestroyed { get; private set; }
        
        /// <inheritdoc />
        public IObject Owner { get; private set; }

#region Unity Events

        // Prepares the object for destruction
        protected virtual void OnDestroy()
        {
            IsBeingDestroyed = true;
        }

#endregion
        
#region Methods
        
        /// <inheritdoc />
        public void SetOwner(IObject owner)
        {
            // There is already a parent.
            if (Owner != null)
            {
                Debug.LogWarning($"{name} was already assigned parent {Owner}.");
                return;
            }
            
            Owner = owner;
        }

#endregion
    }
}