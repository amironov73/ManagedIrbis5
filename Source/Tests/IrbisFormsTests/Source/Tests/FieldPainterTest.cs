// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldPainterTest.cs --
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
    public sealed class FieldPainterTest
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

            form.Paint += form_Paint;

            form.ShowDialog(ownerWindow);
        }

        void form_Paint
            (
                object sender,
                PaintEventArgs e
            )
        {
            var painter = new FieldPainter
                (
                    Color.Red,
                    Color.Black
                );

            if (sender is Form form)
            {
                var font = form.Font;
                var point = new PointF(10, 10);
                var text = "Text1^atext2^btext3";

                painter.DrawLine
                (
                    e.Graphics,
                    font,
                    point,
                    text
                );
            }
        }
    }
}
