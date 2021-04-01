// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Runtime;

using ManagedIrbis.Gbl;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void GblFile_Construction_1()
        {
            var gblFile = new GblFile();

            Assert.AreEqual(0, gblFile.Statements.Count);
            Assert.AreEqual(0, gblFile.Parameters.Count);

            var statement = new GblStatement
            {
                Command = GblCode.Add,
                Parameter1 = "300",
                Format1 = "Add field 300"
            };

            gblFile.Statements.Add(statement);
        }

        private void _TestSerialization
            (
                GblFile first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<GblFile>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Statements.Count, second!.Statements.Count);
            for (var i = 0; i < first.Statements.Count; i++)
            {
                Assert.AreEqual(first.Statements[i].Command, second.Statements[i].Command);
                Assert.AreEqual(first.Statements[i].Parameter1, second.Statements[i].Parameter1);
                Assert.AreEqual(first.Statements[i].Parameter2, second.Statements[i].Parameter2);
                Assert.AreEqual(first.Statements[i].Format1, second.Statements[i].Format1);
                Assert.AreEqual(first.Statements[i].Format2, second.Statements[i].Format2);
            }

            Assert.AreEqual(first.Parameters.Count, second.Parameters.Count);
            for (var i = 0; i < first.Parameters.Count; i++)
            {
                Assert.AreEqual(first.Parameters[i].Name, second.Parameters[i].Name);
                Assert.AreEqual(first.Parameters[i].Value, second.Parameters[i].Value);
            }
        }

        private GblFile _GetGbl()
        {
            var result = new GblFile();

            result.Statements.Add(new GblStatement
            {
                Command = GblCode.Add,
                Parameter1 = "300",
                Format1 = "Add field 300"
            });
            result.Statements.Add(new GblStatement
            {
                Command = GblCode.Delete,
                Parameter1 = "300",
                Parameter2 = "*"
            });
            result.Statements.Add(new GblStatement
            {
                Command = "NOP"
            });

            return result;
        }

        [TestMethod]
        public void GblFile_Serialization_1()
        {
            var gbl = new GblFile();
            _TestSerialization(gbl);

            gbl = _GetGbl();
            _TestSerialization(gbl);
        }

        [TestMethod]
        public void GblFile_ParseLocalFile_1()
        {
            var fileName = Path.Combine
                (
                    TestDataPath,
                    "Del910s.gbl"
                );

            var gbl = GblFile.ParseLocalFile
                (
                    fileName,
                    IrbisEncoding.Ansi
                );

            Assert.IsNotNull(gbl);
            Assert.AreEqual(3, gbl.Statements.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);
            Assert.AreEqual("DEL", gbl.Statements[0].Command);

            _TestSerialization(gbl);
        }

        /*

        [TestMethod]
        public void TestGblFileToJson()
        {
            var gbl = _GetGbl();

            string actual = gbl.ToJson()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            const string expected = "{'items':[{'command':'ADD','parameter1':'300','format1':'Add field 300'},{'command':'DEL','parameter1':'300','parameter2':'*'},{'command':'NOP'}]}";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GblFile_FromJson_1()
        {
            var text = "{'items':[{'command':'ADD','parameter1':'300','format1':'Add field 300'},{'command':'DEL','parameter1':'300','parameter2':'*'},{'command':'NOP'}]}"
                .Replace("'", "\"");

            GblFile gbl = GblUtility.FromJson(text);

            Assert.AreEqual(3, gbl.Statements.Count);
            Assert.AreEqual("ADD", gbl.Statements[0].Command);
        }

        */

        [TestMethod]
        public void TestGblFileToXml()
        {
            var gbl = _GetGbl();

            var actual = gbl.ToXml()
                .Replace("\r", "").Replace("\n", "")
                .Replace("\"", "'");
            const string expected = "<?xml version='1.0' encoding='utf-16'?><gbl xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>  <item>    <command>ADD</command>    <parameter1>300</parameter1>    <format1>Add field 300</format1>  </item>  <item>    <command>DEL</command>    <parameter1>300</parameter1>    <parameter2>*</parameter2>  </item>  <item>    <command>NOP</command>  </item></gbl>";

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void GblFile_FromXml_1()
        {
            var text = "<?xml version='1.0' encoding='utf-16'?><gbl xmlns:xsi='http://www.w3.org/2001/XMLSchema-instance' xmlns:xsd='http://www.w3.org/2001/XMLSchema'>  <item>    <command>ADD</command>    <parameter1>300</parameter1>    <format1>Add field 300</format1>  </item>  <item>    <command>DEL</command>    <parameter1>300</parameter1>    <parameter2>*</parameter2>  </item>  <item>    <command>NOP</command>  </item></gbl>"
                .Replace("'", "\"");

            var gbl = GblUtility.FromXml(text);

            Assert.IsNotNull(gbl);
            Assert.AreEqual(3, gbl.Statements.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);
        }

        /*

        [TestMethod]
        public void GblFile_ParseLocalJsonFile_1()
        {
            string fileName = Path.Combine
                (
                    TestDataPath,
                    "test-gbl.json"
                );

            GblFile gbl = GblUtility.ParseLocalJsonFile(fileName);

            Assert.IsNotNull(gbl);
            Assert.AreEqual(3, gbl.Statements.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);
        }

        */

        [TestMethod]
        public void GblFile_ParseLocalXmlFile_1()
        {
            var fileName = Path.Combine
                (
                    TestDataPath,
                    "test-gbl.xml"
                );

            var gbl = GblUtility.ParseLocalXmlFile(fileName);

            Assert.IsNotNull(gbl);
            Assert.AreEqual(3, gbl.Statements.Count);
            Assert.AreEqual(0, gbl.Parameters.Count);
        }

        [TestMethod]
        public void GblFile_Verify_1()
        {
            var gbl = _GetGbl();

            Assert.IsTrue(gbl.Verify(false));
        }
    }
}
