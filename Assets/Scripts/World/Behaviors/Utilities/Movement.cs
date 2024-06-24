using UnityEngine;

namespace Game.World
{
    public class Movement
    {
        /// <summary>
        /// The motion speed.
        /// </summary>
        public float LinearSpeed { get; set; }
        
        public float AngularSpeed { get; set; }

        /// <summary>
        /// The motion direction.
        /// </summary>
        public Vector3 Direction { get; set; }

        public Movement(float linearSpeed, float angularSpeed)
        {
            LinearSpeed = linearSpeed;
            AngularSpeed = angularSpeed;
        }

#region Methods

        /// <summary>
        /// Moves a Rigidbody in the direction of motion.
        /// </summary>
        /// <param name="rigidbody">The Rigidbody instance.</param>
        public void Move(Rigidbody rigidbody)
        {
            // Set the velocity
            var velocity = Direction * LinearSpeed;
            velocity.y = rigidbody.linearVelocity.y;
            rigidbody.linearVelocity = velocity;

            // Rotate towards the direction of motion
            if (Mathf.Abs(Direction.x) > Mathf.Epsilon || Mathf.Abs(Direction.z) > Mathf.Epsilon)
            {
                var targetRotation = Quaternion.LookRotation(Direction, Vector3.up);
                var sRotation = Quaternion.Slerp(rigidbody.rotation, targetRotation, Time.deltaTime * AngularSpeed);
                
                rigidbody.MoveRotation(sRotation);
            }
        }

#endregion
    }
}