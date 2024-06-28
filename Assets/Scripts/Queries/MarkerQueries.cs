using UnityEngine;

namespace Game.Queries
{
    public static class MarkerQueries
    {
        public struct Place : IMessage, ISource, IDirection
        {
            /// <inheritdoc />
            public Vector3 Source { get; set; }

            /// <inheritdoc />
            public float Distance { get; set; }

            /// <inheritdoc />
            public Vector3 Direction { get; set; }

            public Vector3 GetPoint()
            {
                return Source + Direction * Distance;
            }
        }
    }
}