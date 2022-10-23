// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ConvertToLocalFunction
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#endregion

#nullable enable

namespace UnitTests.AM.Text;

[TestClass]
public class UnsafeTextNavigatorTest
{
    [TestMethod]
    [Description ("Состояние навигатора сразу после создания")]
    public unsafe void UnsafeTextNavigator_Constructor_1()
    {
        const string text = "ABC";
        fixed (char* ptr = text)
        {
            var navigator = new UnsafeTextNavigator (ptr, text.Length);
            Assert.AreEqual (0, navigator.Position);
            Assert.AreEqual (3, navigator.Length);
        }
    }

    [TestMethod]
    [Description ("Преобразование в диапазон памяти")]
    public void UnsafeTextNavigator_AsSpan_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        var span = navigator.AsSpan;
        Assert.AreEqual (3, span.Length);
    }

    [TestMethod]
    [Description ("Проверка на достижение конца текста")]
    public void UnsafeTextNavigator_IsEOF_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsFalse (navigator.IsEOF);
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsEOF);
        navigator.SkipChar (2);
        Assert.IsTrue (navigator.IsEOF);
    }

    [TestMethod]
    [Description ("Вычисление длины текста")]
    public void UnsafeTextNavigator_Length_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual (text.Length, navigator.Length);
    }

    [TestMethod]
    [Description ("Отслеживание текущей позиции в тексте")]
    public void UnsafeTextNavigator_Position_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual (0, navigator.Position);
        navigator.ReadChar();
        Assert.AreEqual (1, navigator.Position);
        navigator.ReadChar();
        Assert.AreEqual (2, navigator.Position);
        navigator.ReadChar();
        Assert.AreEqual (3, navigator.Position);
        navigator.ReadChar();
        Assert.AreEqual (3, navigator.Position);
    }

    [TestMethod]
    [Description ("Доступ к оригинальному тексту")]
    public void UnsafeTextNavigator_Text_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual (text, navigator.Text);
    }

    [TestMethod]
    [Description ("Клонирование навигатора")]
    public void UnsafeTextNavigator_Clone_1()
    {
        const string text = "ABC";
        var first = new UnsafeTextNavigator (text);
        first.ReadChar();
        var second = first.Clone();
        Assert.AreEqual (first.Text, second.Text);
        Assert.AreEqual (first.Position, second.Position);
    }

    [TestMethod]
    [Description ("Получение оставшегося текста")]
    public void UnsafeTextNavigator_GetRemainingText_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual (text, navigator.GetRemainingText().ToString());
        navigator.ReadChar();
        Assert.AreEqual ("BC", navigator.GetRemainingText().ToString());
        navigator.ReadChar();
        Assert.AreEqual ("C", navigator.GetRemainingText().ToString());
        navigator.ReadChar();
        Assert.IsTrue (navigator.GetRemainingText().IsEmpty);
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции управляющий символ")]
    public void UnsafeTextNavigator_IsControl_1()
    {
        const string text = "A\tBC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsFalse (navigator.IsControl());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsControl());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsControl());
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции находится цифра")]
    public void UnsafeTextNavigator_IsDigit_1()
    {
        const string text = "A1BC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsFalse (navigator.IsDigit());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsDigit());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsDigit());
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции находится буква")]
    public void UnsafeTextNavigator_IsLetter_1()
    {
        const string text = "A1BC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsTrue (navigator.IsLetter());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsLetter());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsLetter());
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции находится буква или цифра")]
    public void UnsafeTextNavigator_IsLetterOrDigit_1()
    {
        const string text = "A_1";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsTrue (navigator.IsLetterOrDigit());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsLetterOrDigit());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsLetterOrDigit());
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции находится числовой символ")]
    public void UnsafeTextNavigator_IsNumber_1()
    {
        const string text = "1+²";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsTrue (navigator.IsNumber());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsNumber());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsNumber());
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции находится символ пунктуации")]
    public void UnsafeTextNavigator_IsPunctuation_1()
    {
        const string text = ".A,";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsTrue (navigator.IsPunctuation());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsPunctuation());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsPunctuation());
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции находится символ-разделитель")]
    public void UnsafeTextNavigator_IsSeparator_1()
    {
        const string text = "\u2028A ";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsTrue (navigator.IsSeparator());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsSeparator());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsSeparator());
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции находится символ (в терминах Unicode)")]
    public void UnsafeTextNavigator_IsSymbol_1()
    {
        const string text = "$A+";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsTrue (navigator.IsSymbol());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsSymbol());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsSymbol());
    }

    [TestMethod]
    [Description ("Проверка на то, что в текущей позиции находится пробельный символ")]
    public void UnsafeTextNavigator_IsWhiteSpace_1()
    {
        const string text = " A\t";
        var navigator = new UnsafeTextNavigator (text);
        Assert.IsTrue (navigator.IsWhiteSpace());
        navigator.ReadChar();
        Assert.IsFalse (navigator.IsWhiteSpace());
        navigator.ReadChar();
        Assert.IsTrue (navigator.IsWhiteSpace());
    }

    [TestMethod]
    [Description ("Заглядывание вперед на одну позицию")]
    public void UnsafeTextNavigator_LookAhead_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ('B', navigator.LookAhead());
        navigator.ReadChar();
        Assert.AreEqual ('C', navigator.LookAhead());
        navigator.ReadChar();
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.LookAhead());
        navigator.ReadChar();
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.LookAhead());
    }

    [TestMethod]
    [Description ("Заглядывание вперед на две позиции")]
    public void UnsafeTextNavigator_LookAhead_2()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ('C', navigator.LookAhead (2));
        navigator.ReadChar();
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.LookAhead (2));
        navigator.ReadChar();
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.LookAhead (2));
        navigator.ReadChar();
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.LookAhead (2));
    }

    [TestMethod]
    [Description ("Заглядывание назад на одну позицию")]
    public void UnsafeTextNavigator_LookBehind_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.LookBehind());
        navigator.ReadChar();
        Assert.AreEqual ('A', navigator.LookBehind());
        navigator.ReadChar();
        Assert.AreEqual ('B', navigator.LookBehind());
        navigator.ReadChar();
        Assert.AreEqual ('C', navigator.LookBehind());
        navigator.ReadChar();
        Assert.AreEqual ('C', navigator.LookBehind());
        navigator.ReadChar();
    }

    [TestMethod]
    [Description ("Заглядывание назад на две позиции")]
    public void UnsafeTextNavigator_LookBehind_2()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.LookBehind (2));
        navigator.ReadChar();
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.LookBehind (2));
        navigator.ReadChar();
        Assert.AreEqual ('A', navigator.LookBehind (2));
        navigator.ReadChar();
        Assert.AreEqual ('B', navigator.LookBehind (2));
        navigator.ReadChar();
        Assert.AreEqual ('B', navigator.LookBehind (2));
        navigator.ReadChar();
    }

    [TestMethod]
    [Description ("Свободное передвижения по тексту")]
    public void UnsafeTextNavigator_Move_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        navigator.Move (2);
        Assert.AreEqual (2, navigator.Position);
        navigator.Move (-2);
        Assert.AreEqual (0, navigator.Position);
    }

    [TestMethod]
    [Description ("Подглядывание следующего символа")]
    public void UnsafeTextNavigator_PeekChar_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ('A', navigator.PeekChar());
        navigator.ReadChar();
        Assert.AreEqual ('B', navigator.PeekChar());
        navigator.ReadChar();
        Assert.AreEqual ('C', navigator.PeekChar());
        navigator.ReadChar();
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.PeekChar());
    }

    [TestMethod]
    [Description ("Подглядывание строки заданной длины")]
    public void UnsafeTextNavigator_PeekString_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ("AB", navigator.PeekString (2).ToString());
        navigator.ReadChar();
        Assert.AreEqual ("BC", navigator.PeekString (2).ToString());
        navigator.ReadChar();
        Assert.AreEqual ("C", navigator.PeekString (2).ToString());
        navigator.ReadChar();
        Assert.IsTrue (navigator.PeekString (2).IsEmpty);
    }

    [TestMethod]
    [Description ("Подглядывание вплоть до указанного символа")]
    public void UnsafeTextNavigator_PeekTo_1()
    {
        const string text = "ABC]DEF";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ("ABC]", navigator.PeekTo (']').ToString());
        navigator.ReadChar();
        Assert.AreEqual ("BC]", navigator.PeekTo (']').ToString());
        navigator.ReadChar();
        navigator.ReadChar();
        Assert.AreEqual ("]", navigator.PeekTo (']').ToString());
        navigator.ReadChar();
        Assert.AreEqual ("DEF", navigator.PeekTo (']').ToString());
        navigator.Move (3);
        Assert.IsTrue (navigator.PeekTo (']').IsEmpty);
    }

    [TestMethod]
    [Description ("Подглядывание вплоть до указанных символов")]
    public void UnsafeTextNavigator_PeekTo_2()
    {
        const string text = "ABC]DE+F";
        char[] stop = { ']', '+' };
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ("ABC]", navigator.PeekTo (stop).ToString());
        navigator.ReadChar();
        Assert.AreEqual ("BC]", navigator.PeekTo (stop).ToString());
        navigator.ReadChar();
        navigator.ReadChar();
        Assert.AreEqual ("]", navigator.PeekTo (stop).ToString());
        navigator.ReadChar();
        Assert.AreEqual ("DE+", navigator.PeekTo (stop).ToString());
        navigator.Move (3);
        Assert.AreEqual ("F", navigator.PeekTo (stop).ToString());
        navigator.ReadChar();
        Assert.IsTrue (navigator.PeekTo (stop).IsEmpty);
    }

    [TestMethod]
    [Description ("Подглядывание вплоть до указанного символа")]
    public void UnsafeTextNavigator_PeekUntil_1()
    {
        const string text = "ABC]DEF";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ("ABC", navigator.PeekUntil (']').ToString());
        navigator.ReadChar();
        Assert.AreEqual ("BC", navigator.PeekUntil (']').ToString());
        navigator.ReadChar();
        navigator.ReadChar();
        Assert.AreEqual (string.Empty, navigator.PeekUntil (']').ToString());
        navigator.ReadChar();
        Assert.AreEqual ("DEF", navigator.PeekUntil (']').ToString());
        navigator.Move (3);
        Assert.IsTrue (navigator.PeekUntil (']').IsEmpty);
    }

    [TestMethod]
    [Description ("Подглядывание вплоть до указанных символов")]
    public void UnsafeTextNavigator_PeekUntil_2()
    {
        const string text = "ABC]DE+F";
        char[] stop = { ']', '+' };
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ("ABC", navigator.PeekUntil (stop).ToString());
        navigator.ReadChar();
        Assert.AreEqual ("BC", navigator.PeekUntil (stop).ToString());
        navigator.ReadChar();
        navigator.ReadChar();
        Assert.AreEqual (string.Empty, navigator.PeekUntil (stop).ToString());
        navigator.ReadChar();
        Assert.AreEqual ("DE", navigator.PeekUntil (stop).ToString());
        navigator.Move (3);
        Assert.AreEqual ("F", navigator.PeekUntil (stop).ToString());
        navigator.ReadChar();
        Assert.IsTrue (navigator.PeekUntil (stop).IsEmpty);
    }

    [TestMethod]
    [Description ("Считывание символов по одному")]
    public void UnsafeTextNavigator_ReadChar_1()
    {
        const string text = "ABC";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ('A', navigator.ReadChar());
        Assert.AreEqual ('B', navigator.ReadChar());
        Assert.AreEqual ('C', navigator.ReadChar());
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.ReadChar());
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.ReadChar());
    }

    [TestMethod]
    [Description ("Считывание экранированной строки вплоть до указанного символа")]
    public void UnsafeTextNavigator_ReadEscapedUntil_1()
    {
        const string text = "AB[tC]D";
        var navigator = new UnsafeTextNavigator (text);
        const string expected = "ABtC";
        var actual = navigator.ReadEscapedUntil ('[', ']');
        Assert.AreEqual (expected, actual);
        Assert.AreEqual ('D', navigator.ReadChar());
        Assert.IsNull (navigator.ReadEscapedUntil ('[', ']'));
    }

    [TestMethod]
    [Description ("Считывание экранированной строки вплоть до указанных символов")]
    public void UnsafeTextNavigator_ReadEscapedUntil_2()
    {
        const string text = "AB[tC";
        var navigator = new UnsafeTextNavigator (text);
        const string expected = "ABtC";
        var actual = navigator.ReadEscapedUntil ('[', ']');
        Assert.AreEqual (expected, actual);
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.ReadChar());
    }

    [TestMethod]
    [ExpectedException (typeof (FormatException))]
    [Description ("Считывание неправильно экранированной строки")]
    public void UnsafeTextNavigator_ReadEscapedUntil_3()
    {
        const string text = "AB[";
        var navigator = new UnsafeTextNavigator (text);
        navigator.ReadEscapedUntil ('[', ']');
    }

    [TestMethod]
    [Description ("Считывание, начиная с указанного символа")]
    public void UnsafeTextNavigator_ReadFrom_1()
    {
        const string text1 = "'ABC'DEF";
        var navigator = new UnsafeTextNavigator (text1);
        var actual = navigator.ReadFrom ('\'', '\'').ToString();
        Assert.AreEqual ("'ABC'", actual);

        const string text2 = "'ABCDEF";
        navigator = new UnsafeTextNavigator (text2);
        actual = navigator.ReadFrom ('\'', '\'').ToString();
        Assert.AreEqual (string.Empty, actual);

        const string text3 = "ABC'DEF";
        navigator = new UnsafeTextNavigator (text3);
        actual = navigator.ReadFrom ('\'', '\'').ToString();
        Assert.AreEqual (string.Empty, actual);

        navigator = new UnsafeTextNavigator (string.Empty);
        var actual2 = navigator.ReadFrom ('\'', '\'');
        Assert.IsTrue (actual2.IsEmpty);
    }

    [TestMethod]
    [Description ("Считывание, начиная с указанных символов")]
    public void UnsafeTextNavigator_ReadFrom_2()
    {
        const string text1 = "[ABC>DEF";
        char[] open = { '[', '<' }, close = { '>', '>' };
        var navigator = new UnsafeTextNavigator (text1);
        var actual = navigator.ReadFrom (open, close).ToString();
        Assert.AreEqual ("[ABC>", actual);

        const string text2 = "[ABCDEF";
        navigator = new UnsafeTextNavigator (text2);
        actual = navigator.ReadFrom (open, close).ToString();
        Assert.AreEqual (string.Empty, actual);

        const string text3 = "ABC[DEF";
        navigator = new UnsafeTextNavigator (text3);
        actual = navigator.ReadFrom (open, close).ToString();
        Assert.AreEqual (string.Empty, actual);

        navigator = new UnsafeTextNavigator (string.Empty);
        var actual2 = navigator.ReadFrom (open, close);
        Assert.IsTrue (actual2.IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение целого числа")]
    public void UnsafeTextNavigator_ReadInteger_1()
    {
        const string text1 = "314abc";
        var navigator = new UnsafeTextNavigator (text1);
        var actual = navigator.ReadInteger().ToString();
        Assert.AreEqual ("314", actual);

        actual = navigator.ReadInteger().ToString();
        Assert.AreEqual (string.Empty, actual);

        navigator = new UnsafeTextNavigator (string.Empty);
        var actual2 = navigator.ReadInteger();
        Assert.IsTrue (actual2.IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение вплоть до перевода строки")]
    public void UnsafeTextNavigator_ReadLine_1()
    {
        const string text = "ABC\r\nDEF";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ("ABC", navigator.ReadLine().ToString());
        Assert.AreEqual ("DEF", navigator.ReadLine().ToString());
        Assert.IsTrue (navigator.ReadLine().IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение строки")]
    public void UnsafeTextNavigator_ReadString_1()
    {
        const string text = "ABCDEF";
        var navigator = new UnsafeTextNavigator (text);
        Assert.AreEqual ("ABC", navigator.ReadString (3).ToString());
        Assert.AreEqual ("DEF", navigator.ReadString (4).ToString());
        Assert.IsTrue (navigator.ReadString (3).IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение вплоть до указанного символа")]
    public void UnsafeTextNavigator_ReadTo_1()
    {
        const string text1 = "'ABC'DEF";
        var navigator = new UnsafeTextNavigator (text1);
        var open = navigator.ReadChar();
        var actual = navigator.ReadTo (open).ToString();
        Assert.AreEqual ("ABC'", actual);

        const string text2 = "'ABC";
        navigator = new UnsafeTextNavigator (text2);
        open = navigator.ReadChar();
        actual = navigator.ReadTo (open).ToString();
        Assert.AreEqual ("ABC", actual);

        navigator = new UnsafeTextNavigator (string.Empty);
        Assert.IsTrue (navigator.ReadTo (open).IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение вплоть до указанных символов")]
    public void UnsafeTextNavigator_ReadTo_2()
    {
        char[] stop = { ']', '>' };
        var navigator = new UnsafeTextNavigator ("ABC]>DEF");
        Assert.AreEqual ("ABC]", navigator.ReadTo (stop).ToString());
        Assert.AreEqual (">", navigator.ReadTo (stop).ToString());
        Assert.AreEqual ("DEF", navigator.ReadTo (stop).ToString());
        Assert.IsTrue (navigator.ReadTo (stop).IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение вплоть до указанных символов")]
    public void UnsafeTextNavigator_ReadTo_3()
    {
        var navigator = new UnsafeTextNavigator ("314abc>>>hello");
        var actual = navigator.ReadToString (">>>").ToString();
        Assert.AreEqual ("314abc", actual);
        Assert.AreEqual ("hello", navigator.GetRemainingText().ToString());

        navigator = new UnsafeTextNavigator ("314abc>>hello");
        actual = navigator.ReadToString (">>>").ToString();
        Assert.IsTrue (string.IsNullOrEmpty (actual));
        Assert.AreEqual ("314abc>>hello", navigator.GetRemainingText().ToString());

        navigator = new UnsafeTextNavigator (string.Empty);
        Assert.IsTrue (navigator.ReadToString (">>>").IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение вплоть до указанного символа")]
    public void UnsafeTextNavigator_ReadUntil_1()
    {
        const string text = "'ABC'DEF";
        var navigator = new UnsafeTextNavigator (text);
        var open = navigator.ReadChar();
        var actual = navigator.ReadUntil (open).ToString();
        Assert.AreEqual ("ABC", actual);

        navigator = new UnsafeTextNavigator (string.Empty);
        Assert.IsTrue (navigator.ReadUntil (open).IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение вплоть до указанных символов")]
    public void UnsafeTextNavigator_ReadUntil_2()
    {
        char[] openChars = { '(' };
        char[] closeChars = { ')' };
        char[] stopChars = { ')' };
        char[] stopChars2 = { ']' };

        var navigator = new UnsafeTextNavigator ("12345)");
        var actual = navigator.ReadUntil (openChars, closeChars, stopChars).ToString();
        Assert.AreEqual ("12345", actual);

        navigator = new UnsafeTextNavigator ("12(3)(4)5)");
        actual = navigator.ReadUntil (openChars, closeChars, stopChars).ToString();
        Assert.AreEqual ("12(3)(4)5", actual);

        navigator = new UnsafeTextNavigator ("12(3(4))5)");
        actual = navigator.ReadUntil (openChars, closeChars, stopChars).ToString();
        Assert.AreEqual ("12(3(4))5", actual);

        navigator = new UnsafeTextNavigator ("12(3(4))5");
        actual = navigator.ReadUntil (openChars, closeChars, stopChars).ToString();
        Assert.IsTrue (string.IsNullOrEmpty (actual));

        navigator = new UnsafeTextNavigator ("12(3(4)5)");
        actual = navigator.ReadUntil (openChars, closeChars, stopChars).ToString();
        Assert.IsTrue (string.IsNullOrEmpty (actual));

        navigator = new UnsafeTextNavigator ("1234]5)");
        actual = navigator.ReadUntil (openChars, closeChars, stopChars2).ToString();
        Assert.AreEqual ("1234", actual);

        navigator = new UnsafeTextNavigator ("123(4])]5)");
        actual = navigator.ReadUntil (openChars, closeChars, stopChars2).ToString();
        Assert.AreEqual ("123(4])", actual);

        navigator = new UnsafeTextNavigator (string.Empty);
        Assert.IsTrue (navigator.ReadUntil (openChars, closeChars, stopChars).IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение вплоть до указанных символов")]
    public void UnsafeTextNavigator_ReadUntil_3()
    {
        char[] stop = { ']', '>' };
        var navigator = new UnsafeTextNavigator ("ABC>]DEF");
        Assert.AreEqual ("ABC", navigator.ReadUntil (stop).ToString());
        navigator.ReadChar();
        Assert.AreEqual (string.Empty, navigator.ReadUntil (stop).ToString());
        navigator.ReadChar();
        Assert.AreEqual ("DEF", navigator.ReadUntil (stop).ToString());
        navigator.ReadChar();
        Assert.IsTrue (navigator.ReadUntil (stop).IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение вплоть до указанной строки")]
    public void UnsafeTextNavigator_ReadUntilString_1()
    {
        var navigator = new UnsafeTextNavigator ("12345<.>");
        var actual = navigator.ReadUntilString ("<.>").ToString();
        Assert.AreEqual ("12345", actual);
        Assert.AreEqual ("<.>", navigator.PeekString (3).ToString());
        Assert.AreEqual ("<.>", navigator.GetRemainingText().ToString());

        navigator = new UnsafeTextNavigator ("12345");
        var actual2 = navigator.ReadUntilString ("<.>");
        Assert.IsTrue (actual2.IsEmpty);

        navigator = new UnsafeTextNavigator ("12345<");
        actual2 = navigator.ReadUntilString ("<.>");
        Assert.IsTrue (actual2.IsEmpty);

        navigator = new UnsafeTextNavigator ("12345<.");
        actual2 = navigator.ReadUntilString ("<.>");
        Assert.IsTrue (actual2.IsEmpty);

        navigator = new UnsafeTextNavigator ("12345<.6>");
        actual2 = navigator.ReadUntilString ("<.>");
        Assert.IsTrue (actual2.IsEmpty);

        navigator = new UnsafeTextNavigator ("12345<.>67890");
        actual = navigator.ReadUntilString ("<.>").ToString();
        Assert.AreEqual ("12345", actual);
        Assert.AreEqual ("<.>", navigator.PeekString (3).ToString());
        Assert.AreEqual ("<.>67890", navigator.GetRemainingText().ToString());

        navigator = new UnsafeTextNavigator (string.Empty);
        Assert.IsTrue (navigator.ReadUntilString ("<.>").IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение указанных символов")]
    public void UnsafeTextNavigator_ReadWhile_1()
    {
        var navigator = new UnsafeTextNavigator ("111234");
        Assert.AreEqual ("111", navigator.ReadWhile ('1').ToString());
        Assert.AreEqual (string.Empty, navigator.ReadWhile ('1').ToString());
        navigator.Move (3);
        Assert.IsTrue (navigator.ReadWhile ('1').IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение указанных символов")]
    public void UnsafeTextNavigator_ReadWhile_2()
    {
        const string text1 = "314abc";
        char[] good = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var navigator = new UnsafeTextNavigator (text1);
        Assert.AreEqual ("314", navigator.ReadWhile (good).ToString());
        Assert.AreEqual (string.Empty, navigator.ReadWhile (good).ToString());
        navigator.Move (3);
        Assert.IsTrue (navigator.ReadWhile (good).IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение слова")]
    public void UnsafeTextNavigator_ReadWord_1()
    {
        var navigator = new UnsafeTextNavigator ("Hello, world!");
        Assert.AreEqual ("Hello", navigator.ReadWord().ToString());
        Assert.AreEqual (string.Empty, navigator.ReadWord().ToString());
        navigator.Move (2);
        Assert.AreEqual ("world", navigator.ReadWord().ToString());
        Assert.AreEqual (string.Empty, navigator.ReadWord().ToString());
        navigator.Move (2);
        Assert.IsTrue (navigator.ReadWord().IsEmpty);
    }

    [TestMethod]
    [Description ("Чтение слова")]
    public void UnsafeTextNavigator_ReadWord_2()
    {
        char[] additional = { '<', '>' };
        var navigator = new UnsafeTextNavigator ("<Hello>, world!");
        Assert.AreEqual ("<Hello>", navigator.ReadWord (additional).ToString());
        Assert.AreEqual (string.Empty, navigator.ReadWord (additional).ToString());
        navigator.Move (2);
        Assert.AreEqual ("world", navigator.ReadWord (additional).ToString());
        Assert.AreEqual (string.Empty, navigator.ReadWord (additional).ToString());
        navigator.Move (2);
        Assert.IsTrue (navigator.ReadWord (additional).IsEmpty);
    }

    [TestMethod]
    [Description ("Получение недавно считанного текста")]
    public void UnsafeTextNavigator_RecentText_1()
    {
        var navigator = new UnsafeTextNavigator ("Hello, world!");
        Assert.AreEqual (string.Empty, navigator.RecentText (4).ToString());
        navigator.Move (4);
        Assert.AreEqual (string.Empty, navigator.RecentText (-1).ToString());
        Assert.AreEqual ("Hell", navigator.RecentText (4).ToString());
        navigator.Move (9);
        Assert.AreEqual ("rld!", navigator.RecentText (4).ToString());
        Assert.AreEqual ("Hello, world!", navigator.RecentText (20).ToString());
        navigator.Move (9);
        Assert.AreEqual ("!", navigator.RecentText (1).ToString());
    }

    [TestMethod]
    [Description ("Пропуск указаного символа")]
    public void UnsafeTextNavigator_SkipChar_1()
    {
        var navigator = new UnsafeTextNavigator ("111234");
        Assert.IsTrue (navigator.SkipChar ('1'));
        Assert.IsTrue (navigator.SkipChar ('1'));
        Assert.IsTrue (navigator.SkipChar ('1'));
        Assert.AreEqual ('2', navigator.ReadChar());
        Assert.IsFalse (navigator.SkipChar ('1'));
    }

    [TestMethod]
    [Description ("Пропуск указанного количества символов")]
    public void UnsafeTextNavigator_SkipChar_2()
    {
        var navigator = new UnsafeTextNavigator ("123456");
        Assert.IsTrue (navigator.SkipChar (3));
        Assert.AreEqual ('4', navigator.ReadChar());
        Assert.IsFalse (navigator.SkipChar (3));
        Assert.IsTrue (navigator.IsEOF);
    }

    [TestMethod]
    [Description ("Пропуск указанных символов")]
    public void UnsafeTextNavigator_SkipChar_3()
    {
        char[] allowed = { '1', '2' };
        var navigator = new UnsafeTextNavigator ("123456");
        Assert.IsTrue (navigator.SkipChar (allowed));
        Assert.IsTrue (navigator.SkipChar (allowed));
        Assert.IsFalse (navigator.SkipChar (allowed));
        Assert.AreEqual ('3', navigator.ReadChar());
    }

    [TestMethod]
    [Description ("Пропуск управляющих символов")]
    public void UnsafeTextNavigator_SkipControl_1()
    {
        var navigator = new UnsafeTextNavigator ("\t\tABC");
        Assert.IsTrue (navigator.SkipControl());
        Assert.AreEqual ('A', navigator.ReadChar());
        navigator.Move (2);
        Assert.IsFalse (navigator.SkipControl());
    }

    [TestMethod]
    [Description ("Пропуск пунктуации")]
    public void UnsafeTextNavigator_SkipPunctuation_1()
    {
        var navigator = new UnsafeTextNavigator (".,ABC");
        Assert.IsTrue (navigator.SkipPunctuation());
        Assert.AreEqual ('A', navigator.ReadChar());
        navigator.Move (2);
        Assert.IsFalse (navigator.SkipPunctuation());
    }

    [TestMethod]
    [Description ("Пропуск не-словных символов")]
    public void UnsafeTextNavigator_SkipNonWord_1()
    {
        var navigator = new UnsafeTextNavigator (". (ABC");
        Assert.IsTrue (navigator.SkipNonWord());
        Assert.AreEqual ('A', navigator.ReadChar());
    }

    [TestMethod]
    [Description ("Пропуск не-словных символов")]
    public void UnsafeTextNavigator_SkipNonWord_2()
    {
        var navigator = new UnsafeTextNavigator (". (<ABC");
        Assert.IsTrue (navigator.SkipNonWord ('<', '>'));
        Assert.AreEqual ('<', navigator.ReadChar());
    }

    [TestMethod]
    [Description ("Пропукск указанного диапазона символов")]
    public void UnsafeTextNavigator_SkipRange_1()
    {
        var navigator = new UnsafeTextNavigator ("123ABC");
        Assert.IsTrue (navigator.SkipRange ('0', '9'));
        Assert.AreEqual ('A', navigator.ReadChar());
        navigator.Move (2);
        Assert.IsFalse (navigator.SkipRange ('0', '9'));
    }

    [TestMethod]
    [Description ("Пропуск вплоть до указанного символа")]
    public void UnsafeTextNavigator_SkipTo_1()
    {
        var navigator = new UnsafeTextNavigator ("123ABC");
        Assert.IsTrue (navigator.SkipTo ('A'));
        Assert.AreEqual ('A', navigator.ReadChar());
        Assert.IsFalse (navigator.SkipTo ('A'));
    }

    [TestMethod]
    [Description ("Пропуск символов, не входящих в заданное множество")]
    public void UnsafeTextNavigator_SkipWhileNot_1()
    {
        char[] good = { 'A', 'B' };
        var navigator = new UnsafeTextNavigator ("123ABC");
        Assert.IsTrue (navigator.SkipWhileNot (good));
        Assert.AreEqual ('A', navigator.ReadChar());
        Assert.IsTrue (navigator.SkipWhileNot (good));
        Assert.AreEqual ('B', navigator.ReadChar());
        Assert.IsFalse (navigator.SkipWhileNot (good));
        Assert.AreEqual (UnsafeTextNavigator.EOF, navigator.ReadChar());
    }

    [TestMethod]
    [Description ("Пропуск указанного символа")]
    public void UnsafeTextNavigator_SkipWhile_1()
    {
        var navigator = new UnsafeTextNavigator ("111ABC");
        Assert.IsTrue (navigator.SkipWhile ('1'));
        Assert.AreEqual ('A', navigator.ReadChar());
        navigator.Move (2);
        Assert.IsFalse (navigator.SkipWhile ('1'));
    }

    [TestMethod]
    [Description ("Пропуск указанных символов")]
    public void UnsafeTextNavigator_SkipWhile_2()
    {
        char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        var navigator = new UnsafeTextNavigator ("314ABC");
        Assert.IsTrue (navigator.SkipWhile (digits));
        Assert.AreEqual ('A', navigator.ReadChar());
        navigator.Move (2);
        Assert.IsFalse (navigator.SkipWhile (digits));
    }

    [TestMethod]
    [Description ("Пропуск пробельный символов")]
    public void UnsafeTextNavigator_SkipWhitespace_1()
    {
        var navigator = new UnsafeTextNavigator (" \t\r\nABC ");
        Assert.IsTrue (navigator.SkipWhitespace());
        Assert.AreEqual ('A', navigator.ReadChar());
        navigator.ReadChar();
        navigator.ReadChar();
        Assert.IsFalse (navigator.SkipWhitespace());
        Assert.IsTrue (navigator.IsEOF);
    }

    [TestMethod]
    [Description ("Пропуск пробелов и пунктуации")]
    public void UnsafeTextNavigator_SkipWhitespaceAndPunctuation_1()
    {
        var navigator = new UnsafeTextNavigator (" \t,\r\nABC. ");
        Assert.IsTrue (navigator.SkipWhitespaceAndPunctuation());
        Assert.AreEqual ('A', navigator.ReadChar());
        navigator.ReadChar();
        navigator.ReadChar();
        Assert.IsFalse (navigator.SkipWhitespaceAndPunctuation());
        Assert.IsTrue (navigator.IsEOF);
    }

    [TestMethod]
    [Description ("Выделение подстроки без явно заданной длины")]
    public void UnsafeTextNavigator_Substring_1()
    {
        var navigator = new UnsafeTextNavigator ("Hello, world!");
        Assert.AreEqual ("world", navigator.Substring (7, 5).ToString());
    }

    [TestMethod]
    [Description ("Выделение подстроки с явно заданной длиной")]
    public void UnsafeTextNavigator_Substring_2()
    {
        var navigator = new UnsafeTextNavigator ("Hello, world!");
        Assert.AreEqual ("world!", navigator.Substring (7).ToString());
    }

    [TestMethod]
    [Description ("Получение текстового представления")]
    public void UnsafeTextNavigator_ToString_1()
    {
        var navigator = new UnsafeTextNavigator ("Hello, world!");
        Assert.AreEqual ("Position=0", navigator.ToString());
    }
}
