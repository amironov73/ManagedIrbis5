using System;
using System.IO;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

#nullable enable

// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class ParFileTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        public void ParFile_Constructor_1()
        {
            var parFile = new ParFile();
            Assert.IsNull(parFile.AnyPath);
            Assert.IsNull(parFile.CntPath);
            Assert.IsNull(parFile.ExtPath);
            Assert.IsNull(parFile.IfpPath);
            Assert.IsNull(parFile.L01Path);
            Assert.IsNull(parFile.L02Path);
            Assert.IsNull(parFile.MstPath);
            Assert.IsNull(parFile.N01Path);
            Assert.IsNull(parFile.N02Path);
            Assert.IsNull(parFile.PftPath);
            Assert.IsNull(parFile.XrfPath);
        }

        [TestMethod]
        public void ParFile_Constructor_2()
        {
            const string mstPath = "IBIS";

            var parFile = new ParFile(mstPath);
            Assert.AreEqual(mstPath, parFile.AnyPath);
            Assert.AreEqual(mstPath, parFile.CntPath);
            Assert.AreEqual(mstPath, parFile.ExtPath);
            Assert.AreEqual(mstPath, parFile.IfpPath);
            Assert.AreEqual(mstPath, parFile.L01Path);
            Assert.AreEqual(mstPath, parFile.L02Path);
            Assert.AreEqual(mstPath, parFile.MstPath);
            Assert.AreEqual(mstPath, parFile.N01Path);
            Assert.AreEqual(mstPath, parFile.N02Path);
            Assert.AreEqual(mstPath, parFile.PftPath);
            Assert.AreEqual(mstPath, parFile.XrfPath);
        }

        private void _TestSerialization
            (
                ParFile first
            )
        {
            var bytes = first.SaveToMemory();

            var second = bytes
                .RestoreObjectFromMemory<ParFile>().ThrowIfNull();

            Assert.AreEqual(first.AnyPath, second.AnyPath);
            Assert.AreEqual(first.CntPath, second.CntPath);
            Assert.AreEqual(first.ExtPath, second.ExtPath);
            Assert.AreEqual(first.IfpPath, second.IfpPath);
            Assert.AreEqual(first.L01Path, second.L01Path);
            Assert.AreEqual(first.L02Path, second.L02Path);
            Assert.AreEqual(first.MstPath, second.MstPath);
            Assert.AreEqual(first.N01Path, second.N01Path);
            Assert.AreEqual(first.N02Path, second.N02Path);
            Assert.AreEqual(first.PftPath, second.PftPath);
            Assert.AreEqual(first.XrfPath, second.XrfPath);
        }

        [TestMethod]
        public void ParFile_Serialization_1()
        {
            var parFile = new ParFile();
            _TestSerialization(parFile);

            parFile.MstPath = @".\datai\ibis\";
            _TestSerialization(parFile);
        }

        private ParFile _GetParFile ()
        {
            var fileName = Path.Combine
                (
                    TestDataPath,
                    "ibis.par"
                );

            var result = ParFile.ParseFile(fileName);

            return result;
        }

        [TestMethod]
        public void ParFile_ParseFile_1()
        {
            var parFile = _GetParFile();
            Assert.AreEqual(@".\datai\ibis\", parFile.MstPath);
            _TestSerialization(parFile);
        }

        [TestMethod]
        public void ParFile_WriteText_1()
        {
            var parFile = _GetParFile();

            var writer = new StringWriter();
            parFile.WriteText(writer);
            var text = writer.ToString();
            Assert.IsTrue(text.Length > 0);
        }

        [TestMethod]
        public void ParFile_ReadDictionary_1()
        {
            var text = "1=IBIS" + Environment.NewLine
                + "2=IBIS" + Environment.NewLine
                + Environment.NewLine
                + "3=IBIS" + Environment.NewLine
                + "4=IBIS" + Environment.NewLine
                + "5=IBIS" + Environment.NewLine
                + "6=IBIS" + Environment.NewLine
                + "7=IBIS" + Environment.NewLine
                + "8=IBIS" + Environment.NewLine
                + "9=IBIS" + Environment.NewLine
                + "10=IBIS" + Environment.NewLine;

            TextReader reader = new StringReader(text);
            var dictionary = ParFile.ReadDictionary(reader);
            Assert.AreEqual(10, dictionary.Count);
            Assert.AreEqual("IBIS", dictionary[1]);
        }

        //[TestMethod]
        //[ExpectedException(typeof(FormatException))]
        //public void ParFile_ReadDictionary_Exception_1()
        //{
        //    string text = "1=IBIS" + Environment.NewLine
        //        + "2=IBIS" + Environment.NewLine
        //        + Environment.NewLine
        //        + "3=" + Environment.NewLine
        //        + "4=IBIS" + Environment.NewLine
        //        + "5=IBIS" + Environment.NewLine
        //        + "6=IBIS" + Environment.NewLine
        //        + "7=IBIS" + Environment.NewLine
        //        + "8=IBIS" + Environment.NewLine
        //        + "9=IBIS" + Environment.NewLine
        //        + "10=IBIS" + Environment.NewLine;

        //    TextReader reader = new StringReader(text);
        //    Dictionary<int, string> dictionary
        //        = ParFile.ReadDictionary(reader);
        //    Assert.AreEqual(10, dictionary.Count);
        //    Assert.AreEqual("IBIS", dictionary[1]);
        //}

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void ParFile_ReadDictionary_Exception_2()
        {
            var text = "1=IBIS" + Environment.NewLine
                                + "2=IBIS" + Environment.NewLine
                                + Environment.NewLine
                                + "3" + Environment.NewLine
                                + "4=IBIS" + Environment.NewLine
                                + "5=IBIS" + Environment.NewLine
                                + "6=IBIS" + Environment.NewLine
                                + "7=IBIS" + Environment.NewLine
                                + "8=IBIS" + Environment.NewLine
                                + "9=IBIS" + Environment.NewLine
                                + "10=IBIS" + Environment.NewLine;

            TextReader reader = new StringReader(text);
            var dictionary = ParFile.ReadDictionary(reader);
            Assert.AreEqual(10, dictionary.Count);
            Assert.AreEqual("IBIS", dictionary[1]);
        }

        //[TestMethod]
        //[ExpectedException(typeof(FormatException))]
        //public void ParFile_ReadDictionary_Exception_3()
        //{
        //    string text = "1=IBIS" + Environment.NewLine
        //        + "2=IBIS" + Environment.NewLine
        //        + Environment.NewLine
        //        + "3 = " + Environment.NewLine
        //        + "4=IBIS" + Environment.NewLine
        //        + "5=IBIS" + Environment.NewLine
        //        + "6=IBIS" + Environment.NewLine
        //        + "7=IBIS" + Environment.NewLine
        //        + "8=IBIS" + Environment.NewLine
        //        + "9=IBIS" + Environment.NewLine
        //        + "10=IBIS" + Environment.NewLine;

        //    TextReader reader = new StringReader(text);
        //    Dictionary<int, string> dictionary
        //        = ParFile.ReadDictionary(reader);
        //    Assert.AreEqual(10, dictionary.Count);
        //    Assert.AreEqual("IBIS", dictionary[1]);
        //}

        //[TestMethod]
        //[ExpectedException(typeof(FormatException))]
        //public void ParFile_ReadDictionary_Exception_4()
        //{
        //    string text = "1=IBIS" + Environment.NewLine
        //        + "2=IBIS" + Environment.NewLine
        //        + Environment.NewLine
        //        + "4=IBIS" + Environment.NewLine
        //        + "5=IBIS" + Environment.NewLine
        //        + "6=IBIS" + Environment.NewLine
        //        + "7=IBIS" + Environment.NewLine
        //        + "8=IBIS" + Environment.NewLine
        //        + "9=IBIS" + Environment.NewLine
        //        + "10=IBIS" + Environment.NewLine;

        //    TextReader reader = new StringReader(text);
        //    Dictionary<int, string> dictionary
        //        = ParFile.ReadDictionary(reader);
        //    Assert.AreEqual(10, dictionary.Count);
        //    Assert.AreEqual("IBIS", dictionary[1]);
        //}

        [TestMethod]
        public void ParFile_Verify_1()
        {
            var parFile = _GetParFile();
            Assert.IsTrue(parFile.Verify(false));
        }

        [TestMethod]
        public void ParFile_WriteFile_1()
        {
            var first = _GetParFile();
            var fileName = Path.GetTempFileName();
            first.WriteFile(fileName);

            var second = ParFile.ParseFile(fileName);
            Assert.AreEqual(first.AnyPath, second.AnyPath);
            Assert.AreEqual(first.CntPath, second.CntPath);
            Assert.AreEqual(first.ExtPath, second.ExtPath);
            Assert.AreEqual(first.IfpPath, second.IfpPath);
            Assert.AreEqual(first.L01Path, second.L01Path);
            Assert.AreEqual(first.L02Path, second.L02Path);
            Assert.AreEqual(first.MstPath, second.MstPath);
            Assert.AreEqual(first.N01Path, second.N01Path);
            Assert.AreEqual(first.N02Path, second.N02Path);
            Assert.AreEqual(first.PftPath, second.PftPath);
            Assert.AreEqual(first.XrfPath, second.XrfPath);
        }

        [TestMethod]
        public void ParFile_ToDictionary_1()
        {
            var parFile = _GetParFile();
            var dictionary = parFile.ToDictionary();
            Assert.AreEqual(11, dictionary.Count);
            Assert.AreEqual(".\\datai\\ibis\\", dictionary[1]);
        }

        [TestMethod]
        public void ParFile_ToString_1()
        {
            var parFile = _GetParFile();
            Assert.AreEqual(".\\datai\\ibis\\", parFile.ToString());
        }
    }
}
