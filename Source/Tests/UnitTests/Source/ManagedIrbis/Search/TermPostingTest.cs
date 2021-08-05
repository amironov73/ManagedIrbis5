// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Runtime;

using ManagedIrbis;

#nullable enable

namespace UnitTests.ManagedIrbis.Search
{
    [Ignore]
    [TestClass]
    public class TermPostingTest
    {
        private void _TestSerialization
            (
                TermPosting first
            )
        {
            var bytes = first.SaveToMemory();

            var second = bytes.RestoreObjectFromMemory<TermPosting>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Count, second!.Count);
            Assert.AreEqual(first.Mfn, second.Mfn);
            Assert.AreEqual(first.Occurrence, second.Occurrence);
            Assert.AreEqual(first.Tag, second.Tag);
            Assert.AreEqual(first.Text, second.Text);
        }

        [TestMethod]
        public void TestTermPosting_Serialization()
        {
            var posting = new TermPosting();
            _TestSerialization(posting);

            posting = new TermPosting
            {
                Count = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40,
                Text = "T=HELLLO"
            };
            _TestSerialization(posting);
        }

        [TestMethod]
        public void TestTermPosting_ToString()
        {
            var posting = new TermPosting
            {
                Count = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40,
                Text = "T=HELLO"
            };
            var actual = posting.ToString();
            Assert.AreEqual
                (
                    "MFN=20 Tag=40 Occurrence=30 Count=10 Text=\"T=HELLO\"",
                    actual
                );
        }

        [TestMethod]
        public void TestTermPosting_Clone()
        {
            var expected = new TermPosting
            {
                Count = 10,
                Mfn = 20,
                Occurrence = 30,
                Tag = 40,
                Text = "T=HELLLO"
            };
            var actual = expected.Clone();
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected.Mfn, actual.Mfn);
            Assert.AreEqual(expected.Occurrence, actual.Occurrence);
            Assert.AreEqual(expected.Tag, actual.Tag);
            Assert.AreEqual(expected.Text, actual.Text);
        }

        [TestMethod]
        public void TestTermPosting_Verify()
        {
            var posting = new TermPosting();
            Assert.IsTrue(posting.Verify(false));
        }
    }
}
