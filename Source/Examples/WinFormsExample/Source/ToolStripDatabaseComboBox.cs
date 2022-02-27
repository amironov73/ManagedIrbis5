// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* ToolStripDatabaseComboBox.cs -- выпадающий список баз данных ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Windows.Forms;
using System.Windows.Forms.Design;

using ManagedIrbis.WinForms;

#endregion

#nullable enable

namespace WinFormsExample;

/// <summary>
/// Выпадающий список баз данных ИРБИС64.
/// </summary>
[System.ComponentModel.DesignerCategory ("Code")]
[ToolStripItemDesignerAvailability (ToolStripItemDesignerAvailability.ToolStrip |
    ToolStripItemDesignerAvailability.StatusStrip)]
public sealed class ToolStripDatabaseComboBox
    : ToolStripControlHost
{
    #region Properties

    /// <summary>
    /// Внутренний контрол <see cref="DatabaseComboBox"/>.
    /// </summary>
    [DesignerSerializationVisibility (DesignerSerializationVisibility.Content)]
    public DatabaseComboBox ComboBox => (DatabaseComboBox)Control;

    #endregion

    #region Construction

    public ToolStripDatabaseComboBox()
        : base (new DatabaseComboBox())
    {
    }

    #endregion
}
