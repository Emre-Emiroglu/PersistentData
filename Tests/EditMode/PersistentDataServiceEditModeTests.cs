using System.IO;
using NUnit.Framework;
using PersistentData.Runtime;

namespace PersistentData.Tests.EditMode
{
    public sealed class PersistentDataServiceEditModeTests
    {
        private const string TestFileExtension = "test";
        private const string Key = "bW9ja0tleUhlcmUxMjM0NQ==";
        private const string Iv = "bW9ja0lWMTIzNDU2Nzg5MA==";
        private string _testPath;
        private IPersistentDataService _service;
        
        [SetUp]
        public void SetUp()
        {
            _testPath = Path.Combine(Path.GetTempPath(), "PersistentDataTests");
            Directory.CreateDirectory(_testPath);
            _service = new PersistentDataService(new JsonSerializer(), TestFileExtension);

            typeof(PersistentDataService)
                .GetField("_dataPath",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(_service, _testPath);
        }
        
        [TearDown]
        public void TearDown()
        {
            if (Directory.Exists(_testPath))
                Directory.Delete(_testPath, true);
        }
        
        [Test]
        public void Save_And_Load_Works()
        {
            TestData data = new TestData { Value = 123 };
            
            _service.Save("data", data);

            TestData loaded = _service.Load<TestData>("data");

            Assert.AreEqual(data.Value, loaded.Value);
        }

        [Test]
        public void Delete_Works()
        {
            _service.Save("data", new TestData { Value = 1 });
            _service.Delete("data");

            Assert.Throws<FileNotFoundException>(() => _service.Load<TestData>("data"));
        }

        [Test]
        public void DeleteAll_Works()
        {
            _service.Save("data1", new TestData());
            _service.Save("data2", new TestData());

            _service.DeleteAll();

            Assert.Throws<FileNotFoundException>(() => _service.Load<TestData>("data1"));
            Assert.Throws<FileNotFoundException>(() => _service.Load<TestData>("data2"));
        }
        
        [Test]
        public void SaveAndLoad_WithEncryptedJson_Works()
        {
            PersistentDataService encryptedService =
                new PersistentDataService(new EncryptedJsonSerializer(Key, Iv), TestFileExtension);

            string key = "enc_edit_key1";
            string expected = "encrypted value";

            encryptedService.Save(key, expected);
            
            string actual = encryptedService.Load<string>(key);

            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Delete_WithEncryptedJson_Works()
        {
            PersistentDataService encryptedService =
                new PersistentDataService(new EncryptedJsonSerializer(Key, Iv), TestFileExtension);

            string key = "enc_edit_key2";
            
            encryptedService.Save(key, "temp");

            encryptedService.Delete(key);

            Assert.Throws<FileNotFoundException>(() => encryptedService.Load<string>(key));
        }

        [Test]
        public void DeleteAll_WithEncryptedJson_Works()
        {
            PersistentDataService encryptedService =
                new PersistentDataService(new EncryptedJsonSerializer(Key, Iv), TestFileExtension);
            
            encryptedService.Save("enc_file1", 100);
            encryptedService.Save("enc_file2", 200);

            encryptedService.DeleteAll();

            Assert.Throws<FileNotFoundException>(() => encryptedService.Load<int>("enc_file1"));
            Assert.Throws<FileNotFoundException>(() => encryptedService.Load<int>("enc_file2"));
        }

        private class TestData
        {
            public int Value;
        }
    }
}