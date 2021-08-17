// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Linq;

#nullable enable

namespace UnitTests.AM.Linq
{
    [TestClass]
    public class SequenceTest
    {
        [TestMethod]
        public void Sequence_FirstOr_1()
        {
            var sequence = new [] { 1, 2, 3 };
            var actual = Sequence.FirstOr(sequence, 0);
            const int expected = 1;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Sequence_FirstOr_2()
        {
            var sequence = Array.Empty<int>();
            var actual = Sequence.FirstOr(sequence, 0);
            const int expected = 0;
            Assert.AreEqual(expected, actual);
        }
    }
}
