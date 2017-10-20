using System;
using System.Collections.Generic;
using RestLess.Helpers;

namespace RestLess
{
    /// <summary>
    /// Represents a component that stores instance of type <typeparamref name="T"/> and have a default value.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class RestSettingStore<T>       
        where T : class
    {
        private readonly Dictionary<string, T> dictionary;
        private T defaultValue;

        /// <summary>
        /// Creates a new instance of <see cref="RestSettingStore{T}"/>.
        /// </summary>
        /// <param name="name">The name of this store.</param>
        public RestSettingStore(string name)
        {
            this.Name = name;
            this.dictionary = new Dictionary<string, T>();
        }

        /// <summary>
        /// Creates a new instance of <see cref="RestSettingStore{T}"/> with a default value.
        /// </summary>
        /// <param name="name">The name of this store.</param>
        /// <param name="defaultValue">The default value.</param>
        public RestSettingStore(string name, T defaultValue) : this(name)
        {
            this.defaultValue = defaultValue ?? throw new ArgumentNullException(nameof(defaultValue));
        }

        /// <summary>
        /// Gets or sets the default value of this store.
        /// </summary>
        public T Default
        {
            get { return this.defaultValue; }
            set { this.defaultValue = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        /// <summary>
        /// Gets or sets the value associated to this <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        /// The value associated to the <paramref name="id"/> or <see cref="Default"/> if the <paramref name="id"/> is not registered.
        /// </returns>
        public T this[string id]
        {
            get { return this.Get(id); }
            set { this.Set(id, value); }
        }

        /// <summary>
        /// Gets the name of this store.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Indicates whether this store has a default value.
        /// </summary>
        public bool HasDefault => this.Default != null;

        /// <summary>
        /// Associates the specified <paramref name="value"/> to this <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The id of the value.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public RestSettingStore<T> Set(string id, T value)
        {
            Check.NotNull(id, nameof(id));
            Check.NotNull(value, nameof(value));

            this.dictionary[id] = value;
            return this;
        }

        /// <summary>
        /// Gets the value associated to the specified <paramref name="id"/>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// The value associated to the <paramref name="id"/> or <see cref="Default"/> if the <paramref name="id"/> is not registered.
        /// </returns>
        public T Get(string id)
        {
            T value = this.Default;
            this.dictionary.TryGetValue(id, out value);
            return value;
        }        
    }
}
