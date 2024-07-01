using Game.Interfaces;
using Unity.Plastic.Newtonsoft.Json.Serialization;
using UnityEngine;

namespace Game.Components
{
    public class Destructible : BaseComponent, IDestructible
    {
        /// <inheritdoc />
        [field: SerializeField] public int Integrity { get; private set; }

        /// <summary>
        /// Handles changes in health with history.
        /// </summary>
        public event Action<int, int> OnIntegrityChanged;

        /// <summary>
        /// Handles the destruction state.
        /// </summary>
        public event Action OnBroken;

        /// <inheritdoc />
        public void Damage(int value)
        {
            var oldIntegrity = Integrity;
            Integrity -= value;
            
            // Invoke the change event
            OnIntegrityChanged?.Invoke(Integrity, oldIntegrity);
            
            // Invoke the depleted event if we're at zero
            if (Integrity <= 0)
            {
                OnBroken?.Invoke();
            }
        }
    }
}