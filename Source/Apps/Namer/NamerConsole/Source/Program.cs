// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using NamerCommon;

#endregion

#nullable enable

namespace NamerConsole;

/// <summary>
/// Точка входа в программу.
/// </summary>
internal static class Program
{
    public static void Main
        (
            string[] args
        )
    {
        if (args.Length < 2)
        {
            return;
        }

        var specification = args[0];
        var processor = new NameProcessor();
        processor.Parse (specification);
        var context = new NamingContext();

        for (var i = 1; i < args.Length; i++)
        {
            var directory = new DirectoryInfo (args[i]);
            var pairs = processor.Render (context, directory);
            NamePair.Render (pairs);
        }
    }
}
