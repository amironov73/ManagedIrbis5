// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global

/* ToolStripMaskedTextBox.cs -- MaskedTextBox, хостящийся в ToolStrip.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// <see cref="T:System.Windows.Forms.MaskedTextBox"/>, хостящийся в
/// <see cref="T:System.Windows.Forms.ToolStrip"/>.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
[ToolStripItemDesignerAvailability
    (ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
public class ToolStripMaskedTextBox
    : ToolStripControlHost
{
    #region Properties

    /// <summary>
    /// Собственно <see cref="MaskedTextBox"/>.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public MaskedTextBox MaskedTextBox => (MaskedTextBox) Control;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ToolStripMaskedTextBox()
        : base (new MaskedTextBox())
    {
        // пустое тело конструктора
    }

    #endregion

    #region ToolStripControlHost members

    /// <inheritdoc cref="ToolStripControlHost.Text"/>
    [Bindable (true)]
    [DefaultValue (null)]
    [Localizable (true)]
    public override string Text
    {
        get => MaskedTextBox.Text;
        set => MaskedTextBox.Text = value;
    }

    #endregion
}
