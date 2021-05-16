// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforJTest
        : CommonUniforTest
    {
        [Ignore]
        [TestMethod]
        public void UniforJ_GetTermRecordCountDB_1()
        {
            // Нормальное выполнение
            Execute("J,K=BARBARICUM", "2");
            Execute("JIBIS,K=BARBARICUM", "2");

            // Обработка ошибок
            Execute("J", "");
            Execute("JK=BARBARICUM", "");
            Execute("JIBIS,", "");
            Execute("J,", "");
        }
    }
}
