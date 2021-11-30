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
    public sealed class InMemoryPerformanceCollectorTest
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
        public void InMemoryPerformanceCollector_Construction_1()
        {
            using var collector = new InMemoryPerformanceCollector ();
            Assert.AreEqual (0, collector.Limit);
            Assert.IsNotNull (collector.List);
            Assert.AreEqual (0, collector.List.Count);
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void InMemoryPerformanceCollector_Construction_2()
        {
            const int limit = 2;
            using var collector = new InMemoryPerformanceCollector (limit);
            Assert.AreEqual (limit, collector.Limit);
            Assert.IsNotNull (collector.List);
            Assert.AreEqual (0, collector.List.Count);
        }

        [TestMethod]
        [Description ("Сбор данных о производительности: не превышаем предел")]
        public void InMemoryPerformanceCollector_Collect_1()
        {
            const int limit = 2;
            using var collector = new InMemoryPerformanceCollector (limit);
            var records = _GetPerf();
            foreach (var perf in records)
            {
                collector.Collect (perf);
            }

            Assert.AreEqual (limit, collector.List.Count);
        }

        [TestMethod]
        [Description ("Сбор данных о производительности: срабатывает событие")]
        public void InMemoryPerformanceCollector_Collect_2()
        {
            var counter = 0;
            using var collector = new InMemoryPerformanceCollector ();
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
            Assert.AreEqual (records.Length, collector.List.Count);
        }
    }
}
