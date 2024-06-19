using System.Collections.Generic;
using System.Linq;

namespace Game
{
    public abstract class Records<T>
    {
        // The entries
        private readonly Dictionary<int, T> _entries = new();
        
        // The deleted (but not yet replaced) entries
        private readonly Queue<int> _emptySlots = new();
        
        // The next calculated ID
        private int _nextId;

        /// <summary>
        /// Gets the count of active entries.
        /// </summary>
        public int Count => _entries.Count;

        /// <summary>
        /// Creates a new entry and assigns it a unique ID.
        /// </summary>
        /// <param name="entry">The new entry.</param>
        public int Create(T entry)
        {
            // Get the next ID
            var id = _emptySlots.Count > 0 ? _emptySlots.Dequeue() : _nextId++;
            
            // Add the entry
            _entries[id] = entry;
            return id;
        }

        /// <summary>
        /// Removes an entry by its ID.
        /// </summary>
        /// <param name="id">The ID of the entry to remove.</param>
        public bool Delete(int id)
        {
            if (!_entries.ContainsKey(id))
            {
                return false;
            }

            _entries.Remove(id);
            _emptySlots.Enqueue(id);
            return true;
        }

        /// <summary>
        /// Retrieves the key of an instance.
        /// </summary>
        /// <param name="entry">The entry instance.</param>
        public int Find(T entry)
        {
            if (!_entries.ContainsValue(entry))
            {
                throw new KeyNotFoundException("Could not find key for this entry.");
            }

            return _entries.First(e => e.Value.Equals(entry)).Key;
        }

        /// <summary>
        /// Retrieves all keys.
        /// </summary>
        public int[] GetKeys()
        {
            return _entries.Keys.ToArray();
        }
        
        /// <summary>
        /// Retrieves all values.
        /// </summary>
        public T[] GetValues()
        {
            return _entries.Values.ToArray();
        }

        /// <summary>
        /// Retrieves a record by its ID.
        /// </summary>
        /// <param name="id">The ID of the record to retrieve.</param>
        public T Get(int id)
        {
            if (!_entries.TryGetValue(id, out var entry))
            {
                throw new KeyNotFoundException("Invalid ID.");
            }

            return entry;
        }

        /// <summary>
        /// Updates a record at the specified ID.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool Update(int id, T entry)
        {
            if (!_entries.ContainsKey(id))
            {
                return false;
            }

            _entries[id] = entry;
            return true;
        }
    }
}