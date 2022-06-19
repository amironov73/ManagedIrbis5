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
    public class GblIfTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblIf_Construction_1()
        {
            var gblIf = new GblIf();
        }

        [TestMethod]
        public void GblAdd_Execute_1()
        {
            var context = new GblContext();
            var gblIf = new GblIf();
            gblIf.Execute(context);
        }

        [TestMethod]
        public void GblIf_Verify_1()
        {
            var gblIf = new GblIf();
            Assert.IsTrue(gblIf.Verify(false));
        }
    }
}
