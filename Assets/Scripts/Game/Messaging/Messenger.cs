using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game
{
    public class Messenger : Singleton<Messenger>
    {
        /// <summary>
        /// The message callback structure.
        /// </summary>
        public delegate void MessageCallback<in T>(T message) where T : IMessage;

        /// <summary>
        /// The query callback structure.
        /// </summary>
        public delegate TResult QueryCallback<in TQuery, out TResult>(TQuery query)
            where TQuery : IQuery
            where TResult : IMessage;

        // The subscribed callbacks
        private readonly Dictionary<Type, Delegate> _subscriptions = new();

        /// <summary>
        /// Initializes an instance of the class.
        /// </summary>
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Init()
        {
            Current = new Messenger();
        }

        /// <summary>
        /// Clears a subscription.
        /// </summary>
        public void Clear<T>()
        {
            var type = typeof(T);
            _subscriptions.Remove(type);
        }

        /// <summary>
        /// Publishes a message.
        /// </summary>
        public void Publish<T>() where T : IMessage => Publish<T>(default);

        /// <summary>
        /// Publishes a message.
        /// </summary>
        public void Publish<T>(T message)
            where T : IMessage
        {
            var type = typeof(T);

            // There are no subscribed methods
            if (!_subscriptions.TryGetValue(type, out var delegates))
            {
                return;
            }

            // Event type mismatch
            if (delegates is not MessageCallback<T> callbacks)
            {
                Debug.LogError("Type mismatch: Attempted to publish a message with incorrect data type.");
                return;
            }

            // Invoke the event
            callbacks.Invoke(message);
        }

        /// <summary>
        /// Receives a response for a specified query.
        /// </summary>
        public TResult Query<TQuery, TResult>(TQuery message)
            where TQuery : IQuery
            where TResult : IMessage
        {
            var type = typeof(TQuery);

            // There are no subscribed methods
            if (!_subscriptions.TryGetValue(type, out var delegates))
            {
                return default;
            }

            // Event type mismatch
            if (delegates is not QueryCallback<TQuery, TResult> callback)
            {
                return default;
            }

            // Invoke the query and get the response
            return callback.Invoke(message);
        }

        /// <summary>
        /// Subscribes a method to a specific message type.
        /// </summary>
        /// <param name="callback">The callback method.</param>
        public void Subscribe<T>(MessageCallback<T> callback)
            where T : IMessage
        {
            // Null callback
            if (callback == null)
            {
                Debug.LogWarning("Attempting to add a null callback.");
                return;
            }

            var type = typeof(T);

            // The key doesn't exist
            if (!_subscriptions.TryGetValue(type, out var callbacks))
            {
                _subscriptions[type] = callback;
                return;
            }

            // Combine callback
            _subscriptions[type] = Delegate.Combine(callbacks, callback);
        }

        /// <summary>
        /// Subscribes a method to a specific query type.
        /// </summary>
        /// <param name="callback">The callback method.</param>
        public void Subscribe<TQuery, TResult>(QueryCallback<TQuery, TResult> callback)
            where TQuery : IQuery
            where TResult : IMessage
        {
            // Null callback
            if (callback == null)
            {
                Debug.LogWarning("Attempting to add a null callback.");
                return;
            }

            var type = typeof(TQuery);

            // The key doesn't exist
            if (!_subscriptions.TryGetValue(type, out var callbacks))
            {
                _subscriptions[type] = callback;
                return;
            }

            // Combine callback
            _subscriptions[type] = Delegate.Combine(callbacks, callback);
        }

        /// <summary>
        /// Unsubscribes a method from a specific message type.
        /// </summary>
        /// <param name="callback">The callback method.</param>
        public void Unsubscribe<T>(MessageCallback<T> callback)
            where T : IMessage
        {
            var type = typeof(T);

            // There is no subscription
            if (!_subscriptions.TryGetValue(type, out var callbacks))
            {
                return;
            }

            // Remove the callback
            callbacks = Delegate.Remove(callbacks, callback);

            // Clear the key if there are no callbacks
            if (callbacks == null)
            {
                Clear<T>();
                return;
            }

            // Reassign the callbacks
            _subscriptions[type] = callbacks;
        }

        /// <summary>
        /// Unsubscribes a method from a specific query type.
        /// </summary>
        /// <param name="callback">The callback method.</param>
        public void Unsubscribe<TQuery, TResult>(QueryCallback<TQuery, TResult> callback)
            where TQuery : IQuery
            where TResult : IMessage
        {
            var type = typeof(TQuery);

            // There is no subscription
            if (!_subscriptions.TryGetValue(type, out var callbacks))
            {
                return;
            }

            // Remove the callback
            callbacks = Delegate.Remove(callbacks, callback);

            // Clear the key if there are no callbacks
            if (callbacks == null)
            {
                Clear<TQuery>();
                return;
            }

            // Reassign the callbacks
            _subscriptions[type] = callbacks;
        }
    }
}