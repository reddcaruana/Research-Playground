using UnityEngine;

namespace Game.Map
{
    public class MarkerUpdater : MonoBehaviour
    {
        [field: SerializeField] public float Distance { get; set; } = 1.25f;

#region Unity Events

        private void FixedUpdate()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            Messenger.Current.Publish(new Move
            {
                Source = transform.position + transform.up,
                Direction = transform.forward,
                Distance = Distance
            });
        }

#endregion

#region Nested Types

        public struct Move : IMessage, ISource, IDirection
        {
            public Vector3 Direction { get; set; }
            public float Distance { get; set; }
            public Vector3 Source { get; set; }

            public Vector3 GetPoint()
            {
                return Source + Direction * Distance;
            }
        }

#endregion
    }
}