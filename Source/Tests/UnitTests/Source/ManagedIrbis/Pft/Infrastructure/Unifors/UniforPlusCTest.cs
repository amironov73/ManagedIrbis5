﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlusCTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlusC_Increment_1()
        {
            // Нормальное выполнение
            Execute("+CHello123", "Hello124");
            Execute("+C123Hello", "124Hello");
            Execute("+C123", "124");
            Execute("+CHello123456790123", "Hello1234567902");

            // Обработка ошибок
            Execute("+C", "");
            Execute("+CHello", "Hello");
            Execute("+C123Hello456", "123Hello456");
        }
    }
}
