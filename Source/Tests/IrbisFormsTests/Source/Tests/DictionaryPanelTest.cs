// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DictionaryPanelTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace IrbisFormsTests
{
    public sealed class DictionaryPanelTest
        : IIrbisFormsTest
    {
        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var form = new Form
            {
                Size = new Size(800, 600)
            };

            using var connection = IrbisFormsTest.GetConnection();

            var adapter = new TermAdapter(connection, "K=");
            adapter.Fill();

            var panel = new DictionaryPanel(adapter)
            {
                Location = new Point(10, 10),
                Size = new Size(300, 300),
            };
            form.Controls.Add(panel);

            var currentBox = new TextBox
            {
                Location = new Point(400, 10),
                Width = 300
            };
            form.Controls.Add(currentBox);

            var choosedBox = new TextBox
            {
                Location = new Point(400, 40),
                Width = 300
            };
            form.Controls.Add(choosedBox);

            adapter.Source.CurrentChanged += (sender, args) => { currentBox.Text = adapter.FullTerm; };
            panel.Choosed += (sender, args) => { choosedBox.Text = adapter.CurrentValue; };

            form.ShowDialog(ownerWindow);
        }
    }
}
