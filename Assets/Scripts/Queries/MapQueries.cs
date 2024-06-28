using System.Collections.Generic;
using UnityEngine;

namespace Game.Queries
{
    public static class MapQueries
    {
        // Get Cell Contents
        // Add Cell Contents
        
#region Queries
        
        public struct GetCellContents : IQuery
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

#region Messages

        public struct AddContents<T> : IMessage
            where T : MonoBehaviour
        {
            public Vector3 Position { get; set; }
            public T Instance { get; set; }
        }

        public struct CellContents<T> : IMessage
            where T : MonoBehaviour
        {
            public Vector3 CellPosition { get; set; }
            public List<T> Contents { get; set; }
        }

#endregion
        
    }
}