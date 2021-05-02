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
    public class OutputWriterTest
    {
        [TestMethod]
        public void OutputWriter_WriteLine_1()
        {
            const string expected = "Quick brown fox";

            var output = new TextOutput();
            var writer = new OutputWriter(output);
            writer.WriteLine(expected);

            var actual = output.ToString()
                .TrimEnd();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void OutputWriter_WriteLine_2()
        {
            const int value = 235;
            var expected = value.ToString();

            var output = new TextOutput();
            var writer = new OutputWriter(output);

            writer.WriteLine(value);

            var actual = output.ToString()
                .TrimEnd();

            Assert.AreEqual(expected, actual);
        }
    }
}
