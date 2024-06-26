using UnityEngine;

namespace Game.Queries
{
    public static class MapData
    {

#region Queries

        public struct CellQuery : IQuery, ISource, IDirection
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

        public struct CellResult : IMessage
        {
            public Vector3 CellCenter { get; set; }
            public Vector3 CellPosition { get; set; }
            public MonoBehaviour Target { get; set; }
        }

#endregion
        
    }
}