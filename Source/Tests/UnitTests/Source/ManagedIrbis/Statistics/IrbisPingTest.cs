// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;
using System.Threading;

using AM.Json;
using AM.PlatformAbstraction;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Statistics;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Statistics
{
    [TestClass]
    public sealed class IrbisPingTest
    {
        private ISyncConnection _GetConnection()
        {
            var mock = new Mock<ISyncConnection>();
            mock.Setup (p => p.Connect()).Returns (true);
            mock.Setup (p => p.Disconnect()).Returns (true);
            mock.Setup (p => p.NoOperation()).Returns (true);

            return mock.Object;
        }

        [TestMethod]
        [Description ("Конструктор")]
        public void IrbisPing_Construction_1()
        {
            var connection = _GetConnection();
            var ping = new IrbisPing (connection);
            Assert.IsFalse (ping.Active);
            Assert.AreSame (connection, ping.Connection);
            Assert.IsNotNull (ping.PlatformAbstraction);
            ping.PlatformAbstraction = new TestingPlatformAbstraction();
            Assert.IsNotNull (ping.Statistics);
        }

        [TestMethod]
        [Description ("Однократный пинг")]
        public void IrbisPing_PingOnce_1()
        {
            var connection = _GetConnection();
            var ping = new IrbisPing (connection);
            var data = ping.PingOnce();
            Assert.IsNotNull (data);
            Assert.IsTrue (data.Success);
        }

        [Ignore]
        [TestMethod]
        [Description ("Опрос по таймеру")]
        public void IrbisPing_Timer_1()
        {
            var counter = 0;
            EventHandler action = (_, _) => { Interlocked.Increment (ref counter); };
            var connection = _GetConnection();
            var ping = new IrbisPing (connection);
            ping.StatisticsUpdated += action;
            ping.Active = true;
            Thread.Sleep (2000);
            ping.Active = false;
            ping.StatisticsUpdated -= action;
            Assert.AreNotEqual (0, counter);
        }

    }
}
