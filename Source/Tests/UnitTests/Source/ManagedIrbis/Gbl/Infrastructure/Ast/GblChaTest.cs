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
    public class GblChaTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblCha_Construction_1()
        {
            var cha = new GblCha();
            Assert.IsNotNull(cha);
        }

        [TestMethod]
        public void GblCha_Execute_1()
        {
            var context = new GblContext();
            var cha = new GblCha();
            cha.Execute(context);
        }

        [TestMethod]
        public void GblCha_Verify_1()
        {
            var cha = new GblCha();
            Assert.IsTrue(cha.Verify(false));
        }
    }
}
