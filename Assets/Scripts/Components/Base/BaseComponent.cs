using Game.Interfaces;
using UnityEngine;

namespace Game.Components
{
    public abstract class BaseComponent : MonoBehaviour, IComponent
    {
        /// <inheritdoc />
        public IObject Owner { get; private set; }

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
    }
}