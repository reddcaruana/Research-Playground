using Game.Interfaces;
using UnityEngine;

namespace Game.Components
{
    public abstract class MaterialUpdater : BaseComponent
    {
        /// <summary>
        /// The material instances assigned to this object. 
        /// </summary>
        protected Material[] Materials { get; private set; }

#region Unity Events

        // Component caching
        protected virtual void Awake()
        {
            // Get the renderers and materials
            var renderers = GetComponentsInChildren<Renderer>();

            Materials = new Material[renderers.Length];
            for (var i = 0; i < renderers.Length; i++)
            {
                Materials[i] = renderers[i].material;
            }
        }

#endregion
        
#region Methods
        
        /// <summary>
        /// Updates the attached materials.
        /// </summary>
        protected abstract void UpdateMaterials();

#endregion
    }
}