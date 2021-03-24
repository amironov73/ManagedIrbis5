using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Search
{
    [Ignore]
    [TestClass]
    public class TermInfoTest
    {
        [TestMethod]
        public void TermInfo_Construction_1()
        {
            Term term = new Term();
            Assert.AreEqual(0, term.Count);
            Assert.IsNull(term.Text);
        }

        private void _TestSerialization
            (
                Term first
            )
        {
            byte[] bytes = first.SaveToMemory();

            Term second
                = bytes.RestoreObjectFromMemory<Term>();

            Assert.AreEqual(first.Count, second.Count);
            Assert.AreEqual(first.Text, second.Text);
        }

        [TestMethod]
        public void TermInfo_Serialization_1()
        {
            Term termInfo = new Term();
            _TestSerialization(termInfo);

            termInfo.Count = 10;
            termInfo.Text = "T=HELLO";
            _TestSerialization(termInfo);
        }

        [TestMethod]
        public void TermInfo_Verify_1()
        {
            Term termInfo = new Term();
            Assert.IsFalse(termInfo.Verify(false));

            termInfo.Count = 10;
            termInfo.Text = "T=HELLO";
            Assert.IsTrue(termInfo.Verify(false));
        }

        [TestMethod]
        public void TermInfo_ToString_1()
        {
            Term termInfo = new Term
            {
                Count = 10,
                Text = "T=HELLO"
            };
            string actual = termInfo.ToString();
            Assert.AreEqual("10#T=HELLO", actual);
        }

        [TestMethod]
        public void TermInfo_TrimPrefix_1()
        {
            Term[] terms =
            {
                new Term {Count=1, Text = "T=HELLO"},
                new Term {Count=2, Text = "T=IRBIS"},
                new Term {Count=3, Text = "T=WORLD"},
            };
            Term[] actual = Term.TrimPrefix(terms, "T=");
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("HELLO", actual[0].Text);
            Assert.AreEqual("IRBIS", actual[1].Text);
            Assert.AreEqual("WORLD", actual[2].Text);

            terms = new []
            {
                new Term {Count=1, Text = "HELLO"},
                new Term {Count=2, Text = "IRBIS"},
                new Term {Count=3, Text = "WORLD"},
            };
            actual = Term.TrimPrefix(terms, string.Empty);
            Assert.AreEqual(3, actual.Length);
            Assert.AreEqual("HELLO", actual[0].Text);
            Assert.AreEqual("IRBIS", actual[1].Text);
            Assert.AreEqual("WORLD", actual[2].Text);
        }

        [TestMethod]
        public void TermInfo_Clone_1()
        {
            Term expected = new Term
            {
                Count = 10,
                Text = "T=HELLLO"
            };
            Term actual = expected.Clone();
            Assert.AreEqual(expected.Count, actual.Count);
            Assert.AreEqual(expected.Text, actual.Text);
        }

        [TestMethod]
        public void TermInfo_ToXml_1()
        {
            Term term = new Term();
            Assert.AreEqual("<term />", XmlUtility.SerializeShort(term));

            term.Count = 10;
            term.Text = "T=HELLO";
            Assert.AreEqual("<term count=\"10\" text=\"T=HELLO\" />", XmlUtility.SerializeShort(term));
        }

        [TestMethod]
        public void TermInfo_ToJson_1()
        {
            Term term = new Term();
            Assert.AreEqual("{}", JsonUtility.SerializeShort(term));

            term.Count = 10;
            term.Text = "T=HELLO";
            Assert.AreEqual("{'count':10,'text':'T=HELLO'}", JsonUtility.SerializeShort(term));
        }
    }
}
