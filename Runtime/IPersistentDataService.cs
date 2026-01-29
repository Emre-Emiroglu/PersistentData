namespace PersistentData.Runtime
{
    /// <summary>
    /// Defines the contract for saving, loading, and deleting persistent data.
    /// </summary>
    public interface IPersistentDataService
    {
        /// <summary>
        /// Saves the provided data to a file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to save the data to.</param>
        /// <param name="data">The data to save.</param>
        /// <param name="overwrite">Whether to overwrite the file if it already exists. Defaults to true.</param>
        public void Save<T>(string name, T data, bool overwrite = true);
        
        /// <summary>
        /// Loads the data from a file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to load data from.</param>
        /// <returns>The deserialized data of type <typeparamref name="T"/>.</returns>
        public T Load<T>(string name);
        
        /// <summary>
        /// Deletes the file with the specified name.
        /// </summary>
        /// <param name="name">The name of the file to delete.</param>
        public void Delete(string name);
        
        /// <summary>
        /// Deletes all files saved by the persistent data service.
        /// </summary>
        public void DeleteAll();
    }
}