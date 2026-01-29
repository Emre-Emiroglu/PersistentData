using System.Collections;
using System.IO;
using NUnit.Framework;
using PersistentData.Runtime;
using UnityEngine.TestTools;

namespace PersistentData.Tests.PlayMode
{
    public sealed class PersistentDataServicePlayModeTests
    {
        private const string TestFileExtension = "test";
        private const string Key = "bW9ja0tleUhlcmUxMjM0NQ==";
        private const string Iv = "bW9ja0tleUhlcmUxMjM0NQ==";

        [SetUp]
        public void SetUp() =>
            PersistentDataServiceUtilities.Initialize(SerializerType.Json, TestFileExtension, "", "");

        [TearDown]
        public void TearDown() => PersistentDataServiceUtilities.DeleteAll();

        [UnityTest]
        public IEnumerator Save_And_Load_Works()
        {
            TestData test = new TestData { Message = "Hello" };
            
            PersistentDataServiceUtilities.Save("file1", test);
            
            yield return null;

            TestData loaded = PersistentDataServiceUtilities.Load<TestData>("file1");

            Assert.AreEqual(test.Message, loaded.Message);
        }

        [UnityTest]
        public IEnumerator Delete_Works()
        {
            PersistentDataServiceUtilities.Save("file1", new TestData { Message = "ToDelete" });
            PersistentDataServiceUtilities.Delete("file1");
            
            yield return null;

            Assert.Throws<FileNotFoundException>(() => PersistentDataServiceUtilities.Load<TestData>("file1"));
        }

        [UnityTest]
        public IEnumerator DeleteAll_Works()
        {
            PersistentDataServiceUtilities.Save("file1", new TestData());
            PersistentDataServiceUtilities.Save("file2", new TestData());
            
            yield return null;

            PersistentDataServiceUtilities.DeleteAll();

            Assert.Throws<FileNotFoundException>(() => PersistentDataServiceUtilities.Load<TestData>("file1"));
            Assert.Throws<FileNotFoundException>(() => PersistentDataServiceUtilities.Load<TestData>("file2"));
        }
        
        [UnityTest]
        public IEnumerator SaveAndLoad_WithEncryptedJson_Works_PlayMode()
        {
            PersistentDataServiceUtilities.Initialize(SerializerType.EncryptedJson, TestFileExtension, Key, Iv);

            string key = "enc_play_key1";
            string data = "playmode secret";

            PersistentDataServiceUtilities.Save(key, data);
            
            yield return null;

            string result = PersistentDataServiceUtilities.Load<string>(key);
            
            Assert.AreEqual(data, result);
        }

        [UnityTest]
        public IEnumerator Delete_WithEncryptedJson_Works_PlayMode()
        {
            PersistentDataServiceUtilities.Initialize(SerializerType.EncryptedJson, TestFileExtension, Key, Iv);

            string key = "enc_play_key2";
            
            PersistentDataServiceUtilities.Save(key, "to delete");
            
            yield return null;

            PersistentDataServiceUtilities.Delete(key);

            Assert.Throws<FileNotFoundException>(() => PersistentDataServiceUtilities.Load<string>(key));
        }

        [UnityTest]
        public IEnumerator DeleteAll_WithEncryptedJson_Works_PlayMode()
        {
            PersistentDataServiceUtilities.Initialize(SerializerType.EncryptedJson, TestFileExtension, Key, Iv);

            PersistentDataServiceUtilities.Save("enc_fileA", "A");
            PersistentDataServiceUtilities.Save("enc_fileB", "B");
            
            yield return null;

            PersistentDataServiceUtilities.DeleteAll();

            Assert.Throws<FileNotFoundException>(() => PersistentDataServiceUtilities.Load<string>("enc_fileA"));
            Assert.Throws<FileNotFoundException>(() => PersistentDataServiceUtilities.Load<string>("enc_fileB"));
        }

        private class TestData
        {
            public string Message;
        }
    }
}