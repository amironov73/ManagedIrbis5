// ReSharper disable CheckNamespace
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#nullable enable

namespace UnitTests.AM.Text
{
    [TestClass]
    public unsafe class ValueStringBuilderTest
    {
        [TestMethod]
        [Description ("Конструктор с предварительно размещенным буфером")]
        public void ValueStringBuilder_Constructor_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            Assert.AreEqual (0, builder.Length);
            Assert.AreEqual (bufferLength, builder.Capacity);
            Assert.AreEqual (string.Empty, builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление по одному символу")]
        public void ValueStringBuilder_Append_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append ('H');
            Assert.AreEqual (1, builder.Length);
            builder.Append ('e');
            Assert.AreEqual (2, builder.Length);
            builder.Append ('l');
            Assert.AreEqual (3, builder.Length);
            builder.Append ('l');
            Assert.AreEqual (4, builder.Length);
            builder.Append ('o');
            Assert.AreEqual (5, builder.Length);
            Assert.AreEqual ("Hello", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление строк по одной")]
        public void ValueStringBuilder_Append_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append ("Hello, ");
            Assert.AreEqual (7, builder.Length);
            builder.Append ("world!");
            Assert.AreEqual (13, builder.Length);
            Assert.AreEqual ("Hello, world!", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление двух строк сразу")]
        public void ValueStringBuilder_Append_3()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append ("Hello, ", "world!");
            Assert.AreEqual (13, builder.Length);
            Assert.AreEqual ("Hello, world!", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление длинной строки")]
        public void ValueStringBuilder_Append_4()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var longString = new string ('x', 80);

            builder.Append (longString);
            Assert.AreEqual (longString.Length, builder.Length);
            Assert.AreEqual (longString, builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление двух длинных строк сразу")]
        public void ValueStringBuilder_Append_5()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var longString = new string ('x', 80);

            builder.Append (longString, longString);
            Assert.AreEqual (longString.Length * 2, builder.Length);
            Assert.AreEqual (longString + longString, builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление длинных строк по одной")]
        public void ValueStringBuilder_Append_6()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var longString = new string ('x', 80);

            builder.Append (longString);
            builder.Append (longString);
            Assert.AreEqual (longString.Length * 2, builder.Length);
            Assert.AreEqual (longString + longString, builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление трех длинных строк сразу")]
        public void ValueStringBuilder_Append_7()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var longString = new string ('x', 80);

            builder.Append (longString, longString, longString);
            Assert.AreEqual (longString.Length * 3, builder.Length);
            Assert.AreEqual (longString + longString + longString, builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление множества символов по одному")]
        public void ValueStringBuilder_Append_8()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            for (var i = 0; i < 100; i++)
            {
                builder.Append ('x');
            }

            Assert.AreEqual (100, builder.Length);
            Assert.AreEqual (new string ('x', 100), builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление целого числа")]
        public void ValueStringBuilder_Append_9()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append (123);
            Assert.AreEqual (3, builder.Length);
            Assert.AreEqual (3, builder.AsSpan().Length);
            Assert.AreEqual ("123", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление целого числа при нехватке места в буфере")]
        public void ValueStringBuilder_Append_10()
        {
            const int bufferLength = 2;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append (123);
            Assert.AreEqual (3, builder.Length);
            Assert.AreEqual (3, builder.AsSpan().Length);
            Assert.AreEqual ("123", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление целого числа без знака")]
        public void ValueStringBuilder_Append_11()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append (123u);
            Assert.AreEqual (3, builder.Length);
            Assert.AreEqual (3, builder.AsSpan().Length);
            Assert.AreEqual ("123", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление целого числа без знака при нехватке места в буфере")]
        public void ValueStringBuilder_Append_12()
        {
            const int bufferLength = 2;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append (123u);
            Assert.AreEqual (3, builder.Length);
            Assert.AreEqual (3, builder.AsSpan().Length);
            Assert.AreEqual ("123", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление длинного целого числа")]
        public void ValueStringBuilder_Append_13()
        {
            const int bufferLength = 20;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append (123L);
            Assert.AreEqual (3, builder.Length);
            Assert.AreEqual (3, builder.AsSpan().Length);
            Assert.AreEqual ("123", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление длинного целого числа при нехватке места в буфере")]
        public void ValueStringBuilder_Append_14()
        {
            const int bufferLength = 2;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append (123L);
            Assert.AreEqual (3, builder.Length);
            Assert.AreEqual (3, builder.AsSpan().Length);
            Assert.AreEqual ("123", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление длинного целого числа без знака")]
        public void ValueStringBuilder_Append_15()
        {
            const int bufferLength = 20;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append (123ul);
            Assert.AreEqual (3, builder.Length);
            Assert.AreEqual (3, builder.AsSpan().Length);
            Assert.AreEqual ("123", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление целого числа без знака при нехватке места в буфере")]
        public void ValueStringBuilder_Append_16()
        {
            const int bufferLength = 2;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append (123ul);
            Assert.AreEqual (3, builder.Length);
            Assert.AreEqual (3, builder.AsSpan().Length);
            Assert.AreEqual ("123", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление перевода строки")]
        public void ValueStringBuilder_AppendLine_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.AppendLine();

            var newLine = Environment.NewLine;
            Assert.AreEqual (builder.Length, newLine.Length);
            Assert.AreEqual (newLine, builder.ToString());
        }

        [TestMethod]
        [Description ("Уменьшение длины извне")]
        public void ValueStringBuilder_Length_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            buffer.Fill ('x');
            builder.Length = 5;
            Assert.AreEqual (5, builder.Length);
            Assert.AreEqual ("xxxxx", builder.ToString());
        }

        [TestMethod]
        [Description ("Доступ к сырому буферу")]
        public void ValueStringBuilder_RawCharacters_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            buffer.Fill ('x');
            Assert.AreEqual ('x', builder.RawCharacters[0]);
        }

        [TestMethod]
        [Description ("Доступ к символам по индексу")]
        public void ValueStringBuilder_Indexer_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            buffer.Fill ('x');
            Assert.AreEqual ('x', builder[1]);
            builder[1] = 'X';
            Assert.AreEqual ('X', builder[1]);
            builder.Length = 2;
            Assert.AreEqual ("xX", builder.ToString());
        }

        [TestMethod]
        [Description ("Увеличение емкости при необходимости")]
        public void ValueStringBuilder_EnsureCapacity_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.EnsureCapacity (100);
            Assert.IsTrue (100 <= builder.Capacity);
            builder.Dispose();
        }

        [TestMethod]
        [Description ("Преобразование в диапазон символов")]
        public void ValueStringBuilder_AsSpan_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append ("Hello, world");
            var span = builder.AsSpan (7);
            Assert.AreEqual ("world", span.ToString());
            builder.Dispose();
        }

        [TestMethod]
        [Description ("Преобразование в диапазон символов")]
        public void ValueStringBuilder_AsSpan_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);

            builder.Append ("Hello, world");
            var span = builder.AsSpan (0, 5);
            Assert.AreEqual ("Hello", span.ToString());
            builder.Dispose();
        }

        [TestMethod]
        [Description ("Перечисление символов")]
        public void ValueStringBuilder_GetEnumerator_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("Hello");

            var enumerator = builder.GetEnumerator();
            Assert.IsTrue (enumerator.MoveNext());
            Assert.AreEqual ('H', enumerator.Current);
            Assert.IsTrue (enumerator.MoveNext());
            Assert.AreEqual ('e', enumerator.Current);
            Assert.IsTrue (enumerator.MoveNext());
            Assert.AreEqual ('l', enumerator.Current);
            Assert.IsTrue (enumerator.MoveNext());
            Assert.AreEqual ('l', enumerator.Current);
            Assert.IsTrue (enumerator.MoveNext());
            Assert.AreEqual ('o', enumerator.Current);
            Assert.IsFalse (enumerator.MoveNext());
        }

        [TestMethod]
        [Description ("Перечисление символов")]
        public void ValueStringBuilder_GetEnumerator_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("Hello");

            var index = 0;
            var array = new char[5];
            foreach (var c in builder)
            {
                array[index++] = c;
            }

            Assert.AreEqual (5, index);
            Assert.AreEqual ('H', array[0]);
            Assert.AreEqual ('e', array[1]);
            Assert.AreEqual ('l', array[2]);
            Assert.AreEqual ('l', array[3]);
            Assert.AreEqual ('o', array[4]);
        }

        [TestMethod]
        [Description ("Добавление символов: вырожденный случай")]
        public void ValueStringBuilder_Insert_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("abcdefgh");

            builder.Insert (3, ' ', 0);
            Assert.AreEqual ("abcdefgh", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление символов: невырожденный случай")]
        public void ValueStringBuilder_Insert_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("abcdefgh");

            builder.Insert (3, '_', 3);
            Assert.AreEqual ("abc___defgh", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление символов: нехватка места в буфере")]
        public void ValueStringBuilder_Insert_3()
        {
            const int bufferLength = 9;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("abcdefgh");

            builder.Insert (3, '_', 3);
            Assert.AreEqual ("abc___defgh", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление текста: вырожденный случай")]
        public void ValueStringBuilder_Insert_4()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("abcdefgh");

            builder.Insert (3, ReadOnlySpan<char>.Empty);
            Assert.AreEqual ("abcdefgh", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление текста: невырожденный случай")]
        public void ValueStringBuilder_Insert_5()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("abcdefgh");

            builder.Insert (3, "123");
            Assert.AreEqual ("abc123defgh", builder.ToString());
        }

        [TestMethod]
        [Description ("Добавление текста: нехватка места в буфере")]
        public void ValueStringBuilder_Insert_6()
        {
            const int bufferLength = 9;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("abcdefgh");

            builder.Insert (3, "123");
            Assert.AreEqual ("abc123defgh", builder.ToString());
        }

        [TestMethod]
        [Description ("Чтение строки: вырожденный случай")]
        public void ValueStringBuilder_ReadLine_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var reader = new StringReader (string.Empty);

            builder.ReadLine (reader);
            Assert.AreEqual (0, builder.Length);
        }

        [TestMethod]
        [Description ("Чтение строки: невырожденный случай")]
        public void ValueStringBuilder_ReadLine_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var reader = new StringReader ("Hello");

            builder.ReadLine (reader);
            Assert.AreEqual ("Hello", builder.ToString());
        }

        [TestMethod]
        [Description ("Чтение строки: есть перевод строки")]
        public void ValueStringBuilder_ReadLine_3()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var reader = new StringReader ("Hello\nworld");

            builder.ReadLine (reader);
            Assert.AreEqual ("Hello", builder.ToString());
        }

        [TestMethod]
        [Description ("Чтение строки: есть перевод строки")]
        public void ValueStringBuilder_ReadLine_4()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var reader = new StringReader ("Hello\nworld");

            builder.ReadLine (reader, true);
            Assert.AreEqual ("Hello\n", builder.ToString());
        }

        [TestMethod]
        [Description ("Чтение строки: есть перевод строки")]
        public void ValueStringBuilder_ReadLine_5()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            var reader = new StringReader ("Hello\r\nworld");

            builder.ReadLine (reader, true);
            Assert.AreEqual ("Hello\n", builder.ToString());
        }

        [TestMethod]
        [Description ("Удаление символов: вырожденный случай")]
        public void ValueStringBuilder_Remove_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Remove (1, 0);
            Assert.AreEqual (10, builder.Length);
        }

        [TestMethod]
        [Description ("Удаление символов: вырожденный случай")]
        public void ValueStringBuilder_Remove_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Remove (0, 10);
            Assert.AreEqual (0, builder.Length);
        }

        [TestMethod]
        [Description ("Удаление символов: невырожденный случай")]
        public void ValueStringBuilder_Remove_3()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Remove (3, 3);
            Assert.AreEqual (7, builder.Length);
            Assert.AreEqual ("0126789", builder.ToString());
        }

        [TestMethod]
        [Description ("Удаление символов: неверный индекс")]
        [ExpectedException (typeof (ArgumentOutOfRangeException))]
        public void ValueStringBuilder_Remove_4()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Remove (3, 13);
        }

        [TestMethod]
        [Description ("Замена подстроки: на такую же")]
        public void ValueStringBuilder_Replace_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Replace ("23", "ab");
            Assert.AreEqual ("01ab456789", builder.ToString());
        }

        [TestMethod]
        [Description ("Замена подстроки: на более короткую")]
        public void ValueStringBuilder_Replace_2()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Replace ("23", "a");
            Assert.AreEqual ("01a456789", builder.ToString());
        }

        [TestMethod]
        [Description ("Замена подстроки: на пустую")]
        public void ValueStringBuilder_Replace_3()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Replace ("23", string.Empty);
            Assert.AreEqual ("01456789", builder.ToString());
        }

        [TestMethod]
        [Description ("Замена подстроки: на более длинную")]
        public void ValueStringBuilder_Replace_4()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Replace ("23", "abcde");
            Assert.AreEqual ("01abcde456789", builder.ToString());
        }

        [TestMethod]
        [Description ("Замена подстроки: на более длинную")]
        public void ValueStringBuilder_Replace_5()
        {
            const int bufferLength = 10;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Replace ("23", "abcdefghijklm");
            Assert.AreEqual ("01abcdefghijklm456789", builder.ToString());
        }

        [TestMethod]
        [Description ("Очистка")]
        public void ValueStringBuilder_Clear_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Clear();
            Assert.AreEqual (0, builder.Length);
        }

        [TestMethod]
        [Description ("Очистка")]
        public void ValueStringBuilder_Dispose_1()
        {
            const int bufferLength = 16;
            Span<char> buffer = stackalloc char[bufferLength];
            var builder = new ValueStringBuilder (buffer);
            builder.Append ("0123456789");

            builder.Dispose();
            Assert.AreEqual (0, builder.Length);
        }
    }
}
