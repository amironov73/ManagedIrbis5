// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* ProgressCircleTest.cs --
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
    public sealed class ProgressCircleTest
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

            var percent = 0f;

            var circle = new ProgressCircle()
            {
                Location = new Point(10, 10),
                Size = new Size(100, 100)
            };
            form.Controls.Add(circle);

            var timer = new Timer
            {
                Enabled = true,
                Interval = 100
            };
            timer.Tick += (sender, args) =>
            {
                percent += 1f;
                if (percent >= 100f)
                {
                    percent = 0f;
                }
                circle.Percent = percent;
            };

            form.ShowDialog(ownerWindow);
            timer.Dispose();
        }

        #endregion
    }
}
