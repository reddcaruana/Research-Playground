using UnityEngine;

namespace Game.Atoms
{
    public abstract class BaseVariable<TValue, TPair, TEvent1, TEvent2> : BaseAtom
        where TPair : struct, IPair<TValue>
        where TEvent1 : BaseEvent<TValue>
        where TEvent2 : BaseEvent<TPair>
    {
        // The initial value of this variable
        [SerializeField] private TValue initialValue;

        // The variable value
        private TValue _value;

        // The previous value
        private TValue _oldValue;

        // Event triggered when the value changes
        private TEvent1 _changed;

        // Event triggered when the value changes
        private TEvent2 _changedWithHistory;
        
        /// <summary>
        /// The variable value.
        /// </summary>
        public TValue Value
        {
            get => _value;
            set => SetValue(value);
        }

        // Component creation
        protected virtual void Awake()
        {
            _changed = GetOrCreateEvent<TEvent1>();
            _changedWithHistory = GetOrCreateEvent<TEvent2>();
        }

        // Set the initial value
        private void Start()
        {
            SetInitialValues();
        }

        /// <summary>
        /// Updates the variable's value.
        /// </summary>
        /// <param name="newValue">The new value.</param>
        private void SetValue(TValue newValue)
        {
            // Store the old value
            _oldValue = _value;
            
            // Change the value
            _value = newValue;
            
            // Emit the changed event
            _changed?.Emit(_value);
            
            // Emit the changed with history event
            var pair = default(TPair);
            pair.Item1 = _value;
            pair.Item2 = _oldValue;
            _changedWithHistory.Emit(pair);
        }

        /// <summary>
        /// Set the initial values.
        /// </summary>
        private void SetInitialValues()
        {
            _oldValue = initialValue;
            _value = initialValue;
        }

        /// <summary>
        /// Get the event by type, or create the event component.
        /// </summary>
        /// <returns></returns>
        private TEvent GetOrCreateEvent<TEvent>()
            where TEvent : BaseAtom
        {
            // Create the component if it doesn't exist
            if (!TryGetComponent<TEvent>(out var eventComponent))
            {
                return gameObject.AddComponent<TEvent>();
            }

            // Return the collected component
            return eventComponent;
        }
    }
}