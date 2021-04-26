﻿// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Workspace;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Worksheet
{
    [TestClass]
    public class WssFileTest
        : Common.CommonUnitTest
    {
        private WssFile _GetFile()
        {
            return new WssFile
            {
                Name = "951m.wss",
                Items =
                {
                    new WorksheetItem
                    {
                        Tag = "A",
                        Title = "Имя файла",
                        EditMode = "0"
                    },
                    new WorksheetItem
                    {
                        Tag = "I",
                        Title = "URL (Адрес в Internet)",
                        EditMode = "1",
                        InputInfo = "951.mnu"
                    }
                }
            };
        }

        [TestMethod]
        public void WssFile_Construction_1()
        {
            var file = new WssFile();
            Assert.IsNull(file.Name);
            Assert.IsNotNull(file.Items);
            Assert.AreEqual(0, file.Items.Count);
        }

        private void _TestSerialization
            (
                WssFile first
            )
        {
            byte[] bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<WssFile>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Name, second!.Name);
            Assert.AreEqual(first.Items.Count, second.Items.Count);
            for (var i = 0; i < first.Items.Count; i++)
            {
                Assert.AreEqual(first.Items[i].Tag, second.Items[i].Tag);
                Assert.AreEqual(first.Items[i].Title, second.Items[i].Title);
            }
        }

        [TestMethod]
        public void WssFile_Serialization_1()
        {
            var file = new WssFile();
            _TestSerialization(file);
        }

        [TestMethod]
        public void WssFile_ReadLocalFile_1()
        {
            var fileName = Path.Combine
                (
                    TestDataPath,
                    "951m.wss"
                );
            var file = WssFile.ReadLocalFile(fileName);

            Assert.IsNotNull(file.Items);
            Assert.AreEqual("951m.wss", file.Name);
            Assert.AreEqual(5, file.Items.Count);

            _TestSerialization(file);
        }

        [Ignore]
        [TestMethod]
        public void WssFile_ReadFromServer_1()
        {
            var mock = new Mock<ISyncProvider>();
            using var provider = mock.Object;
            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "951m.wss"
            };
            var file = WssFile.ReadFromServer(provider, specification);
            Assert.IsNotNull(file);
            Assert.IsNotNull(file!.Items);
            Assert.AreEqual(5, file.Items.Count);
        }

        [TestMethod]
        public void WssFile_ReadFromServer_2()
        {
            var mock = new Mock<ISyncProvider>();
            using var provider = mock.Object;
            var specification = new FileSpecification
            {
                Path = IrbisPath.MasterFile,
                Database = "IBIS",
                FileName = "nosuchfile.wss"
            };
            var file = WssFile.ReadFromServer(provider, specification);
            Assert.IsNull(file);
        }

        [TestMethod]
        public void WssFile_Verify_1()
        {
            var file = new WssFile();
            Assert.IsTrue(file.Verify(false));

            file = _GetFile();
            Assert.IsTrue(file.Verify(false));
        }

        [TestMethod]
        public void WssFile_ToXml_1()
        {
            var file = new WssFile();
            Assert.AreEqual("<wss />", XmlUtility.SerializeShort(file));

            file = _GetFile();
            Assert.AreEqual("<wss name=\"951m.wss\"><items><item><tag>A</tag><title>Имя файла</title><input-mode>0</input-mode></item><item><tag>I</tag><title>URL (Адрес в Internet)</title><input-mode>1</input-mode><input-info>951.mnu</input-info></item></items></wss>", XmlUtility.SerializeShort(file));
        }

        [Ignore]
        [TestMethod]
        public void WssFile_ToJson_1()
        {
            var file = new WssFile();
            Assert.AreEqual("{\"items\":[]}", JsonUtility.SerializeShort(file));

            file = _GetFile();
            Assert.AreEqual("{'name':'951m.wss','items':[{'tag':'A','title':'Имя файла','input-mode':'0'},{'tag':'I','title':'URL (Адрес в Internet)','input-mode':'1','input-info':'951.mnu'}]}", JsonUtility.SerializeShort(file));
        }

        [TestMethod]
        public void WssFile_ToString_1()
        {
            var file = new WssFile();
            Assert.AreEqual("(null)", file.ToString());

            file = _GetFile();
            Assert.AreEqual("951m.wss", file.ToString());
        }
    }
}
