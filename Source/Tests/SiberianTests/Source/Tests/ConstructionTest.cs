// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ConstructionTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using ManagedIrbis.WinForms.Grid;

#endregion

#nullable enable

namespace SiberianTests;

public sealed class ConstructionTest
    : ISiberianTest
{
    #region ISiberianTest

    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var form = new DummyForm
        {
            Width = 800,
            Height = 600
        };

        var grid = new SiberianGrid
        {
            Dock = DockStyle.Fill
        };
        form.Controls.Add (grid);

        var firstColumn = grid.CreateColumn<SiberianTextColumn>();
        var secondColumn = grid.CreateColumn<SiberianButtonColumn>();
        var thirdColumn = grid.CreateColumn<SiberianTextColumn>();
        var fourthColumn = grid.CreateColumn<SiberianTextColumn>();
        var fifthColumn = grid.CreateColumn<SiberianCheckColumn>();

        firstColumn.Title = "Title";
        firstColumn.ReadOnly = true;
        firstColumn.Palette.BackColor = Color.PaleVioletRed;
        firstColumn.Member = "Title";

        secondColumn.ReadOnly = true;
        secondColumn.Palette.BackColor = Color.DarkGreen;
        secondColumn.Width = 20;

        thirdColumn.Title = "Value";
        thirdColumn.FillWidth = 100;
        thirdColumn.Member = "Value";

        fourthColumn.Title = "Appendix1";
        fourthColumn.FillWidth = 100;

        fifthColumn.Title = "Appendix2";
        fifthColumn.FillWidth = 30;
        fifthColumn.Member = "Check";

        for (var i = 0; i < 70; i++)
        {
            var description = new SubfieldDescription
            {
                Title = "Subfield " + (i + 1),
                Value = "Value " + (i + 1)
            };

            var row = grid.CreateRow (null);
            row.Data = description;
            row.GetData();
        }

        grid.Goto (2, 0);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
