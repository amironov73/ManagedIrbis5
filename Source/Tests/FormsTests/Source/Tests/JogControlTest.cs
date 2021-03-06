﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* JogControlTest.cs --
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
    public sealed class JogControlTest
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

            var jogControl = new JogControl()
            {
                Location = new Point(10, 10),
                Size = new Size(100, 100)
            };
            form.Controls.Add(jogControl);

            var textBox = new TextBox
            {
                Location = new Point(220, 10),
                Width = 200
            };
            form.Controls.Add(textBox);

            jogControl.AngleChanged += (sender, args) =>
            {
                textBox.Text = jogControl.Angle.ToString();
            };

            form.ShowDialog(ownerWindow);
        }

        #endregion
    }
}
