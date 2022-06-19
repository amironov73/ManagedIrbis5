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
    public class GblUndelTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblUndel_Construction_1()
        {
            var undel = new GblUndel();
        }

        [TestMethod]
        public void GblUndel_Execute_1()
        {
            var context = new GblContext();
            var undel = new GblUndel();
            undel.Execute(context);
        }

        [TestMethod]
        public void GblUndel_Verify_1()
        {
            var undel = new GblUndel();
            Assert.IsTrue(undel.Verify(false));
        }
    }
}
