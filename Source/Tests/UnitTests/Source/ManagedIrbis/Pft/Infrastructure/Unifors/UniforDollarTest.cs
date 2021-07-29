// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforDollarTest
    {
        private void _Check
            (
                string input,
                string expected
            )
        {
            var context = new PftContext(null);
            var unifor = new Unifor();
            var expression = "$" + input;
            unifor.Execute(context, null, expression);
            var actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void UniforDollar_Md5Hash_1()
        {
            _Check(string.Empty, "D41D8CD98F00B204E9800998ECF8427E");
            _Check("md5", "1BC29B36F623BA82AAF6724FD3B16718");
            _Check("md4", "C93D3BF7A7C4AFE94B64E30C2CE39F4F");
        }
    }
}
