// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TreeGridTextBox.cs -- редактор для простого текста в ячейке грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Редактор для простого текста в ячейке грида <see cref="TreeGrid"/>.
/// </summary>
public class TreeGridTextBox
    : TreeGridEditor
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridTextBox()
    {
        TextBox = new TextBox();
    }

    #endregion

    #region Properties

    /// <inheritdoc cref="TreeGridEditor.Control"/>
    public override Control Control => TextBox;

    /// <summary>
    /// Собственно текстбокс, осуществляющий редактирование.
    /// </summary>
    public TextBox TextBox { get; }

    #endregion

    #region TreeGridEditor members

    /// <inheritdoc cref="TreeGridEditor.SetValue"/>
    public override void SetValue
        (
            string? value
        )
    {
        TextBox.Text = value;
    }

    /// <inheritdoc cref="TreeGridEditor.GetValue"/>
    public override string GetValue()
    {
        return TextBox.Text;
    }

    /// <inheritdoc cref="TreeGridEditor.SelectText"/>
    public override void SelectText (int start, int length)
    {
        TextBox.SelectionStart = start;
        TextBox.SelectionLength = length;
    }

    #endregion
}
