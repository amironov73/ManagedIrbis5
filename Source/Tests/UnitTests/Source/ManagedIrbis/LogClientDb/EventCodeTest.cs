// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.LogClientDb;

#nullable enable

namespace UnitTests.ManagedIrbis.LogClientDb
{
    [TestClass]
    public sealed class EventCodeTest
    {
        [TestMethod]
        [Description ("Получение массива значений констант")]
        public void EventCode_ListValues_1()
        {
            var values = EventCode.ListValues();
            Assert.AreEqual (13, values.Length);
        }

        [TestMethod]
        [Description ("Получение словаря код-значение")]
        public void EventCode_ListValuesWithDescriptions_1()
        {
            var values = EventCode.ListValuesWithDescriptions();
            Assert.AreEqual (13, values.Count);
        }
    }
}
