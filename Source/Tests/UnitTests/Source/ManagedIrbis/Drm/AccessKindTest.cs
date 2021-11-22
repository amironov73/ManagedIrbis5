// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Drm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Drm
{
    [TestClass]
    public sealed class AccessKindTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void AccessKind_ListValues_1()
        {
            var values = AccessKind.ListValues();
            Assert.AreEqual (3, values.Length);
        }
    }
}
