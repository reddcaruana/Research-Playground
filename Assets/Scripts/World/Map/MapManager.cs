using UnityEngine;

namespace Game.World
{
    [RequireComponent(typeof(Grid))]
    public class MapManager : MonoSingleton<MapManager>
    {
        [SerializeField] private GameObject markerObject;
        
        /// <summary>
        /// The grid controlling all coordinates.
        /// </summary>
        public Grid MainGrid { get; private set; }

#region Unity Events

        /// <inheritdoc />
        protected override void Awake()
        {
            base.Awake();
            MainGrid = GetComponent<Grid>();
        }

        // Unsubscribes from the event listeners
        private void OnDisable()
        {
            // Messages
            Messenger.Current.Unsubscribe<MarkerUpdater.Move>(MoveMarker);
        }

        // Subscribes to the event listeners
        private void OnEnable()
        {
            // Messages
            Messenger.Current.Subscribe<MarkerUpdater.Move>(MoveMarker);
            
            // Queries
            Messenger.Current.Subscribe<Interaction.Query, Interaction.Result>(QueryCell);
        }

#endregion

#region Queries

        private Interaction.Result QueryCell(Interaction.Query query)
        {
            const float offset = 0.05f;
            
            // Get the center of the cell
            var cell = MainGrid.WorldToCell(query.GetPoint());
            var center = MainGrid.GetCellCenterWorld(cell);
            
            // Set the box size
            var extents = (MainGrid.cellSize - Vector3.one * offset) * 0.5f;
            
            // Get the colliders
            var colliders = new Collider[16];
            var colliderCount = Physics.OverlapBoxNonAlloc(center, extents, colliders);

            // Get the first interactable object
            for (var i = 0; i < colliderCount; i++)
            {
                if (colliders[i].TryGetComponent<BaseInteractable>(out var interactable))
                {
                    return new Interaction.Result
                    {
                        Target = interactable
                    };
                }
            }
            
            // Return null
            return new Interaction.Result
            {
                Target = null
            };
        }

#endregion

#region Messages

        /// <summary>
        /// Moves the map marker.
        /// </summary>
        /// <param name="message">The message.</param>
        private void MoveMarker(MarkerUpdater.Move message)
        {
            // Find the cell position
            var cell = MainGrid.WorldToCell(message.GetPoint());
            markerObject.transform.position = MainGrid.GetCellCenterWorld(cell);
        }
        
#endregion
    }
}