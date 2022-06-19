// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Gbl.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl.Infrastructure.Ast
{
    [TestClass]
    public class GblEmptyTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblEmpty_Construction_1()
        {
            var empty = new GblEmpty();
            Assert.IsNotNull(empty);
        }

        [TestMethod]
        public void GblEmpty_Execute_1()
        {
            var context = new GblContext();
            var empty = new GblEmpty();
            empty.Execute(context);
        }

        [TestMethod]
        public void GblEmpty_Verify_1()
        {
            var empty = new GblEmpty();
            Assert.IsTrue(empty.Verify(false));
        }
    }
}
