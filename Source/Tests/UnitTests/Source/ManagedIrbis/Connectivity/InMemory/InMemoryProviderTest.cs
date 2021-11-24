// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.InMemory;
using ManagedIrbis.Providers;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory
{
    [TestClass]
    public sealed class InMemoryProviderTest
        : Common.CommonUnitTest
    {
        private string _GetDatabasePath()
        {
            return Path.Combine (TestDataPath, "NoDataYet");
        }

        private string _GetTemporaryPath()
        {
            return Path.Combine (Path.GetTempPath(), "WriteHere");
        }

        private InMemoryProvider _GetProvider()
        {
            var resources = new InMemoryResourceProvider();
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            return new InMemoryProvider (resources, serviceProvider);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void InMemoryProvider_Construction_1()
        {
            var resources = new InMemoryResourceProvider();
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var provider = new InMemoryProvider (resources, serviceProvider);
            Assert.AreSame (resources, provider.Resources);
            Assert.IsNotNull (provider.Databases);
            Assert.IsFalse (provider.Connected);
        }

        [TestMethod]
        [Description ("Дамп всех баз данных")]
        [ExpectedException (typeof (NotImplementedException))]
        public void InMemoryProvider_DumpAll_1()
        {
            var provider = _GetProvider();
            var output = new StringWriter();
            provider.DumpAll (output);
            var dump = output.ToString();
            Assert.IsNotNull (dump);
        }

        [TestMethod]
        [Description ("Загрузка данных")]
        [ExpectedException (typeof (NotImplementedException))]
        public void InMemoryProvider_LoadData_1()
        {
            var provider = _GetProvider();
            var path = _GetDatabasePath();
            provider.LoadData (path);

        }

        [TestMethod]
        [Description ("Сохранение данных")]
        [ExpectedException (typeof (NotImplementedException))]
        public void InMemoryProvider_SaveData_1()
        {
            var provider = _GetProvider();
            var path = _GetTemporaryPath();
            provider.SaveData (path);

        }

        [TestMethod]
        [Description ("Конфигурация провайдера")]
        public void InMemoryProvider_Configure_1()
        {
            var provider = _GetProvider();
            const string configurationString = "some string";
            provider.Configure (configurationString);
        }

        [TestMethod]
        [Description ("Актуализация записи")]
        [ExpectedException (typeof (NotImplementedException))]
        public void InMemoryProvider_ActualizeRecord_1()
        {
            var provider = _GetProvider();
            var parameters = new ActualizeRecordParameters();
            provider.ActualizeRecord (parameters);
        }

        [TestMethod]
        [Description ("Подключение")]
        public void InMemoryProvider_Connect_1()
        {
            var provider = _GetProvider();
            provider.Connect();
            Assert.IsTrue (provider.Connected);
            Assert.AreEqual (0, provider.LastError);
        }

        [TestMethod]
        [Description ("Создание базы данных")]
        [ExpectedException (typeof (ArgumentException))]
        public void InMemoryProvider_CreateDatabase_1()
        {
            var provider = _GetProvider();
            var parameters = new CreateDatabaseParameters();
            provider.CreateDatabase (parameters);
        }

        [TestMethod]
        [Description ("Создание словаря")]
        [ExpectedException (typeof (NotImplementedException))]
        public void InMemoryProvider_CreateDictionary_1()
        {
            var provider = _GetProvider();
            provider.CreateDictionary();
        }

        [TestMethod]
        [Description ("Удаление базы данных")]
        [ExpectedException (typeof (IrbisException))]
        public void InMemoryProvider_DeleteDatabase_1()
        {
            var provider = _GetProvider();
            provider.DeleteDatabase ();
        }

        [TestMethod]
        [Description ("Отключение")]
        public void InMemoryProvider_Disconnect_1()
        {
            var provider = _GetProvider();
            provider.Connect();
            Assert.IsTrue (provider.Connected);
            Assert.AreEqual (0, provider.LastError);
            provider.Disconnect();
            Assert.IsFalse (provider.Connected);
            Assert.AreEqual (0, provider.LastError);
        }

        [TestMethod]
        [Description ("Проверка существования файла")]
        [ExpectedException (typeof (ArgumentException))]
        public void InMemoryProvider_FileExist_1()
        {
            var provider = _GetProvider();
            var specification = new FileSpecification();
            var result = provider.FileExist (specification);
            Assert.IsFalse (result);
        }
    }
}
