// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.IO;

using AM.Kotik;
using AM.Kotik.Tokenizers;

#endregion

#nullable enable

namespace KotikTests;

public class CommonParserTest
{
    /// <summary>
    /// Папка, в которой расположена UnitTests.dll.
    /// </summary>
    public string UnitTestDllPath => AppContext.BaseDirectory;

    /// <summary>
    /// Папка с данными для тестов.
    /// </summary>
    public string TestDataPath
    {
        get
        {
            var result = Path.Combine
                (
                    UnitTestDllPath,
                    @"../../../../../../TestData"
                );
            result = Path.GetFullPath (result);

            return result;
        }
    }

    protected static object IntegerArithmetic
        (
            object leftOperand,
            string operationCode,
            object rightOperand
        )
    {
        var left = (int) leftOperand;
        var right = (int) rightOperand;
        var result = operationCode switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            "&" => left & right,
            "|" => left | right,
            "<<" => left << right,
            ">>" => left >> right,
            _ => throw new InvalidOperationException()
        };

        return result;
    }

    protected ParseState _GetState
        (
            string text,
            bool enableTracing = false
        )
    {
        var tokenizer = Tokenizer.CreateDefault();
        var tokens = tokenizer.Tokenize (text);
        var traceOutput = enableTracing ? Console.Out : null;

        return new ParseState (tokens, traceOutput);
    }
}
