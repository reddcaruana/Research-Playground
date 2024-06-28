using System.Collections.Generic;
using System.Linq;
using Game.Components;
using Game.Queries;
using UnityEngine;

namespace Game.Objects
{
    public abstract class BaseObject : MonoBehaviour
    {
        // The definition of attached properties
        private readonly Dictionary<System.Type, IObjectComponent> components = new();

        // Component caching
        protected virtual void Awake()
        {
            RegisterComponents();
        }

        // Object setup
        protected virtual void Start()
        {
            Messenger.Current.Publish(new GridQueries.RegisterContents<BaseObject>
            {
                Position = transform.position,
                Contents = this
            });
        }

        /// <summary>
        /// Returns a component of a specified type.
        /// </summary>
        public T Get<T>() where T : IObjectComponent
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
        /// Returns the first interactable in the known collection.
        /// </summary>
        public IInteractable GetInteractable()
        {
            var values = components.Values.ToArray();
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
        /// Registers the object's components.
        /// </summary>
        private void RegisterComponents()
        {
            var components = GetComponents<IObjectComponent>();

            foreach (var component in components)
            {
                var type = component.GetType();
                this.components[type] = component;
            }
        }
    }
}