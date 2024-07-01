using System.Collections.Generic;
using Game.Interfaces;
using UnityEngine;

namespace Game.Components
{
    [RequireComponent(typeof(Rigidbody))]
    public class FireSource : BaseComponent, IFireSource
    {
        // The collider expansion rate
        private const float RadiusExpansion = 0.4f;

        private readonly HashSet<IIgnitable> contacts = new();
        
        // Components
        private Collider baseCollider;
        private Collider triggerCollider;
        
#region Unity Events

        // Component caching
        private void Awake()
        {
            // Get the collider
            baseCollider = GetComponent<Collider>();
            
            // The collider is already a trigger
            if (baseCollider.isTrigger)
            {
                triggerCollider = baseCollider;
                baseCollider = null;
                return;
            }
            
            ExpandCollider();
        }

        // Starts the burning process
        private void Start()
        {
            // The object cannot be burned
            if (!Owner.TryGet<Burnable>(out var burnable))
            {
                return;
            }
            
            burnable.Burn();
        }

        // Clean up the values
        private void OnDestroy()
        {
            // Destroy the trigger collider
            if (triggerCollider)
            {
                Destroy(triggerCollider);
            }
            
            // Tell any linked ignitables to stop burning
            foreach (var ignitable in contacts)
            {
                ignitable.OnSeparate(this);
            }
        }

        // Handles contact with ignitables
        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<IIgnitable>(out var ignitable))
            {
                return;
            }

            ignitable.OnContact(this);
            contacts.Add(ignitable);
        }

        // Handles separation from ignitables
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<IIgnitable>(out var ignitable))
            {
                return;
            }
            
            ignitable.OnSeparate(this);
            contacts.Remove(ignitable);
        }

        // Defaults setup
        private void Reset()
        {
            // GameObject tag
            gameObject.tag = "FireSource";
            
            // Rigidbody Setup
            var rb = GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }

#endregion

#region Methods

        /// <summary>
        /// Expands on the existing collider
        /// </summary>
        private void ExpandCollider()
        {
            // There is no base collider
            if (!baseCollider)
            {
                AttachDefaultCollider();
                return;
            }

            // Clone the collider
            var type = baseCollider.GetType();
            var component = gameObject.AddComponent(type) as Collider;
            
            // The component could not be retrieved
            if (!component)
            {
                return;
            }

            // Set the component properties
            component.isTrigger = true;
            
            // Handle the component sizing
            switch (component)
            {
                case SphereCollider sphereCollider:
                {
                    var sc = (SphereCollider)baseCollider;
                    sphereCollider.center = sc.center;
                    sphereCollider.radius = sc.radius + RadiusExpansion;
                    break;
                }

                case BoxCollider boxCollider:
                {
                    var bc = (BoxCollider)baseCollider;
                    boxCollider.center = bc.center;
                    boxCollider.size = bc.size + Vector3.one * RadiusExpansion;
                    break;
                }

                case CapsuleCollider capsuleCollider:
                {
                    var cc = (CapsuleCollider)baseCollider;
                    capsuleCollider.center = cc.center;
                    capsuleCollider.radius = cc.radius + RadiusExpansion;
                    capsuleCollider.height = cc.height + RadiusExpansion;
                    break;
                }
            }

            triggerCollider = component;
        }

        /// <summary>
        /// Attaches a default sphere collider.
        /// </summary>
        [ContextMenu("Add Default Collider")]
        private void AttachDefaultCollider()
        {
            const float radius = 0.8f;
            
            // Create a sphere collider
            var sphereCollider = gameObject.AddComponent<SphereCollider>();
            sphereCollider.isTrigger = true;
            sphereCollider.radius = radius;
            sphereCollider.center = Vector3.up * radius * 0.5f;

            triggerCollider = sphereCollider;
        }

#endregion
    }
}