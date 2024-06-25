using UnityEngine;

namespace Game.World
{
    public class MapRecords : MonoBehaviour
    {
        // The object database.
        private readonly ObjectDatabase _objectDatabase = new();

        private void OnDisable()
        {
            Messenger.Current.Unsubscribe<CreateQuery, CreateResult>(CreateInstance);
        }

        private void OnEnable()
        {
            Messenger.Current.Subscribe<CreateQuery, CreateResult>(CreateInstance);
        }

#region Query Handling

        /// <summary>
        /// Handles object registration.
        /// </summary>
        /// <param name="query">The query message.</param>
        private CreateResult CreateInstance(CreateQuery query)
        {
            // Prepare a result
            var result = new CreateResult { ID = -1 };
            
            // Stop if we're inserting a null object
            if (!query.Instance)
            {
                Debug.LogWarning("Trying to add a null object.");
                return result;
            }

            result.ID = _objectDatabase.Create(query.Instance);
            return result;
        }

#endregion

#region Messages

        public struct CreateResult : IMessage
        {
            public int ID { get; set; }
        }

#endregion

#region Queries

        public struct CreateQuery : IQuery
        {
            public BaseObject Instance { get; set; }
        }

#endregion
    }
}