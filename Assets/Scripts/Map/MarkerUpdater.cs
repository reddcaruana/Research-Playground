using UnityEngine;

namespace Game.Map
{
    public class MarkerUpdater : MonoBehaviour
    {
        [SerializeField] private float distance = 1f;

#region Unity Events

        private void FixedUpdate()
        {
            if (!isActiveAndEnabled)
            {
                return;
            }
            
            Messenger.Current.Publish(new Move
            {
                Source = transform.position,
                Direction = transform.forward,
                Distance = distance
            });
        }

#endregion

#region Nested Types

        public struct Move : IMessage
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