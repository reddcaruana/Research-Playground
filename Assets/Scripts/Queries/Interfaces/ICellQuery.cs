using UnityEngine;

namespace Game.Queries
{
    public interface ICellQuery : IQuery, ISource, IDirection
    {
        /// <summary>
        /// Generates a position value based on the inputted data.
        /// </summary>
        Vector3 GetPoint();
    }
}