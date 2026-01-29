using System.IO;
using UnityEngine;

namespace PersistentData.Runtime
{
    /// <summary>
    /// Provides static utility methods for initializing and interacting with the persistent data service.
    /// </summary>
    public static class PersistentDataServiceUtilities
    {
        #region Fields
        private static IPersistentDataService _persistentDataService;
        #endregion

        #region Core
        /// <summary>
        /// Initializes the persistent data service using the <c>PersistentDataConfig</c> ScriptableObject 
        /// located in the <c>Resources/PersistentData/PersistentDataConfig.asset</c> path.
        /// </summary>
        /// <para>
        /// If the config is not found, initialization is aborted and an error is logged.
        /// </para>
        public static void Initialize()
        {
            PersistentDataConfig config =
                Resources.Load<PersistentDataConfig>(nameof(PersistentData) + "/PersistentDataConfig");
            
            if (!config)
            {
                LogConfigIsNullError();
                
                return;
            }

            Initialize(config.SerializerType, config.FileExtension, config.Key, config.Iv);
        }

        /// <summary>
        /// Initializes the persistent data service with a specified serializer, file extension, key, and IV.
        /// </summary>
        /// <param name="serializerType">The type of serializer to use (e.g., JSON or EncryptedJson).</param>
        /// <param name="fileExtension">The file extension for saved files.</param>
        /// <param name="key">The AES encryption key (Base64 encoded) for encrypted JSON serializer.</param>
        /// <param name="iv">The AES initialization vector (Base64 encoded) for encrypted JSON serializer.</param>
        public static void Initialize(SerializerType serializerType, string fileExtension, string key, string iv) =>
            _persistentDataService = serializerType switch
            {
                SerializerType.Json => new PersistentDataService(new JsonSerializer(), fileExtension),
                SerializerType.EncryptedJson => new PersistentDataService(new EncryptedJsonSerializer(key, iv),
                    fileExtension),
                _ => new PersistentDataService(new JsonSerializer(), fileExtension)
            };
        #endregion

        #region Executes
        /// <summary>
        /// Saves the provided data to a file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to save the data to.</param>
        /// <param name="data">The data to save.</param>
        /// <param name="overwrite">Whether to overwrite the file if it already exists. Defaults to true.</param>
        public static void Save<T>(string name, T data, bool overwrite = true) =>
            _persistentDataService.Save(name, data, overwrite);

        /// <summary>
        /// Loads the data from a file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to load data from.</param>
        /// <returns>The deserialized data of type <typeparamref name="T"/>.</returns>
        public static T Load<T>(string name) => _persistentDataService.Load<T>(name);

        /// <summary>
        /// Deletes the file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to delete.</param>
        public static void Delete(string name) => _persistentDataService.Delete(name);

        /// <summary>
        /// Deletes all files saved by the persistent data service.
        /// </summary>
        public static void DeleteAll() => _persistentDataService.DeleteAll();
        
        /// <summary>
        /// Logs a message indicating that the persistent data service has been initialized.
        /// </summary>
        public static void LogPersistentDataServiceInitialized() => Debug.Log("Persistent Data Service initialized");
        
        /// <summary>
        /// Logs a message indicating that a file was saved successfully.
        /// </summary>
        /// <param name="filePath">The full file path of the saved file.</param>
        public static void LogPersistentDataServiceFileSaved(string filePath) => Debug.Log($"File '{filePath}' saved");
        
        /// <summary>
        /// Logs a message indicating that a file was loaded successfully.
        /// </summary>
        /// <param name="filePath">The full file path of the loaded file.</param>
        public static void LogPersistentDataServiceFileLoaded(string filePath) =>
            Debug.Log($"File '{filePath}' loaded");
        
        /// <summary>
        /// Logs a message indicating that a file was deleted successfully.
        /// </summary>
        /// <param name="filePath">The full file path of the deleted file.</param>
        public static void LogPersistentDataServiceFileDeleted(string filePath) =>
            Debug.Log($"File '{filePath}' deleted");
        
        /// <summary>
        /// Throws an IOException indicating that a file already exists and overwriting is not allowed.
        /// </summary>
        /// <param name="filePath">The file path that caused the conflict.</param>
        /// <exception cref="IOException">Thrown when a file already exists and overwrite is false.</exception>
        public static void ThrowSaveIOException(string filePath) =>
            throw new IOException($"File '{filePath}' already exists and overwrite is false.");
        
        /// <summary>
        /// Throws a FileNotFoundException indicating that a specified file does not exist.
        /// </summary>
        /// <param name="filePath">The file path that could not be found.</param>
        /// <exception cref="FileNotFoundException">Thrown when the specified file is not found.</exception>
        public static void ThrowFileNotFoundException(string filePath) =>
            throw new FileNotFoundException($"File '{filePath}' not found.");
        private static void LogConfigIsNullError() =>
            Debug.LogError("[PersistentData] Config is null. Initialization aborted.");
        #endregion
    }
}