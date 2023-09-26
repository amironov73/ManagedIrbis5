// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement

/* Workshop02.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Lexey.Parsing;
using AM.Lexey.Tokenizing;

#endregion

namespace ParsingTutorial;

/// <summary>
/// Сохранение разобранных значений.
/// </summary>
internal static class Workshop02
{
    private static TermParser Term (params string[] terms) => new (terms);

    public static void Step1()
    {
        // текст, подлежащий разбору
        var sourceCode = "-1, 2, -3";

        // термы, которые могут встретиться в тексте
        var knownTerms = new[] { ",", "-" };

        var comma = Term (",");
        var number = new LiteralParser().Map (Convert.ToInt32);
        var first = number.Instance ("First").StoreValue();
        var second = number.Instance ("Second").StoreValue();
        var third = number.Instance ("Third").StoreValue();
        var parser = Parser.Sequence (first, comma, second, comma, third)
            .End();

        var tokenizer = new Tokenizer
        {
            Refiner = new StandardTokenRefiner(),
            Recognizers =
            {
                new IntegerRecognizer(),
                new WhitespaceRecognizer(),
                new TermRecognizer (knownTerms)
            }
        };
        var tokens = tokenizer.ScanForTokens (sourceCode);
        var state = new ParseState (tokens);
        parser.ParseOrThrow (state);
        Console.WriteLine ($"{first}, {second}, {third}");
    }
}
