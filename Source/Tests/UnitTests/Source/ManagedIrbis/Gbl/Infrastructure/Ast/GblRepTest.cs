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
    public class GblRepTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblRep_Construction_1()
        {
            var rep = new GblRep();
        }

        [TestMethod]
        public void GblRep_Execute_1()
        {
            var context = new GblContext();
            var rep = new GblRep();
            rep.Execute(context);
        }

        [TestMethod]
        public void GblRep_Verify_1()
        {
            var rep = new GblRep();
            Assert.IsTrue(rep.Verify(false));
        }
    }
}
