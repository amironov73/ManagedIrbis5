// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusKTest
        : CommonUniforTest
    {
        [Ignore]
        [TestMethod]
        public void UniforPlusK_GetAuthorSign_1()
        {
            // Нормальное выполнение
            Execute("+Khav.mnu!Abstact", "A 16");
            Execute("+Khav.mnu!дополнительный", "Д 68");
            Execute("+Khav.mnu!Маркс", "М 27");
            Execute("+Khav.mnu!Ыстыбаев", "Ы");

            // Обработка ошибок
            Execute("+K", "");
            Execute("+K!", "");
            Execute("+K!дополнительный", "");
            Execute("+Khav.mnu!", "");
            Execute("+Khav.mnu?дополнительный", "");
        }
    }
}
