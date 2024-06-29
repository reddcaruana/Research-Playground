using Game.Components;
using UnityEngine;

namespace Game.Objects
{
    public class Door : BaseObject
    {
        [Header("References")]
        [SerializeField] private GameObject doorObject;

        // Components
        private Destructible destructible;

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
        {
            Destroy(gameObject);
        }
        
        /// <summary>
        /// The destructible integrity was changed.
        /// </summary>
        /// <param name="current"></param>
        /// <param name="previous"></param>
        private void OnDestructibleIntegrityChanged(int current, int previous)
        {
            var damage = previous - current;
            Debug.Log($"{name} integrity took {damage} damage. (Currently {current}).");
        }

#endregion
    }
}