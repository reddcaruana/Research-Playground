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
            Messenger.Current.Unsubscribe<GridQueries.UnregisterContents>(UnregisterContents);
            
            Messenger.Current.Unsubscribe<GridQueries.GetCellContentsQuery, GridQueries.GetCellContentsResult>(GetCellContents);
        }
        
        // Event Coupling
        private void OnEnable()
        {
            // Grid Queries
            Messenger.Current.Subscribe<GridQueries.RegisterContents>(RegisterContents);
            Messenger.Current.Subscribe<GridQueries.UnregisterContents>(UnregisterContents);
            
            Messenger.Current.Subscribe<GridQueries.GetCellContentsQuery, GridQueries.GetCellContentsResult>(GetCellContents);
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

        /// <summary>
        /// Unregisters content from a cell.
        /// </summary>
        /// <param name="message">The deregistration parameters.</param>
        private void UnregisterContents(GridQueries.UnregisterContents message)
        {
            // Prepare the key information
            var cell = MainGrid.WorldToCell(message.Position);
            var key = (cell.x, cell.z);
            
            // There are no contents
            if (!gridContents.TryGetValue(key, out var contents))
            {
                throw new System.Exception($"Cell ({key.x}, {key.z}) has no contents.");
            }
            
            // The contents match
            if (contents == message.Contents)
            {
                gridContents[key] = null;
                return;
            }
            
            // The object is not stackable
            if (!contents.TryGet<Stackable>(out var stackable))
            {
                throw new System.Exception(
                    $"Object in cell ({key.x}, {key.z}) is not the same as the requested.");
            }
            
            // Find the object within the stack
            var child = stackable.Child;
            while (child != null)
            {
                // The child is the same as requested object
                if (child == message.Contents)
                {
                    stackable.Unstack();
                    return;
                }
                
                // The child is not stackable
                if (!child.TryGet<Stackable>(out var subStackable))
                {
                    throw new System.Exception($"Cell ({key.x}, {key.z}) has no contents.");
                }

                // Loop again
                child = subStackable.Child;
            }
            
            // The object could not be found
            throw new System.Exception(
                $"Object in cell ({key.x}, {key.z}) is not the same as the requested.");
        }

#endregion
    }
}