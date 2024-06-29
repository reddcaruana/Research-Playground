using Game.Interfaces;
using UnityEngine;

namespace Game.Queries
{
    public static class GridQueries
    {
#region Queries

        /// <summary>
        /// Used to query the contents of a cell.
        /// </summary>
        public struct GetCellContentsQuery : ICellQuery
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

        public struct GetCellContentsResult : IMessage
        {
            /// <summary>
            /// The Cell value.
            /// </summary>
            public Vector3Int Cell { get; set; }
            
            /// <summary>
            /// The cell position.
            /// </summary>
            public Vector3 Position { get; set; }
            
            /// <summary>
            /// The cell contents.
            /// </summary>
            public IObject Contents { get; set; }
        }

        public struct RegisterContents : IMessage
        {
            /// <summary>
            /// The object's world position.
            /// </summary>
            public Vector3 Position { get; set; }

            /// <summary>
            /// The object instanced.
            /// </summary>
            public IObject Contents { get; set; }
        }

        public struct UnregisterContents : IMessage
        {
            /// <summary>
            /// The object's world position.
            /// </summary>
            public Vector3 Position { get; set; }

            /// <summary>
            /// The object instanced.
            /// </summary>
            public IObject Contents { get; set; }
        }

#endregion
    }
}