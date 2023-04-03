// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.IO;
using System.Text;

using AM.Kotik;
using AM.Kotik.Barsik;
using AM.Kotik.Tokenizers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class TraceParserTest
{
    ParseState _GetState
        (
            string text,
            StringBuilder output
        )
    {
        var tokenizer = KotikUtility.CreateTokenizerForBarsik();
        var tokens = tokenizer.Tokenize (text);
        var writer = new StringWriter (output);

        return new ParseState (tokens, writer);
    }

    [TestMethod]
    [Description ("Успешный разбор")]
    public void TraceParser_Parse_1()
    {
        var output = new StringBuilder();
        var state = _GetState ("hello", output);
        var parser = Parser.Identifier.Trace();
        var value = parser.ParseOrThrow (state);
        Assert.AreEqual ("hello", value);
        Assert.IsTrue (output.Length > 0);
    }
}
