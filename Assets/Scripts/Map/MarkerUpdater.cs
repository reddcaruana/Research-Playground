using Game.Queries;
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

            Messenger.Current.Publish(new MarkerQueries.Place
            {
                Source = transform.position,
                Direction = transform.forward,
                Distance = Distance
            });
        }

#endregion
    }
}