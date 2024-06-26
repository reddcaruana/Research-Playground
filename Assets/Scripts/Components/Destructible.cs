using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Game.Components
{
    public class Destructible : MonoBehaviour, IDestructible
    {
        /// <summary>
        /// The object's health.
        /// </summary>
        [field: SerializeField] public int Health { get; private set; }

        /// <summary>
        /// Handles changes in health with history.
        /// </summary>
        public event Action<int, int> OnHealthChanged;

        /// <summary>
        /// Handles the destruction state.
        /// </summary>
        public event Action OnDepleted;
        
        /// <inheritdoc />
        public void Damage(int value)
        {
            var oldHealth = Health;
            Health -= value;
            
            // Invoke the change event
            OnHealthChanged?.Invoke(Health, oldHealth);
            
            // Invoke the depleted event if we're at zero
            if (Health <= 0)
            {
                OnDepleted?.Invoke();
            }
        }
    }
}