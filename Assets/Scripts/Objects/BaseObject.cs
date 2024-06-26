using System.Collections.Generic;
using System.Linq;
using Game.Components;
using UnityEngine;

namespace Game.Objects
{
    public abstract class BaseObject : MonoBehaviour
    {
        // The definition of attached properties
        private readonly Dictionary<System.Type, IObjectComponent> _components = new();

        // Component caching
        protected virtual void Awake()
        {
            RegisterComponents();
        }

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
        /// Registers the object's components.
        /// </summary>
        private void RegisterComponents()
        {
            var components = GetComponents<IObjectComponent>();

            foreach (var component in components)
            {
                var type = component.GetType();
                _components[type] = component;
            }
        }
    }
}