using UnityEngine;

namespace Game.Utilities
{
    public class Movement
    {
        /// <summary>
        /// The motion speed.
        /// </summary>
        public float Speed { get; set; }

        /// <summary>
        /// The motion direction.
        /// </summary>
        public Vector3 Direction { get; set; }

        public Movement(float speed)
        {
            Speed = speed;
        }

#region Methods

        /// <summary>
        /// Moves a Rigidbody in the direction of motion.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody instance.</param>
        public void Move(Rigidbody rigidbody)
        {
            rigidbody.linearVelocity = Direction * Speed;

            if (Mathf.Abs(Direction.x) > Mathf.Epsilon || Mathf.Abs(Direction.z) > Mathf.Epsilon)
            {
                var lookRotation = Quaternion.LookRotation(Direction, Vector3.up);
                rigidbody.MoveRotation(lookRotation);
            }
        }

#endregion
    }
}