using UnityEngine;

namespace Game.World
{
    public abstract class BaseComponent : MonoBehaviour
    {
        /// <summary>
        /// The owner object.
        /// </summary>
        public BaseObject Owner { get; private set; }

        // Component caching
        protected virtual void Awake()
        {
            Owner = GetComponentInParent<BaseObject>();
            if (!Owner)
            {
                Debug.LogWarning($"{name} could not find owner behavior.");
            }
            
            Owner?.Add(this);
        }

        // Reference clearance
        protected virtual void OnDestroy()
        {
            Owner?.Remove(this);
        }
    }
}