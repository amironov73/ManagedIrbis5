// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace KotikTests;

[TestClass]
public class TokenizerTest
{
    [TestMethod]
    [Description ("Пустая строка")]
    public void Tokenizer_Tokenize_1()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (string.Empty);
        Assert.IsNotNull (tokens);
        Assert.AreEqual (0, tokens.Count);
    }

    [TestMethod]
    [Description ("Терм из одного символа")]
    public void Tokenizer_Tokenize_2()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" = ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("=", tokens[0].Value);

        tokens = tokenizer.Tokenize (" = = ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("=", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("=", tokens[1].Value);
    }

    [TestMethod]
    [Description ("Терм из двух символов")]
    public void Tokenizer_Tokenize_3()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" == ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("==", tokens[0].Value);

        tokens = tokenizer.Tokenize (" == == ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("==", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("==", tokens[1].Value);

        tokens = tokenizer.Tokenize (" ++ x ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("++", tokens[0].Value);
        Assert.AreEqual (TokenKind.Identifier, tokens[1].Kind);
        Assert.AreEqual ("x", tokens[1].Value);

        tokens = tokenizer.Tokenize (" x ++ ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Identifier, tokens[0].Kind);
        Assert.AreEqual ("x", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("++", tokens[1].Value);
    }

    [TestMethod]
    [Description ("Слипшиеся термы")]
    public void Tokenizer_Tokenize_4()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" === ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("==", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("=", tokens[1].Value);
    }

    [TestMethod]
    [Description ("32-битное целое число со знаком")]
    public void Tokenizer_Tokenize_5()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" 1234 ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("1234(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("1234+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-1234");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("-1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-1234 -4567");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("-1234", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("-", tokens[1].Value);
        Assert.AreEqual (TokenKind.Int32, tokens[2].Kind);
        Assert.AreEqual ("4567", tokens[2].Value);

        tokens = tokenizer.Tokenize ("-1234 + -4567");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("-1234", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("+", tokens[1].Value);
        Assert.AreEqual (TokenKind.Int32, tokens[2].Kind);
        Assert.AreEqual ("-4567", tokens[2].Value);

        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("12uu")
            );
        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("12ll")
            );
        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("12fl")
            );
        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("12fm")
            );
        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("12ff")
            );
        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("12mm")
            );
    }

    [TestMethod]
    [Description ("64-битное целое число со знаком")]
    public void Tokenizer_Tokenize_6()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" 1234l ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Int64, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("1234l(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Int64, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("1234l+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Int64, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-1234l");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Int64, tokens[0].Kind);
        Assert.AreEqual ("-1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-1234l -4567l");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Int64, tokens[0].Kind);
        Assert.AreEqual ("-1234", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("-", tokens[1].Value);
        Assert.AreEqual (TokenKind.Int64, tokens[2].Kind);
        Assert.AreEqual ("4567", tokens[2].Value);

        tokens = tokenizer.Tokenize ("-1234l + -4567l");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Int64, tokens[0].Kind);
        Assert.AreEqual ("-1234", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("+", tokens[1].Value);
        Assert.AreEqual (TokenKind.Int64, tokens[2].Kind);
        Assert.AreEqual ("-4567", tokens[2].Value);
    }

    [TestMethod]
    [Description ("32-битное целое число без знака")]
    public void Tokenizer_Tokenize_7()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" 1234u ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.UInt32, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("1234u(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.UInt32, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("1234u+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.UInt32, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-1234u");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("-", tokens[0].Value);
        Assert.AreEqual (TokenKind.UInt32, tokens[1].Kind);
        Assert.AreEqual ("1234", tokens[1].Value);

        tokens = tokenizer.Tokenize ("-1234u -4567u");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("-", tokens[0].Value);
        Assert.AreEqual (TokenKind.UInt32, tokens[1].Kind);
        Assert.AreEqual ("1234", tokens[1].Value);
        Assert.AreEqual (TokenKind.Term, tokens[2].Kind);
        Assert.AreEqual ("-", tokens[2].Value);
        Assert.AreEqual (TokenKind.UInt32, tokens[3].Kind);
        Assert.AreEqual ("4567", tokens[3].Value);

        tokens = tokenizer.Tokenize ("-1234u + -4567u");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (5, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("-", tokens[0].Value);
        Assert.AreEqual (TokenKind.UInt32, tokens[1].Kind);
        Assert.AreEqual ("1234", tokens[1].Value);
        Assert.AreEqual (TokenKind.Term, tokens[2].Kind);
        Assert.AreEqual ("+", tokens[2].Value);
        Assert.AreEqual (TokenKind.Term, tokens[3].Kind);
        Assert.AreEqual ("-", tokens[3].Value);
        Assert.AreEqual (TokenKind.UInt32, tokens[4].Kind);
        Assert.AreEqual ("4567", tokens[4].Value);

        tokens = tokenizer.Tokenize ("0x1234 0xABCDEF 0xabcdef");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Hex32, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);
        Assert.AreEqual (TokenKind.Hex32, tokens[1].Kind);
        Assert.AreEqual ("ABCDEF", tokens[1].Value);
        Assert.AreEqual (TokenKind.Hex32, tokens[2].Kind);
        Assert.AreEqual ("abcdef", tokens[2].Value);

        tokens = tokenizer.Tokenize ("0x12_34 0x_ABC_DEF_ 0x_abc_def_");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Hex32, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);
        Assert.AreEqual (TokenKind.Hex32, tokens[1].Kind);
        Assert.AreEqual ("ABCDEF", tokens[1].Value);
        Assert.AreEqual (TokenKind.Hex32, tokens[2].Kind);
        Assert.AreEqual ("abcdef", tokens[2].Value);
    }

    [TestMethod]
    [Description ("64-битное целое число без знака")]
    public void Tokenizer_Tokenize_8()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" 1234ul ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("1234ul(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("1234ul+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-1234ul");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("-", tokens[0].Value);
        Assert.AreEqual (TokenKind.UInt64, tokens[1].Kind);
        Assert.AreEqual ("1234", tokens[1].Value);

        tokens = tokenizer.Tokenize ("-1234ul -4567ul");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("-", tokens[0].Value);
        Assert.AreEqual (TokenKind.UInt64, tokens[1].Kind);
        Assert.AreEqual ("1234", tokens[1].Value);
        Assert.AreEqual (TokenKind.Term, tokens[2].Kind);
        Assert.AreEqual ("-", tokens[2].Value);
        Assert.AreEqual (TokenKind.UInt64, tokens[3].Kind);
        Assert.AreEqual ("4567", tokens[3].Value);

        tokens = tokenizer.Tokenize ("-1234ul + -4567ul");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (5, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("-", tokens[0].Value);
        Assert.AreEqual (TokenKind.UInt64, tokens[1].Kind);
        Assert.AreEqual ("1234", tokens[1].Value);
        Assert.AreEqual (TokenKind.Term, tokens[2].Kind);
        Assert.AreEqual ("+", tokens[2].Value);
        Assert.AreEqual (TokenKind.Term, tokens[3].Kind);
        Assert.AreEqual ("-", tokens[3].Value);
        Assert.AreEqual (TokenKind.UInt64, tokens[4].Kind);
        Assert.AreEqual ("4567", tokens[4].Value);

        tokens = tokenizer.Tokenize ("0x1234L 0xABCDEFL 0xabcdefl");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Hex64, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);
        Assert.AreEqual (TokenKind.Hex64, tokens[1].Kind);
        Assert.AreEqual ("ABCDEF", tokens[1].Value);
        Assert.AreEqual (TokenKind.Hex64, tokens[2].Kind);
        Assert.AreEqual ("abcdef", tokens[2].Value);

        tokens = tokenizer.Tokenize ("0x12_34l 0x_ABC_DEF_l 0x_abc_def_l");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Hex64, tokens[0].Kind);
        Assert.AreEqual ("1234", tokens[0].Value);
        Assert.AreEqual (TokenKind.Hex64, tokens[1].Kind);
        Assert.AreEqual ("ABCDEF", tokens[1].Value);
        Assert.AreEqual (TokenKind.Hex64, tokens[2].Kind);
        Assert.AreEqual ("abcdef", tokens[2].Value);
    }

    [TestMethod]
    [Description ("Идентификатор")]
    public void Tokenizer_Tokenize_10()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize ("hello");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Identifier, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value);

        tokens = tokenizer.Tokenize (" hello ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Identifier, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value);

        tokens = tokenizer.Tokenize ("hello(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Identifier, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("(", tokens[1].Value);

        tokens = tokenizer.Tokenize ("hello(world");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Identifier, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("(", tokens[1].Value);
        Assert.AreEqual (TokenKind.Identifier, tokens[2].Kind);
        Assert.AreEqual ("world", tokens[2].Value);

        tokens = tokenizer.Tokenize ("у попа была собака");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Identifier, tokens[0].Kind);
        Assert.AreEqual ("у", tokens[0].Value);
        Assert.AreEqual (TokenKind.Identifier, tokens[1].Kind);
        Assert.AreEqual ("попа", tokens[1].Value);
        Assert.AreEqual (TokenKind.Identifier, tokens[2].Kind);
        Assert.AreEqual ("была", tokens[2].Value);
        Assert.AreEqual (TokenKind.Identifier, tokens[3].Kind);
        Assert.AreEqual ("собака", tokens[3].Value);

        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("№")
            );
        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("hello№")
            );
    }

    [TestMethod]
    [Description ("Одиночный символ")]
    public void Tokenizer_Tokenize_11()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize ("'a'");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Char, tokens[0].Kind);
        Assert.AreEqual ("a", tokens[0].Value);

        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("''")
            );
        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("'a")
            );
        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("'a)")
            );
    }

    [TestMethod]
    [Description ("Строка символов")]
    public void Tokenizer_Tokenize_12()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize ("\"a\"");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.String, tokens[0].Kind);
        Assert.AreEqual ("a", tokens[0].Value);

        tokens = tokenizer.Tokenize ("\"\"");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.String, tokens[0].Kind);
        Assert.AreEqual (string.Empty, tokens[0].Value);

        tokens = tokenizer.Tokenize ("\"hello\"");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.String, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value);

        tokens = tokenizer.Tokenize (" \"hello\" ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.String, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value);

        tokens = tokenizer.Tokenize (" \"у попа была собака\" ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.String, tokens[0].Kind);
        Assert.AreEqual ("у попа была собака", tokens[0].Value);

        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize (" \"hello ")
            );
    }

    [TestMethod]
    [Description ("Число с плавающей точкой двойной точности")]
    public void Tokenizer_Tokenize_13()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" 123.45 ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Double, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("123.45(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Double, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("123.45+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Double, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-123.45");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Double, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-123.45 -456.78");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Double, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("-", tokens[1].Value);
        Assert.AreEqual (TokenKind.Double, tokens[2].Kind);
        Assert.AreEqual ("456.78", tokens[2].Value);

        tokens = tokenizer.Tokenize ("-123.45 + -456.78");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Double, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("+", tokens[1].Value);
        Assert.AreEqual (TokenKind.Double, tokens[2].Kind);
        Assert.AreEqual ("-456.78", tokens[2].Value);

        tokens = tokenizer.Tokenize ("1.23e-45 1.23E-45 1.23e+45 1e23");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Double, tokens[0].Kind);
        Assert.AreEqual ("1.23e-45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Double, tokens[1].Kind);
        Assert.AreEqual ("1.23E-45", tokens[1].Value);
        Assert.AreEqual (TokenKind.Double, tokens[2].Kind);
        Assert.AreEqual ("1.23e+45", tokens[2].Value);
        Assert.AreEqual (TokenKind.Double, tokens[3].Kind);
        Assert.AreEqual ("1e23", tokens[3].Value);

        tokens = tokenizer.Tokenize ("1. .1 1.e23 .1e23");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Double, tokens[0].Kind);
        Assert.AreEqual ("1.", tokens[0].Value);
        Assert.AreEqual (TokenKind.Double, tokens[1].Kind);
        Assert.AreEqual (".1", tokens[1].Value);
        Assert.AreEqual (TokenKind.Double, tokens[2].Kind);
        Assert.AreEqual ("1.e23", tokens[2].Value);
        Assert.AreEqual (TokenKind.Double, tokens[3].Kind);
        Assert.AreEqual (".1e23", tokens[3].Value);
    }

    [TestMethod]
    [Description ("Число с плавающей точкой одинарной точности")]
    public void Tokenizer_Tokenize_14()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" 123.45f ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Single, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("123.45f(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Single, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("123.45f+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Single, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-123.45f");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Single, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-123.45f -456.78f");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Single, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("-", tokens[1].Value);
        Assert.AreEqual (TokenKind.Single, tokens[2].Kind);
        Assert.AreEqual ("456.78", tokens[2].Value);

        tokens = tokenizer.Tokenize ("-123.45f + -456.78f");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Single, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("+", tokens[1].Value);
        Assert.AreEqual (TokenKind.Single, tokens[2].Kind);
        Assert.AreEqual ("-456.78", tokens[2].Value);

        tokens = tokenizer.Tokenize ("1.23e-45f 1.23E-45f 1.23e+45f 1e23f");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Single, tokens[0].Kind);
        Assert.AreEqual ("1.23e-45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Single, tokens[1].Kind);
        Assert.AreEqual ("1.23E-45", tokens[1].Value);
        Assert.AreEqual (TokenKind.Single, tokens[2].Kind);
        Assert.AreEqual ("1.23e+45", tokens[2].Value);
        Assert.AreEqual (TokenKind.Single, tokens[3].Kind);
        Assert.AreEqual ("1e23", tokens[3].Value);

        tokens = tokenizer.Tokenize ("1.f .1f 1.e23f .1e23f");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Single, tokens[0].Kind);
        Assert.AreEqual ("1.", tokens[0].Value);
        Assert.AreEqual (TokenKind.Single, tokens[1].Kind);
        Assert.AreEqual (".1", tokens[1].Value);
        Assert.AreEqual (TokenKind.Single, tokens[2].Kind);
        Assert.AreEqual ("1.e23", tokens[2].Value);
        Assert.AreEqual (TokenKind.Single, tokens[3].Kind);
        Assert.AreEqual (".1e23", tokens[3].Value);
    }

    [TestMethod]
    [Description ("Число с фиксированной точкой")]
    public void Tokenizer_Tokenize_15()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" 123.45m ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize (" 123m ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("123", tokens[0].Value);

        tokens = tokenizer.Tokenize ("123.45m(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("123.45m+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-123.45m");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);

        tokens = tokenizer.Tokenize ("-123.45m -456.78m");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("-", tokens[1].Value);
        Assert.AreEqual (TokenKind.Decimal, tokens[2].Kind);
        Assert.AreEqual ("456.78", tokens[2].Value);

        tokens = tokenizer.Tokenize ("-123.45m + -456.78m");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("-123.45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("+", tokens[1].Value);
        Assert.AreEqual (TokenKind.Decimal, tokens[2].Kind);
        Assert.AreEqual ("-456.78", tokens[2].Value);

        tokens = tokenizer.Tokenize ("1.23e-45m 1.23E-45m 1.23e+45m 1e23m");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("1.23e-45", tokens[0].Value);
        Assert.AreEqual (TokenKind.Decimal, tokens[1].Kind);
        Assert.AreEqual ("1.23E-45", tokens[1].Value);
        Assert.AreEqual (TokenKind.Decimal, tokens[2].Kind);
        Assert.AreEqual ("1.23e+45", tokens[2].Value);
        Assert.AreEqual (TokenKind.Decimal, tokens[3].Kind);
        Assert.AreEqual ("1e23", tokens[3].Value);

        tokens = tokenizer.Tokenize ("1.m .1m 1.e23m .1e23m");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (4, tokens.Count);
        Assert.AreEqual (TokenKind.Decimal, tokens[0].Kind);
        Assert.AreEqual ("1.", tokens[0].Value);
        Assert.AreEqual (TokenKind.Decimal, tokens[1].Kind);
        Assert.AreEqual (".1", tokens[1].Value);
        Assert.AreEqual (TokenKind.Decimal, tokens[2].Kind);
        Assert.AreEqual ("1.e23", tokens[2].Value);
        Assert.AreEqual (TokenKind.Decimal, tokens[3].Kind);
        Assert.AreEqual (".1e23", tokens[3].Value);
    }

    [TestMethod]
    [Description ("Зарезервированные слова")]
    public void Tokenizer_Tokenize_16()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (" using true false ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.ReservedWord, tokens[0].Kind);
        Assert.AreEqual ("using", tokens[0].Value);
        Assert.AreEqual (TokenKind.ReservedWord, tokens[1].Kind);
        Assert.AreEqual ("true", tokens[1].Value);
        Assert.AreEqual (TokenKind.ReservedWord, tokens[2].Kind);
        Assert.AreEqual ("false", tokens[2].Value);

        tokens = tokenizer.Tokenize (" Using True False ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Identifier, tokens[0].Kind);
        Assert.AreEqual ("Using", tokens[0].Value);
        Assert.AreEqual (TokenKind.Identifier, tokens[1].Kind);
        Assert.AreEqual ("True", tokens[1].Value);
        Assert.AreEqual (TokenKind.Identifier, tokens[2].Kind);
        Assert.AreEqual ("False", tokens[2].Value);

        tokens = tokenizer.Tokenize (" ,using-true-false ) ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (7, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual (",", tokens[0].Value);
        Assert.AreEqual (TokenKind.ReservedWord, tokens[1].Kind);
        Assert.AreEqual ("using", tokens[1].Value);
        Assert.AreEqual (TokenKind.Term, tokens[2].Kind);
        Assert.AreEqual ("-", tokens[2].Value);
        Assert.AreEqual (TokenKind.ReservedWord, tokens[3].Kind);
        Assert.AreEqual ("true", tokens[3].Value);
        Assert.AreEqual (TokenKind.Term, tokens[4].Kind);
        Assert.AreEqual ("-", tokens[4].Value);
        Assert.AreEqual (TokenKind.ReservedWord, tokens[5].Kind);
        Assert.AreEqual ("false", tokens[5].Value);
        Assert.AreEqual (TokenKind.Term, tokens[6].Kind);
        Assert.AreEqual (")", tokens[6].Value);
    }

    [TestMethod]
    [Description ("Комментарии")]
    public void Tokenizer_Tokenize_17()
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize ("1 /* Comment */ 2u");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("1", tokens[0].Value);
        Assert.AreEqual (TokenKind.UInt32, tokens[1].Kind);
        Assert.AreEqual ("2", tokens[1].Value);

        tokens = tokenizer.Tokenize ("1 //Comment  \n 2u");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("1", tokens[0].Value);
        Assert.AreEqual (TokenKind.UInt32, tokens[1].Kind);
        Assert.AreEqual ("2", tokens[1].Value);

        tokens = tokenizer.Tokenize ("1 / 2u");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (TokenKind.Int32, tokens[0].Kind);
        Assert.AreEqual ("1", tokens[0].Value);
        Assert.AreEqual (TokenKind.Term, tokens[1].Kind);
        Assert.AreEqual ("/", tokens[1].Value);
        Assert.AreEqual (TokenKind.UInt32, tokens[2].Kind);
        Assert.AreEqual ("2", tokens[2].Value);

        Assert.ThrowsException<SyntaxException>
            (
                () => tokenizer.Tokenize ("1 /*Comment  \n 2u")
            );
    }
}
