// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Drm;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Drm
{
    [TestClass]
    public sealed class AccessElementTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void AccessElement_ListValues_1()
        {
            var values = AccessElement.ListValues();
            Assert.AreEqual (7, values.Length);
        }
    }
}
