using Game.Components;
using UnityEngine;

namespace Game.Objects
{
    public class Lever : BaseObject
    {
#region Unity Events

        // // Unregister all actions
        // private void OnDisable()
        // {
        //     // Trigger
        //     var trigger = Get<Trigger>();
        //     if (trigger)
        //     {
        //         trigger.OnChange -= TriggerHandler;
        //     }
        // }
        //
        // // Register all actions
        // private void OnEnable()
        // {
        //     // Trigger
        //     var trigger = Get<Trigger>();
        //     if (trigger)
        //     {
        //         trigger.OnChange += TriggerHandler;
        //     }
        // }

#endregion

#region Action Handling

        // private void TriggerHandler(bool value)
        // {
        //     Debug.Log($"Lever value is {value}");
        // }

#endregion
    }
}