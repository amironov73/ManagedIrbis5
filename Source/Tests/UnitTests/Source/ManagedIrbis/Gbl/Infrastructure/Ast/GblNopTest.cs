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
    public class GblNopTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblNop_Construction_1()
        {
            GblNop nop = new GblNop();
            Assert.IsNotNull(nop);
        }

        [TestMethod]
        public void GblNop_Execute_1()
        {
            GblContext context = new GblContext();
            GblNop nop = new GblNop();
            nop.Execute(context);
        }

        [TestMethod]
        public void GblNop_Verify_1()
        {
            GblNop nop = new GblNop();
            Assert.IsTrue(nop.Verify(false));
        }
    }
}
