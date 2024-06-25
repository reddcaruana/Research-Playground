using Game.Atoms;
using UnityEngine;

namespace Game.Objects
{
    [RequireComponent(typeof(BoolEvent))]
    public class DoorObject : MonoBehaviour
    {
        // The physics components
        private Collider _collider;
        
        // Atomic components
        private BoolEvent _stateEvent;

        // Component caching
        private void Awake()
        {
            // Get the physics components
            _collider = GetComponent<Collider>();
            
            // Get the atomic components
            _stateEvent = GetComponent<BoolEvent>();
        }

        private void OnDisable()
        {
            _stateEvent.Unregister(OnStateChanged);
        }

        private void OnEnable()
        {
            _stateEvent.Register(OnStateChanged);
        }

        private void OnStateChanged(bool value)
        {
            _collider.isTrigger = value;
        }
    }
}