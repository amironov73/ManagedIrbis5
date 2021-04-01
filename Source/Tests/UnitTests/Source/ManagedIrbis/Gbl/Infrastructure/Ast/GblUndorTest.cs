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
    public class GblUndorTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblUndor_Construction_1()
        {
            GblUndor undor = new GblUndor();
        }

        [TestMethod]
        public void GblUndor_Execute_1()
        {
            GblContext context = new GblContext();
            GblUndor undor = new GblUndor();
            undor.Execute(context);
        }

        [TestMethod]
        public void GblUndor_Verify_1()
        {
            GblUndor undor = new GblUndor();
            Assert.IsTrue(undor.Verify(false));
        }
    }
}
