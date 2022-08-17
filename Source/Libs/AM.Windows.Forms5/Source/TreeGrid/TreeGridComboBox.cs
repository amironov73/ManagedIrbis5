// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TreeGridComboBox.cs -- редактор для грида в виде комбобокса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Редактор для грида в виде комбобокса.
/// </summary>
public class TreeGridComboBox
    : TreeGridEditor
{
    #region Properties

    /// <summary>
    /// Собственно комбобокс.
    /// </summary>
    public ComboBox ComboBox { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridComboBox()
    {
        ComboBox = new ComboBox();
    }

    #endregion

    #region TreeGridEditor members

    /// <inheritdoc cref="TreeGridEditor.Control"/>
    public override Control Control => ComboBox;

    /// <inheritdoc cref="TreeGridEditor.SetValue"/>
    public override void SetValue
        (
            string? value
        )
    {
        Control.Text = value;
    }

    /// <inheritdoc cref="TreeGridEditor.GetValue"/>
    public override string GetValue()
    {
        return Control.Text;
    }

    /// <inheritdoc cref="TreeGridEditor.SelectText"/>
    public override void SelectText (int start, int length)
    {
        ComboBox.SelectionStart = start;
        ComboBox.SelectionLength = length;
    }

    #endregion
}
