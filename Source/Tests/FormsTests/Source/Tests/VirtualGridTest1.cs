// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable CoVariantArrayConversion
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* VirtualGridTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public class VirtualGridTest1
    : IFormsTest
{
    #region NestedClasses

    class SquareAdapter
        : VirtualAdapter
    {
        public override VirtualData PullData
            (
                int firstLine,
                int lineCount
            )
        {
            lineCount = Math.Min (1_000_000 - firstLine, lineCount);
            var lines = new List<object[]> (lineCount);
            for (var i = 0; i < lineCount; i++)
            {
                long number = firstLine + i + 1;
                var line = new object[] { number, number * number };
                lines.Add (line);
            }

            var result = new VirtualData
            {
                FirstLine = firstLine,
                LineCount = lineCount,
                Lines = lines.ToArray(),
                TotalCount = 1_000_000
            };

            return result;
        }
    }

    #endregion

    #region IFormsTest members

    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var form = new Form
        {
            Size = new Size (800, 600)
        };

        var grid = new VirtualGrid
        {
            Location = new Point (10, 10),
            Size = new Size (600, 300)
        };
        DataGridViewColumn column1 = new DataGridViewTextBoxColumn
        {
            HeaderText = "Число",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        };
        grid.Columns.Add (column1);
        DataGridViewColumn column2 = new DataGridViewTextBoxColumn
        {
            HeaderText = "Квадрат",
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        };
        grid.Columns.Add (column2);
        form.Controls.Add (grid);
        grid.Adapter = new SquareAdapter();
        grid.InitialFill();

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
