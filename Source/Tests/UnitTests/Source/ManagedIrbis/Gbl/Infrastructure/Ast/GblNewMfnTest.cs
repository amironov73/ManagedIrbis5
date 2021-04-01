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
    public class GblNewMfnTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblNewMfn_Construction_1()
        {
            GblNewMfn newMfn = new GblNewMfn();
            Assert.IsNotNull(newMfn);
        }

        [TestMethod]
        public void GblNewMfn_Execute_1()
        {
            GblContext context = new GblContext();
            GblNewMfn newMfn = new GblNewMfn();
            newMfn.Execute(context);
        }

        [TestMethod]
        public void GblNewMfn_Verify_1()
        {
            GblNewMfn newMfn = new GblNewMfn();
            Assert.IsTrue(newMfn.Verify(false));
        }
    }
}
