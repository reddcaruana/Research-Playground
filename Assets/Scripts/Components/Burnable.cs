using System.Collections;
using UnityEngine;

namespace Game.Components
{
    public class Burnable : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;

        /// <summary>
        /// The current health value.
        /// </summary>
        public float CurrentHealth { get; private set; }
        
        /// <summary>
        /// The burning state.
        /// </summary>
        public bool IsBurning { get; private set; }
        
        // Coroutine references
        private Coroutine _activeCoroutine;

#region Unity Events

        private void Start()
        {
            CurrentHealth = maxHealth;
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

#endregion

#region Coroutines

        /// <summary>
        /// The burning coroutine.
        /// </summary>
        private IEnumerator BurnCoroutine()
        {
            IsBurning = true;
            
            while (IsBurning)
            {
                CurrentHealth -= Time.deltaTime;
                if (CurrentHealth <= 0)
                {
                    Debug.Log("Burned");
                    yield break;
                }
            }

            yield return null;
        }

#endregion
    }
}