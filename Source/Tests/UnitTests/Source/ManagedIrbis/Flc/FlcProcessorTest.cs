// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Client;
using ManagedIrbis.Flc;

using Moq;

#nullable enable

namespace UnitTests.ManagedIrbis.Flc
{
    [TestClass]
    public class FlcProcessorTest
        : Common.CommonUnitTest
    {
        private new ISyncProvider GetProvider()
        {
            var mock = new Mock<ISyncProvider>();
            var provider = mock.Object;

            return provider;
        }

        [Ignore]
        [TestMethod]
        public void FlcProcessor_CheckRecord_1()
        {
            using var provider = GetProvider();
            var format = "if a(v200) then '1 Missing title data' else '0' fi";
            var processor = new FlcProcessor();
            var record = new Record();
            var flc = processor.CheckRecord
                (
                    provider,
                    record,
                    format
                );
            Assert.IsFalse(flc.CanContinue);
            Assert.AreEqual(1, flc.Messages.Count);
            Assert.AreEqual("Missing title data", flc.Messages[0]);
        }

        [Ignore]
        [TestMethod]
        public void FlcProcessor_CheckRecord_2()
        {
            using var provider = GetProvider();
            var format = "if a(v200) then '1 Missing title data' else '0' fi";
            var processor = new FlcProcessor();
            var record = provider.ReadRecord(1);

            Assert.IsNotNull(record);
            var flc = processor.CheckRecord
                (
                    provider,
                    record!,
                    format
                );

            Assert.IsTrue(flc.CanContinue);
            Assert.AreEqual(0, flc.Messages.Count);
        }
    }
}
