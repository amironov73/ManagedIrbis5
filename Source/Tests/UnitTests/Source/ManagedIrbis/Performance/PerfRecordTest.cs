// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.Performance;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Performance
{
    [TestClass]
    public sealed class PerfRecordTest
    {
        private PerfRecord _GetPerf()
        {
            return new ()
            {
                Moment = new DateTime (2021, 11, 30, 12, 42, 0),
                Host = "libeller",
                Code = "A",
                OutgoingSize = 150,
                IncomingSize = 10234,
                ElapsedTime = 66
            };
        }

        private void _Compare
            (
                PerfRecord first,
                PerfRecord second
            )
        {
            Assert.AreEqual (first.Moment, second.Moment);
            Assert.AreEqual (first.Host, second.Host);
            Assert.AreEqual (first.Code, second.Code);
            Assert.AreEqual (first.OutgoingSize, second.OutgoingSize);
            Assert.AreEqual (first.IncomingSize, second.IncomingSize);
            Assert.AreEqual (first.ElapsedTime, second.ElapsedTime);
            Assert.AreEqual (first.ErrorMessage, second.ErrorMessage);
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void PerfRecord_Construction_1()
        {
            var perf = new PerfRecord();
            Assert.AreEqual (default, perf.Moment);
            Assert.IsNull (perf.Host);
            Assert.IsNull (perf.Code);
            Assert.AreEqual (default, perf.OutgoingSize);
            Assert.AreEqual (default, perf.IncomingSize);
            Assert.AreEqual (default, perf.ElapsedTime);
            Assert.IsNull (perf.ErrorMessage);
        }

        [TestMethod]
        [Description ("Присвоение")]
        public void PerfRecord_Construction_2()
        {
            var date = new DateTime (2021, 11, 30, 12, 42, 0);
            var perf = new PerfRecord
            {
                Moment = date,
                Host = "libeller",
                Code = "A",
                OutgoingSize = 150,
                IncomingSize = 10234,
                ElapsedTime = 66,
                ErrorMessage = "Bang!"
            };
            Assert.AreEqual (date, perf.Moment);
            Assert.AreEqual ("libeller", perf.Host);
            Assert.AreEqual ("A", perf.Code);
            Assert.AreEqual (150, perf.OutgoingSize);
            Assert.AreEqual (10234, perf.IncomingSize);
            Assert.AreEqual (66, perf.ElapsedTime);
            Assert.AreEqual ("Bang!", perf.ErrorMessage);
        }

        private void _TestSerialization
            (
                PerfRecord first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<PerfRecord>();
            Assert.IsNotNull (second);
            _Compare (first, second);
        }

        [TestMethod]
        [Description ("Сериализация")]
        public void PerfRecord_Serialization_1()
        {
            var perf = new PerfRecord();
            _TestSerialization (perf);

            perf = _GetPerf();
            _TestSerialization (perf);
        }

        [TestMethod]
        [Description ("Верификация")]
        public void PerfRecord_Verify_1()
        {
            var perf = new PerfRecord();
            Assert.IsFalse (perf.Verify (false));

            perf = _GetPerf();
            Assert.IsTrue (perf.Verify (false));
        }

        [TestMethod]
        [Description ("XML-представление")]
        public void PerfRecord_ToXml_1()
        {
            var perf = new PerfRecord();
            Assert.AreEqual
                (
                    "<transaction moment=\"0001-01-01T00:00:00\" outgoing=\"0\" incoming=\"0\" elapsed=\"0\" />",
                    XmlUtility.SerializeShort (perf)
                );

            perf = _GetPerf();
            Assert.AreEqual
                (
                    "<transaction moment=\"2021-11-30T12:42:00\" host=\"libeller\" code=\"A\" outgoing=\"150\" incoming=\"10234\" elapsed=\"66\" />",
                    XmlUtility.SerializeShort (perf)
                );
        }

        [TestMethod]
        [Description ("JSON-представление")]
        public void PerfRecord_ToJson_1()
        {
            var perf = new PerfRecord ();
            Assert.AreEqual
                (
                    "{\"moment\":\"0001-01-01T00:00:00\",\"outgoing\":0,\"incoming\":0,\"elapsed\":0}",
                    JsonUtility.SerializeShort (perf)
                );

            perf = _GetPerf();
            var expected = "{\"moment\":\"2021-11-30T12:42:00\",\"host\":\"libeller\",\"code\":\"A\",\"outgoing\":150,\"incoming\":10234,\"elapsed\":66}";
            var actual = JsonUtility.SerializeShort (perf);
            Assert.AreEqual (expected, actual);
        }

        [TestMethod]
        [Description ("Текстовое представление")]
        public void PerfRecord_ToString_1()
        {
            var perf = new PerfRecord();
            Assert.AreEqual
                (
                    "01/01/0001 00:00:00\t\t\t0\t0\t0\t",
                    perf.ToString()
                );

            perf = _GetPerf();
            Assert.AreEqual
                (
                    "11/30/2021 12:42:00\tlibeller\tA\t150\t10234\t66\t",
                    perf.ToString()
                );
        }

    }
}
