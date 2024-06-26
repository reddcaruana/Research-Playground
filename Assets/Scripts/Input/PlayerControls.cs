using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Input
{
    [RequireComponent(typeof(PlayerInput))]
    public class PlayerControls : MonoBehaviour
    {
        // The attached components
        private PlayerInput _playerInput;
        
        // The Input Action references
        private InputAction _moveAction;
        private InputAction _interactAction;
        private InputAction _attackAction;
        
#region Unity Events

        // Component caching
        private void Awake()
        {
            _playerInput = GetComponent<PlayerInput>();
        }

        // Release the controls when the script is disabled
        private void OnDisable()
        {
            Release();
        }
        
        // Bind the controls when the script is enabled
        private void OnEnable()
        {
            Bind();
        }

#endregion

#region Input Management

        /// <summary>
        /// Binds the input actions.
        /// </summary>
        private void Bind()
        {
            // Input setup
            _playerInput.notificationBehavior = PlayerNotifications.InvokeCSharpEvents;
            _playerInput.SwitchCurrentActionMap("Player");
            
            // Move action
            _moveAction = _playerInput.currentActionMap.FindAction("Move");
            _moveAction.started += MoveActionHandler;
            _moveAction.performed += MoveActionHandler;
            _moveAction.canceled += MoveActionHandler;

            // Attack action
            _attackAction = _playerInput.currentActionMap.FindAction("Attack");
            _attackAction.started += AttackActionHandler;

            // Interact action
            _interactAction = _playerInput.currentActionMap.FindAction("Interact");
            _interactAction.started += InteractActionHandler;
        }

        /// <summary>
        /// Releases the input actions.
        /// </summary>
        private void Release()
        {
            // Clear the Move action
            if (_moveAction != null)
            {
                _moveAction.started -= MoveActionHandler;
                _moveAction.performed -= MoveActionHandler;
                _moveAction.canceled -= MoveActionHandler;
                _moveAction = null;
            }

            // Clear the Attack action
            if (_attackAction != null)
            {
                _attackAction.started -= AttackActionHandler;
                _attackAction = null;
            }
            
            // Clear the Interact action
            if (_interactAction != null)
            {
                _interactAction.started -= InteractActionHandler;
                _interactAction = null;
            }
        }

#endregion

#region Input Event Handling

        /// <summary>
        /// Handles the attack action.
        /// </summary>
        private void AttackActionHandler(InputAction.CallbackContext ctx)
        {
            if (ctx.phase is not InputActionPhase.Started)
            {
                return;
            }
            
            Messenger.Current.Publish<Attack>();
        }

        /// <summary>
        /// Handles the interact action.
        /// </summary>
        private void InteractActionHandler(InputAction.CallbackContext ctx)
        {
            if (ctx.phase is not InputActionPhase.Started)
            {
                return;
            }
            
            Messenger.Current.Publish<Interact>();
        }

        /// <summary>
        /// Handles the move action.
        /// </summary>
        private void MoveActionHandler(InputAction.CallbackContext ctx)
        {
            var value = ctx.ReadValue<Vector2>();
            Messenger.Current.Publish(new Move
            {
                Direction = new Vector3(value.x, 0, value.y)
            });
        }

#endregion

#region Messages

        public struct Move : IMessage
        {
            public Vector3 Direction { get; set; }
        }

        public struct Interact : IMessage
        { }
        
        public struct Attack : IMessage
        { }

#endregion
    }
}