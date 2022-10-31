// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RadioButtonGroup.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AeroSuite.Controls;

/// <summary>
/// A list of radio buttons based on a collection of objects for easy
/// creation of radio button groups.
/// </summary>
/// <remarks>
/// Basically a FlowLayoutPanel that flows "TopDown" with a radio button
/// for every entry in the input list.
/// </remarks>
[DesignerCategory ("Code")]
[DisplayName ("RadioButton Group")]
[Description ("A list of radio buttons based on a collection of objects " +
              "for easy creation of radio button groups.")]
[ToolboxItem (true)]
[ToolboxBitmap (typeof (CheckedListBox))]
public class RadioButtonGroup
    : Control
{
    /// <summary>
    /// Initializes a new instance of the <see cref="RadioButtonGroup"/> class.
    /// </summary>
    public RadioButtonGroup()
    {
        Size = new Size (200, 350);

        RadioButtonContainer = new FlowLayoutPanel
        {
            AutoScroll = true,
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown,
            WrapContents = false

        };
        for (var i = 0; i < 10; i++)
        {
            var radioButton = new RadioButton
            {
                AutoEllipsis = true,
                AutoSize = false,
                Text = "Option " + i,
                Margin = new Padding (0)
            };
            radioButton.Width = RadioButtonContainer.ClientRectangle.Width;
            RadioButtonContainer.Controls.Add (radioButton);
        }

        Controls.Add (RadioButtonContainer);
    }

    /// <summary>
    /// Raises the <see cref="E:Resize" /> event.
    /// </summary>
    /// <param name="eventArgs">The <see cref="EventArgs"/> instance containing the event data.</param>
    protected override void OnResize
        (
            EventArgs eventArgs
        )
    {
        base.OnResize (eventArgs);

        if (RadioButtonContainer != null)
        {
            foreach (RadioButton radioButton in RadioButtonContainer.Controls)
            {
                radioButton.Width = RadioButtonContainer.ClientRectangle.Width;
            }
        }
    }

    private FlatStyle _flatStyle = FlatStyle.Standard;

    /// <summary>
    /// Gets or sets the flat style.
    /// </summary>
    /// <value>
    /// The flat style.
    /// </value>
    public FlatStyle FlatStyle
    {
        get => _flatStyle;
        set
        {
            _flatStyle = value;

            if (RadioButtonContainer != null)
            {
                foreach (RadioButton radioButton in RadioButtonContainer.Controls)
                {
                    radioButton.FlatStyle = _flatStyle;
                }
            }
        }
    }

    /// <summary>
    /// Gets the RadioButton container.
    /// </summary>
    /// <value>
    /// The RadioButton container.
    /// </value>
    protected FlowLayoutPanel RadioButtonContainer { get; private set; }
}
