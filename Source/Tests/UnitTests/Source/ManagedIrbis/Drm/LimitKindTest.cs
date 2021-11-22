// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Drm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Drm
{
    [TestClass]
    public sealed class LimitKindTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void LimitKind_ListValues_1()
        {
            var values = LimitKind.ListValues();
            Assert.AreEqual (2, values.Length);
        }
    }
}
