// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* CollapsibleGroupBoxTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests
{
    public sealed class CollapsibleGroupBoxTest
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

            var box1 = new CollapsibleGroupBox
            {
                BackColor = Color.DarkSeaGreen,
                Text = "Group box 1",
                Location = new Point(10, 10),
                Size = new Size(400, 200)
            };
            form.Controls.Add(box1);
            var box2 = new CollapsibleGroupBox
            {
                BackColor = Color.BlanchedAlmond,
                Text = "Group box 2",
                Location = new Point(420, 10),
                Size = new Size(300, 200)
            };
            form.Controls.Add(box2);

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
