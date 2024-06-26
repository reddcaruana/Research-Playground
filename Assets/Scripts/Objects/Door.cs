using Game.Components;
using UnityEngine;

namespace Game.Objects
{
    public class Door : BaseObject
    {
        // Components
        private Collider _collider;

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();

            _collider = GetComponent<Collider>();
        }

        // Unregister all actions
        private void OnDisable()
        {
            // Openable
            var openable = Get<Openable>();
            if (openable)
            {
                openable.OnChange -= OpenHandler;
            }
            
            // Destructible
            var destructible = Get<Destructible>();
            if (destructible)
            {
                destructible.OnHealthChanged -= DamageHandler;
                destructible.OnDepleted -= DestroyedHandler;
            }
        }

        // Register all actions
        private void OnEnable()
        {
            // Openable
            var openable = Get<Openable>();
            if (openable)
            {
                openable.OnChange += OpenHandler;
            }
            
            // Destructible
            var destructible = Get<Destructible>();
            if (destructible)
            {
                destructible.OnHealthChanged += DamageHandler;
                destructible.OnDepleted += DestroyedHandler;
            }
        }

#region Action Handling

        /// <summary>
        /// Handles the damaged event.
        /// </summary>
        /// <param name="current">The current health.</param>
        /// <param name="previous">The previous health.</param>
        private void DamageHandler(int current, int previous)
        {
            var damage = previous - current;
            Debug.Log($"{name} received {damage} damage!");
        }

        /// <summary>
        /// Handles the Destroyed event.
        /// </summary>
        private void DestroyedHandler()
        {
            Destroy(gameObject);
        }
        
        /// <summary>
        /// Handles the Open event.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        private void OpenHandler(bool value)
        {
            _collider.isTrigger = value;
        }

#endregion
    }
}