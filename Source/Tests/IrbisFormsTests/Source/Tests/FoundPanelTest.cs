// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FoundPanelTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace IrbisFormsTests;

public sealed class FoundPanelTest
    : IIrbisFormsTest
{
    public void RunTest
        (
            IWin32Window? ownerWindow
        )
    {
        using var connection = IrbisFormsTest.GetConnection();

        var adapter = new RecordAdapter (connection);

        using var form = new Form
        {
            Size = new Size (800, 600)
        };

        var panel = new FoundPanel (adapter)
        {
            Location = new Point (10, 10),
            Size = new Size (600, 200)
        };
        form.Controls.Add (panel);
        panel.Fill();

        form.ShowDialog (ownerWindow);
    }
}
