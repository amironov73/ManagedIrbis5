// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis.Readers;

#nullable enable

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class DebtorInfoTest
    {
        private void _TestSerialization
            (
                DebtorInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<DebtorInfo>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Age, second!.Age);
            Assert.AreEqual(first.Category, second.Category);
            Assert.AreEqual(first.Name, second.Name);
        }

        [TestMethod]
        public void DebtorInfo_Serialization_1()
        {
            var debtorInfo = new DebtorInfo();
            _TestSerialization(debtorInfo);

            debtorInfo.Name = "Иванов Иван Иванович";
            _TestSerialization(debtorInfo);
        }
    }
}
