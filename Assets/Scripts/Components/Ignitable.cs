using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Burnable))]
    public class Ignitable : MonoBehaviour
    {
        private static readonly int IgniteValueProperty = Shader.PropertyToID("_Ignite_Value");
        
        [SerializeField] private float maxDurability = 10f;
        [SerializeField] private float replenishRate = 1f;

        // The registered fire sources
        private readonly HashSet<FireSource> _fireSources = new();
        
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
        
        // Materials
        private Material[] _materials;
        
        // Coroutine references
        private Coroutine _activeCoroutine;

#region Unity Events

        // Component caching
        private void Awake()
        {
            _burnable = GetComponent<Burnable>();

            var renderers = GetComponentsInChildren<Renderer>();
            
            _materials = new Material[renderers.Length];
            for (var i = 0; i < renderers.Length; i++)
            {
                _materials[i] = renderers[i].material;
            }
        }
        
        // Variable setup
        private void Start()
        {
            Durability = maxDurability;
        }

        // Adds the fire source contact
        private void OnTriggerEnter(Collider other)
        {
            // There is no fire source
            if (!other.TryGetComponent<FireSource>(out var fireSource))
            {
                return;
            }

            // This fire source is registered
            if (!_fireSources.Add(fireSource))
            {
                return;
            }

            Ignite();
        }

        // Removes the fire source contact
        private void OnTriggerExit(Collider other)
        {
            Debug.Log("Lost object");
            // There is no fire source
            if (!other.TryGetComponent<FireSource>(out var fireSource))
            {
                return;
            }

            // This fire source is registered
            if (!_fireSources.Remove(fireSource))
            {
                return;
            }

            if (!_fireSources.Any())
            {
                Extinguish();
            }
        }

#endregion

#region Methods

        /// <summary>
        /// Starts the ignition process.
        /// </summary>
        private void Ignite()
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
        private void Extinguish()
        {
            // We're not igniting
            if (!IsIgniting || _burnable.IsBurning)
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

        /// <summary>
        /// Updates the material values.
        /// </summary>
        private void UpdateMaterials()
        {
            var t = 1 - (Durability / maxDurability);
            for (var i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetFloat(IgniteValueProperty, t);
            }
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
                UpdateMaterials();
                
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
                UpdateMaterials();
                
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