// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Drm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Drm
{
    [TestClass]
    public sealed class ValidityPeriodTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void ValidityPeriod_Construction_1()
        {
            var period = new ValidityPeriod();
            Assert.IsNull (period.Field);
            Assert.IsNull (period.From);
            Assert.IsNull (period.Till);
            Assert.IsNull (period.UserData);
            Assert.IsNull (period.UnknownSubFields);
        }
    }
}
