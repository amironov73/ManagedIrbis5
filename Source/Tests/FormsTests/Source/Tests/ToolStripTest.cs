// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ToolStripTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

namespace FormsTests
{
    public sealed class ToolStripTest
        : IFormsTest
    {
        #region IFormsTest members

        public void RunTest
            (
                IWin32Window? ownerWindow
            )
        {
            using var form = new Form
            {
                Size = new Size(800, 600)
            };

            var container = new ToolStripContainer
            {
                Dock = DockStyle.Fill
            };
            form.Controls.Add(container);
            var toolStrip = new ToolStrip
            {
                Dock = DockStyle.Top
            };
            container.TopToolStripPanel.Controls.Add(toolStrip);

            var checkBox = new ToolStripCheckBox
            {
                Text = "CheckBox"
            };
            toolStrip.Items.Add(checkBox);

            var colorBox = new ToolStripColorComboBox();
            toolStrip.Items.Add(colorBox);

            var datePicker = new ToolStripDateTimePicker();
            toolStrip.Items.Add(datePicker);

            var upDown = new ToolStripNumericUpDown();
            toolStrip.Items.Add(upDown);

            var ordinaryButton = new ToolStripOrdinaryButton
            {
                Text = "Button"
            };
            toolStrip.Items.Add(ordinaryButton);

            var trackBar = new ToolStripTrackBar
            {
                Height = toolStrip.ClientSize.Height
            };
            toolStrip.Items.Add(trackBar);

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
