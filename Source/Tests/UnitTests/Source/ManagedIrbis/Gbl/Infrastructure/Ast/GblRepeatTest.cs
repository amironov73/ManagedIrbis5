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
    public class GblRepeatTest
        : CommonGblAstTest
    {
        [TestMethod]
        public void GblRepeat_Construction_1()
        {
            GblRepeat repeat = new GblRepeat();
            Assert.IsNotNull(repeat);
        }

        [TestMethod]
        public void GblRepeat_Execute_1()
        {
            GblContext context = new GblContext();
            GblRepeat repeat = new GblRepeat();
            repeat.Execute(context);
        }

        [TestMethod]
        public void GblRepeat_Verify_1()
        {
            GblRepeat repeat = new GblRepeat();
            Assert.IsTrue(repeat.Verify(false));
        }
    }
}
