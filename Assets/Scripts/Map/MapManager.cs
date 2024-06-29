using System.Collections.Generic;
using Game.Objects;
using Game.Queries;
using UnityEngine;

namespace Game.Map
{
    [RequireComponent(typeof(Grid))]
    public class MapManager : MonoSingleton<MapManager>
    {
        [SerializeField] private GameObject markerObject;
        
        // The grid contents
        private readonly Dictionary<(int x, int z), List<BaseObject>> _gridContents = new();
        
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
            // Marker
            Messenger.Current.Unsubscribe<MarkerQueries.Place>(PlaceMarker);
            
            // Contents
            Messenger.Current.Unsubscribe<MapQueries.AddContents<BaseObject>>(AddContents);
        }

        // Subscribes to the event listeners
        private void OnEnable()
        {
            // Marker
            Messenger.Current.Subscribe<MarkerQueries.Place>(PlaceMarker);
            
            // Contents
            Messenger.Current.Subscribe<MapQueries.AddContents<BaseObject>>(AddContents);
        }

#endregion

#region Methods

        /// <summary>
        /// Converts a position into a cell position tuple.
        /// </summary>
        /// <param name="position">The world position.</param>
        private (int x, int z) GetCell(Vector3 position)
        {
            var cell = MainGrid.WorldToCell(position);
            return (cell.x, cell.z);
        }

#endregion

#region Queries
        
        /// <summary>
        /// Returns the contents of a cell.
        /// </summary>
        /// <param name="query">The query data.</param>
        /// <returns></returns>
        private MapQueries.CellContents<BaseObject> GetCellContents(MapQueries.GetCellContents query)
        {
            // Prepare the data
            var data = new MapQueries.CellContents<BaseObject>();
            var cell = GetCell(query.GetPoint());
            
            // There is no information
            if (!_gridContents.TryGetValue(cell, out var contents))
            {
                return data;
            }

            // Set the contents
            data.Contents = contents;
            return data;
        }

#endregion

#region Messages

        /// <summary>
        /// Adds contents to a cell.
        /// </summary>
        /// <param name="message">The message value.</param>
        private void AddContents(MapQueries.AddContents<BaseObject> message)
        {
            // Find the Map Coordinate
            var cell = GetCell(message.Position);

            // No grid contents
            if (!_gridContents.ContainsKey(cell))
            {
                _gridContents[cell] = new List<BaseObject>();
            }
            
            // Add the instance
            _gridContents[cell].Add(message.Instance);
        }
        
        /// <summary>
        /// Places the map marker over a cell.
        /// </summary>
        /// <param name="message">The message.</param>
        private void PlaceMarker(MarkerQueries.Place message)
        {
            // Find the cell position
            var cell = MainGrid.WorldToCell(message.GetPoint());
            markerObject.transform.position = MainGrid.GetCellCenterWorld(cell);
        }
        
#endregion
    }
}