﻿// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.Globalization;
using System.IO;
using System.Text;

using AM;

using ManagedIrbis;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Pft.Infrastructure;
using ManagedIrbis.Pft.Infrastructure.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Pft.Infrastructure.Unifors
{
    [TestClass]
    public class UniforPlus9Test
        : CommonUniforTest
    {
        [TestMethod]
        public void UniforPlus9_GetCharacter_1()
        {
            // Нормальное выполнение
            Execute("+9F33", "!");

            // Обработка ошибок
            Execute("+9F", "");
        }

        [Ignore]
        [TestMethod]
        public void UniforPlus9_GetDirectoryName_1()
        {
            // TODO: Придумать что-нибудь насчет Linux и OSX

            // Нормальное выполнение
            Execute(@"+92C:\Windows\System32\esent.dll", @"C:\Windows\System32\");

            // Обработка ошибок
            Execute("+92", "");
            Execute("+92C:", "");
        }

        [Ignore]
        [TestMethod]
        public void UniforPlus9_GetDrive_1()
        {
            // TODO: Придумать что-нибудь насчет Linux и OSX

            // Нормальное выполнение
            Execute(@"+94C:\Windows\System32\esent.dll", "C:");

            // Обработка ошибок
            Execute("+94", "");
            Execute(@"+92\Windows", @"\");
            Execute(@"+92Windows", "");
        }

        [TestMethod]
        public void UniforPlus9_GetExtension_1()
        {
            // Нормальное выполнение
            Execute(@"+93C:\Windows\System32\esent.dll", ".dll");
            Execute(@"+93Windows.exe.dll", ".dll");

            // Обработка ошибок
            Execute("+93", "");
            Execute(@"+93\Windows", "");
            Execute("+93Windows", "");
        }

        [Ignore]
        [TestMethod]
        public void UniforPlus9_GetFileContent_1()
        {
            // Нормальное выполнение
            Execute("+9C2,IBIS,_test_hello.pft", "'Hello'");

            // Обработка ошибок
            Execute("+9C", "");
            Execute("+9C2,", "");
            Execute("+9C2,IBIS,", "");
            Execute("+9C2,IBIS,notexist.pft", "");
        }

        [Ignore]
        [TestMethod]
        public void UniforPlus9_FileExist_1()
        {
            // Нормальное выполнение
            Execute("+9L2,IBIS,_test_hello.pft", "1");
            Execute("+9L2,IBIS,notexist.pft", "0");

            // Не работают без Интернета
            //Execute("+9L0,,http://google.com", "1");
            //Execute("+9L0,,http://google.c", "0");

            // Обработка ошибок
            Execute("+9L", "");
            Execute("+9L2,", "");
            Execute("+9L2,IBIS,", "");
        }

        [Ignore]
        [TestMethod]
        public void UniforPlus9_GetFileName_1()
        {
            // TODO: Придумать что-нибудь насчет Linux и OSX

            // Нормальное выполнение
            Execute(@"+91C:\Windows\System32\esent.dll", "esent.dll");
            Execute(@"+91Windows.exe.dll", "Windows.exe.dll");
            Execute("+91.dll", ".dll");

            // Обработка ошибок
            Execute("+91", "");
            Execute(@"+91\", "");
        }

        [TestMethod]
        public void UniforPlus9_GetFileSize_1()
        {
            const string fileName = @"C:\Windows\System32\esent.dll";
            if (File.Exists(fileName))
            {
                var fileInfo = new FileInfo(fileName);
                var fileSize = fileInfo.Length;
                Execute(@"+9A" + fileName, fileSize.ToInvariantString());
            }
            Execute("+9A.dll", "0");

            // Обработка ошибок
            Execute("+9A", "");
        }

        [TestMethod]
        public void UniforPlus9_GetIndex_1()
        {
            Execute(null, null, 0, "+90", "0");
            Execute(null, null, 1, "+90", "1");

            var group = new PftGroup();
            Execute(group, null, 0, "+90", "1");
            Execute(group, null, 1, "+90", "2");
        }

        [Ignore]
        [TestMethod]
        public void UniforPlus9_GetGeneration_1()
        {
            Execute("+9V", "64");
        }

        [TestMethod]
        public void UniforPlus9_StringLength_1()
        {
            Execute("+95", "0");
            Execute("+95Hello", "5");
            Execute("+95Привет", "6");
        }

        [TestMethod]
        public void UniforPlus9_ReplaceCharacter_1()
        {
            // Нормальное выполнение
            Execute("+98ABHere is A letter, here is another A letter", "Here is B letter, here is another B letter");
            Execute("+98АЯА последняя буква в алфавите", "Я последняя буква в алфавите");
            Execute("+9BAB ", " ");

            // Обработка ошибок
            Execute("+9B", "");
            Execute("+9BA", "");
            Execute("+9BAB", "");
        }

        [TestMethod]
        public void UniforPlus9_ReplaceString_1()
        {
            // Нормальное выполнение
            Execute("+9I!A!/B/Here is A letter, here is another A letter", "Here is B letter, here is another B letter");
            Execute("+9I!А!/Я/А последняя буква в алфавите", "Я последняя буква в алфавите");
            Execute("+9I!A!/B/ ", " ");

            // Обработка ошибок
            Execute("+9I", "");
            Execute("+9I!", "");
            Execute("+9I!A", "");
            Execute("+9I!A!", "");
            Execute("+9I!A!/", "");
            Execute("+9I!A!/B", "");
            Execute("+9I!A!/B/", "");
        }

        [TestMethod]
        public void UniforPlus9_SplitWords_1()
        {
            // Нормальное выполнение
            Execute(@"+9GC:\Windows\System32\esent.dll", "C\nWINDOWS\nSYSTEM\nESENT\nDLL\n");
            Execute(@"+9GЂорђе Балашевић", "ЂОРЂЕ\nБАЛАШЕВИЋ\n");
            Execute(@"+9G123,456", "");

            // Обработка ошибок
            Execute("+9G", "");
        }

        [TestMethod]
        public void UniforPlus9_Substring_1()
        {
            // Нормальное выполнение
            Execute("+960*5.2#Here is A letter, here is another A letter", "is");
            Execute("+960*5#Here is A letter, here is another A letter", "is A letter, here is another A letter");
            Execute("+960.2#Here is A letter, here is another A letter", "He");

            Execute("+961*2.6#Here is A letter, here is another A letter", "A lett");
            Execute("+961.6#Here is A letter, here is another A letter", "letter");
            Execute("+961*2#Here is A letter, here is another A letter", "Here is A letter, here is another A lett");

            // Обработка ошибок
            Execute("+96", "");
            Execute("+96Here is A letter, here is another A letter", "ere is A letter, here is another A letter");
            Execute("+96Q*2.6#Here is A letter, here is another A letter", "A lett");
            Execute("+960#Here is A letter, here is another A letter", "Here is A letter, here is another A letter");
        }

        [TestMethod]
        public void UniforPlus9_ToUpper_1()
        {
            // Нормальное выполнение
            Execute(@"+97C:\Windows\System32\esent.dll", @"C:\WINDOWS\SYSTEM32\ESENT.DLL");
            Execute("+97У попа была собака", "У ПОПА БЫЛА СОБАКА");

            // Обработка ошибок
            Execute("+97", "");
        }

        [TestMethod]
        public void UniforPlus9_FindSubstring_1()
        {
            // Нормальное выполнение
            Execute("+9S!9!98A7", "1");
            Execute("+9S!a!98A7", "3");
            Execute("+9S!b!98A7", "0");

            // Обработка ошибок
            Execute("+9S", "0");
            Execute("+9S!", "0");
            Execute("+9S!b", "0");
            Execute("+9S!b!", "0");
            Execute("+9S!!98A7", "0");
        }

        [TestMethod]
        public void UniforPlus9_PrintNumbers_1()
        {
            // Нормальное выполнение
            Execute("+9T123/130", "123\n124\n125\n126\n127\n128\n129\n130");
            Execute("+9T123/30", "");
            Execute("+9T3/10", "3\n4\n5\n6\n7\n8\n9\n0");
            Execute("+9T-3/10", "-3\n-2\n-1\n00\n01\n02\n03\n04\n05\n06\n07\n08\n09\n10");
            Execute("+9TA3A/10", "003\n004\n005\n006\n007\n008\n009\n010");

            // Обработка ошибок
            Execute("+9T", "");
            Execute("+9Tnumber", "number");
            Execute("+9T123", "123");
            Execute("+9T123/", "");
        }

        [TestMethod]
        public void UniforPlus9_FormatFileSize_1()
        {
            // Нормальное выполнение
            Execute("+9E0", "0");
            Execute("+9E1", "1 b");
            Execute("+9E12", "12 b");
            Execute("+9E123", "123 b");
            Execute("+9E1234", "1.234 Kb");
            Execute("+9E12345", "12.345 Kb");
            Execute("+9E123456", "123.456 Kb");
            Execute("+9E1234567", "1.235 Mb");
            Execute("+9E12345678", "12.346 Mb");
            Execute("+9E123456789", "123.457 Mb");
            Execute("+9E1234567890", "1.235 Gb");

            // Обработка ошибок
            Execute("+9E", "0");
            Execute("+9EABC", "0");
            Execute("+9E-123", "0");
            Execute("+9E123Q", "0");
        }

        [Ignore]
        [TestMethod]
        public void UniforPlus9_ReadFileAsBinaryResource_1()
        {
            var fileName = Path.Combine
                (
                    TestDataPath,
                    "EMPTY.MST"
                );
            Execute("+9J"+fileName, "^aMST^b%00%00%00%00%00%00%00%01%00%00%00%24%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00%00");

            // Обработка ошибок
            Execute("+9J", "");
            Execute("+9JC:\\nosuchfile", "");
        }

        [TestMethod]
        public void UniforPlus9_ConcatenateStrings_1()
        {
            // Нормальное ыполнение
            Execute("+9H!Hello, !world!", "Hello, world!");
            Execute("+9H!Hello, !", "");
            Execute("+9H!!Hello!", "Hello!");
            Execute("+9H!!!", "!");
            Execute("+9H!!", "");

            // Обработка ошибок
            Execute("+9H", "");
            Execute("+9H!", "");
            Execute("+9HHello", "");
        }

        [TestMethod]
        public void UniforPlus9_AssignGlobals_1()
        {
            Field[] globals =
            {
                new (100, "Field100"),
                new (200, "^aFirst"),
                new (200, "^aSecond"),
                new (300, "Field300"),
            };
            var builder = new StringBuilder();
            foreach (var field in globals)
            {
                builder.AppendLine
                    (
                        string.Format
                            (
                                CultureInfo.InvariantCulture,
                                "{0}#{1}",
                                field.Tag,
                                field.ToText()
                            )
                    );
            }
            var encoded = Utility.UrlEncode
                (
                    builder.ToString(),
                    IrbisEncoding.Utf8
                );

            var context = new PftContext(null);
            var unifor = new Unifor();
            unifor.Execute(context, null, "+99" + encoded);

            var fields = context.Globals.Get(100);
            Assert.AreEqual(1, fields.Length);

            fields = context.Globals.Get(200);
            Assert.AreEqual(2, fields.Length);

            fields = context.Globals.Get(300);
            Assert.AreEqual(1, fields.Length);

            fields = context.Globals.Get(400);
            Assert.AreEqual(0, fields.Length);
        }

        [Ignore]
        [TestMethod]
        public void UniforPlus9_NextTerm_1()
        {
            // Нормальное выполнение
            Execute("+9NISTU,K=ALG", "K=ALGEBRAS");
            Execute("+9NISTU,K=ANALYTICAL", "K=ANNALES");
            Execute("+9NISTU,K=ALGEBRAS", "K=ALMA");
            Execute("+9NISTU,A=ЯЯЯ", "AF=-S2");

            // Обработка ошибок
            Execute("+9N", "");
        }

        // TODO: починить
        //[TestMethod]
        //public void UniforPlus9_PreviousTerm_1()
        //{
        //    Execute("+9PISTU,K=ALMA", "K=ALGEBRAS");
        //    Execute("+9PISTU,K=ALGEBRAS", "K=ANALYTICAL");
        //    Execute("+9PISTU,K=ALMA", "K=ALGEBRAS");
        //    Execute("+9PISTU,K=AAA", "");

        //    // Обработка ошибок
        //    Execute("+9P", "");
        //}

        [TestMethod]
        public void UniforPlus9_RomanToArabic_1()
        {
            // Нормальное выполнение
            Execute("+9RI", "1");
            Execute("+9Ri", "1");
            Execute("+9RII", "2");
            Execute("+9RIII", "3");
            Execute("+9RIIII", "4");
            Execute("+9RIV", "4");
            Execute("+9RV", "5");
            Execute("+9RVI", "6");
            Execute("+9RVII", "7");
            Execute("+9RVIII", "8");
            Execute("+9RVIIII", "9");
            Execute("+9RIX", "9");
            Execute("+9RX", "10");
            Execute("+9RXI", "11");
            Execute("+9RXII", "12");
            Execute("+9RXV", "15");
            Execute("+9RXX", "20");
            Execute("+9RXXX", "30");
            Execute("+9RXXXX", "40");
            Execute("+9RXL", "40");
            Execute("+9RL", "50");
            Execute("+9RLI", "51");
            Execute("+9RLXI", "61");
            Execute("+9RC", "100");
            Execute("+9RCLXI", "161");
            Execute("+9RDCLXI", "661");
            Execute("+9Rmmxviii", "2018");

            // Обработка ошибок
            Execute("+9R", "");
        }

        [TestMethod]
        public void UniforPlus9_ArabicToRoman_1()
        {
            // Нормальное выполнение
            Execute("+9X4", "IV");
            Execute("+9X6", "VI");
            Execute("+9X9", "IX");
            Execute("+9X23", "XXIII");
            Execute("+9X43", "XLIII");
            Execute("+9X53", "LIII");
            Execute("+9X93", "XCIII");
            Execute("+9X123", "CXXIII");
            Execute("+9X423", "CDXXIII");
            Execute("+9X523", "DXXIII");
            Execute("+9X923", "CMXXIII");
            Execute("+9X1234", "MCCXXXIV");
            Execute("+9X3234", "MMMCCXXXIV");

            // Обработка ошибок
            Execute("+9X", "");
            Execute("+9X6234", "");
        }
    }
}
