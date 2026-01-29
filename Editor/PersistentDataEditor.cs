using System.IO;
using PersistentData.Runtime;
using UnityEditor;
using UnityEngine;

namespace PersistentData.Editor
{
    public sealed class PersistentDataEditor : EditorWindow
    {
        #region Constants
        private const string MenuItem = "Tools/PersistentData/PersistentDataEditor";
        private const string Name = "Persistent Data Editor";
        private const float MinWidth = 256;
        private const float MaxWidth = 512;
        private const float MinHeight = 128;
        private const float MaxHeight = 256;
        private const string ConfigAssetNotFoundMessage = "Config asset not found or could not be loaded.";
        private const string InitializeButtonText = "Initialize Persistent Data Service";
        private const string OpenPersistentDataButtonText = "Open Persistent Data Path";
        private const string DeleteAllSavedDataButtonText = "Delete All Saved Data";
        private const string ConfigFolderPath = "Assets/Resources/PersistentData";
        private const string ConfigAssetPath = ConfigFolderPath + "/PersistentDataConfig.asset";
        #endregion

        #region Fields
        private PersistentDataConfig _config;
        #endregion

        #region Core
        [MenuItem(MenuItem)]
        public static void ShowWindow()
        {
            PersistentDataEditor editor = GetWindow<PersistentDataEditor>(Name);
            editor.minSize = new Vector2(MinWidth, MinHeight);
            editor.maxSize = new Vector2(MaxWidth, MaxHeight);

            editor.LoadOrCreateConfig();
        }
        private void OnEnable() => LoadOrCreateConfig();
        private void OnGUI()
        {
            if (!_config)
            {
                EditorGUILayout.HelpBox(ConfigAssetNotFoundMessage, MessageType.Error);
                
                return;
            }

            DrawConfigEditor();

            GUILayout.Space(10);
            
            if (GUILayout.Button(InitializeButtonText))
                PersistentDataServiceUtilities.Initialize();

            if (GUILayout.Button(OpenPersistentDataButtonText))
                EditorUtility.RevealInFinder(Application.persistentDataPath);

            if (GUILayout.Button(DeleteAllSavedDataButtonText))
                PersistentDataServiceUtilities.DeleteAll();

            if (!GUI.changed)
                return;
            
            EditorUtility.SetDirty(_config);
            
            AssetDatabase.SaveAssets();
        }
        #endregion

        #region Utilities
        private void LoadOrCreateConfig()
        {
            _config = AssetDatabase.LoadAssetAtPath<PersistentDataConfig>(ConfigAssetPath);

            if (_config != null)
                return;
            
            if (!Directory.Exists(ConfigFolderPath))
                Directory.CreateDirectory(ConfigFolderPath);

            _config = CreateInstance<PersistentDataConfig>();
            
            AssetDatabase.CreateAsset(_config, ConfigAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
        private void DrawConfigEditor()
        {
            SerializedObject so = new SerializedObject(_config);
            so.Update();

            EditorGUILayout.PropertyField(so.FindProperty("serializerType"));
            EditorGUILayout.PropertyField(so.FindProperty("fileExtension"));

            if (_config.SerializerType == SerializerType.EncryptedJson)
            {
                EditorGUILayout.PropertyField(so.FindProperty("key"));
                EditorGUILayout.PropertyField(so.FindProperty("iv"));
            }

            so.ApplyModifiedProperties();
        }
        #endregion
    }
}