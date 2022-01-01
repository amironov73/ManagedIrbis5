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
public sealed class RemainderParserTest
{
    [TestMethod]
    [Description ("Остаток текста: что-то осталось")]
    public void RemainderParser_Parse_1()
    {
        var parser = Parser<char>.Any.Then (Resolve.Remainder);
        var result = parser.Parse ("abc");
        Assert.IsTrue (result.Success);
        Assert.AreEqual ("bc", result.Value);
    }

    [TestMethod]
    [Description ("Остаток текста: ничего осталось")]
    public void RemainderParser_Parse_2()
    {
        var parser = Parser<char>.Any.Then (Resolve.Remainder);
        var result = parser.Parse ("a");
        Assert.IsTrue (result.Success);
        Assert.AreEqual (string.Empty, result.Value);
    }

}
