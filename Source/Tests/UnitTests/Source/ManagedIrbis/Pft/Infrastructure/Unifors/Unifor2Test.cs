// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor2Test
        : CommonUniforTest
    {
        [Ignore]
        [TestMethod]
        public void Unifor2_GetMaxMfn_1()
        {
            // Нормальное выполнение
            Execute("2", "0000000333");
            Execute("21", "3");
            Execute("212", "000000000333");

            // Обработка ошибок
            Execute("2Q", "0000000333");
            Execute("20", "");
            Execute("2-1", "");
        }
    }
}
