using UnityEngine;

namespace Game.World
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

#region Queries

        public struct Query : IQuery, ISource, IDirection
        {
            public Vector3 Source { get; set; }
            public float Distance { get; set; }
            public Vector3 Direction { get; set; }

            public Vector3 GetPoint()
            {
                return Source + Direction * Distance;
            }
        }

#endregion

#region Messages

        public struct Result : IMessage
        {
            public BaseInteractable Target { get; set; }
        }

#endregion
    }
}