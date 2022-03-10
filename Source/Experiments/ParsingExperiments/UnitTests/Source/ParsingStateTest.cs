// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;
using AM.Text;

using Pidgin;

#nullable enable

namespace UnitTests;

[TestClass]
public sealed class ParsingStateTest
{
    private sealed class DummyParser
        : Parser<char, string>
    {
        internal TextWriter? _writer;

        public override bool TryParse
            (
                ref ParseState<char> state,
                ref PooledList<Expected<char>> expecteds,
                out string result
            )
        {
            result = null!;
            ParseStateUtility.DumpChar (ref state, _writer);

            return false;
        }
    }


    [TestMethod]
    public void ParsingStateUtility_DumpChar_1()
    {
        const string input = "input";
        var parser = new DummyParser()
        {
            _writer = new StringWriter()
        };
        var result = parser.Parse (input);
        Assert.IsFalse (result.Success);
        const string expected = "'i' => 105\n";
        var actual = parser._writer.ToString().DosToUnix();
        Assert.AreEqual (expected, actual);
    }
}
