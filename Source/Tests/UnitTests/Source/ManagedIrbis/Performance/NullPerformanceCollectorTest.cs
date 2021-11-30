// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Performance;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Performance
{
    [TestClass]
    public sealed class NullPerformanceCollectorTest
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
        [Description ("Конструктор по умолчанию")]
        public void NullPerformanceCollector_Construction_1()
        {
            using var collector = new NullPerformanceCollector ();
            Assert.IsNotNull (collector);
        }

        [TestMethod]
        [Description ("Сбор данных о производительности: срабатывает событие")]
        public void NullPerformanceCollector_Collect_1()
        {
            var counter = 0;
            using var collector = new NullPerformanceCollector ();
            collector.RecordCollected += (_, _) =>
            {
                ++counter;
            };
            var records = _GetPerf();
            foreach (var perf in records)
            {
                collector.Collect (perf);
            }

            Assert.AreEqual (records.Length, counter);
        }
    }
}
