// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

namespace UnitTests.ManagedIrbis.Search
{
    [TestClass]
    public class TermLinkTest
    {
        private void _TestSerialization
            (
                TermLink first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<TermLink>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Index, second!.Index);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Occurrence, second.Occurrence);
            Assert.AreEqual(first.Tag, second.Tag);
        }

        [TestMethod]
        public void TestTermLink_Serialization()
        {
            var termLink = new TermLink();
            _TestSerialization(termLink);

            termLink = new TermLink
            {
                Index = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40
            };
            _TestSerialization(termLink);
        }

        [TestMethod]
        public void TestTermLink_Clone()
        {
            var first = new TermLink
            {
                Index = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40
            };
            var second = first.Clone();

            Assert.AreEqual(first.Index, second.Index);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Occurrence, second.Occurrence);
            Assert.AreEqual(first.Tag, second.Tag);
        }

        [TestMethod]
        public void TestTermLink_ToString()
        {
            var termLink = new TermLink
            {
                Index = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40
            };
            var actual = termLink.ToString();
            Assert.AreEqual("[20] 40/30 10", actual);
        }

        [TestMethod]
        public void TestTermLink_Equals()
        {
            var first = new TermLink
            {
                Index = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40
            };
            var second = first.Clone();
            Assert.IsTrue(first.Equals(second));
            Assert.IsTrue(first.Equals((object)second));

            Assert.IsTrue(first.Equals((object)first));

            second.Mfn = 220;
            Assert.IsFalse(first.Equals(second));

            Assert.IsFalse(first.Equals((object?) null));
        }
    }
}
