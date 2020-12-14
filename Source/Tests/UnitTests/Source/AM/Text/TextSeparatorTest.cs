using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

// ReSharper disable CheckNamespace

#nullable enable

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextSeparatorTest
    {
        class MySeparator
            : TextSeparator
        {
            public StringBuilder Accumulator { get; }

            public MySeparator()
            {
                Accumulator = new StringBuilder();
            }

            protected override void HandleChunk(bool inner, string text)
            {
                if (inner)
                {
                    Accumulator.Append(text);
                }
                else
                {
                    if (!string.IsNullOrEmpty(text))
                    {
                        Accumulator.Append("<<<");
                        Accumulator.Append(text);
                        Accumulator.Append(">>>");
                    }
                }
            }
        }

        private void _TestSeparator
            (
                string source,
                string expected
            )
        {
            var separator = new MySeparator();
            var endState = separator.SeparateText(source);
            Assert.IsFalse(endState);
            var actual = separator.Accumulator.ToString();
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void TextSeparator_Construction_1()
        {
            var separator = new TextSeparator();
            Assert.AreEqual(TextSeparator.DefaultOpen, separator.Open);
            Assert.AreEqual(TextSeparator.DefaultClose, separator.Close);
        }

        [TestMethod]
        public void TextSeparator_SeparateText_1()
        {
            _TestSeparator("", "");
            _TestSeparator("Hello", "<<<Hello>>>");
            _TestSeparator
                (
                    "Hello, <%v200^a%> World!",
                    "<<<Hello, >>>v200^a<<< World!>>>"
                );
            _TestSeparator
                (
                    "Hello, <%%> World!",
                    "<<<Hello, >>><<< World!>>>"
                );
            _TestSeparator
                (
                    "<%v200^a, |:|v200^e%>",
                    "v200^a, |:|v200^e"
                );
        }

        [TestMethod]
        public void TextSeparator_SeparateText_2()
        {
            _TestSeparator("", "");
            _TestSeparator("<Hello>!", "<<<<Hello>!>>>");
            _TestSeparator
                (
                    "1% < 2%",
                    "<<<1% < 2%>>>"
                );
        }
    }
}
