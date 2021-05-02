// ReSharper disable CheckNamespace

using System.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#nullable enable

namespace UnitTests.AM.Collections
{
    [TestClass]
    public class BitArrayUtilityTest
    {
        [TestMethod]
        public void BitArrayUtility_AreEqual_1()
        {
            var left = new BitArray(10) { [1] = true };
            var right = new BitArray(10) { [1] = true };

            Assert.IsTrue(BitArrayUtility.AreEqual(left, right));

            right[2] = true;
            Assert.IsFalse(BitArrayUtility.AreEqual(left, right));

            right = new BitArray(11) { [1] = true };
            Assert.IsFalse(BitArrayUtility.AreEqual(left, right));
        }
    }
}
