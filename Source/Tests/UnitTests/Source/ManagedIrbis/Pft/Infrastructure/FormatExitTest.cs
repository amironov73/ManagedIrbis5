// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Pft;
using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure
{
    [TestClass]
    public class FormatExitTest
    {
        [TestMethod]
        public void FormatExit_Execute_1()
        {
            var context = new PftContext(null);
            FormatExit.Execute(context, null, "uf", "+9V");
            Assert.AreEqual("64", context.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(PftSemanticException))]
        public void FormatExit_Execute_2()
        {
            var context = new PftContext(null);
            FormatExit.Execute(context, null, "no", "+9V");
            Assert.AreEqual("64", context.Text);
        }
    }
}
