// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridCheckBox.cs -- редактор данных в виде чекбокса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

namespace AM.Windows.Forms;

/// <summary>
/// Редактор данных в виде чекбокса.
/// </summary>
public class TreeGridCheckBox
    : TreeGridEditor
{
    #region Properties

    /// <inheritdoc cref="TreeGridEditor.Control"/>
    public override Control Control => CheckBox;

    /// <summary>
    /// Собственно чекбокс, реализующий фукнциональность.
    /// </summary>
    public CheckBox CheckBox { get; } = new ();

    #endregion

    #region TreeGridEditor members

    /// <inheritdoc cref="TreeGridEditor.SetValue"/>
    public override void SetValue
        (
            string? value
        )
    {
        CheckBox.Checked = Convert.ToBoolean (value);
    }

    /// <inheritdoc cref="TreeGridEditor.GetValue"/>
    public override string GetValue()
    {
        return CheckBox.Checked.ToString();
    }

    /// <inheritdoc cref="TreeGridEditor.SelectText"/>
    public override void SelectText
        (
            int start,
            int length
        )
    {
        start.NotUsed();
        length.NotUsed();
    }

    #endregion
}
