// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforLTest
        : CommonUniforTest
    {
        [Ignore]
        [TestMethod]
        public void UniforL_ContinueTerm_1()
        {
            Execute("LJAZ=рус", "СКИЙ");
        }

        [TestMethod]
        public void UniforL_ContinueTerm_2()
        {
            Execute("L", "");
        }

        [TestMethod]
        public void UniforL_ContinueTerm_3()
        {
            Execute("LJUK=", "");
        }
    }
}
