using UnityEngine;

namespace Game
{
    public abstract class MonoSingleton<T> : MonoBehaviour
        where T : MonoBehaviour
    {
        /// <summary>
        /// The object instance.
        /// </summary>
        public static T Instance { get; private set; }
        
        /// <summary>
        /// Instance setup.
        /// </summary>
        protected virtual void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                return;
            }
            
            Destroy(gameObject);
        }

        /// <summary>
        /// Instance cleanup.
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}
