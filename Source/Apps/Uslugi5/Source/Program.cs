// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

using AM;

#endregion

#nullable enable

namespace Uslugi5;

static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main
        (
            string[] args
        )
    {
        try
        {
            Magna.Initialize (args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault (false);
            Application.Run (new MainForm());
        }
        catch (Exception exception)
        {
            MessageBox.Show (exception.ToString());
        }
    }
}
