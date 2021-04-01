// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Fst;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fst
{
    [TestClass]
    public class FstLineTest
    {
        private FstLine _GetLine()
        {
            return new FstLine
            {
                LineNumber = 4,
                Tag = 12252,
                Method = FstIndexMethod.Method8,
                Format = "MHL,'/K=/'(v225^i,|%|d225/)"
            };
        }

        [TestMethod]
        public void FstLine_Construciton_1()
        {
            var line = new FstLine();
            Assert.AreEqual(0, line.LineNumber);
            Assert.AreEqual(0, line.Tag);
            Assert.AreEqual(FstIndexMethod.Method0, line.Method);
            Assert.IsNull(line.Format);
            Assert.IsNull(line.UserData);
        }

        private void _TestSerialization
            (
                FstLine first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<FstLine>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.LineNumber, second!.LineNumber);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.Method, second.Method);
            Assert.AreEqual(first.Format, second.Format);
            Assert.IsNull(second.UserData);
        }

        [TestMethod]
        public void FstLine_Serialization_1()
        {
            var line = new FstLine();
            _TestSerialization(line);

            line.UserData = "User data";
            _TestSerialization(line);

            line = _GetLine();
            _TestSerialization(line);
        }

        [TestMethod]
        public void FstLine_Verify_1()
        {
            var line = new FstLine();
            Assert.IsFalse(line.Verify(false));

            line = _GetLine();
            Assert.IsTrue(line.Verify(false));
        }

        [TestMethod]
        public void FstLine_ToFormat_1()
        {
            var line = _GetLine();
            Assert.AreEqual("mpl,\'12252\',/,MHL,\'/K=/\'(v225^i,|%|d225/),\'\a\'", line.ToFormat());
        }

        [Ignore]
        [TestMethod]
        public void FstLine_ToXml_1()
        {
            var line = new FstLine();
            Assert.AreEqual("<line method=\"Method0\" />", XmlUtility.SerializeShort(line));

            line = _GetLine();
            Assert.AreEqual("<line tag=\"12252\" method=\"Method8\"><format>MHL,\'/K=/\'(v225^i,|%|d225/)</format></line>", XmlUtility.SerializeShort(line));
        }

        [Ignore]
        [TestMethod]
        public void FstLine_ToJson_1()
        {
            var line = new FstLine();
            Assert.AreEqual("{'method':0}", JsonUtility.SerializeShort(line));

            line = _GetLine();
            Assert.AreEqual("{'tag':12252,'method':8,'format':'MHL,\\'/K=/\\'(v225^i,|%|d225/)'}", JsonUtility.SerializeShort(line));
        }

        [TestMethod]
        public void FstLine_ToString_1()
        {
            var line = new FstLine();
            Assert.AreEqual("0 0 (null)", line.ToString());

            line = _GetLine();
            Assert.AreEqual("12252 8 MHL,'/K=/'(v225^i,|%|d225/)", line.ToString());
        }
    }
}
