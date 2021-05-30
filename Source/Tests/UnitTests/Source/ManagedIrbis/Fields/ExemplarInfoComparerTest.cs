// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public class ExemplarInfoComparerTest
    {
        private ExemplarInfo _GetFirst()
        {
            return new ()
            {
                Number = "1234",
                Description = "First description"
            };
        }

        private ExemplarInfo _GetSecond()
        {
            return new ()
            {
                Number = "234",
                Description = "Second description"
            };
        }

        [TestMethod]
        public void ExemplarInfoComparer_ByNumber_1()
        {
            var first = _GetFirst();
            var second = _GetSecond();
            var comparer = ExemplarInfoComparer.ByNumber();

            Assert.IsTrue
                (
                    comparer.Compare(first, second) > 0
                );
        }

        [TestMethod]
        public void ExemplarInfoComparer_ByDescription_1()
        {
            var first = _GetFirst();
            var second = _GetSecond();
            var comparer = ExemplarInfoComparer.ByDescription();

            Assert.IsTrue
            (
                comparer.Compare(first, second) < 0
            );
        }
    }
}
