// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable RedundantNameQualifier

/* ToolStripCheckBox.cs -- CheckBox, хостящийся в ToolStrip
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// <see cref="T:System.Windows.Forms.CheckBox"/>, хостящийся в
/// <see cref="T:System.Windows.Forms.ToolStrip"/>.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
[ToolStripItemDesignerAvailability
    (ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
public class ToolStripCheckBox
    : ToolStripControlHost
{
    #region Properties

    /// <summary>
    /// Собственно <see cref="CheckBox"/>.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public CheckBox CheckBox => (CheckBox) Control;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ToolStripCheckBox()
        : base (new CheckBox())
    {
        CheckBox.BackColor = Color.Transparent;
    }

    #endregion

    #region ToolStripControlHost members

    /// <inheritdoc cref="ToolStripControlHost.Text"/>
    [Bindable (true)]
    [DefaultValue (null)]
    [Localizable (true)]
    public override string Text
    {
        get => CheckBox.Text;
        set => CheckBox.Text = value;
    }

    #endregion
}
