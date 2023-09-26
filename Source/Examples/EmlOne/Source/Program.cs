// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* MainWindow.cs -- главное окно приложения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace EmlOne;

internal static class Program
{
    #region Program entry point

    /// <summary>
    /// Точка входа в программу.
    /// </summary>
    /// <param name="args">Аргументы командной строки.</param>
    [STAThread]
    public static void Main
        (
            string[] args
        )
    {
        if (args.Length != 0)
        {
            var program = Grammar.Load (args[0]);
            var context = new AM.Lexey.Eml.Context();
            var window = program.Execute (context);
            Console.WriteLine ($"Got window: {window}");
        }
    }

    #endregion
}
