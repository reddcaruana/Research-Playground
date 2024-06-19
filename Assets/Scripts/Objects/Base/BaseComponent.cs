using UnityEngine;

namespace Game.Objects
{
    public abstract class BaseComponent : MonoBehaviour
    {
        /// <summary>
        /// The owner object.
        /// </summary>
        public BaseBehavior Owner { get; private set; }

        // Component caching
        protected virtual void Awake()
        {
            Owner = GetComponentInParent<BaseBehavior>();
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
    
    public abstract class BaseProperty : BaseComponent
    { }
    
    public abstract class BaseState : BaseComponent
    { }
    
    public abstract class BaseTrigger : BaseComponent
    { }
}