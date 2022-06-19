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
    public class GblDelrTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblDelr_Construction_1()
        {
            var delr = new GblDelr();
            Assert.IsNotNull(delr);
        }

        [TestMethod]
        public void GblDelr_Execute_1()
        {
            var context = new GblContext();
            var delr = new GblDelr();
            delr.Execute(context);
        }

        [TestMethod]
        public void GblDelr_Verify_1()
        {
            var delr = new GblDelr();
            Assert.IsTrue(delr.Verify(false));
        }
    }
}
