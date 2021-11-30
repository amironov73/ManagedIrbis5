// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using ManagedIrbis.Performance;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Performance
{
    [TestClass]
    public sealed class TextPerformanceCollectorTest
    {
        private PerfRecord[] _GetPerf()
        {
            return new PerfRecord []
            {
                new ()
                {
                    Moment = new DateTime (2021, 11, 30, 12, 42, 0),
                    Host = "libeller",
                    Code = "A",
                    OutgoingSize = 150,
                    IncomingSize = 10234,
                    ElapsedTime = 66
                },
                new ()
                {
                    Moment = new DateTime (2021, 11, 30, 12, 42, 1),
                    Host = "libeller",
                    Code = "C",
                    OutgoingSize = 173,
                    IncomingSize = 2121,
                    ElapsedTime = 33
                },
                new ()
                {
                    Moment = new DateTime (2021, 11, 30, 12, 42, 2),
                    Host = "libeller",
                    Code = "C",
                    OutgoingSize = 101,
                    IncomingSize = 110,
                    ElapsedTime = 30
                },
            };
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void TextPerformanceCollector_Construction_1()
        {
            using var writer = new StringWriter();
            using var collector = new TextPerformanceCollector (writer);
            Assert.AreSame (writer, collector.Writer);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void TextPerformanceCollector_Construction_2()
        {
            using var stream = new MemoryStream();
            using var collector = new TextPerformanceCollector (stream);
            Assert.IsNotNull (collector);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void TextPerformanceCollector_Construction_3()
        {
            var fileName = Path.GetTempFileName();
            using var collector = new TextPerformanceCollector (fileName);
            Assert.IsNotNull (collector);
        }

        [TestMethod]
        [Description ("Сбор данных о производительности")]
        public void TextPerformanceCollector_Collect_1()
        {
            using var writer = new StringWriter();
            using var collector = new TextPerformanceCollector (writer);
            var records = _GetPerf();
            foreach (var perf in records)
            {
                collector.Collect (perf);
            }
        }
    }
}
