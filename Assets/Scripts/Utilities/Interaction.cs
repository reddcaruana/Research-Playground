using Game.Components;
using UnityEngine;

namespace Game.Utilities
{
    public class Interaction
    {
        /// <summary>
        /// The object container.
        /// </summary>
        public Transform HandContainer { get; private set; }

        /// <summary>
        /// The hand contents.
        /// </summary>
        // public IPickable Contents { get; private set; }

        /// <summary>
        /// True if the contents are assigned.
        /// </summary>
        // public bool IsFull => Contents != null;
        
        public Interaction(Transform handContainer)
        {
            HandContainer = handContainer;
        }

        // public bool Hold(IPickable pickable)
        // {
        //     if (Contents != null)
        //     {
        //         return false;
        //     }
        //
        //     Contents = pickable;
        //     pickable.PickUp(HandContainer);
        // return true;
        // }

        // public void Drop()
        // {
        //     if (Contents == null)
        //     {
        //         return;
        //     }
        //
        //     Contents.Drop();
        //     Contents = null;
        // }
    }
}