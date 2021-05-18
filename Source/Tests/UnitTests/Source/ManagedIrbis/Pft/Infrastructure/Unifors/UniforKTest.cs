// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforKTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforK_GetMenuEntry_1()
        {
            // Нормальное выполнение
            Execute("Kfo.mnu!д/о", "Дневное отделение");
            Execute("Kfo.mnu|д/о", "Дневное отделение");

            // Обработка ошибок
            Execute("Kfo.mnu?д/о", "");
            Execute("K!", "");
            Execute("Kfo.mnu!", "");
        }
    }
}
