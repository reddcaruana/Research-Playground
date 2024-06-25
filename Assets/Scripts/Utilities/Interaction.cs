using UnityEngine;

namespace Game.Utilities
{
    public class Interaction
    {
        /// <summary>
        /// The object container.
        /// </summary>
        public Transform HandContainer { get; private set; }
        
        public Interaction(Transform handContainer)
        {
            HandContainer = handContainer;
        }
    }
}