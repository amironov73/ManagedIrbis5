// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor9Test
    {
        private void _9
            (
                string input,
                string expected
            )
        {
            var context = new PftContext(null);
            var unifor = new Unifor();
            var expression = input;
            unifor.Execute(context, null, expression);
            var actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Unifor9_RemoveDoubleQuotes_1()
        {
            _9("9", "");
            _9("912345", "12345");
            _9("912\"345", "12345");
            _9("9\"12345\"", "12345");
            _9("9\"\"\"", "");
        }
    }
}
