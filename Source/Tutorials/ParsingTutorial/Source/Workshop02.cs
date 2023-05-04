// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable LocalizableElement

/* Workshop02.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using AM;
using AM.Purr.Parsers;
using AM.Purr.Tokenizers;

#endregion

#nullable enable

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
        var sourceCode = "-1, \"2\", -3";

        // термы, которые могут встретиться в тексте
        var knownTerms = new[] { ",", "-" };

        var comma = Term (",");
        var number = new LiteralParser().Map (Convert.ToInt32);
        var first = number.Instance ("First").StoreValue();
        var second = number.Instance ("Second").StoreValue();
        var third = number.Instance ("Third").StoreValue();
        var parser = Parser.Sequence (first, comma, second, comma, third)
            .End();

        parser.ParseOrThrow (sourceCode, knownTerms);
        Console.WriteLine ($"{first}, {second}, {third}");
    }
}
