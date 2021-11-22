// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Drm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Drm
{
    [TestClass]
    public sealed class RightRecordTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void RightRecord_Construction_1()
        {
            var period = new RightRecord();
            Assert.IsNull (period.Description);
            Assert.IsNull (period.Id);
            Assert.IsNull (period.Period);
            Assert.IsNull (period.Record);
            Assert.IsNull (period.Rights);
            Assert.IsNull (period.UserData);
        }
    }
}
