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
    public class GblChacTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblChac_Construction_1()
        {
            var chac = new GblChac();
            Assert.IsNotNull(chac);
        }

        [TestMethod]
        public void GblChac_Execute_1()
        {
            var context = new GblContext();
            var chac = new GblChac();
            chac.Execute(context);
        }

        [TestMethod]
        public void GblChac_Verify_1()
        {
            var chac = new GblChac();
            Assert.IsTrue(chac.Verify(false));
        }
    }
}
