// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM.Json;
using AM.Xml;

using ManagedIrbis.Statistics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Statistics
{
    [TestClass]
    public sealed class PingDataTest
    {
        private PingData _GetPingData()
        {
            return new PingData
            {
                Moment = new DateTime (2021, 11, 26, 16, 39, 0),
                Success = true,
                RoundTripTime = 123
            };
        }

        private void _Compare
            (
                PingData first,
                PingData second
            )
        {
            Assert.AreEqual (first.Moment, second.Moment);
            Assert.AreEqual (first.Success, second.Success);
            Assert.AreEqual (first.RoundTripTime, second.RoundTripTime);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void PingData_Construction_1()
        {
            var data = new PingData();
            Assert.AreEqual (default, data.Moment);
            Assert.AreEqual (default, data.Success);
            Assert.AreEqual (default, data.RoundTripTime);
        }

        private void _Serialization
            (
                PingData first
            )
        {
            using var memory1 = new MemoryStream();
            using var writer = new BinaryWriter (memory1);
            first.SaveToStream (writer);

            using var memory2 = new MemoryStream (memory1.ToArray());
            using var reader = new BinaryReader (memory2);
            var second = new PingData();
            second.RestoreFromStream (reader);

            _Compare (first, second);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void PingData_Serialization_1()
        {
            var data = new PingData();
            _Serialization (data);

            data = _GetPingData();
            _Serialization (data);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void PingData_Verify_1()
        {
            var data = new PingData();
            Assert.IsFalse (data.Verify (false));

            data = _GetPingData();
            Assert.IsTrue (data.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void PingData_ToXml_1()
        {
            var data = new PingData();
            Assert.AreEqual
                (
                    "<ping moment=\"0001-01-01T00:00:00\" success=\"false\" roundtrip=\"0\" />",
                    XmlUtility.SerializeShort (data)
                );

            data = _GetPingData();
            Assert.AreEqual
                (
                    "<ping moment=\"2021-11-26T16:39:00\" success=\"true\" roundtrip=\"123\" />",
                    XmlUtility.SerializeShort (data)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void PingData_ToJson_1()
        {
            var data = new PingData();
            Assert.AreEqual
                (
                    "{\"moment\":\"0001-01-01T00:00:00\",\"success\":false,\"roundtrip\":0}",
                    JsonUtility.SerializeShort (data)
                );

            data = _GetPingData();
            Assert.AreEqual
                (
                    "{\"moment\":\"2021-11-26T16:39:00\",\"success\":true,\"roundtrip\":123}",
                    JsonUtility.SerializeShort (data)
                );
        }

        [TestMethod]
        [Description ("Плоское текстовое представление")]
        public void PingData_ToString_1()
        {
            var data = new PingData();
            Assert.AreEqual
                (
                    "00:00:00 False 0",
                    data.ToString()
                );

            data = _GetPingData();
            Assert.AreEqual
                (
                    "16:39:00 True 123",
                    data.ToString()
                );
        }

    }
}
