using UnityEngine;

namespace Game
{
    public interface IDirection
    {
        float Distance { get; }
        Vector3 Direction { get; }
    }
}