using Game.Components;
using Game.Queries;
using UnityEngine;

namespace Game.Objects
{
    public abstract class BaseMovable : BaseObject
    {
// #region Unity Events
//
//         // Unregister all actions
//         protected virtual void OnDisable()
//         {
//             // Pickable
//             var pickable = Get<Pickable>();
//             if (pickable)
//             {
//                 pickable.OnStateChanged -= PickUpHandler;
//             }
//         }
//
//         // Register all actions
//         protected virtual void OnEnable()
//         {
//             // Pickable
//             var pickable = Get<Pickable>();
//             if (pickable)
//             {
//                 pickable.OnStateChanged += PickUpHandler;
//             }
//         }
//
// #endregion
//         
// #region Action Handling
//
//         /// <summary>
//         /// Handles the pickup event.
//         /// </summary>
//         /// <param name="pickupState">The pickup state value.</param>
//         protected virtual void PickUpHandler(bool pickupState)
//         {
//             // If the state is true
//             if (pickupState)
//             {
//                 transform.localPosition = Vector3.zero;
//                 transform.localRotation = Quaternion.identity;
//                 return;
//             }
//
//             // var result = Messenger.Current.Query<MapQueries.CellQuery, MapQueries.CellResult>(new MapQueries.CellQuery
//             // {
//             //     Source = transform.position,
//             // });
//             //
//             // transform.position = result.CellPosition;
//             // transform.rotation = Quaternion.identity;
//         }
//
// #endregion
    }
}