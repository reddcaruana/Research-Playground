using System;
using System.Collections;
using UnityEngine;

namespace Game.Components
{
    public class Burnable : MaterialUpdater
    {
        // The shader property ID
        private static readonly int BurnValuePropertyID = Shader.PropertyToID("_Burn_Value");
        
        // The integrity values
        [SerializeField] private float maxIntegrity = 50f;
        [SerializeField] private float burnThreshold = 0.1f;
        
        /// <summary>
        /// The current integrity value.
        /// </summary>
        public float Integrity { get; private set; }
        
        /// <summary>
        /// Confirms if the object is currently burning.
        /// </summary>
        public bool IsBurning { get; private set; }

        /// <summary>
        /// Fires when the object has burned.
        /// </summary>
        public event Action OnBurned;
        
        // Coroutines
        private Coroutine activeCoroutine;

#region Unity Events

        // Variable setup
        private void Start()
        {
            Integrity = maxIntegrity;
        }

#endregion
        
#region Methods

        public void Burn()
        {
            // Already burning
            if (IsBurning)
            {
                return;
            }
            
            // Stop any active coroutines
            if (activeCoroutine != null)
            {
                StopCoroutine(activeCoroutine);
            }

            activeCoroutine = StartCoroutine(BurnCoroutine());
        }
        
        /// <inheritdoc />
        protected override void UpdateMaterials()
        {
            // Normalized integrity
            var integrityT = Integrity / maxIntegrity;
            var burnT = 0f;

            // Set the burn value
            if (integrityT <= burnThreshold)
            {
                burnT = 1f - integrityT / burnThreshold;
            }

            for (var i = 0; i < Materials.Length; i++)
            {
                var material = Materials[i];
                
                // The float property doesn't exist[
                if (!material.HasFloat(BurnValuePropertyID))
                {
                    continue;
                }
                
                // Update the material
                material.SetFloat(BurnValuePropertyID, burnT);
            }
        }

#endregion

#region Coroutines

        private IEnumerator BurnCoroutine()
        {
            // Start burning
            IsBurning = true;

            while (IsBurning)
            {
                // Lower the integrity
                Integrity -= Time.deltaTime;
                UpdateMaterials();
                
                if (Integrity <= 0)
                {
                    Integrity = 0;
                    IsBurning = false;
                }
                
                yield return null;
            }
            
            // Invoke the burned event
            OnBurned?.Invoke();
            
            // Destroy the affecting components
            if (Owner.TryGet<FireSource>(out var fireSource))
            {
                Destroy(fireSource);
            }
            Destroy(this);
        }

#endregion
        
    }
}