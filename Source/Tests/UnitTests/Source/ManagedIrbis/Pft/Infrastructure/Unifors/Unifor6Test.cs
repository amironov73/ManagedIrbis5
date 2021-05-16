﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor6Test
        : CommonUniforTest
    {
        [Ignore]
        [TestMethod]
        public void Unifor6_ExecuteNestedFormat_1()
        {
            // Нормальное выполнение
            Execute("6_test_hello", "Hello");
            Execute("6_test_onearg#700", "Field v700 present");
            Execute("6_test_twoarg#700,701", "Field v700 present\nField v701 present");

            // Обработка ошибок
            Execute("6", "");
            Execute("6#700", "");
            Execute("6notexist", "");
        }
    }
}
