using UnityEngine;

namespace Game.Atoms
{
    public abstract class BaseAtom : MonoBehaviour
    {
        /// <summary>
        /// The instance identifier.
        /// </summary>
        [field: SerializeField] public string ID { get; private set; }
    }
}