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
public sealed class ParsingStateUtilityTest
{
    private delegate void ParseDelegate (DummyParser parser, ref ParseState<char> state);

    private sealed class DummyParser
        : Parser<char, string>
    {
        internal TextWriter? writer;
        internal ParseDelegate? action;

        public override bool TryParse
            (
                ref ParseState<char> state,
                ref PooledList<Expected<char>> expecteds,
                out string result
            )
        {
            result = null!;
            action! (this, ref state);
            //ParseStateUtility.DumpChar (ref state, _writer);

            return false;
        }
    }


    [TestMethod]
    public void ParsingStateUtility_DumpChar_1()
    {
        const string input = "input";
        var parser = new DummyParser()
        {
            writer = new StringWriter(),
            action = (DummyParser parser, ref ParseState<char> state) =>
            {
                ParseStateUtility.DumpChar (ref state, parser.writer);
            }
        };
        var result = parser.Parse (input);
        Assert.IsFalse (result.Success);
        const string expected = "'i' => 105\n";
        var actual = parser.writer.ToString().DosToUnix();
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void ParsingStateUtility_EatChar_1()
    {
        const string input = "input";
        var parser = new DummyParser()
        {
            writer = new StringWriter(),
            action = (DummyParser parser, ref ParseState<char> state) =>
            {
                ParseStateUtility.EatChar (ref state, 'i');
                ParseStateUtility.DumpChar (ref state, parser.writer);
            }
        };
        var result = parser.Parse (input);
        Assert.IsFalse (result.Success);
        const string expected = "'n' => 110\n";
        var actual = parser.writer.ToString().DosToUnix();
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void ParsingStateUtility_EatChar_2()
    {
        const string input = "input";
        var parser = new DummyParser()
        {
            writer = new StringWriter(),
            action = (DummyParser parser, ref ParseState<char> state) =>
            {
                ParseStateUtility.EatChar (ref state, 'n');
                ParseStateUtility.DumpChar (ref state, parser.writer);
            }
        };
        var result = parser.Parse (input);
        Assert.IsFalse (result.Success);
        const string expected = "'i' => 105\n";
        var actual = parser.writer.ToString().DosToUnix();
        Assert.AreEqual (expected, actual);
    }
}
