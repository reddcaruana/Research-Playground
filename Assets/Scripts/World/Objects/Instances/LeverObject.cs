using UnityEngine;

namespace Game.World
{
    [RequireComponent(typeof(BooleanState))]
    public class LeverObject : BaseObject
    {
        [SerializeField] private Transform handleObject;
        
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
            var angle = value ? 45f : 0;
            handleObject.rotation = Quaternion.Euler(angle, 0, 0);
        }
    }
}