// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Onix;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Onix
{
    [TestClass]
    public sealed class NotificationTypeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void NotificationType_ListValues_1()
        {
            var values = NotificationType.ListValues();
            Assert.AreEqual (5, values.Length);
        }
    }
}
