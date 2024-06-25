using Game.Components;
using Game.Input;
using Game.Map;
using Game.Utilities;
using UnityEngine;
using MapData = Game.Map.MapData;

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
            // Messages
            Messenger.Current.Unsubscribe<PlayerControls.Move>(OnMove);
            Messenger.Current.Unsubscribe<PlayerControls.Interact>(OnInteract);
        }

        // Subscription handling
        private void OnEnable()
        {
            Messenger.Current.Subscribe<PlayerControls.Move>(OnMove);
            Messenger.Current.Subscribe<PlayerControls.Interact>(OnInteract);
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

        /// <summary>
        /// Handles interaction.
        /// </summary>
        /// <param name="message">The interaction message data.</param>
        private void OnInteract(PlayerControls.Interact message)
        {
            // Get the interaction result
            var result = Messenger.Current.Query<MapData.CellQuery, MapData.CellResult>(new MapData.CellQuery
            {
                Source = transform.position,
                Distance = _updater.Distance,
                Direction = transform.forward
            });

            // Handle the interaction types
            switch (result.Target)
            {
                case Usable usable:
                {
                    usable.Use();
                    break;
                }
            }
        }

#endregion
    }
}