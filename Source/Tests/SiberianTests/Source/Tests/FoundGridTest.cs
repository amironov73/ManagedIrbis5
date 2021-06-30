// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* FoundGridTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Windows.Forms;

using AM;

using ManagedIrbis;
using ManagedIrbis.ImportExport;
using ManagedIrbis.WinForms.Grid;
using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace SiberianTests
{
    public sealed class FoundGridTest
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

            var grid = new SiberianFoundGrid
            {
                Dock = DockStyle.Fill
            };
            form.Controls.Add(grid);

            const string connectionString = "host=127.0.0.1;port=6666;user=librarian;password=secret;db=IBIS;";
            using var connection = ConnectionFactory.Shared.CreateSyncConnection();
            connection.ParseConnectionString(connectionString);
            connection.Connect();

            var lines = new List<FoundLine>();
            for (int i = 1; i < 100; i++)
            {
                try
                {
                    var description = connection.FormatRecord("@brief", i);
                    var line = new FoundLine
                    {
                        Mfn = i,
                        Description = description
                    };
                    lines.Add(line);
                }
                catch
                {
                    // Nothing to do
                }
            }

            connection.Disconnect();

            grid.Load(lines.ToArray());

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
