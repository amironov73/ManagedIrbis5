// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

using Pidgin;

#nullable enable

namespace UnitTests.Scripting;

[TestClass]
public sealed class ClearParserTest
{
    [TestMethod]
    [Description ("Нет начальных пробелов")]
    public void ClearParser_Parse_1()
    {
        var parser = Resolve.Clear (Parser.LetterOrDigit.ManyString());
        var result = parser.Parse ("hello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Есть начальые пробелы")]
    public void ClearParser_Parse_2()
    {
        var parser = Resolve.Clear (Parser.LetterOrDigit.ManyString());
        var result = parser.Parse ("hello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("\thello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse ("\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse (" \t\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);

        result = parser.Parse (" \t\r\nhello, world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    [Description ("Есть только пробелы")]
    public void ClearParser_Parse_3()
    {
        var parser = Resolve.Clear (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse (" ");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("\t");
        Assert.IsFalse (result.Success);

        result = parser.Parse ("\n");
        Assert.IsFalse (result.Success);

        result = parser.Parse (" \t\r\n");
        Assert.IsFalse (result.Success);
    }

    [TestMethod]
    [Description ("Нет ничего, даже пробелов")]
    public void ClearParser_Parse_4()
    {
        var parser = Resolve.Clear (Parser.LetterOrDigit.AtLeastOnceString());
        var result = parser.Parse (string.Empty);
        Assert.IsFalse (result.Success);
    }
}
