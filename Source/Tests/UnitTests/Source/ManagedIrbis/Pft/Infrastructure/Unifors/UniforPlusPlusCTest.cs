// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusPlusCTest
        : CommonUniforTest
    {
        [Ignore]
        [TestMethod]
        public void UniforPlusPlusC_WorkWithGlobalCounter_1()
        {
            // Нормальное выполнение
            Execute("++C01", "11");
            Execute("++C01#", "11");

            // Обработка ошибок
            Execute("++C", "");
            Execute("++C#1", "");
            Execute("++CnoSuchCounter#1", "");
        }
    }
}
