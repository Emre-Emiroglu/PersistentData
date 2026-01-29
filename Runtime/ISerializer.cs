namespace PersistentData.Runtime
{
    /// <summary>
    /// Defines serialization and deserialization methods for converting data to and from string representations.
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// Serializes the specified object to a string representation.
        /// </summary>
        /// <typeparam name="T">The type of the object to serialize.</typeparam>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>A string representation of the serialized object.</returns>
        public string Serialize<T>(T obj);
        
        /// <summary>
        /// Deserializes the specified string into an object of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of the object to deserialize.</typeparam>
        /// <param name="str">The string representation of the object.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        public T Deserialize<T>(string str);
    }
}