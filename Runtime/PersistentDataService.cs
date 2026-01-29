using System.IO;
using UnityEngine;

namespace PersistentData.Runtime
{
    /// <summary>
    /// Provides functionality for saving, loading, and deleting persistent data using a specified serializer.
    /// </summary>
    public sealed class PersistentDataService : IPersistentDataService
    {
        #region ReadonlyFields
        private readonly ISerializer _serializer;
        private readonly string _dataPath;
        private readonly string _fileExtension;
        #endregion

        #region Getters
        private string GetPathToFile(string fileName) => Path.Combine(_dataPath, $"{fileName}.{_fileExtension}");
        #endregion

        #region Constructor
        /// <summary>
        /// Initializes the PersistentDataService with a specified serializer and file extension.
        /// </summary>
        /// <param name="serializer">The serializer used to serialize and deserialize data.</param>
        /// <param name="fileExtension">The file extension used for saved files.</param>
        public PersistentDataService(ISerializer serializer, string fileExtension)
        {
            _serializer = serializer;
            _dataPath = Application.persistentDataPath;
            _fileExtension = fileExtension;
            
            PersistentDataServiceUtilities.LogPersistentDataServiceInitialized();
        }
        #endregion

        #region Executes
        /// <summary>
        /// Saves the provided data to a file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to save the data to.</param>
        /// <param name="data">The data to save.</param>
        /// <param name="overwrite">Whether to overwrite the file if it already exists. Defaults to true.</param>
        public void Save<T>(string name, T data, bool overwrite = true)
        {
            string filePath = GetPathToFile(name);
            
            if (!overwrite && File.Exists(filePath))
                PersistentDataServiceUtilities.ThrowSaveIOException(filePath);

            string serialized = _serializer.Serialize(data);
            
            File.WriteAllText(filePath, serialized);
            
            PersistentDataServiceUtilities.LogPersistentDataServiceFileSaved(filePath);
        }
        
        /// <summary>
        /// Loads the data from a file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to load data from.</param>
        /// <returns>The deserialized data of type <typeparamref name="T"/>.</returns>
        public T Load<T>(string name)
        {
            string filePath = GetPathToFile(name);
            
            if (!File.Exists(filePath))
                PersistentDataServiceUtilities.ThrowFileNotFoundException(filePath);

            string content = File.ReadAllText(filePath);
            
            T deserialized = _serializer.Deserialize<T>(content);
            
            PersistentDataServiceUtilities.LogPersistentDataServiceFileLoaded(filePath);

            return deserialized;
        }
        
        /// <summary>
        /// Deletes the file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to delete.</param>
        public void Delete(string name)
        {
            string filePath = GetPathToFile(name);

            if (!File.Exists(filePath))
                PersistentDataServiceUtilities.ThrowFileNotFoundException(filePath);
            
            File.Delete(filePath);
            
            PersistentDataServiceUtilities.LogPersistentDataServiceFileDeleted(filePath);
        }
        
        /// <summary>
        /// Deletes all files saved by the persistent data service.
        /// </summary>
        public void DeleteAll()
        {
            string[] files = Directory.GetFiles(_dataPath, $"*.{_fileExtension}");

            foreach (string file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                
                Delete(fileName);
            }
        }
        #endregion
    }
}