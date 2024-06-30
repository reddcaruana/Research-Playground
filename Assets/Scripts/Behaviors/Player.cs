using Game.Components;
using Game.Input;
using Game.Interfaces;
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
        private new Rigidbody rigidbody;
        private MarkerUpdater updater;
        
        // Utility classes
        private Movement movement;
        private Interaction interaction;

#region Unity Events

        // Component caching
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            updater = GetComponentInChildren<MarkerUpdater>();
        }

        // Physics update
        private void FixedUpdate()
        {
            movement.Move(rigidbody);
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
            movement = new Movement(linearSpeed, angularSpeed);
            interaction = new Interaction(hand);
        }

#endregion

#region Subscriptions

        /// <summary>
        /// Handles attacking.
        /// </summary>
        /// <param name="message">The attack message data.</param>
        private void OnAttack(PlayerControls.Attack message)
        {
            // Get the cell contents
            var result = Messenger.Current.Query<GridQueries.GetCellContentsQuery, GridQueries.GetCellContentsResult>(
                new GridQueries.GetCellContentsQuery
                {
                    Source = transform.position,
                    Distance = updater.Distance,
                    Direction = transform.forward
                });

            // There is no object
            if (result.Contents is not BaseObject baseObject)
            {
                return;
            }
            
            // If the object should take damage, apply it
            if (baseObject.TryGet<Destructible>(out var destructible))
            {
                destructible.Damage(1);
            }
        }
        
        /// <summary>
        /// Handles interaction.
        /// </summary>
        /// <param name="message">The interaction message data.</param>
        private void OnInteract(PlayerControls.Interact message)
        {
            // Get the cell contents
            var result = Messenger.Current.Query<GridQueries.GetCellContentsQuery, GridQueries.GetCellContentsResult>(
                new GridQueries.GetCellContentsQuery
                {
                    Source = transform.position,
                    Distance = updater.Distance,
                    Direction = transform.forward
                });
            
            // There is no object
            if (result.Contents is not BaseObject baseObject)
            {
                return;
            }
            
            // Prioritize object actions
            // Carryable
            if (result.Contents.TryGet<Carryable>(out var carryable))
            {
                Debug.Log("This is a carryable object.");
                return;
            }
            
            // Wieldable
            if (result.Contents.TryGet<Wieldable>(out var wieldable))
            {
                Debug.Log("This is a wieldable object.");
                return;
            }
            
            // Interactables
            if (result.Contents.TryGet<IInteractable>(out var interactable))
            {
                interactable.Interact();
            }
        }

        /// <summary>
        /// Handles movement.
        /// </summary>
        /// <param name="message">The move message data.</param>
        private void OnMove(PlayerControls.Move message)
        {
            movement.Direction = message.Direction;
        }

#endregion
    }
}