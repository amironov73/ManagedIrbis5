// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* FieldGridTest.cs --
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

namespace SiberianTests
{
    public class FieldGridTest
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

            var grid = new SiberianFieldGrid
            {
                Dock = DockStyle.Fill
            };
            form.Controls.Add(grid);

            var record = PlainText.ReadOneRecord("record.txt", Encoding.UTF8)
                .ThrowIfNull("record");
            var worksheet = WsFile.ReadLocalFile("pazk31.ws");
            var page = worksheet.Pages[1];

            grid.Load(page, record);

            form.ShowDialog(ownerWindow);

        }

        #endregion
    }
}
