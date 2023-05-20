// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

/*

    WinForms-приложение, демонстрирующее окно с заданным сообщением
    (например, об успешном выполнении действия).

    MsgBox <message> [caption]

 */

/* Program.cs -- вся логика программы
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace MsgBox;

/// <summary>
/// Вся логика программы.
/// </summary>
static class Program
{
    /// <summary>
    /// Точка входа.
    /// </summary>
    [STAThread]
    static void Main (string[] args)
    {
        Application.SetHighDpiMode (HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault (false);

        switch (args.Length)
        {
            case 1:
                MessageBox.Show (args[0]);
                break;

            case 2:
                MessageBox.Show (args[0], args[1]);
                break;
        }
    }
}
