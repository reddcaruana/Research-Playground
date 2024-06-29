using System.Collections.Generic;
using System.Linq;
using Game.Components;
using UnityEngine;

namespace Game.Objects
{
    public abstract class BaseObject : MonoBehaviour
    {
        // The definition of attached properties
<<<<<<< Updated upstream
        private readonly Dictionary<System.Type, IObjectComponent> _components = new();
=======
        private readonly Dictionary<System.Type, IComponent> components = new();
>>>>>>> Stashed changes

#region Unity Events
        
        // Component caching
        protected virtual void Awake()
        {
            RegisterComponents();
        }

<<<<<<< Updated upstream
=======
        // Object setup
        protected virtual void Start()
        {
            Messenger.Current.Publish(new GridQueries.RegisterContents
            {
                Position = transform.position,
                Contents = this
            });
        }

        // Object removal
        protected virtual void OnDestroy()
        {
            Messenger.Current.Publish(new GridQueries.UnregisterContents
            {
                Position = transform.position,
                Contents = this
            });
        }

#endregion

#region Methods
        
>>>>>>> Stashed changes
        /// <summary>
        /// Returns a component of a specified type.
        /// </summary>
        public T Get<T>() where T : IObjectComponent
        {
            var type = typeof(T);
            
            // There is no component to return
            if (!_components.TryGetValue(type, out var component))
            {
                return default;
            }

            // Return the stored component
            return (T)component;
        }

        /// <summary>
<<<<<<< Updated upstream
        /// Returns the first interactable in the known collection.
        /// </summary>
        public IInteractable GetInteractable()
        {
            var values = _components.Values.ToArray();
            for (var i = 0; i < values.Length; i++)
            {
                var value = values[i];
                if (value is IInteractable interactable)
                {
                    return interactable;
                }
            }

            return null;
        }

        /// <summary>
=======
>>>>>>> Stashed changes
        /// Registers the object's components.
        /// </summary>
        private void RegisterComponents()
        {
            var attached = GetComponents<IComponent>();

            foreach (var component in attached)
            {
                var type = component.GetType();
<<<<<<< Updated upstream
                _components[type] = component;
=======
                components[type] = component;
>>>>>>> Stashed changes
            }
        }

        /// <inheritdoc />
        public bool TryGet<T>(out T instance) where T : IComponent
        {
            instance = Get<T>();
            return instance != null;
        }

#endregion
    }
}