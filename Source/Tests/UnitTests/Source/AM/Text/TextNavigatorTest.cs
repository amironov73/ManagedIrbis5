using System;
using System.IO;
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#nullable enable

// ReSharper disable CheckNamespace
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo

namespace UnitTests.AM.Text
{
    [TestClass]
    public class TextNavigatorTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TextNavigator_Construction_1()
        {
            new TextNavigator("\uD801", true);
        }

        [TestMethod]
        public void TextNavigator_Column_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual(1, navigator.Column);
            navigator.ReadChar();
            Assert.AreEqual(2, navigator.Column);
        }

        [TestMethod]
        public void TextNavigator_IsEOF_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.IsFalse(navigator.IsEOF);
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsEOF);
            navigator.SkipChar(2);
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void TextNavigator_Length_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual(text.Length, navigator.Length);
        }

        [TestMethod]
        public void TextNavigator_Line_1()
        {
            const string text = "ABC\nDEF";
            var navigator = new TextNavigator(text);
            Assert.AreEqual(1, navigator.Line);
            navigator.ReadChar();
            Assert.AreEqual(1, navigator.Line);
            navigator.SkipChar(3);
            Assert.AreEqual(2, navigator.Line);
        }

        [TestMethod]
        public void TextNavigator_Position_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual(0, navigator.Position);
            navigator.ReadChar();
            Assert.AreEqual(1, navigator.Position);
            navigator.ReadChar();
            Assert.AreEqual(2, navigator.Position);
            navigator.ReadChar();
            Assert.AreEqual(3, navigator.Position);
            navigator.ReadChar();
            Assert.AreEqual(3, navigator.Position);
        }

        [TestMethod]
        public void TextNavigator_Text_1()
        {
            var text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreSame(text, navigator.Text);
        }

        [TestMethod]
        public void TextNavigator_Clone_1()
        {
            const string text = "ABC";
            var first = new TextNavigator(text);
            first.ReadChar();
            var second = first.Clone();
            Assert.AreSame(first.Text, second.Text);
            Assert.AreEqual(first.Column, second.Column);
            Assert.AreEqual(first.Line, second.Line);
            Assert.AreEqual(first.Position, second.Position);
        }

        // [TestMethod]
        // public void TextNavigator_FromFile_1()
        // {
        //     string fileName = Path.Combine(TestDataPath, "record.txt");
        //     TextNavigator navigator = TextNavigator.FromFile(fileName);
        //     Assert.AreEqual('#', navigator.ReadChar());
        // }

        // [TestMethod]
        // public void TextNavigator_FromFile_2()
        // {
        //     string fileName = Path.Combine(TestDataPath, "record.txt");
        //     TextNavigator navigator = TextNavigator.FromFile(fileName, Encoding.UTF8);
        //     Assert.AreEqual('#', navigator.ReadChar());
        // }

        [TestMethod]
        public void TextNavigator_GetRemainingText_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual(text, navigator.GetRemainingText().ToString());
            navigator.ReadChar();
            Assert.AreEqual("BC", navigator.GetRemainingText().ToString());
            navigator.ReadChar();
            Assert.AreEqual("C", navigator.GetRemainingText().ToString());
            navigator.ReadChar();
            Assert.IsTrue(navigator.GetRemainingText().IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_IsControl_1()
        {
            const string text = "A\tBC";
            var navigator = new TextNavigator(text);
            Assert.IsFalse(navigator.IsControl());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsControl());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsControl());
        }

        [TestMethod]
        public void TextNavigator_IsDigit_1()
        {
            const string text = "A1BC";
            var navigator = new TextNavigator(text);
            Assert.IsFalse(navigator.IsDigit());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsDigit());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsDigit());
        }

        [TestMethod]
        public void TextNavigator_IsLetter_1()
        {
            const string text = "A1BC";
            var navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsLetter());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsLetter());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsLetter());
        }

        [TestMethod]
        public void TextNavigator_IsLetterOrDigit_1()
        {
            const string text = "A_1";
            var navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsLetterOrDigit());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsLetterOrDigit());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsLetterOrDigit());
        }

        [TestMethod]
        public void TextNavigator_IsNumber_1()
        {
            const string text = "1+²";
            var navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsNumber());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsNumber());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsNumber());
        }

        [TestMethod]
        public void TextNavigator_IsPunctuation_1()
        {
            const string text = ".A,";
            var navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsPunctuation());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsPunctuation());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsPunctuation());
        }

        [TestMethod]
        public void TextNavigator_IsSeparator_1()
        {
            const string text = "\u2028A ";
            var navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsSeparator());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsSeparator());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsSeparator());
        }

        [TestMethod]
        public void TextNavigator_IsSymbol_1()
        {
            const string text = "$A+";
            var navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsSymbol());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsSymbol());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsSymbol());
        }

        [TestMethod]
        public void TextNavigator_IsWhiteSpace_1()
        {
            const string text = " A\t";
            var navigator = new TextNavigator(text);
            Assert.IsTrue(navigator.IsWhiteSpace());
            navigator.ReadChar();
            Assert.IsFalse(navigator.IsWhiteSpace());
            navigator.ReadChar();
            Assert.IsTrue(navigator.IsWhiteSpace());
        }

        [TestMethod]
        public void TextNavigator_LookAhead_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual('B', navigator.LookAhead());
            navigator.ReadChar();
            Assert.AreEqual('C', navigator.LookAhead());
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead());
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead());
        }

        [TestMethod]
        public void TextNavigator_LookAhead_2()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual('C', navigator.LookAhead(2));
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead(2));
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead(2));
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookAhead(2));
        }

        [TestMethod]
        public void TextNavigator_LookBehind_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual(TextNavigator.EOF, navigator.LookBehind());
            navigator.ReadChar();
            Assert.AreEqual('A', navigator.LookBehind());
            navigator.ReadChar();
            Assert.AreEqual('B', navigator.LookBehind());
            navigator.ReadChar();
            Assert.AreEqual('C', navigator.LookBehind());
            navigator.ReadChar();
            Assert.AreEqual('C', navigator.LookBehind());
            navigator.ReadChar();
        }

        [TestMethod]
        public void TextNavigator_LookBehind_2()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual(TextNavigator.EOF, navigator.LookBehind(2));
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.LookBehind(2));
            navigator.ReadChar();
            Assert.AreEqual('A', navigator.LookBehind(2));
            navigator.ReadChar();
            Assert.AreEqual('B', navigator.LookBehind(2));
            navigator.ReadChar();
            Assert.AreEqual('B', navigator.LookBehind(2));
            navigator.ReadChar();
        }

        [TestMethod]
        public void TextNavigator_Move_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreSame(navigator, navigator.Move(2));
            Assert.AreEqual(2, navigator.Position);
            Assert.AreSame(navigator, navigator.Move(-2));
            Assert.AreEqual(0, navigator.Position);
        }

        [TestMethod]
        public void TextNavigator_PeekChar_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual('A', navigator.PeekChar());
            navigator.ReadChar();
            Assert.AreEqual('B', navigator.PeekChar());
            navigator.ReadChar();
            Assert.AreEqual('C', navigator.PeekChar());
            navigator.ReadChar();
            Assert.AreEqual(TextNavigator.EOF, navigator.PeekChar());
        }

        [TestMethod]
        public void TextNavigator_PeekString_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual("AB", navigator.PeekString(2).ToString());
            navigator.ReadChar();
            Assert.AreEqual("BC", navigator.PeekString(2).ToString());
            navigator.ReadChar();
            Assert.AreEqual("C", navigator.PeekString(2).ToString());
            navigator.ReadChar();
            Assert.IsTrue(navigator.PeekString(2).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_PeekTo_1()
        {
            const string text = "ABC]DEF";
            var navigator = new TextNavigator(text);
            Assert.AreEqual("ABC]", navigator.PeekTo(']').ToString());
            navigator.ReadChar();
            Assert.AreEqual("BC]", navigator.PeekTo(']').ToString());
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.AreEqual("]", navigator.PeekTo(']').ToString());
            navigator.ReadChar();
            Assert.AreEqual("DEF", navigator.PeekTo(']').ToString());
            navigator.Move(3);
            Assert.IsTrue(navigator.PeekTo(']').IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_PeekTo_2()
        {
            const string text = "ABC]DE+F";
            char[] stop = { ']', '+' };
            var navigator = new TextNavigator(text);
            Assert.AreEqual("ABC]", navigator.PeekTo(stop).ToString());
            navigator.ReadChar();
            Assert.AreEqual("BC]", navigator.PeekTo(stop).ToString());
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.AreEqual("]", navigator.PeekTo(stop).ToString());
            navigator.ReadChar();
            Assert.AreEqual("DE+", navigator.PeekTo(stop).ToString());
            navigator.Move(3);
            Assert.AreEqual("F", navigator.PeekTo(stop).ToString());
            navigator.ReadChar();
            Assert.IsTrue(navigator.PeekTo(stop).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_PeekUntil_1()
        {
            const string text = "ABC]DEF";
            var navigator = new TextNavigator(text);
            Assert.AreEqual("ABC", navigator.PeekUntil(']').ToString());
            navigator.ReadChar();
            Assert.AreEqual("BC", navigator.PeekUntil(']').ToString());
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.AreEqual(string.Empty, navigator.PeekUntil(']').ToString());
            navigator.ReadChar();
            Assert.AreEqual("DEF", navigator.PeekUntil(']').ToString());
            navigator.Move(3);
            Assert.IsTrue(navigator.PeekUntil(']').IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_PeekUntil_2()
        {
            const string text = "ABC]DE+F";
            char[] stop = { ']', '+' };
            var navigator = new TextNavigator(text);
            Assert.AreEqual("ABC", navigator.PeekUntil(stop).ToString());
            navigator.ReadChar();
            Assert.AreEqual("BC", navigator.PeekUntil(stop).ToString());
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.AreEqual(string.Empty, navigator.PeekUntil(stop).ToString());
            navigator.ReadChar();
            Assert.AreEqual("DE", navigator.PeekUntil(stop).ToString());
            navigator.Move(3);
            Assert.AreEqual("F", navigator.PeekUntil(stop).ToString());
            navigator.ReadChar();
            Assert.IsTrue(navigator.PeekUntil(stop).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadChar_1()
        {
            const string text = "ABC";
            var navigator = new TextNavigator(text);
            Assert.AreEqual('A', navigator.ReadChar());
            Assert.AreEqual('B', navigator.ReadChar());
            Assert.AreEqual('C', navigator.ReadChar());
            Assert.AreEqual(TextNavigator.EOF, navigator.ReadChar());
            Assert.AreEqual(TextNavigator.EOF, navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_ReadEscapedUntil_1()
        {
            const string text = "AB[tC]D";
            var navigator = new TextNavigator(text);
            var expected = "ABtC";
            var actual = navigator.ReadEscapedUntil('[', ']');
            Assert.AreEqual(expected, actual);
            Assert.AreEqual('D', navigator.ReadChar());
            Assert.IsNull(navigator.ReadEscapedUntil('[', ']'));
        }

        [TestMethod]
        public void TextNavigator_ReadEscapedUntil_2()
        {
            const string text = "AB[tC";
            var navigator = new TextNavigator(text);
            var expected = "ABtC";
            var actual = navigator.ReadEscapedUntil('[', ']');
            Assert.AreEqual(expected, actual);
            Assert.AreEqual(TextNavigator.EOF, navigator.ReadChar());
        }

        [TestMethod]
        [ExpectedException(typeof(FormatException))]
        public void TextNavigator_ReadEscapedUntil_3()
        {
            const string text = "AB[";
            var navigator = new TextNavigator(text);
            navigator.ReadEscapedUntil('[', ']');
        }

        [TestMethod]
        public void TextNavigator_ReadFrom_1()
        {
            const string text1 = "'ABC'DEF";
            var navigator = new TextNavigator(text1);
            var actual = navigator.ReadFrom('\'', '\'').ToString();
            Assert.AreEqual("'ABC'", actual);

            const string text2 = "'ABCDEF";
            navigator = new TextNavigator(text2);
            actual = navigator.ReadFrom('\'', '\'').ToString();
            Assert.AreEqual(string.Empty, actual);

            const string text3 = "ABC'DEF";
            navigator = new TextNavigator(text3);
            actual = navigator.ReadFrom('\'', '\'').ToString();
            Assert.AreEqual(string.Empty, actual);

            navigator = new TextNavigator(string.Empty);
            var actual2 = navigator.ReadFrom('\'', '\'');
            Assert.IsTrue(actual2.IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadFrom_2()
        {
            const string text1 = "[ABC>DEF";
            char[] open = { '[', '<' }, close = { '>', '>' };
            var navigator = new TextNavigator(text1);
            var actual = navigator.ReadFrom(open, close).ToString();
            Assert.AreEqual("[ABC>", actual);

            const string text2 = "[ABCDEF";
            navigator = new TextNavigator(text2);
            actual = navigator.ReadFrom(open, close).ToString();
            Assert.AreEqual(string.Empty, actual);

            const string text3 = "ABC[DEF";
            navigator = new TextNavigator(text3);
            actual = navigator.ReadFrom(open, close).ToString();
            Assert.AreEqual(string.Empty, actual);

            navigator = new TextNavigator(string.Empty);
            var actual2 = navigator.ReadFrom(open, close);
            Assert.IsTrue(actual2.IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadInteger_1()
        {
            const string text1 = "314abc";
            var navigator = new TextNavigator(text1);
            var actual = navigator.ReadInteger().ToString();
            Assert.AreEqual("314", actual);

            actual = navigator.ReadInteger().ToString();
            Assert.AreEqual(string.Empty, actual);

            navigator = new TextNavigator(string.Empty);
            var actual2 = navigator.ReadInteger();
            Assert.IsTrue(actual2.IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadLine_1()
        {
            const string text = "ABC\r\nDEF";
            var navigator = new TextNavigator(text);
            Assert.AreEqual("ABC", navigator.ReadLine().ToString());
            Assert.AreEqual("DEF", navigator.ReadLine().ToString());
            Assert.IsTrue(navigator.ReadLine().IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadString_1()
        {
            const string text = "ABCDEF";
            var navigator = new TextNavigator(text);
            Assert.AreEqual("ABC", navigator.ReadString(3).ToString());
            Assert.AreEqual("DEF", navigator.ReadString(4).ToString());
            Assert.IsTrue(navigator.ReadString(3).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadTo_1()
        {
            const string text1 = "'ABC'DEF";
            var navigator = new TextNavigator(text1);
            var open = navigator.ReadChar();
            var actual = navigator.ReadTo(open).ToString();
            Assert.AreEqual("ABC'", actual);

            const string text2 = "'ABC";
            navigator = new TextNavigator(text2);
            open = navigator.ReadChar();
            actual = navigator.ReadTo(open).ToString();
            Assert.AreEqual("ABC", actual);

            navigator = new TextNavigator(string.Empty);
            Assert.IsTrue(navigator.ReadTo(open).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadTo_2()
        {
            char[] stop = { ']', '>' };
            var navigator = new TextNavigator("ABC]>DEF");
            Assert.AreEqual("ABC]", navigator.ReadTo(stop).ToString());
            Assert.AreEqual(">", navigator.ReadTo(stop).ToString());
            Assert.AreEqual("DEF", navigator.ReadTo(stop).ToString());
            Assert.IsTrue(navigator.ReadTo(stop).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadTo_3()
        {
            var navigator = new TextNavigator("314abc>>>hello");
            var actual = navigator.ReadTo(">>>").ToString();
            Assert.AreEqual("314abc", actual);
            Assert.AreEqual("hello", navigator.GetRemainingText().ToString());

            navigator = new TextNavigator("314abc>>hello");
            actual = navigator.ReadTo(">>>").ToString();
            Assert.IsTrue(string.IsNullOrEmpty(actual));
            Assert.AreEqual("314abc>>hello", navigator.GetRemainingText().ToString());

            navigator = new TextNavigator(string.Empty);
            Assert.IsTrue(navigator.ReadTo(">>>").IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadUntil_1()
        {
            const string text = "'ABC'DEF";
            var navigator = new TextNavigator(text);
            var open = navigator.ReadChar();
            var actual = navigator.ReadUntil(open).ToString();
            Assert.AreEqual("ABC", actual);

            navigator = new TextNavigator(string.Empty);
            Assert.IsTrue(navigator.ReadUntil(open).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadUntil_2()
        {
            char[] openChars = { '(' };
            char[] closeChars = { ')' };
            char[] stopChars = { ')' };
            char[] stopChars2 = { ']' };

            var navigator = new TextNavigator("12345)");
            var actual = navigator.ReadUntil(openChars, closeChars, stopChars).ToString();
            Assert.AreEqual("12345", actual);

            navigator = new TextNavigator("12(3)(4)5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars).ToString();
            Assert.AreEqual("12(3)(4)5", actual);

            navigator = new TextNavigator("12(3(4))5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars).ToString();
            Assert.AreEqual("12(3(4))5", actual);

            navigator = new TextNavigator("12(3(4))5");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars).ToString();
            Assert.IsTrue(string.IsNullOrEmpty(actual));

            navigator = new TextNavigator("12(3(4)5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars).ToString();
            Assert.IsTrue(string.IsNullOrEmpty(actual));

            navigator = new TextNavigator("1234]5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars2).ToString();
            Assert.AreEqual("1234", actual);

            navigator = new TextNavigator("123(4])]5)");
            actual = navigator.ReadUntil(openChars, closeChars, stopChars2).ToString();
            Assert.AreEqual("123(4])", actual);

            navigator = new TextNavigator(string.Empty);
            Assert.IsTrue(navigator.ReadUntil(openChars, closeChars, stopChars).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadUntil_3()
        {
            var navigator = new TextNavigator("12345<.>");
            var actual = navigator.ReadUntil("<.>").ToString();
            Assert.AreEqual("12345", actual);
            Assert.AreEqual("<.>", navigator.PeekString(3).ToString());
            Assert.AreEqual("<.>", navigator.GetRemainingText().ToString());

            navigator = new TextNavigator("12345");
            var actual2 = navigator.ReadUntil("<.>");
            Assert.IsTrue(actual2.IsEmpty);

            navigator = new TextNavigator("12345<");
            actual2 = navigator.ReadUntil("<.>");
            Assert.IsTrue(actual2.IsEmpty);

            navigator = new TextNavigator("12345<.");
            actual2 = navigator.ReadUntil("<.>");
            Assert.IsTrue(actual2.IsEmpty);

            navigator = new TextNavigator("12345<.6>");
            actual2 = navigator.ReadUntil("<.>");
            Assert.IsTrue(actual2.IsEmpty);

            navigator = new TextNavigator("12345<.>67890");
            actual = navigator.ReadUntil("<.>").ToString();
            Assert.AreEqual("12345", actual);
            Assert.AreEqual("<.>", navigator.PeekString(3).ToString());
            Assert.AreEqual("<.>67890", navigator.GetRemainingText().ToString());

            navigator = new TextNavigator(string.Empty);
            Assert.IsTrue(navigator.ReadUntil("<.>").IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadUntil_4()
        {
            char[] stop = { ']', '>' };
            var navigator = new TextNavigator("ABC>]DEF");
            Assert.AreEqual("ABC", navigator.ReadUntil(stop).ToString());
            navigator.ReadChar();
            Assert.AreEqual(string.Empty, navigator.ReadUntil(stop).ToString());
            navigator.ReadChar();
            Assert.AreEqual("DEF", navigator.ReadUntil(stop).ToString());
            navigator.ReadChar();
            Assert.IsTrue(navigator.ReadUntil(stop).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadWhile_1()
        {
            var navigator = new TextNavigator("111234");
            Assert.AreEqual("111", navigator.ReadWhile('1').ToString());
            Assert.AreEqual(string.Empty, navigator.ReadWhile('1').ToString());
            navigator.Move(3);
            Assert.IsTrue(navigator.ReadWhile('1').IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadWhile_2()
        {
            const string text1 = "314abc";
            char[] good = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var navigator = new TextNavigator(text1);
            Assert.AreEqual("314", navigator.ReadWhile(good).ToString());
            Assert.AreEqual(string.Empty, navigator.ReadWhile(good).ToString());
            navigator.Move(3);
            Assert.IsTrue(navigator.ReadWhile(good).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadWord_1()
        {
            var navigator = new TextNavigator("Hello, world!");
            Assert.AreEqual("Hello", navigator.ReadWord().ToString());
            Assert.AreEqual(string.Empty, navigator.ReadWord().ToString());
            navigator.Move(2);
            Assert.AreEqual("world", navigator.ReadWord().ToString());
            Assert.AreEqual(string.Empty, navigator.ReadWord().ToString());
            navigator.Move(2);
            Assert.IsTrue(navigator.ReadWord().IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_ReadWord_2()
        {
            char[] additional = { '<', '>' };
            var navigator = new TextNavigator("<Hello>, world!");
            Assert.AreEqual("<Hello>", navigator.ReadWord(additional).ToString());
            Assert.AreEqual(string.Empty, navigator.ReadWord(additional).ToString());
            navigator.Move(2);
            Assert.AreEqual("world", navigator.ReadWord(additional).ToString());
            Assert.AreEqual(string.Empty, navigator.ReadWord(additional).ToString());
            navigator.Move(2);
            Assert.IsTrue(navigator.ReadWord(additional).IsEmpty);
        }

        [TestMethod]
        public void TextNavigator_RecentText_1()
        {
            var navigator = new TextNavigator("Hello, world!");
            Assert.AreEqual(string.Empty, navigator.RecentText(4).ToString());
            navigator.Move(4);
            Assert.AreEqual(string.Empty, navigator.RecentText(-1).ToString());
            Assert.AreEqual("Hell", navigator.RecentText(4).ToString());
            navigator.Move(9);
            Assert.AreEqual("rld!", navigator.RecentText(4).ToString());
            Assert.AreEqual("Hello, world!", navigator.RecentText(20).ToString());
            navigator.Move(9);
            Assert.AreEqual(string.Empty, navigator.RecentText(1).ToString());
        }

        // [TestMethod]
        // public void TextNavigator_RestorePosition_1()
        // {
        //     TextNavigator navigator = new TextNavigator("Hello, world!");
        //     TextPosition saved = navigator.SavePosition();
        //     navigator.ReadChar();
        //     navigator.RestorePosition(saved);
        //     Assert.AreEqual(saved.Position, navigator.Position);
        //     Assert.AreEqual(saved.Line, navigator.Line);
        //     Assert.AreEqual(saved.Column, navigator.Column);
        //     Assert.AreEqual('H', navigator.ReadChar());
        // }

        // [TestMethod]
        // public void TextNavigator_SavePosition_1()
        // {
        //     TextNavigator navigator = new TextNavigator("Hello, world!");
        //     TextPosition saved = navigator.SavePosition();
        //     Assert.AreEqual(navigator.Position, saved.Position);
        //     Assert.AreEqual(navigator.Line, saved.Line);
        //     Assert.AreEqual(navigator.Column, saved.Column);
        // }

        [TestMethod]
        public void TextNavigator_SkipChar_1()
        {
            var navigator = new TextNavigator("111234");
            Assert.IsTrue(navigator.SkipChar('1'));
            Assert.IsTrue(navigator.SkipChar('1'));
            Assert.IsTrue(navigator.SkipChar('1'));
            Assert.AreEqual('2', navigator.ReadChar());
            Assert.IsFalse(navigator.SkipChar('1'));
        }

        [TestMethod]
        public void TextNavigator_SkipChar_2()
        {
            var navigator = new TextNavigator("123456");
            Assert.IsTrue(navigator.SkipChar(3));
            Assert.AreEqual('4', navigator.ReadChar());
            Assert.IsFalse(navigator.SkipChar(3));
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void TextNavigator_SkipChar_3()
        {
            char[] allowed = { '1', '2' };
            var navigator = new TextNavigator("123456");
            Assert.IsTrue(navigator.SkipChar(allowed));
            Assert.IsTrue(navigator.SkipChar(allowed));
            Assert.IsFalse(navigator.SkipChar(allowed));
            Assert.AreEqual('3', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SkipControl_1()
        {
            var navigator = new TextNavigator("\t\tABC");
            Assert.IsTrue(navigator.SkipControl());
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipControl());
        }

        [TestMethod]
        public void TextNavigator_SkipPunctuation_1()
        {
            var navigator = new TextNavigator(".,ABC");
            Assert.IsTrue(navigator.SkipPunctuation());
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipPunctuation());
        }

        [TestMethod]
        public void TextNavigator_SkipNonWord_1()
        {
            var navigator = new TextNavigator(". (ABC");
            Assert.IsTrue(navigator.SkipNonWord());
            Assert.AreEqual('A', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SkipNonWord_2()
        {
            var navigator = new TextNavigator(". (<ABC");
            Assert.IsTrue(navigator.SkipNonWord('<', '>'));
            Assert.AreEqual('<', navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SkipRange_1()
        {
            var navigator = new TextNavigator("123ABC");
            Assert.IsTrue(navigator.SkipRange('0', '9'));
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipRange('0', '9'));
        }

        [TestMethod]
        public void TextNavigator_SkipTo_1()
        {
            var navigator = new TextNavigator("123ABC");
            Assert.IsTrue(navigator.SkipTo('A'));
            Assert.AreEqual('A', navigator.ReadChar());
            Assert.IsFalse(navigator.SkipTo('A'));
        }

        [TestMethod]
        public void TextNavigator_SkipWhileNot_1()
        {
            char[] good = { 'A', 'B' };
            var navigator = new TextNavigator("123ABC");
            Assert.IsTrue(navigator.SkipWhileNot(good));
            Assert.AreEqual('A', navigator.ReadChar());
            Assert.IsTrue(navigator.SkipWhileNot(good));
            Assert.AreEqual('B', navigator.ReadChar());
            Assert.IsFalse(navigator.SkipWhileNot(good));
            Assert.AreEqual(TextNavigator.EOF, navigator.ReadChar());
        }

        [TestMethod]
        public void TextNavigator_SkipWhile_1()
        {
            var navigator = new TextNavigator("111ABC");
            Assert.IsTrue(navigator.SkipWhile('1'));
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipWhile('1'));
        }

        [TestMethod]
        public void TextNavigator_SkipWhile_2()
        {
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            var navigator = new TextNavigator("314ABC");
            Assert.IsTrue(navigator.SkipWhile(digits));
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.Move(2);
            Assert.IsFalse(navigator.SkipWhile(digits));
        }

        [TestMethod]
        public void TextNavigator_SkipWhitespace_1()
        {
            var navigator = new TextNavigator(" \t\r\nABC ");
            Assert.IsTrue(navigator.SkipWhitespace());
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.IsFalse(navigator.SkipWhitespace());
            Assert.IsTrue(navigator.IsEOF);
        }

        [TestMethod]
        public void TextNavigator_SkipWhitespaceAndPunctuation_1()
        {
            var navigator = new TextNavigator(" \t,\r\nABC. ");
            Assert.IsTrue(navigator.SkipWhitespaceAndPunctuation());
            Assert.AreEqual('A', navigator.ReadChar());
            navigator.ReadChar();
            navigator.ReadChar();
            Assert.IsFalse(navigator.SkipWhitespaceAndPunctuation());
            Assert.IsTrue(navigator.IsEOF);
        }

//        [TestMethod]
//        public void TextNavigator_SplitByGoodCharacters_1()
//        {
//            char[] good = { 'A', 'B', 'C', 'a', 'b', 'c' };
//            TextNavigator navigator = new TextNavigator("HELLOaworldBc!");
//            string[] result = navigator.SplitByGoodCharacters(good);
//            Assert.AreEqual(2, result.Length);
//            Assert.AreEqual("a", result[0]);
//            Assert.AreEqual("Bc", result[1]);
//        }
//
//        [TestMethod]
//        public void TextNavigator_SplitToWords_1()
//        {
//            TextNavigator navigator = new TextNavigator("Hello, world!");
//            string[] result = navigator.SplitToWords();
//            Assert.AreEqual(2, result.Length);
//            Assert.AreEqual("Hello", result[0]);
//            Assert.AreEqual("world", result[1]);
//        }
//
//        [TestMethod]
//        public void TextNavigator_SplitToWords_2()
//        {
//            char[] additional = { '<', '>' };
//            TextNavigator navigator = new TextNavigator("<Hello>, world!");
//            string[] result = navigator.SplitToWords(additional);
//            Assert.AreEqual(2, result.Length);
//            Assert.AreEqual("<Hello>", result[0]);
//            Assert.AreEqual("world", result[1]);
//        }

        [TestMethod]
        public void TextNavigator_Substring_1()
        {
            var navigator = new TextNavigator("Hello, world!");
            Assert.AreEqual("world", navigator.Substring(7, 5).ToString());
        }

        [TestMethod]
        public void TextNavigator_ToString_1()
        {
            var navigator = new TextNavigator("Hello, world!");
            Assert.AreEqual("Line=1, Column=1", navigator.ToString());
        }
    }
}
