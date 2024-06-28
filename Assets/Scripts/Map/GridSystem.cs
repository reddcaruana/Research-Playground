using System.Collections.Generic;
using Game.Components;
using Game.Interfaces;
using Game.Queries;
using UnityEngine;

namespace Game.Map
{
    [RequireComponent(typeof(Grid))]
    public class GridSystem : MonoBehaviour
    {
        // The contents of this grid
        // NOTE: If a BaseObject is stackable, the child information will be
        //       stored inside an attached component
        private readonly Dictionary<(int x, int z), IObject> gridContents = new();

        /// <summary>
        /// The grid on which all calculations are made.
        /// </summary>
        public Grid MainGrid { get; private set; }

#region Unity Events

        // Component caching
        private void Awake()
        {
            MainGrid = GetComponent<Grid>();
        }
        
        // Event Decoupling
        private void OnDisable()
        {
            // Grid Queries
            Messenger.Current.Unsubscribe<GridQueries.RegisterContents>(RegisterContents);
        }
        
        // Event Coupling
        private void OnEnable()
        {
            // Grid Queries
            Messenger.Current.Subscribe<GridQueries.RegisterContents>(RegisterContents);
        }

#endregion

#region Queries

        /// <summary>
        /// Queries a cell and returns its contents.
        /// </summary>
        /// <param name="query">The query parameters.</param>
        private GridQueries.GetCellContentsResult GetCellContents(GridQueries.GetCellContentsQuery query)
        {
            // Prepare the coordinates
            var cell = MainGrid.WorldToCell(query.GetPoint());
            var position = MainGrid.CellToWorld(cell);
            
            // Generate the basic data
            var result = new GridQueries.GetCellContentsResult
            {
                Cell = cell,
                Position = position
            };

            // Get the dictionary entry
            var key = (cell.x, cell.z);
            
            // There are no contents
            if (!gridContents.TryGetValue(key, out var contents))
            {
                return result;
            }

            // Populate the contents
            result.Contents = contents;
            return result;
        }

#endregion

#region Messages

        /// <summary>
        /// Registers contents to a cell.
        /// </summary>
        /// <param name="message">The registration parameters.</param>
        private void RegisterContents(GridQueries.RegisterContents message)
        {
            // Prepare the key information
            var cell = MainGrid.WorldToCell(message.Position);
            var key = (cell.x, cell.z);
            
            // There is already a key here
            if (gridContents.TryGetValue(key, out var contents))
            {
                // Check if the object can be stacked
                var stackable = contents.Get<Stackable>();
                if (!stackable)
                {
                   throw new System.Exception($"Object already exists on cell ({key.x}, {key.z}).");
                }
                
                // Stacking was unsuccessful
                if (!stackable.Stack(message.Contents))
                {
                    throw new System.Exception(
                        $"Object already exists and cannot be stacked further on cell ({key.x}, {key.z}).");
                }

                // Object was stacked
                return;
            }

            // Register the cell contents
            gridContents[key] = message.Contents;
        }

#endregion
    }
}