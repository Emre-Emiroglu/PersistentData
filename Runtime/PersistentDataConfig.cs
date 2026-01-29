using UnityEngine;

namespace PersistentData.Runtime
{
    [CreateAssetMenu(fileName = "PersistentDataConfig", menuName = "PersistentData/PersistentDataConfig", order = 0)]
    public sealed class PersistentDataConfig : ScriptableObject
    {
        [Header("Persistent Data Settings")]
        [SerializeField] private SerializerType serializerType = SerializerType.Json;
        [SerializeField] private string fileExtension = "dat";
        [SerializeField] private string key = "";
        [SerializeField] private string iv = "";

        public SerializerType SerializerType => serializerType;
        public string FileExtension => fileExtension;
        public string Key => key;
        public string Iv => iv;
    }
}