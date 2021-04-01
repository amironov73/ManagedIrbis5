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
    public class GblAllTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblAll_Construction_1()
        {
            GblAll all = new GblAll();
        }

        [TestMethod]
        public void GblAdd_Execute_1()
        {
            GblContext context = new GblContext();
            GblAll all = new GblAll();
            all.Execute(context);
        }

        [TestMethod]
        public void GblAll_Verify_1()
        {
            GblAll all = new GblAll();
            Assert.IsTrue(all.Verify(false));
        }
    }
}
