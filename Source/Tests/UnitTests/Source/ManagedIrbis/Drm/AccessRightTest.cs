// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Drm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Drm
{
    [TestClass]
    public sealed class AccessRightTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AccessRight_Construction_1()
        {
            var period = new AccessRight();
            Assert.IsNull (period.Field);
            Assert.IsNull (period.AccessKind);
            Assert.IsNull (period.ElementKind);
            Assert.IsNull (period.ElementValue);
            Assert.IsNull (period.FromDate);
            Assert.IsNull (period.LimitKind);
            Assert.IsNull (period.LimitValue);
            Assert.IsNull (period.TillDate);
            Assert.IsNull (period.UserData);
            Assert.IsNull (period.UnknownSubFields);
        }
    }
}
