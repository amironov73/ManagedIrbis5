// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable RedundantNameQualifier

/* ToolStripColorComboBox.cs -- ComboBox, хостящийся в ToolStrip
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
/// <see cref="T:System.Windows.Forms.ComboBox"/>, хостящийся в
/// <see cref="T:System.Windows.Forms.ToolStrip"/>.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
[ToolStripItemDesignerAvailability
    (ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
public class ToolStripColorComboBox
    : ToolStripControlHost
{
    #region Properties

    /// <summary>
    /// Собственно <see cref="ColorComboBox"/>.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public ColorComboBox ColorComboBox => (ColorComboBox) Control;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public ToolStripColorComboBox()
        : base (new ColorComboBox())
    {
        // пустое тело конструктора
    }

    #endregion
}
