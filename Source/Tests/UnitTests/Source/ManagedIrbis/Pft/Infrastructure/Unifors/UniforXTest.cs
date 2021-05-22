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
    public class UniforXTest
    {
        private void _X
            (
                string? text,
                string expected
            )
        {
            var context = new PftContext(null);
            var unifor = new Unifor();
            var expression = "X" + text;
            unifor.Execute(context, null, expression);
            var actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforX_RemoveAngleBrackets_1()
        {
            _X(null, string.Empty);
            _X(string.Empty, string.Empty);
            _X("abc", "abc");
            _X("a<bc", "a<bc");
            _X("a<b>c", "ac");
            _X("a<<b>c", "ac");
            _X("<abc>", string.Empty);
            _X("<>abc<>", "abc");
            _X("<>a<b>c<>", "ac");
            _X("<>", string.Empty);
            _X("<<>", string.Empty);
            _X("<><>", string.Empty);
        }
    }
}
