using UnityEngine;

namespace Game.World
{
    [RequireComponent(typeof(BooleanState))]
    public class DoorObject : BaseObject
    {
        private void OnDisable()
        {
            var booleanState = Get<BooleanState>();
            booleanState.OnChange -= OnBooleanStateChanged;
        }

        private void OnEnable()
        {
            var booleanState = Get<BooleanState>();
            booleanState.OnChange += OnBooleanStateChanged;
        }

        public void OnBooleanStateChanged(bool value)
        {
            gameObject.SetActive(!value);
        }
    }
}