// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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
    public void BarsikTokenizer_Tokenize_5()
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
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (TokenKind.Term, tokens[0].Kind);
        Assert.AreEqual ("-", tokens[0].Value);
        Assert.AreEqual (TokenKind.Int32, tokens[1].Kind);
        Assert.AreEqual ("1234", tokens[1].Value);
    }



#if NOTDEF


    [TestMethod]
    [Description ("64-битное целое число со знаком")]
    public void BarsikTokenizer_Tokenize_6()
    {
        var tokens = BarsikTokenizer.Tokenize ("1234l");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Int64, tokens[0].Kind);
        Assert.AreEqual ("1234l".AsMemory(), tokens[0].Value);

        tokens = BarsikTokenizer.Tokenize ("1234L");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Int64, tokens[0].Kind);
        Assert.AreEqual ("1234L".AsMemory(), tokens[0].Value);

        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("123ll")
            );
        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("123lL")
            );

        tokens = BarsikTokenizer.Tokenize ("1234l ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Int64, tokens[0].Kind);
        Assert.AreEqual ("1234l", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("1234l(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (BarsikToken.Int64, tokens[0].Kind);
        Assert.AreEqual ("1234l", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("1234l+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (BarsikToken.Int64, tokens[0].Kind);
        Assert.AreEqual ("1234l", tokens[0].Value.ToString());
    }

    [TestMethod]
    [Description ("32-битное целое число без знака")]
    public void BarsikTokenizer_Tokenize_7()
    {
        var tokens = BarsikTokenizer.Tokenize ("1234u");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt32, tokens[0].Kind);
        Assert.AreEqual ("1234u".AsMemory(), tokens[0].Value);

        tokens = BarsikTokenizer.Tokenize ("1234U");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt32, tokens[0].Kind);
        Assert.AreEqual ("1234U".AsMemory(), tokens[0].Value);

        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("123uu")
            );
        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("123uU")
            );

        tokens = BarsikTokenizer.Tokenize ("1234u ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt32, tokens[0].Kind);
        Assert.AreEqual ("1234u", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("1234u(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt32, tokens[0].Kind);
        Assert.AreEqual ("1234u", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("1234u+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt32, tokens[0].Kind);
        Assert.AreEqual ("1234u", tokens[0].Value.ToString());
    }

    [TestMethod]
    [Description ("64-битное целое число без знака")]
    public void BarsikTokenizer_Tokenize_8()
    {
        var tokens = BarsikTokenizer.Tokenize ("1234lu");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234lu".AsMemory(), tokens[0].Value);

        tokens = BarsikTokenizer.Tokenize ("1234LU");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234LU".AsMemory(), tokens[0].Value);

        tokens = BarsikTokenizer.Tokenize ("1234ul");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234ul".AsMemory(), tokens[0].Value);

        tokens = BarsikTokenizer.Tokenize ("1234UL");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234UL".AsMemory(), tokens[0].Value);

        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("123lul")
            );
        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("123ulU")
            );

        tokens = BarsikTokenizer.Tokenize ("1234lu ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234lu", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("1234lu(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234lu", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("1234lu+");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (BarsikToken.UInt64, tokens[0].Kind);
        Assert.AreEqual ("1234lu", tokens[0].Value.ToString());
    }

    [TestMethod]
    [Description ("Простое арифметическое выражение")]
    public void BarsikTokenizer_Tokenize_9()
    {
        var tokens = BarsikTokenizer.Tokenize ("12+34");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (BarsikToken.Int32, tokens[0].Kind);
        Assert.AreEqual ("+", tokens[1].Kind);
        Assert.AreEqual (BarsikToken.Int32, tokens[2].Kind);

        tokens = BarsikTokenizer.Tokenize (" 12 + 34 ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (BarsikToken.Int32, tokens[0].Kind);
        Assert.AreEqual ("+", tokens[1].Kind);
        Assert.AreEqual (BarsikToken.Int32, tokens[2].Kind);
    }

    [TestMethod]
    [Description ("Идентификатор")]
    public void BarsikTokenizer_Tokenize_10()
    {
        var tokens = BarsikTokenizer.Tokenize ("hello");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Identifier, tokens[0].Kind);
        Assert.AreEqual ("hello".AsMemory(), tokens[0].Value);

        tokens = BarsikTokenizer.Tokenize (" hello ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Identifier, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("hello(");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (2, tokens.Count);
        Assert.AreEqual (BarsikToken.Identifier, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value.ToString());
        Assert.AreEqual ("(", tokens[1].Kind);

        tokens = BarsikTokenizer.Tokenize ("hello(world");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (3, tokens.Count);
        Assert.AreEqual (BarsikToken.Identifier, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value.ToString());
        Assert.AreEqual ("(", tokens[1].Kind);
        Assert.AreEqual (BarsikToken.Identifier, tokens[2].Kind);
        Assert.AreEqual ("world", tokens[2].Value.ToString());

        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("№")
            );
        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("hello№")
            );
    }

    [TestMethod]
    [Description ("Одиночный символ")]
    public void BarsikTokenizer_Tokenize_11()
    {
        var tokens = BarsikTokenizer.Tokenize ("'a'");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Char, tokens[0].Kind);
        Assert.AreEqual ("a", tokens[0].Value.ToString());

        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("''")
            );
        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("'a")
            );
        Assert.ThrowsException<FormatException>
            (
                () => BarsikTokenizer.Tokenize ("'a)")
            );
    }

    [TestMethod]
    [Description ("Строка символов")]
    public void BarsikTokenizer_Tokenize_12()
    {
        var tokens = BarsikTokenizer.Tokenize ("\"a\"");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.String, tokens[0].Kind);
        Assert.AreEqual ("a", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("\"\"");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.String, tokens[0].Kind);
        Assert.AreEqual (string.Empty, tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize ("\"hello\"");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.String, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value.ToString());

        tokens = BarsikTokenizer.Tokenize (" \"hello\" ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.String, tokens[0].Kind);
        Assert.AreEqual ("hello", tokens[0].Value.ToString());
    }

    [TestMethod]
    [Description ("Число с плавающей точкой двойной точности")]
    public void BarsikTokenizer_Tokeninze_13()
    {
        var tokens = BarsikTokenizer.Tokenize ("123.45");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Double, tokens[0].Kind);
        Assert.AreEqual ("123.45".AsMemory(), tokens[0].Value);
    }

    [TestMethod]
    [Description ("Число с плавающей точкой одинарной точности")]
    public void BarsikTokenizer_Tokeninze_14()
    {
        var tokens = BarsikTokenizer.Tokenize ("123.45f");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Single, tokens[0].Kind);
        Assert.AreEqual ("123.45f".AsMemory(), tokens[0].Value);

        tokens = BarsikTokenizer.Tokenize ("123.45F");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (BarsikToken.Single, tokens[0].Kind);
        Assert.AreEqual ("123.45F".AsMemory(), tokens[0].Value);
    }

#endif
}
