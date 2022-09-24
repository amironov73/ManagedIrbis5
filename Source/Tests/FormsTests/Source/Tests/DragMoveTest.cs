// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* DragMoveTest.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

using AM;
using AM.Windows.Forms;

#endregion

#nullable enable

namespace FormsTests;

public sealed class DragMoveTest
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
            Size = new Size (800, 600)
        };

        var button1 = new Button
        {
            Location = new Point (118, 124),
            Size = new Size (75, 23),
            TabIndex = 1,
            Text = "Close",
            UseVisualStyleBackColor = true
        };
        form.Controls.Add (button1);
        button1.Click += (_, _) => { form.Close(); };

        var label1 = new Label
        {
            AutoSize = true,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point (23, 21),
            Size = new Size (37, 15),
            Text = "label1"
        };
        form.Controls.Add (label1);
        label1.EnableDragMove (true);

        var label2 = new Label
        {
            AutoSize = true,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point (81, 51),
            Size = new Size (37, 15),
            Text = "label2"
        };
        form.Controls.Add (label2);
        label2.MouseDown += (_, _) => { label2.DragMove(); };

        var label3 = new Label
        {
            AutoSize = true,
            BorderStyle = BorderStyle.FixedSingle,
            Location = new Point (141, 77),
            Size = new Size (37, 15),
            Text = "label3"
        };
        form.Controls.Add (label3);

        var checkBox1 = new CheckBox
        {
            AutoSize = true,
            Location = new Point (12, 100),
            Size = new Size (75, 17),
            Text = "Drag Form",
            UseVisualStyleBackColor = true
        };
        form.Controls.Add (checkBox1);
        checkBox1.CheckedChanged += (_, _) => { form.EnableDragMove (checkBox1.Checked); };

        IContainer components = new Container();

        var toolTip1 = new ToolTip (components);
        var provider = new DragMoveProvider (components);
        provider.NotUsed();

        toolTip1.SetToolTip (label1, "Drag the label to move it (automatic mode using EnableDragMove)");
        toolTip1.SetToolTip (label2, "Drag the label to move it (manual mode using the MouseDown event)");
        toolTip1.SetToolTip (label3, "Drag the label to move it (automatic mode using the DragMoveProvider)");
        toolTip1.SetToolTip (form, "Déplacez la fenêtre en la trainant sur le bureau (mode automatique avec RegisterForDragMove)");


        form.ShowDialog (ownerWindow);
        components.Dispose();
    }

    #endregion
}
