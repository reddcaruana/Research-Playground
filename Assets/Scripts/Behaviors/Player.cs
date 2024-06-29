using Game.Components;
using Game.Input;
using Game.Map;
using Game.Objects;
using Game.Queries;
using Game.Utilities;
using UnityEngine;

namespace Game.Behaviors
{
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
            _updater = GetComponentInChildren<MarkerUpdater>();
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
            Messenger.Current.Unsubscribe<PlayerControls.Attack>(OnAttack);
        }

        // Subscription handling
        private void OnEnable()
        {
            Messenger.Current.Subscribe<PlayerControls.Move>(OnMove);
            Messenger.Current.Subscribe<PlayerControls.Interact>(OnInteract);
            Messenger.Current.Subscribe<PlayerControls.Attack>(OnAttack);
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
        /// Handles attacking.
        /// </summary>
        /// <param name="message">The attack message data.</param>
        private void OnAttack(PlayerControls.Attack message)
        {
<<<<<<< Updated upstream
            var result = Messenger.Current.Query<MapData.CellQuery, MapData.CellResult>(new MapData.CellQuery
            {
                Source = transform.position,
                Distance = _updater.Distance,
                Direction = transform.forward
            });
            
            // There is no object
            if (!result.Target || result.Target is not BaseObject baseObject)
=======
            // Get the cell contents
            var result = Messenger.Current.Query<GridQueries.GetCellContentsQuery, GridQueries.GetCellContentsResult>(
                new GridQueries.GetCellContentsQuery
                {
                    Source = transform.position,
                    Distance = _updater.Distance,
                    Direction = transform.forward
                });

            // There is no object
            if (result.Contents is not BaseObject baseObject)
>>>>>>> Stashed changes
            {
                return;
            }
            
<<<<<<< Updated upstream
            // Handle damage
            var destructible = baseObject.Get<Destructible>();
            destructible?.Damage(1);
=======
            // If the object should take damage, apply it.
            if (baseObject.TryGet<Destructible>(out var destructible))
            {
                destructible.Damage(1);
            }
>>>>>>> Stashed changes
        }
        
        /// <summary>
        /// Handles interaction.
        /// </summary>
        /// <param name="message">The interaction message data.</param>
        private void OnInteract(PlayerControls.Interact message)
        {
<<<<<<< Updated upstream
            // We're already holding something
            if (_interaction.IsFull)
            {
                _interaction.Drop();
                return;
            }
            
            // Get the interaction result
            var result = Messenger.Current.Query<MapData.CellQuery, MapData.CellResult>(new MapData.CellQuery
            {
                Source = transform.position,
                Distance = _updater.Distance,
                Direction = transform.forward
            });
            
            // There is no usable object
            if (!result.Target || result.Target is not BaseObject baseObject)
            {
                return;
            }
            
            // Handle interactions
            var interactable = baseObject.GetInteractable();
            if (interactable != null)
            {
                interactable.Interact();
            }
            
            // Handle pickups
            var pickable = baseObject.Get<Pickable>();
            if (pickable != null)
            {
                _interaction.Hold(pickable);
            }
=======

>>>>>>> Stashed changes
        }

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