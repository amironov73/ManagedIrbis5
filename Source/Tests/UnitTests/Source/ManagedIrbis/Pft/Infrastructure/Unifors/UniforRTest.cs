﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforRTest
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforR_RandomNumber_1()
        {
            Execute("R", "984556");
            Execute("R5", "98455");

            // Обработка ошибок
            Execute("R0", "");
            Execute("R10", "");
            Execute("R-1", "");
        }
    }
}
