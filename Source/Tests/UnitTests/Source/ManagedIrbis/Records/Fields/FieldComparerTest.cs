// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Records.Fields
{
    [TestClass]
    public class FieldComparerTest
    {
        [TestMethod]
        [Description ("Сравнение по метке поля")]
        public void FieldComparer_ByTag_1()
        {
            var comparer = FieldComparer.ByTag();

            var left = new Field { Tag = 100 };
            var right = new Field { Tag = 101 };
            Assert.IsTrue (comparer.Compare (left, right) < 0);

            right = new Field { Tag = 99 };
            Assert.IsTrue (comparer.Compare (left, right) > 0);

            right = new Field { Tag = 100 };
            Assert.IsTrue (comparer.Compare (left, right) == 0);
        }
    }
}
