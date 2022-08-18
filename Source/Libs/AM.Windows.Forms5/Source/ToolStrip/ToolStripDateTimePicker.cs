// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable RedundantNameQualifier
// ReSharper disable UnusedMember.Global

/* ToolStripDateTimePicker.cs -- DateTimePicker, хостящийся в ToolStrip.
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
/// <see cref="T:System.Windows.Forms.DateTimePicker"/>, хостящийся в
/// <see cref="T:System.Windows.Forms.ToolStrip"/>.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
[ToolStripItemDesignerAvailability
    (ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
public class ToolStripDateTimePicker
    : ToolStripControlHost
{
    #region Properties

    /// <summary>
    /// Собственно <see cref="DateTimePicker"/>.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public DateTimePicker DateTimePicker => (DateTimePicker) Control;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ToolStripDateTimePicker()
        : base (new DateTimePicker())
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
        get => DateTimePicker.Text;
        set => DateTimePicker.Text = value;
    }

    #endregion
}
