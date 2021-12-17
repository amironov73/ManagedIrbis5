// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* Program.cs -- точка входа в программу
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Linq;
using System.Text;

using AM.Windows.DevExpress;

#endregion

#nullable enable

/// <summary>
/// Вся логика программы в одном классе.
/// </summary>
static class Program
{
    public static int Main (string[] args)
    {
        using var excel = new EasyExcel();
        excel.WriteText ("Hello");
        excel.WriteText ("world");
        excel.CurrentRow.Bold();
        excel.NewLine();
        excel.WriteText ("По-русски").SetBorders();
        excel.SaveAs ("sample.xlsx");

        return 0;
    }
}
