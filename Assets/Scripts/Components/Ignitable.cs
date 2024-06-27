using System.Collections;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Burnable))]
    public class Ignitable : MonoBehaviour
    {
        [SerializeField] private float maxDurability = 10f;
        [SerializeField] private float replenishRate = 1f;

        /// <summary>
        /// The current durability value.
        /// </summary>
        public float Durability { get; private set; }

        /// <summary>
        /// Determines if this object is burning.
        /// </summary>
        public bool IsIgniting { get; private set; }
        
        // Components
        private Burnable _burnable;
        
        // Coroutine references
        private Coroutine _activeCoroutine;

#region Unity Events

        // Component caching
        private void Awake()
        {
            _burnable = GetComponent<Burnable>();
        }
        
        // Variable setup
        private void Start()
        {
            Durability = maxDurability;
        }

#endregion

#region Methods

        /// <summary>
        /// Starts the ignition process.
        /// </summary>
        public void Ignite()
        {
            // We're already igniting
            if (IsIgniting)
            {
                return;
            }

            // Clear the active coroutine
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
            }

            _activeCoroutine = StartCoroutine(IgniteCoroutine());
        }

        /// <summary>
        /// Starts the replenishing process.
        /// </summary>
        public void Extinguish()
        {
            // We're not igniting
            if (!IsIgniting)
            {
                return;
            }
            
            // Clear the active coroutine
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
            }

            _activeCoroutine = StartCoroutine(ReplenishCoroutine());
        }

#endregion

#region Coroutines

        /// <summary>
        /// The ignition process.
        /// </summary>
        private IEnumerator IgniteCoroutine()
        {
            IsIgniting = true;
            
            while (IsIgniting)
            {
                Durability -= Time.deltaTime;
                if (Durability <= 0)
                {
                    _burnable.Burn();
                    yield break;
                }

                yield return null;
            }
        }

        /// <summary>
        /// The replenishment process.
        /// </summary>
        private IEnumerator ReplenishCoroutine()
        {
            IsIgniting = false;
            
            while (!IsIgniting)
            {
                Durability += replenishRate * Time.deltaTime;
                if (Durability >= maxDurability)
                {
                    Durability = maxDurability;
                    yield break;
                }

                yield return null;
            }
        }

#endregion
    }
}