using UnityEngine;

namespace Game.Map
{
    [RequireComponent(typeof(Grid))]
    public class MapManager : MonoSingleton<MapManager>
    {
        [SerializeField] private GameObject previewDecal;
        
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
            Messenger.Current.Unsubscribe<MarkerUpdater.Move>(MoveMarker);
        }

        // Subscribes to the event listeners
        private void OnEnable()
        {
            Messenger.Current.Subscribe<MarkerUpdater.Move>(MoveMarker);
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
            previewDecal.transform.position = MainGrid.GetCellCenterWorld(cell);
        }

#endregion
    }
}