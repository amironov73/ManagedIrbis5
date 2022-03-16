// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.BuildSystem;

#endregion

#nullable enable

namespace Barsik;

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    static int Main
        (
            string[] args
        )
    {
        if (args.Length != 2)
        {
            Console.Error.WriteLine ("USAGE: NugetReserver <projectRoot> <reserveRoot>");
            return 1;
        }

        try
        {
            var projectRoot = args[0];
            var reserveRoot = args[1];
            ProjectParser.ReserveNugetPackages (projectRoot, reserveRoot);
        }
        catch (Exception exception)
        {
            Console.Error.WriteLine (exception.ToString());
            return 2;
        }

        return 0;
    }
}
