// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using ManagedIrbis.Flc;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Flc
{
    [TestClass]
    public class FlcResultTest
    {
        [TestMethod]
        public void FlcResult_Construction_1()
        {
            var flc = new FlcResult();
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_1()
        {
            string? text = null;
            var flc = FlcResult.Parse(text);
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_2()
        {
            var flc = FlcResult.Parse(string.Empty);
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_2a()
        {
            var flc = FlcResult.Parse(" ");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_3()
        {
            var flc = FlcResult.Parse("0");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_3a()
        {
            var flc = FlcResult.Parse("0\r\n0\r\n0\r\n0");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_3b()
        {
            var flc = FlcResult.Parse("0 \r\n0  \r\n0   \r\n0\t");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_3c()
        {
            var flc = FlcResult.Parse("0\r\n\r\n0\r\n");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_3d()
        {
            var flc = FlcResult.Parse("0\r\n \r\n  \r\n");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(0, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_4()
        {
            var flc = FlcResult.Parse("0 Some message");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.OK, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(1, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_5()
        {
            var flc = FlcResult.Parse("1 Some message");
            Assert.IsFalse(flc.CanContinue);
            Assert.AreEqual(FlcStatus.Error, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(1, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_5a()
        {
            var flc = FlcResult.Parse("2 Some message");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.Warning, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(1, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_6()
        {
            var flc = FlcResult.Parse("0 First message\r\n1 Second message");
            Assert.IsFalse(flc.CanContinue);
            Assert.AreEqual(FlcStatus.Error, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(2, flc.Messages.Count);
        }

        [TestMethod]
        public void FlcResult_Parse_6a()
        {
            var flc = FlcResult.Parse("0 First message\r\n2 Second message");
            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(FlcStatus.Warning, flc.Status);
            Assert.IsNotNull(flc.Messages);
            Assert.AreEqual(2, flc.Messages.Count);
        }

        private void _TestSerialization
            (
                FlcResult first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<FlcResult>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Status, second!.Status);
            Assert.AreEqual(first.Messages.Count, second.Messages.Count);

            for (var i = 0; i < first.Messages.Count; i++)
            {
                Assert.AreEqual(first.Messages[i], second.Messages[i]);
            }
        }

        [TestMethod]
        public void FlcResult_Serialization_1()
        {
            var flc = new FlcResult();
            _TestSerialization(flc);

            flc.Status = FlcStatus.Warning;
            flc.Messages.Add("First message");
            _TestSerialization(flc);

            flc.Status = FlcStatus.Error;
            flc.Messages.Add("Second message");
            _TestSerialization(flc);
        }

        [TestMethod]
        public void FlcResult_ToXml_1()
        {
            var flc = new FlcResult();
            Assert.AreEqual("<flc-result status=\"OK\" />", XmlUtility.SerializeShort(flc));

            flc.Status = FlcStatus.Warning;
            flc.Messages.Add("First message");
            Assert.AreEqual("<flc-result status=\"Warning\"><message>First message</message></flc-result>", XmlUtility.SerializeShort(flc));

            flc.Status = FlcStatus.Error;
            flc.Messages.Add("Second message");
            Assert.AreEqual("<flc-result status=\"Error\"><message>First message</message><message>Second message</message></flc-result>", XmlUtility.SerializeShort(flc));
        }

        [Ignore]
        [TestMethod]
        public void FlcResult_ToJson_1()
        {
            var flc = new FlcResult();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(flc));

            flc.Status = FlcStatus.Warning;
            flc.Messages.Add("First message");
            Assert.AreEqual("{'status':2,'messages':['First message']}", JsonUtility.SerializeShort(flc));

            flc.Status = FlcStatus.Error;
            flc.Messages.Add("Second message");
            Assert.AreEqual("{'status':1,'messages':['First message','Second message']}", JsonUtility.SerializeShort(flc));
        }

        [Ignore]
        [TestMethod]
        public void FlcResult_ToString_1()
        {
            var flc = new FlcResult();
            Assert.AreEqual("Status: OK, No messages", flc.ToString());

            flc.Status = FlcStatus.Warning;
            flc.Messages.Add("First message");
            Assert.AreEqual("Status: Warning, Messages: First message", flc.ToString());

            flc.Status = FlcStatus.Error;
            flc.Messages.Add("Second message");
            Assert.AreEqual("Status: Error, Messages: First message\nSecond message", flc.ToString().DosToUnix());
        }
    }
}
