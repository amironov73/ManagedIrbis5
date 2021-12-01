// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Systematization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Systematization
{
    [TestClass]
    public sealed class AthruRecordTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthruRecord_Construction_1()
        {
            var athru = new AthruRecord();
            Assert.IsNull (athru.MainHeading);
            Assert.IsNull (athru.LinkedHeadings);
            Assert.IsNull (athru.Guidelines);
        }
    }
}
