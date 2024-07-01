using System.Collections.Generic;
using Game.Interfaces;
using Game.Queries;
using UnityEngine;

namespace Game.Objects
{
    public abstract class BaseObject : MonoBehaviour, IObject
    {
        // The definition of attached properties
        private readonly Dictionary<System.Type, IComponent> components = new();

#region Unity Events
        
        // Component caching
        protected virtual void Awake()
        {
            RegisterComponents();
        }

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
        
        /// <inheritdoc />
        public void Add<T>(T component) where T : IComponent
        {
            var type = component.GetType();
                
            // Check for specific categories
            Check<IInteractable>(component);
            Check<IPickable>(component);
                
            // Add the component under its same type
            components[type] = component;
            component.SetOwner(this);
        }
        
        /// <summary>
        /// Checks the component for a specific type, and duplicates it under a different key.
        /// </summary>
        /// <param name="component">The component instance.</param>
        /// <typeparam name="TComponent">The component type.</typeparam>
        private void Check<TComponent>(IComponent component)
            where TComponent : IComponent
        {
            // Get the component type for error purposes
            var type = typeof(TComponent);
            var printType = component.GetType();

            // Stop here if not the same component
            if (component is not TComponent)
            {
                return;
            }
            
            // There is already a key for this
            if (!components.TryAdd(type, component))
            {
                Debug.LogError($"Second instance of {type.Name} in {name}. Type: {printType}");
            }
        }

        /// <summary>
        /// Returns a component of a specified type.
        /// </summary>
        public T Get<T>() where T : IComponent
        {
            var type = typeof(T);
            
            // There is no component to return
            if (!components.TryGetValue(type, out var component))
            {
                return default;
            }

            // Return the stored component
            return (T)component;
        }

        /// <summary>
        /// Registers the object's components.
        /// </summary>
        private void RegisterComponents()
        {
            var attached = GetComponentsInChildren<IComponent>();

            foreach (var component in attached)
            {
                Add(component);
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