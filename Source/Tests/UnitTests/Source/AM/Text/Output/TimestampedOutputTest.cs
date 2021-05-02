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
    public class TimestampedOutputTest
    {
        [TestMethod]
        public void TimestampedOutput_ToString_1()
        {
            var innerOutput = new TextOutput();
            var output = new TimestampedOutput(innerOutput);

            output.Write("Hello");

            var actual = innerOutput.ToString();
            Assert.IsNotNull(actual);
        }
    }
}
