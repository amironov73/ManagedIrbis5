// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using AM.Runtime;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl
{
    [TestClass]
    public class GblParameterTest
    {
        [TestMethod]
        public void GblParameter_Construction_1()
        {
            var parameter = new GblParameter();

            Assert.AreEqual(null, parameter.Name);
            Assert.AreEqual(null, parameter.Value);
        }

        [TestMethod]
        public void GblParameter_Decode_1()
        {
            const string text = "Value\r\nName";
            var reader = new StringReader(text);
            var parameter = GblParameter.Decode(reader);

            Assert.AreEqual("Name", parameter.Name);
            Assert.AreEqual("Value", parameter.Value);
        }

        private void _TestSerialization
            (
                GblParameter first
            )
        {
            byte[] bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<GblParameter>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Name, second!.Name);
            Assert.AreEqual(first.Value, second.Value);
        }

        [TestMethod]
        public void GblParameter_Serialization()
        {
            var parameter = new GblParameter();
            _TestSerialization(parameter);

            parameter = new GblParameter
            {
                Name = "Name",
                Value = "Value"
            };
            _TestSerialization(parameter);
        }

        [TestMethod]
        public void GblParameter_Verify()
        {
            var parameter = new GblParameter();
            Assert.AreEqual(false, parameter.Verify(false));

            parameter = new GblParameter
            {
                Name = "Name",
                Value = "Value"
            };
            Assert.AreEqual(true, parameter.Verify(false));
        }

        [TestMethod]
        public void GblParameter_ToString()
        {
            var parameter = new GblParameter
            {
                Name = "Start",
                Value = "Now"
            };

            Assert.AreEqual
                (
                    "Name: Start, Value: Now",
                    parameter.ToString()
                );
        }
    }
}
