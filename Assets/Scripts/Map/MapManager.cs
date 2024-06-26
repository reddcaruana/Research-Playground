using Game.Objects;
using Game.Queries;
using UnityEngine;

namespace Game.Map
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
            
            // Queries
            Messenger.Current.Unsubscribe<MapData.CellQuery, MapData.CellResult>(QueryCell);
        }

        // Subscribes to the event listeners
        private void OnEnable()
        {
            // Messages
            Messenger.Current.Subscribe<MarkerUpdater.Move>(MoveMarker);
            
            // Queries
            Messenger.Current.Subscribe<MapData.CellQuery, MapData.CellResult>(QueryCell);
        }

#endregion

#region Queries

        private MapData.CellResult QueryCell(MapData.CellQuery query)
        {
            const float offset = 0.05f;
            
            // Get the center of the cell
            var cell = MainGrid.WorldToCell(query.GetPoint());
            var center = MainGrid.GetCellCenterWorld(cell);
            
            // Set up the position
            var position = MainGrid.CellToWorld(cell);
            (position.x, position.z) = (center.x, center.z);
            
            // Set the box size
            var extents = (MainGrid.cellSize - Vector3.one * offset) * 0.5f;
            
            // Get the colliders
            var colliders = new Collider[16];
            var colliderCount = Physics.OverlapBoxNonAlloc(center, extents, colliders);
            
            // Prepare a result
            var result = new MapData.CellResult
            {
                CellCenter = center,
                CellPosition = position
            };
            
            // Get the first interactable object
            for (var i = 0; i < colliderCount; i++)
            {
                // There is no data
                if (!colliders[i].TryGetComponent<BaseObject>(out var target))
                {
                    continue;
                }

                result.Target = target;
                return result;
            }
            
            // Return the basic information
            return result;
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