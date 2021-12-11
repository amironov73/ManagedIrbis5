// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using ManagedIrbis.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory
{
    [TestClass]
    public sealed class InMemoryResourceProviderTest
        : Common.CommonUnitTest
    {
        private string _GetDataiPath()
        {
            return Path.Combine (Irbis64RootPath, "Datai");
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void InMemoryResourceProvider_Constructor_1()
        {
            var provider = new InMemoryResourceProvider();
            Assert.IsFalse (provider.ReadOnly);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void InMemoryResourceProvider_Constructor_2()
        {
            var provider = new InMemoryResourceProvider (true);
            Assert.IsTrue (provider.ReadOnly);
        }

        [TestMethod]
        [Description ("Восстановление состояния")]
        public void InMemoryResourceProvider_RestoreFrom_1()
        {
            var provider = new InMemoryResourceProvider();
            var dataiPath = _GetDataiPath();
            provider.RestoreFrom (dataiPath);
            var files = provider.ListResources (".");
            Assert.AreEqual (5, files.Length);
        }

        [TestMethod]
        [Description ("Дамп")]
        [ExpectedException (typeof (NotImplementedException))]
        public void InMemoryResourceProvider_Dump_1()
        {
            var provider = new InMemoryResourceProvider();
            using var output = new StringWriter();
            provider.Dump (output);
            var dump = output.ToString();
            Assert.IsNotNull (dump);
        }

        [TestMethod]
        [Description ("Перечисление ресурсов")]
        public void InMemoryResourceProvider_ListResources_1()
        {
            var provider = new InMemoryResourceProvider();
            var resources = provider.ListResources (".");
            Assert.IsNotNull (resources);
        }

        [TestMethod]
        [Description ("Чтение ресурса: без путей")]
        public void InMemoryResourceProvider_ReadResource_1()
        {
            var provider = new InMemoryResourceProvider();
            var dataiPath = _GetDataiPath();
            provider.RestoreFrom (dataiPath);
            var result = provider.ReadResource ("ibis.par");
            Assert.IsNotNull (result);

            result = provider.ReadResource ("IBIS/fo.mnu");
            Assert.IsNotNull (result);
        }

        [TestMethod]
        [Description ("Чтение ресурса: с путем")]
        public void InMemoryResourceProvider_ReadResource_2()
        {
            var provider = new InMemoryResourceProvider();
            var dataiPath = _GetDataiPath();
            provider.RestoreFrom (dataiPath);
            var result = provider.ReadResource ("IBIS/fo.mnu");
            Assert.IsNotNull (result);
        }

        [Ignore]
        [TestMethod]
        [Description ("Чтение ресурса: с путем")]
        public void InMemoryResourceProvider_ReadResource_3()
        {
            var provider = new InMemoryResourceProvider();
            var dataiPath = _GetDataiPath();
            provider.RestoreFrom (dataiPath);
            var result = provider.ReadResource (@"IBIS\fo.mnu");
            Assert.IsNotNull (result);
        }

        [TestMethod]
        [Description ("Чтение ресурса: с путем")]
        public void InMemoryResourceProvider_ReadResource_4()
        {
            var provider = new InMemoryResourceProvider();
            var dataiPath = _GetDataiPath();
            provider.RestoreFrom (dataiPath);
            var result = provider.ReadResource ("NOSUCHBASE/fo.mnu");
            Assert.IsNull (result);
        }

        [TestMethod]
        [Description ("Чтение ресурса: с путем")]
        public void InMemoryResourceProvider_ReadResource_5()
        {
            var provider = new InMemoryResourceProvider();
            var dataiPath = _GetDataiPath();
            provider.RestoreFrom (dataiPath);
            var result = provider.ReadResource ("IBIS/nosuchfile.mnu");
            Assert.IsNull (result);
        }

        [TestMethod]
        [Description ("Проверка существования ресурса")]
        public void InMemoryResourceProvider_ResourceExists_1()
        {
            var provider = new InMemoryResourceProvider();
            var dataiPath = _GetDataiPath();
            provider.RestoreFrom (dataiPath);
            var result = provider.ResourceExists ("ibis.par");
            Assert.IsTrue (result);

            result = provider.ResourceExists ("notexist.par");
            Assert.IsFalse (result);
        }

        [TestMethod]
        [Description ("Проверка существования ресурса")]
        public void InMemoryResourceProvider_ResourceExists_2()
        {
            var provider = new InMemoryResourceProvider();
            var result = provider.ResourceExists ("nosuchdir/nosuchfile.txt");
            Assert.IsFalse (result);
        }

        [TestMethod]
        [Description ("Запись ресурса")]
        public void InMemoryResourceProvider_WriteResource_1()
        {
            const string canary = "canary.txt";
            const string content = "content";
            var provider = new InMemoryResourceProvider();
            Assert.IsFalse (provider.ResourceExists (canary));
            var result = provider.WriteResource (canary, content);
            Assert.IsTrue (result);
            Assert.IsTrue (provider.ResourceExists (canary));
            var read = provider.ReadResource (canary);
            Assert.AreEqual (content, read);
        }

        [TestMethod]
        [Description ("Запись ресурса")]
        public void InMemoryResourceProvider_WriteResource_2()
        {
            const string canary = "canary.txt";
            const string content = "content";
            var provider = new InMemoryResourceProvider (true);
            Assert.IsFalse (provider.ResourceExists (canary));
            var result = provider.WriteResource (canary, content);
            Assert.IsFalse (result);
            Assert.IsFalse (provider.ResourceExists (canary));
            var read = provider.ReadResource (canary);
            Assert.IsNull (read);
        }

        [TestMethod]
        [Description ("Запись ресурса: удаление")]
        public void InMemoryResourceProvider_WriteResource_3()
        {
            const string ibis = "ibis.par";
            var provider = new InMemoryResourceProvider ();
            Assert.IsFalse (provider.ResourceExists (ibis));
            var result = provider.WriteResource (ibis, "Hello");
            Assert.IsTrue (result);
            Assert.IsTrue (provider.ResourceExists (ibis));
            result = provider.WriteResource (ibis, null);
            Assert.IsTrue (result);
            Assert.IsFalse (provider.ResourceExists (ibis));
            var read = provider.ReadResource (ibis);
            Assert.IsNull (read);
        }
    }
}
