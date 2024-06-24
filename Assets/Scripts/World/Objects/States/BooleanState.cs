namespace Game.World
{
    public class BooleanState : BaseState
    {
        /// <summary>
        /// The value of this state.
        /// </summary>
        public bool Value { get; private set; }

        /// <summary>
        /// Toggles the state value.
        /// </summary>
        public void Toggle()
        {
            Value = !Value;
        }
    }
}