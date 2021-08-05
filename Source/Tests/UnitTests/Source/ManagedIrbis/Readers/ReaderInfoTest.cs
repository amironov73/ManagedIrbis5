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
    public class ReaderInfoTest
    {
        private void _TestSerialization
            (
                ReaderInfo first
            )
        {
            var bytes = first.SaveToMemory();

            var second = bytes.RestoreObjectFromMemory<ReaderInfo>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Age, second!.Age);
            Assert.AreEqual(first.DateOfBirth, second.DateOfBirth);
            Assert.AreEqual(first.Category, second.Category);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.FullName, second.FullName);
        }

        [TestMethod]
        public void ReaderInfo_Serialization_1()
        {
            var readerInfo = new ReaderInfo();
            _TestSerialization(readerInfo);

            readerInfo.Category = "студент";
            readerInfo.FullName = "Иванов Иван Иванович";
            _TestSerialization(readerInfo);
        }
    }
}
