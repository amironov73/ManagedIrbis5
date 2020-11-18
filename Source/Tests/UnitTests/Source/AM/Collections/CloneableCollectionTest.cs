using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

// ReSharper disable CheckNamespace

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class CloneableCollectionTest
    {
        [TestMethod]
        public void CloneableCollection_Clone()
        {
            var source = new CloneableCollection<int>
                {
                    212,
                    85,
                    06
                };
            var clone = (CloneableCollection<int>) source.Clone();

            Assert.AreEqual(source.Count, clone.Count);
            for (var i = 0; i < source.Count; i++)
            {
                Assert.AreEqual(source[i], clone[i]);
            }
        }
    }
}
