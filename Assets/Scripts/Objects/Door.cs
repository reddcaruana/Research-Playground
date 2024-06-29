using Game.Components;
using UnityEngine;

namespace Game.Objects
{
    public class Door : BaseObject
    {
<<<<<<< Updated upstream
        [SerializeField] private GameObject model;
        
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
=======
        [Header("References")]
        [SerializeField] private GameObject doorObject;
>>>>>>> Stashed changes

        // Components
        private Destructible destructible;

<<<<<<< Updated upstream
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
=======
#region Unity Events

        /// <inheritdoc />
        protected override void Start()
        {
            base.Start();

            if (TryGet(out destructible))
            {
                destructible.OnIntegrityChanged += OnDestructibleIntegrityChanged;
                destructible.OnBroken += OnDestructibleBroken;
            }
        }

#endregion

#region Destructible Events

        /// <summary>
        /// The destructible has been broken.
        /// </summary>
        private void OnDestructibleBroken()
>>>>>>> Stashed changes
        {
            Destroy(gameObject);
        }
        
        /// <summary>
<<<<<<< Updated upstream
        /// Handles the Open event.
        /// </summary>
        /// <param name="value">The boolean value.</param>
        private void OpenHandler(bool value)
        {
            model.SetActive(!value);
            _collider.isTrigger = value;
=======
        /// The destructible integrity was changed.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        private void OnDestructibleIntegrityChanged(int current, int previous)
        {
            var damage = previous - current;
            Debug.Log($"{name} integrity took {damage} damage. (Currently {current}).");
>>>>>>> Stashed changes
        }

#endregion
    }
}