// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusBTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusB_ByteSum_1()
        {
            Execute("+B", "0");
            Execute("+B1", "49");
            Execute("+B12", "99");
            Execute("+B123", "150");
            Execute("+B1234", "202");
            Execute("+BПривет", "2210");
        }
    }
}
