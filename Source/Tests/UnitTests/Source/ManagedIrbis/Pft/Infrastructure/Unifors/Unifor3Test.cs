﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM.PlatformAbstraction;

using ManagedIrbis.Pft.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class Unifor3Test
    {
        private PftContext _GetContext()
        {
            var result = new PftContext(null);
            var layer = new TestingPlatformAbstraction()
            {
                NowValue = new DateTime(2007, 8, 9, 10, 11, 12)
            };
            result.Provider.PlatformAbstraction = layer;

            return result;
        }

        private void _Execute
            (
                string input,
                string expected
            )
        {
            var context = _GetContext();
            var unifor = new Unifor();
            var expression = input;
            unifor.Execute(context, null, expression);
            var actual = context.Text;
            Assert.AreEqual(expected, actual);
        }

        // Выдача текущей даты
        [TestMethod]
        public void Unifor3_PrintDate_1()
        {
            _Execute("3",  "20070809");
            _Execute("30", "2007");
            _Execute("31", "08");
            _Execute("32", "09");
            _Execute("33", "07");
            _Execute("34", "8");
            _Execute("35", "9");
        }

        // Названия месяцев
        [TestMethod]
        public void Unifor3_PrintDate_2()
        {
            _Execute("3601", "январь");
            _Execute("3701", "января");
            _Execute("3801", "january");

            // Обработка ошибок
            _Execute("36", "");
            _Execute("36qq", "");
            _Execute("3622", "");
        }

        // Выдача текущего времени
        [TestMethod]
        public void Unifor3_PrintDate_3()
        {
            _Execute("39", "10:11:12");
        }

        // Прибавление-убавление дат
        [TestMethod]
        public void Unifor3_PrintDate_4()
        {
            _Execute("3A", "221");
            _Execute("3B20070101/220", "20070809");
            _Execute("3C20070809/220", "20070101");

            // Обработка ошибок
            _Execute("3Bqwer/220", "");
            _Execute("3Bqwertyui/220", "");
            _Execute("3B20070101/qwe", "");
        }

        // Юлианскую дату в григрианскую
        [TestMethod]
        public void Unifor3_PrintDate_5()
        {
            _Execute("3J20070101", "20070114");

            // Обработка ошибок
            _Execute("3J2007", "");
            _Execute("3Jqwertyui", "");
        }

        // Неожиданные коды
        [TestMethod]
        public void Unifor3_PrintDate_6()
        {
            _Execute("3Q", "");
        }

        // Дельфийская дата
        [TestMethod]
        public void Unifor3_PrintDate_7()
        {
            _Execute("3M", "18991230 000000");
            _Execute("3M1", "18991231 000000");
            _Execute("3M1.5", "18991231 120000");
            _Execute("3M-1", "18991229 000000");
            _Execute("3M-1.5", "18991229 120000");
            _Execute("3M-1.25", "18991229 060000");
        }
    }
}
