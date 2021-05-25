// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class CounterDatabaseTest
        : Common.CommonUnitTest
    {
        [Ignore]
        [TestMethod]
        public void CounterDatabase_GetCounter_1()
        {
            using var provider = GetProvider();
            var database = new CounterDatabase(provider);
            var counter = database.GetCounter("01");
            Assert.IsNotNull(counter);
            Assert.AreEqual(11, counter!.NumericValue);

            counter = database.GetCounter("noSuchCounter");
            Assert.IsNull(counter);
        }
    }
}
