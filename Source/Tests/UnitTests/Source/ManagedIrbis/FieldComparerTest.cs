// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.Collections.Generic;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis
{
    [TestClass]
    public class FieldComparerTest
    {
        [TestMethod]
        public void FieldComparer_ByTag_1()
        {
            Comparer<Field> comparer = FieldComparer.ByTag();

            Field left = new Field { Tag = 100 };
            Field right = new Field { Tag = 101 };
            Assert.IsTrue(comparer.Compare(left, right) < 0);

            right = new Field { Tag = 99 };
            Assert.IsTrue(comparer.Compare(left, right) > 0);

            right = new Field {Tag = 100 };
            Assert.IsTrue(comparer.Compare(left, right) == 0);
        }
    }
}
