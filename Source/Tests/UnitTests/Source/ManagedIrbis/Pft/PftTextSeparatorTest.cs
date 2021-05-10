// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft
{
    [TestClass]
    public class PftTextSeparatorTest
    {
        [TestMethod]
        public void PftTextSeparator_Construction_1()
        {
            var separator = new PftTextSeparator();
            Assert.AreEqual("<%", separator.Open);
            Assert.AreEqual("%>", separator.Close);
        }

        [TestMethod]
        public void PftTextSeparator_Construction_2()
        {
            const string open = "<<", close = ">>";
            var separator = new PftTextSeparator(open, close);
            Assert.AreEqual(open, separator.Open);
            Assert.AreEqual(close, separator.Close);
        }

        [TestMethod]
        public void PftTextSeparator_SeparateText_1()
        {
            var separator = new PftTextSeparator();
            Assert.IsFalse(separator.SeparateText(""));
            Assert.AreEqual("", separator.Accumulator);
        }

        [TestMethod]
        public void PftTextSeparator_SeparateText_2()
        {
            var separator = new PftTextSeparator();
            Assert.IsFalse(separator.SeparateText("<html> <%'Hello'%>"));
            Assert.AreEqual("<<<<html> >>>'Hello'", separator.Accumulator);
        }

        [TestMethod]
        public void PftTextSeparator_SeparateText_3()
        {
            var separator = new PftTextSeparator();
            Assert.IsTrue(separator.SeparateText("<html> <%'Hello'"));
            Assert.AreEqual("<<<<html> >>>'Hello'", separator.Accumulator);
        }
    }
}
