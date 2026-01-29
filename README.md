# PersistentData
PersistentData is a flexible and extendable data persistence system designed for Unity applications. It provides a simple API for saving, loading, and deleting data using various serialization strategies, including encrypted and unencrypted JSON.

## Features
PersistentData offers a modular and secure data persistence solution:
* Save / Load / Delete / DeleteAll: Save and retrieve any serializable object type or delete them individually or all at once.
* Serializer Selection: Choose between default JSON and AES-encrypted JSON serializers.
* Custom Key & IV Support: For encrypted serialization, specify your own key and IV values to secure data.
* Editor Utility Tools: Manage saved data through an intuitive Editor window. This includes:
  * Opening the save file location.
  * Clearing all saved data.
  * Changing serializer settings via a `ScriptableObject` config.
  * Automatically creating and updating the config asset at `Assets/Resources/PersistentData/PersistentDataConfig.asset`.
* Static Utility Access: Use PersistentDataServiceUtilities to perform all operations statically. You can initialize the system in two ways:
  * Automatically, using the `PersistentDataConfig` asset from `Resources/PersistentData`.
  * Manually, by providing the serializer type, file extension, and (if needed) encryption Key & IV directly via parameters.

## Getting Started
Install via UPM with git URL

`https://github.com/Emre-Emiroglu/PersistentData.git`

Clone the repository
```bash
git clone https://github.com/Emre-Emiroglu/PersistentData.git
```
This project is developed using Unity version 6000.2.6f2.

## Usage
* Saving Data:
    ```csharp
    MyData data = new MyData();
    PersistentDataUtilities.Save("saveKey", data);
    ```
* Loading Data:
    ```csharp
    MyData loaded = PersistentDataUtilities.Load<MyData>("saveKey");
    ```
* Deleting Data:
    ```csharp
    PersistentDataUtilities.Delete("saveKey");
    ```
* Deleting All Data:
    ```csharp
    PersistentDataUtilities.DeleteAll();
    ```
* Changing Serializer Type (Editor Only):
  * Open the Editor window: `Tools > PersistentData > PersistentDataEditor`.
  * Select between `JsonSerializer` and `EncryptedJsonSerializer`.
  * For `EncryptedJsonSerializer`, input your custom AES Key and IV in Base64.
  * All settings are saved into a ScriptableObject located at: `Assets/Resources/PersistentData/PersistentDataConfig.asset`.
  * This file is auto-created if missing.

* Error Handling:
  * If deserialization fails due to incompatible types or corrupted data, an exception is thrown.
  * If loading a nonexistent key, a default value is returned.
  * Encrypted serializer requires both Key and IV to be valid 16-character Base64 strings.

## Dependencies
* Newtonsoft JSON

## Acknowledgments
Special thanks to the Unity community for their invaluable resources and tools.

For more information, visit the GitHub repository.
