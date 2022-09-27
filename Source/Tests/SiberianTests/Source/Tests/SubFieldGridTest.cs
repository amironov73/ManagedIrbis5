// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SubFieldGridTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text;
using System.Windows.Forms;

using AM;

using ManagedIrbis.ImportExport;
using ManagedIrbis.WinForms.Grid;
using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace SiberianTests;

public sealed class SubFieldGridTest
    : ISiberianTest
{
    #region ISiberianTest members

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

        var grid = new SiberianSubFieldGrid
        {
            Dock = DockStyle.Fill
        };
        form.Controls.Add (grid);

        var record = PlainText.ReadOneRecord ("record.txt", Encoding.UTF8)
            .ThrowIfNull ("record");
        var field = record.GetField (692).ThrowIfNull ();
        var worksheet = WssFile.ReadLocalFile ("692.wss");

        grid.Load (worksheet, field);

        form.ShowDialog (ownerWindow);
    }

    #endregion
}
