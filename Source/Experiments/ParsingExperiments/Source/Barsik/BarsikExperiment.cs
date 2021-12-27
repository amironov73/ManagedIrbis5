// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* BarsikUtility.cs -- полезные методы для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;

#endregion

#nullable enable

namespace ParsingExperiments.Barsik;

static class BarsikExperiment
{
    static void ParseAndExecute
        (
            string sourceCode
        )
    {
        Console.WriteLine (sourceCode);
        try
        {
            using var interpreter = new Interpreter();
            interpreter.Execute (sourceCode);
            Console.WriteLine (new string ('.', 60));
            interpreter.Context.DumpVariables();
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception.Message);
        }

        Console.WriteLine (new string ('-', 60));
    }

    public static void Interprete()
    {
        ParseAndExecute ("x = 1 y = 2 z = x + y");
        ParseAndExecute ("x = 1.0 y = 2.0 z = x + y");
        ParseAndExecute ("/* 1 */ x = /* 2 */ 1 /* 3 */ y = /* 4 */ 2 z = x + y /* 5 */");
        ParseAndExecute ("x = \"1\" y = \"2\" z = x + y");
    }
}
