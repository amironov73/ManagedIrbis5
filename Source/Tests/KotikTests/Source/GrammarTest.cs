// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class GrammarTest
    : CommonParserTest
{
    // [TestMethod]
    // [Description ("null")]
    // public void Grammar_Literal_1()
    // {
    //     var state = _GetState (" null hello ");
    //     var literal = Grammar.LiteralValue;
    //     Assert.IsNull (literal.ParseOrThrow (state));
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => literal.ParseOrThrow (state)
    //         );
    // }
    //
    // [TestMethod]
    // [Description ("bool")]
    // public void Grammar_Literal_2()
    // {
    //     var state = _GetState (" true false hello ");
    //     var literal = Grammar.LiteralValue;
    //     Assert.AreEqual (true, literal.ParseOrThrow (state));
    //     Assert.AreEqual (false, literal.ParseOrThrow (state));
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => literal.ParseOrThrow (state)
    //         );
    // }
    //
    // [TestMethod]
    // [Description ("Int32")]
    // public void Grammar_Literal_3()
    // {
    //     var state = _GetState (" 1, -1 hello ");
    //     var literal = Grammar.LiteralValue;
    //     Assert.AreEqual (1, literal.ParseOrThrow (state));
    //     state.Advance();
    //     Assert.AreEqual (-1, literal.ParseOrThrow (state));
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => literal.ParseOrThrow (state)
    //         );
    // }
    //
    // [TestMethod]
    // [Description ("Int64")]
    // public void Grammar_Literal_4()
    // {
    //     var state = _GetState (" 1l, -1L hello ");
    //     var literal = Grammar.LiteralValue;
    //     Assert.AreEqual (1L, literal.ParseOrThrow (state));
    //     state.Advance();
    //     Assert.AreEqual (-1L, literal.ParseOrThrow (state));
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => literal.ParseOrThrow (state)
    //         );
    // }
    //
    // [TestMethod]
    // [Description ("Hex32")]
    // public void Grammar_Literal_5()
    // {
    //     var state = _GetState ("0x123_456");
    //     var literal = Grammar.LiteralValue;
    //     Assert.AreEqual (0x123_456u, literal.ParseOrThrow (state));
    // }
    //
    // [TestMethod]
    // [Description ("Hex64")]
    // public void Grammar_Literal_6()
    // {
    //     var state = _GetState ("0x123_456_789L");
    //     var literal = Grammar.LiteralValue;
    //     Assert.AreEqual (0x123_456_789ul, literal.ParseOrThrow (state));
    // }

    // [TestMethod]
    // [Description ("Термы")]
    // public void Grammar_Term_1()
    // {
    //     var state = _GetState ("+ ++ - --");
    //     var parser = Grammar.Term ("+", "++", "-", "--");
    //     Assert.AreEqual ("+", parser.ParseOrThrow (state));
    //     Assert.AreEqual ("++", parser.ParseOrThrow (state));
    //     Assert.AreEqual ("-", parser.ParseOrThrow (state));
    //     Assert.AreEqual ("--", parser.ParseOrThrow (state));
    //     Assert.IsFalse (state.HasCurrent);
    // }
    //
    // [TestMethod]
    // [Description ("Зарезервированные слова")]
    // public void Grammar_Reserved_1()
    // {
    //     var state = _GetState ("using for break");
    //     Assert.AreEqual ("using", Grammar.Reserved ("using").ParseOrThrow (state));
    //     Assert.AreEqual ("for", Grammar.Reserved ("for").ParseOrThrow (state));
    //     Assert.AreEqual ("break", Grammar.Reserved ("break").ParseOrThrow (state));
    //     Assert.IsFalse (state.HasCurrent);
    // }

    // [TestMethod]
    // [Description ("Успешная проверка окончания")]
    // public void Grammar_End_1()
    // {
    //     var state = _GetState (" hello ");
    //     var parser = Grammar.Identifier.End();
    //     Assert.AreEqual ("hello", parser.ParseOrThrow (state));
    // }
    //
    // [TestMethod]
    // [Description ("Неуспешная проверка окончания")]
    // public void Grammar_End_2()
    // {
    //     var state = _GetState (" hello world");
    //     var parser = Grammar.Identifier.End();
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => parser.ParseOrThrow (state)
    //         );
    // }
    //
    // [TestMethod]
    // [Description ("Неуспешная проверка окончания")]
    // public void Grammar_End_3()
    // {
    //     var state = _GetState (" 1 hello");
    //     var parser = Grammar.Identifier.End();
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => parser.ParseOrThrow (state)
    //         );
    // }
    //
    // [TestMethod]
    // [Description ("Неуспешная проверка окончания")]
    // public void Grammar_End_4()
    // {
    //     var state = _GetState (" 1");
    //     var parser = Grammar.Identifier.End();
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => parser.ParseOrThrow (state)
    //         );
    // }
    //
    // [TestMethod]
    // [Description ("Неуспешная проверка окончания")]
    // public void Grammar_End_5()
    // {
    //     var state = _GetState (string.Empty);
    //     var parser = Grammar.Identifier.End();
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => parser.ParseOrThrow (state)
    //         );
    // }

    // [TestMethod]
    // [Description ("Нужное перед ненужным - успешно")]
    // public void Grammar_Before_1()
    // {
    //     var state = _GetState ("hello 1");
    //     var parser = Grammar.Identifier.Before (Grammar.LiteralValue).End();
    //     Assert.AreEqual ("hello", parser.ParseOrThrow (state));
    // }
    //
    // [TestMethod]
    // [Description ("Нужное перед ненужным - неуспешно")]
    // public void Grammar_Before_2()
    // {
    //     var state = _GetState ("hello world");
    //     var parser = Grammar.Identifier.Before (Grammar.LiteralValue).End();
    //     Assert.ThrowsException<SyntaxException>
    //         (
    //             () => parser.ParseOrThrow (state)
    //         );
    // }
}
