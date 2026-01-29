using Newtonsoft.Json;

namespace PersistentData.Runtime
{
    /// <summary>
    /// Provides JSON-based serialization and deserialization using Newtonsoft.Json.
    /// </summary>
    public sealed class JsonSerializer : ISerializer
    {
        #region Executes
        /// <summary>
        /// Serializes the provided object to a JSON string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The JSON string representing the object.</returns>
        public string Serialize<T>(T obj) => JsonConvert.SerializeObject(obj);
        
        /// <summary>
        /// Deserializes a JSON string to the specified object type.
        /// </summary>
        /// <param name="str">The JSON string to deserialize.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        public T Deserialize<T>(string str) => JsonConvert.DeserializeObject<T>(str);
        #endregion
    }
}