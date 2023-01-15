// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using AM.Kotik;

#endregion

#nullable enable

namespace KotikTests;

public class CommonParserTest
{
    protected ParseState _GetState
        (
            string text,
            bool enableTracing = false
        )
    {
        var tokenizer = new Tokenizer();
        var tokens = tokenizer.Tokenize (text);
        var traceOutput = enableTracing ? Console.Out : null;

        return new ParseState (tokens, traceOutput);
    }
}
