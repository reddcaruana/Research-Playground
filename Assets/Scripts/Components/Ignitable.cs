using System.Collections;
using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

namespace Game.Components
{
    public class Ignitable : MaterialUpdater, IIgnitable
    {
        // The shader property ID
        private static readonly int IgniteValuePropertyID = Shader.PropertyToID("_Ignite_Value");

        // The durability values
        [SerializeField] private float maxDurability = 10f;
        [SerializeField] private float replenishRate = 1f;

        // The registered fire sources
        private readonly HashSet<IFireSource> fireSources = new();

        /// <summary>
        /// The current durability value.
        /// </summary>
        public float Durability { get; private set; }

        /// <summary>
        /// Determines if the object is in range of fire and igniting.
        /// </summary>
        public bool InRangeOfFire => fireSources.Count > 0;

        // Determines if the object is burning
        private bool isBurning;
        
        // The active coroutine
        private Coroutine activeCoroutine;

#region Unity Events

        // Variable setup
        private void Start()
        {
            Durability = maxDurability;
            
            // Check for fire sources
            if (Owner.TryGet<FireSource>(out _))
            {
                Debug.LogError("Ignitable objects cannot also be a fire source!");
            }
        }

#endregion

#region Methods

        /// <inheritdoc />
        public void OnContact(IFireSource fireSource)
        {
            // The fire source already exists
            if (!fireSources.Add(fireSource))
            {
                return;
            }

            // The object is already burning
            if (isBurning)
            {
                return;
            }
            
            // Stop the active coroutine
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }

            // Start the burning routine
            activeCoroutine = StartCoroutine(BurnCoroutine());
        }

        /// <inheritdoc />
        public void OnSeparate(IFireSource fireSource)
        {
            // The fire source is not recorded
            if (!fireSources.Contains(fireSource))
            {
                return;
            }

            // Remove the fire source
            fireSources.Remove(fireSource);

            // There is more than one fire source still
            if (fireSources.Count > 0)
            {
                return;
            }

            // The object is not burning
            if (!isBurning)
            {
                return;
            }
            
            // The component is being destroyed
            if (IsBeingDestroyed)
            {
                return;
            }

            // Stop the active coroutine
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }

            // Start the replenish routine
            activeCoroutine = StartCoroutine(ReplenishRoutine());
        }

        /// <inheritdoc />
        protected override void UpdateMaterials()
        {
            var t = Mathf.Clamp01(1 - Durability / maxDurability);
            for (var i = 0; i < Materials.Length; i++)
            {
                var material = Materials[i];

                // The float property doesn't exist
                if (!material.HasFloat(IgniteValuePropertyID))
                {
                    continue;
                }

                // Update the material
                material.SetFloat(IgniteValuePropertyID, t);
            }
        }

#endregion

#region Coroutines

        /// <summary>
        /// The coroutine handling ignition.
        /// </summary>
        private IEnumerator BurnCoroutine()
        {
            isBurning = true;
            
            while (isBurning)
            {
                Durability -= Time.deltaTime;
                UpdateMaterials();
                
                if (Durability <= 0)
                {
                    // No longer burning
                    isBurning = false;
                    
                    // Add a fire source
                    var fireSource = gameObject.AddComponent<FireSource>();
                    Owner.Add(fireSource);
                    
                    // Destroy this component
                    Destroy(this);
                }

                yield return null;
            }
        }

        /// <summary>
        /// The coroutine handling replenishment.
        /// </summary>
        private IEnumerator ReplenishRoutine()
        {
            isBurning = false;
            
            while (Durability < maxDurability)
            {
                Durability += Time.deltaTime * replenishRate;
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