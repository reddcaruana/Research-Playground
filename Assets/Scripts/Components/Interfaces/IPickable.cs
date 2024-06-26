using UnityEngine;

namespace Game.Components

{
    public interface IPickable : IObjectComponent
    {
        /// <summary>
        /// Drops the item.
        /// </summary>
        void Drop();
        
        /// <summary>
        /// Picks up the item.
        /// </summary>
        void PickUp(Transform holder);
    }
}