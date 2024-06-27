using System.Collections;
using UnityEngine;

namespace Game.Components
{
    public class Burnable : MonoBehaviour
    {
        private static readonly int BurnValueProperty = Shader.PropertyToID("_Burn_Value");
        
        [SerializeField] private float maxDurability = 100f;
        [SerializeField] private float burnThreshold = 0.1f;

        /// <summary>
        /// The current health value.
        /// </summary>
        public float Durability { get; private set; }
        
        /// <summary>
        /// The burning state.
        /// </summary>
        public bool IsBurning { get; private set; }
        
        // Coroutine references
        private Coroutine _activeCoroutine;
        
        // Materials
        private Material[] _materials;
        private BoxCollider _fireCollider;
        private FireSource _fireSource;

#region Unity Events

        // Component caching
        private void Awake()
        {
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

#endregion

#region Methods

        /// <summary>
        /// Starts the burning process.
        /// </summary>
        public void Burn()
        {
            // We're already burning
            if (IsBurning)
            {
                return;
            }

            // Stop any active coroutines
            if (_activeCoroutine != null)
            {
                StopCoroutine(_activeCoroutine);
            }

            _activeCoroutine = StartCoroutine(BurnCoroutine());
        }

        /// <summary>
        /// Adds the fire source components.
        /// </summary>
        private void CreateFireSource()
        {
            // Add the script
            _fireSource = gameObject.AddComponent<FireSource>();

            // Create the collider
            var activeCollider = GetComponent<BoxCollider>();
            _fireCollider = gameObject.AddComponent<BoxCollider>();
            _fireCollider.isTrigger = true;
            _fireCollider.center = activeCollider.center;
            _fireCollider.size = activeCollider.size * 2f;
        }

        /// <summary>
        /// Removes the fire source components.
        /// </summary>
        private void RemoveFireSource()
        {
            Destroy(_fireSource);
            Destroy(_fireCollider);
        }

        /// <summary>
        /// Updates the material values.
        /// </summary>
        private void UpdateMaterials()
        {
            var durabilityT = Durability / maxDurability;
            var t = 0f;
            
            if (durabilityT <= burnThreshold)
            {
                t = 1 - durabilityT / burnThreshold;
            }
            
            for (var i = 0; i < _materials.Length; i++)
            {
                _materials[i].SetFloat(BurnValueProperty, t);
            }
        }

#endregion

#region Coroutines

        /// <summary>
        /// The burning coroutine.
        /// </summary>
        private IEnumerator BurnCoroutine()
        {
            IsBurning = true;
            CreateFireSource();
            
            while (IsBurning)
            {
                Durability -= Time.deltaTime;
                UpdateMaterials();
                
                if (Durability <= 0)
                {
                    RemoveFireSource();
                    yield break;
                }

                yield return null;
            }
            
            RemoveFireSource();
        }

#endregion
    }
}