using System.Linq;
using UnityEngine;

namespace Game.World
{
    [RequireComponent(typeof(BooleanState))]
    public class UsableProperty : BaseInteractable
    {
        // The associated components
        private BooleanState _state;
        
        protected void Start()
        {
            _state = Owner?.Get<BooleanState>();
        }

        /// <summary>
        /// Toggles the attached state.
        /// </summary>
        public void Use()
        {
            _state.Toggle();
        }
    }
}