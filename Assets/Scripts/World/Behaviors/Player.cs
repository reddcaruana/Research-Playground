using Game.Input;
using UnityEngine;

namespace Game.World
{
    [RequireComponent(typeof(MarkerUpdater))]
    public class Player : MonoBehaviour
    {
        [Header("Properties")]
        [SerializeField] private float linearSpeed = 5f;
        [SerializeField] private float angularSpeed = 90f;

        [Header("References")]
        [SerializeField] private Transform hand;
        
        // The attached components
        private Rigidbody _rigidbody;
        private MarkerUpdater _updater;
        
        // Utility classes
        private Movement _movement;
        private Interaction _interaction;

#region Unity Events

        // Component caching
        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
            _updater = GetComponent<MarkerUpdater>();
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
            
            _interaction = new Interaction(hand);
        }

#endregion

#region Subscriptions

        /// <summary>
        /// Handles movement.
        /// </summary>
        /// <param name="message">The move message data.</param>
        private void OnMove(PlayerControls.Move message)
        {
            _movement.Direction = message.Direction;
        }

#endregion
    }
}