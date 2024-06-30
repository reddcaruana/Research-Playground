using UnityEngine;

namespace Game.Queries
{
    public static class MarkerQueries
    {
#region Messages

        public struct Update : ICellQuery
        {
            /// <inheritdoc />
            public Vector3 Source { get; set; }

            /// <inheritdoc />
            public float Distance { get; set; }

            /// <inheritdoc />
            public Vector3 Direction { get; set; }

            /// <inheritdoc />
            public Vector3 GetPoint()
            {
                return Source + Direction * Distance;
            }
        }

#endregion
    }
}