// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting.Barsik;
using AM.Text;

using Pidgin;
using static Pidgin.Parser<char>;

#nullable enable

namespace UnitTests;

[TestClass]
public sealed class IdentifierParserTest
{
    [TestMethod]
    public void IdentifierParser_TryParse_1()
    {
        var parser = new IdentifierParser();
        var result = parser.Parse ("hello");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    public void IdentifierParser_TryParse_2()
    {
        var parser = new IdentifierParser();
        var result = parser.Parse ("hello world");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello", result.Value);
    }

    [TestMethod]
    public void IdentifierParser_TryParse_3()
    {
        var parser = new IdentifierParser();
        var result = parser.Parse ("+-=/");
        Assert.IsFalse (result.Success);
    }

    [TestMethod]
    public void IdentifierParser_TryParse_4()
    {
        var parser = new IdentifierParser();
        var result = parser.Parse (string.Empty);
        Assert.IsFalse (result.Success);
    }

    [TestMethod]
    public void IdentifierParser_TryParse_5()
    {
        var parser = new IdentifierParser();
        var result = parser.Parse ("hello1()");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("hello1", result.Value);
    }
}
