using Game.Input;
using Game.Utilities;
using UnityEngine;

namespace Game.Behaviors
{
    public class Player : MonoBehaviour
    {
        [SerializeField] private float linearSpeed = 5f;
        [SerializeField] private float angularSpeed = 90f;
        
        // The attached components
        private Rigidbody _rigidbody;
        
        // Utility classes
        private Movement _movement;

#region Unity Events

        // Component caching
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        // Physics update
        private void FixedUpdate()
        {
            _movement.Move(_rigidbody);
        }

        // Subscription handling
        private void OnDisable()
        {
            Messenger.Current.Unsubscribe<PlayerControls.Move>(OnMove);
        }

        // Subscription handling
        private void OnEnable()
        {
            Messenger.Current.Subscribe<PlayerControls.Move>(OnMove);
        }

        // Setup
        private void Start()
        {
            _movement = new Movement(linearSpeed, angularSpeed);
        }

#endregion

#region Subscriptions

        /// <summary>
        /// Handles movement.
        /// </summary>
        /// <param name="message">The message data.</param>
        private void OnMove(PlayerControls.Move message)
        {
            _movement.Direction = message.Direction;
        }

#endregion
    }
}