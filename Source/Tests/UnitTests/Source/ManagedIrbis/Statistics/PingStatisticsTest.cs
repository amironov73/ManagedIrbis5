// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Statistics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Statistics
{
    [TestClass]
    public sealed class PingStatisticsTest
    {
        private PingData[] _GetData()
        {
            return new PingData[]
            {
                new () { RoundTripTime = 100, Success = true },
                new () { RoundTripTime = 200, Success = true },
                new () { RoundTripTime = 300, Success = true },
                new () { RoundTripTime = 400, Success = false },
            };
        }

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void PingStatistics_Construction_1()
        {
            var statistics = new PingStatistics();
            Assert.IsNotNull (statistics.Data);
        }

        [TestMethod]
        [Description ("Добавление данных")]
        public void PingStatistics_Add_1()
        {
            var data = _GetData();
            var statistics = new PingStatistics();
            foreach (var one in data)
            {
                statistics.Add (one);
            }

            Assert.AreEqual (data.Length, statistics.Data.Count);
        }

        [TestMethod]
        [Description ("Очистка данных")]
        public void PingStatistics_Clear_1()
        {
            var data = _GetData();
            var statistics = new PingStatistics();
            foreach (var one in data)
            {
                statistics.Add (one);
            }

            statistics.Clear();
            Assert.AreEqual (0, statistics.Data.Count);
        }

        [TestMethod]
        [Description ("Среднее")]
        public void PingStatistics_Average_1()
        {
            var data = _GetData();
            var statistics = new PingStatistics();
            foreach (var one in data)
            {
                statistics.Add (one);
            }

            var average = statistics.AverageTime;
            Assert.AreEqual (200, average);
        }

        [TestMethod]
        [Description ("Максимум")]
        public void PingStatistics_Max_1()
        {
            var data = _GetData();
            var statistics = new PingStatistics();
            foreach (var one in data)
            {
                statistics.Add (one);
            }

            var max = statistics.MaxTime;
            Assert.AreEqual (300, max);
        }

        [TestMethod]
        [Description ("Минимум")]
        public void PingStatistics_Min_1()
        {
            var data = _GetData();
            var statistics = new PingStatistics();
            foreach (var one in data)
            {
                statistics.Add (one);
            }

            var min = statistics.MinTime;
            Assert.AreEqual (100, min);
        }
    }
}
