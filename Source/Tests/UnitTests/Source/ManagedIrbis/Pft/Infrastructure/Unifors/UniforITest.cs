// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforITest
        : CommonUniforTest
    {
        [Ignore]
        [TestMethod]
        public void UniforI_GetIniFileEntry_1()
        {
            // Нормальное выполнение
            Execute("IPRIVATE,NAME,NONAME", "NONAME");
            Execute("IPRIVATE,FIO,NONAME", "kladovka");
            Execute("IPRIVATE,FIO", "kladovka");

            // Обработка ошибок
            Execute("IPRIVATE", "");
            Execute("I", "");
        }
    }
}
