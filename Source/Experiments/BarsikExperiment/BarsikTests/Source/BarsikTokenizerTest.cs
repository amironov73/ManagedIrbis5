// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

namespace BarsikTests;

[TestClass]
public class BarsikTokenizerTest
{
    [TestMethod]
    [Description ("Пустая строка")]
    public void BarsikTokenizer_Tokenize_1()
    {
        var tokens = BarsikTokinizer.Tokenize (string.Empty);
        Assert.IsNotNull (tokens);
        Assert.AreEqual (0, tokens.Count);
    }

    [TestMethod]
    [Description ("Простой токен")]
    public void BarsikTokenizer_Tokenize_2()
    {
        var tokens = BarsikTokinizer.Tokenize (" = ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.Equal, tokens[0].Kind);
    }

    [TestMethod]
    [Description ("Составной токен")]
    public void BarsikTokenizer_Tokenize_3()
    {
        var tokens = BarsikTokinizer.Tokenize (" == ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.EqualEqual, tokens[0].Kind);
    }

    [TestMethod]
    [Description ("Ещё более составной токен")]
    public void BarsikTokenizer_Tokenize_4()
    {
        var tokens = BarsikTokinizer.Tokenize (" === ");
        Assert.IsNotNull (tokens);
        Assert.AreEqual (1, tokens.Count);
        Assert.AreEqual (TokenKind.EqualEqualEqual, tokens[0].Kind);
    }
}
