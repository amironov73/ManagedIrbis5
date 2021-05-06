// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text.Output;

#nullable enable

namespace UnitTests.AM.Text.Output
{
    [TestClass]
    public class TextOutputTest
    {
        [TestMethod]
        public void TextOutput_Construction_1()
        {
            var output = new TextOutput();

            Assert.IsFalse(output.HaveError);
        }

        [TestMethod]
        public void TextOutput_ToString_1()
        {
            const string expected = "Quick brown fox";

            var output = new TextOutput();
            output.Write(expected);

            var actual = output.ToString();

            Assert.AreEqual(expected, actual);
        }
    }
}
