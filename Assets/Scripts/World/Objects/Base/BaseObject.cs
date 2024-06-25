using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Game.World
{
    public abstract class BaseObject : MonoBehaviour
    {
        // The coroutines for each property
        private readonly Dictionary<System.Type, Coroutine> _coroutines = new();

        // The list of attached components
        private readonly HashSet<BaseComponent> _components = new();

        /// <summary>
        /// The object's database ID.
        /// </summary>
        public int ID { get; private set; }

#region Unity Events

        // Handles object registration
        protected virtual void Start()
        {
            var result = Messenger.Current.Query<MapRecords.CreateQuery, MapRecords.CreateResult>(new MapRecords.CreateQuery
            {
                Instance = this
            });
            ID = result.ID;
        }

#endregion

#region Component Methods

        /// <summary>
        /// Adds a component to the known list.
        /// </summary>
        /// <param name="instance">The component instance.</param>
        public void Add<T>(T instance) where T : BaseComponent
        {
            _components.Add(instance);
        }

        /// <summary>
        /// Removes a component from the known list.
        /// </summary>
        /// <param name="instance">The component instance.</param>
        public void Remove<T>(T instance) where T : BaseComponent
        {
            _components.Remove(instance);
        }
        
        /// <summary>
        /// Retrieves all instances of the specified component class.
        /// </summary>
        public T[] GetAll<T>() where T : BaseComponent
        {
            var search = _components.Where(component => component is T).Cast<T>();
            return search.ToArray();
        }

        /// <summary>
        /// Retrieves the first instance of the specified component class.
        /// </summary>
        public T Get<T>() where T : BaseComponent
        {
            var search = GetAll<T>();
            return search.First();
        }

#endregion

#region Coroutine Methods

        /// <summary>
        /// Adds a coroutine for a component type.
        /// </summary>
        /// <param name="func">The coroutine function.</param>
        public void AddCoroutine<T>(IEnumerator func) where T : BaseComponent
        {
            var type = typeof(T);
            
            // The coroutine already exists
            if (_coroutines.ContainsKey(type))
            {
                Debug.LogWarning($"{name} is already running a coroutine for {type.Name}.");
                return;
            }
            
            // Add a new coroutine entry
            var coroutine = StartCoroutine(func);
            _coroutines.Add(type, coroutine);
        }

        /// <summary>
        /// Returns the active coroutine for a component, if it exists.
        /// </summary>
        public Coroutine GetCoroutine<T>() where T : BaseComponent
        {
            var type = typeof(T);
            return _coroutines.GetValueOrDefault(type, null);
        }

        /// <summary>
        /// Removes an active coroutine.
        /// </summary>
        public void RemoveCoroutine<T>() where T : BaseComponent
        {
            var type = typeof(T);
            
            // No coroutine is running
            if (!_coroutines.TryGetValue(type, out var coroutine))
            {
                return;
            }
            
            // Stop and remove the active coroutine
            StopCoroutine(coroutine);
            _coroutines.Remove(type);
        }
        
#endregion

#region Database

        /// <summary>
        /// Sets the object's ID.
        /// </summary>
        /// <param name="newID">The new object ID.</param>
        protected void SetID(int newID)
        {
            ID = newID;
        }

#endregion
    }
}