using System;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace PersistentData.Runtime
{
    /// <summary>
    /// Provides encrypted JSON serialization and deserialization using AES encryption.
    /// </summary>
    public sealed class EncryptedJsonSerializer : ISerializer
    {
        #region ReadonlyFields
        private readonly byte[] _key;
        private readonly byte[] _iv;
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the EncryptedJsonSerializer with an AES key and IV.
        /// </summary>
        /// <param name="base64Key">The AES encryption key (Base64 encoded).</param>
        /// <param name="base64Iv">The AES initialization vector (Base64 encoded).</param>
        public EncryptedJsonSerializer(string base64Key, string base64Iv)
        {
            _key = Convert.FromBase64String(base64Key);
            _iv = Convert.FromBase64String(base64Iv);
        }
        #endregion
        
        #region Executes
        /// <summary>
        /// Serializes the provided object to an encrypted JSON string.
        /// </summary>
        /// <param name="obj">The object to serialize.</param>
        /// <returns>The encrypted JSON string representing the object.</returns>
        public string Serialize<T>(T obj) => Encrypt(JsonConvert.SerializeObject(obj));
        
        /// <summary>
        /// Deserializes an encrypted JSON string to the specified object type.
        /// </summary>
        /// <param name="encryptedJson">The encrypted JSON string to deserialize.</param>
        /// <returns>The deserialized object of type <typeparamref name="T"/>.</returns>
        public T Deserialize<T>(string encryptedJson) => JsonConvert.DeserializeObject<T>(Decrypt(encryptedJson));
        private string Encrypt(string plainText)
        {
            using Aes aes = Aes.Create();
            
            aes.Key = _key;
            aes.IV = _iv;

            using MemoryStream ms = new();
            using CryptoStream cs = new(ms, aes.CreateEncryptor(), CryptoStreamMode.Write);
            using StreamWriter sw = new(cs);
            
            sw.Write(plainText);
            sw.Close();

            return Convert.ToBase64String(ms.ToArray());
        }
        private string Decrypt(string cipherText)
        {
            byte[] buffer = Convert.FromBase64String(cipherText);

            using Aes aes = Aes.Create();
            
            aes.Key = _key;
            aes.IV = _iv;

            using MemoryStream ms = new(buffer);
            using CryptoStream cs = new(ms, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using StreamReader sr = new(cs);

            return sr.ReadToEnd();
        }
        #endregion
    }
}