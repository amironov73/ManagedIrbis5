// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

using Pidgin;
using static Pidgin.Parser<char>;

#nullable enable

namespace UnitTests.Scripting;

[TestClass]
public sealed class SwallowParserTest
{
    [TestMethod]
    [Description ("Нет ни начальных пробелов, ни комментариев")]
    public void SwallowParser_Parse_1()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("hello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Есть начальные пробелы, нет комментариев")]
    public void SwallowParser_Parse_2()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse (" hello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("\thello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse (" \t\r\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Есть однострочный комментарий")]
    public void SwallowParser_Parse_3()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("//comment\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("//comment\rhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("//comment\r\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Есть многострочный комментарий")]
    public void SwallowParser_Parse_4()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("/*comment*/hello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Есть несколько однострочных комментариев")]
    public void SwallowParser_Parse_5()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("//comment1\n//comment2\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("//comment1\r//comment2\rhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("//comment1\r\n//comment2\r\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Есть несколько многострочных комментариев")]
    public void SwallowParser_Parse_6()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("/*comment1*//*comment2*/hello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("/*comment1*//*comment2*//*comment3*/hello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Нет ничего, кроме однострочного комментария")]
    public void SwallowParser_Parse_7()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("//comment1");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("//comment1\n");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("//comment1\r");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("//comment1\r\n");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("//comment1\n//comment2\n");
        Assert.IsFalse (result.Success);
    }

    [TestMethod]
    [Description ("Нет ничего, кроме многострочного комментария")]
    public void SwallowParser_Parse_8()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("/*comment1*/");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("/*comment1*/\n/*comment2*/");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("/*comment1*/ /*comment2*/ /*comment3*/");
        Assert.IsFalse (result.Success);
    }

    [TestMethod]
    [Description ("Неверно сформированный многострочный комментарий")]
    public void SwallowParser_Parse_9()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("/*comment");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("/*comment*");
        Assert.IsFalse (result.Success);
    }

    [TestMethod]
    [Description ("Оргия комментариев и пробелов")]
    public void SwallowParser_Parse_10()
    {
        var parser = Resolve.Swallow().Then (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse ("/*comment1*/\n //comment2 \n/*comment3\n*/\t//comment4\nhello");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

    }

    [Ignore]
    [TestMethod]
    [Description ("На сцену выходит разделитель")]
    public void SwallowParser_Parse_11()
    {
        var parser = Parser.LetterOrDigit.AtLeastOnceString()
            .SeparatedAndOptionallyTerminated (Resolve.Swallow (';'));
        var result = parser.Parse ("  hello; world ");
        Assert.IsTrue (result.Success);
        var words = result.Value.ToArray();
        Assert.AreEqual (2, words.Length);
        Assert.AreEqual ("hello", words[0]);
        Assert.AreEqual ("world", words[1]);
    }
}
