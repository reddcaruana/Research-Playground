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
            var attached = GetComponents<IComponent>();

            foreach (var component in attached)
            {
                var type = component.GetType();
                
                // Filter interactables
                if (component is IInteractable)
                {
                    // There is already an interactables
                    if (components.ContainsKey(typeof(IInteractable)))
                    {
                        Debug.LogError($"{name} found second interactable ({type.Name}) which will be ignored!");
                        continue;
                    }
                    
                    // Add the interactable
                    components[typeof(IInteractable)] = component;
                }
                
                // Add the component under its same type
                components[type] = component;
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