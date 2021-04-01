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
    public class GblPutLogTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblPutLog_Construction_1()
        {
            GblPutLog putLog = new GblPutLog();
            Assert.IsNotNull(putLog);
        }

        [TestMethod]
        public void GblPutLog_Execute_1()
        {
            GblContext context = new GblContext();
            GblPutLog putLog = new GblPutLog();
            putLog.Execute(context);
        }

        [TestMethod]
        public void GblPutLog_Verify_1()
        {
            GblPutLog putLog = new GblPutLog();
            Assert.IsTrue(putLog.Verify(false));
        }
    }
}
