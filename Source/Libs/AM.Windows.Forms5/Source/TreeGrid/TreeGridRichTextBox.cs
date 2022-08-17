// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TreeGridRichTextBox.cs -- RTF-редактор для ячейки грида
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// RTF-редактор для ячейки грида.
/// </summary>
public class TreeGridRichTextBox
    : TreeGridEditor
{
    #region Properties

    /// <summary>
    /// Собственно RTF-редактор.
    /// </summary>
    public RichTextBox RichTextBox { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridRichTextBox()
    {
        RichTextBox = new RichTextBox();
    }

    #endregion

    #region TreeGridEditor members

    /// <inheritdoc cref="TreeGridEditor.Control"/>
    public override Control Control => RichTextBox;

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
    public override void SelectText
        (
            int start,
            int length
        )
    {
        RichTextBox.SelectionStart = start;
        RichTextBox.SelectionLength = length;
    }

    #endregion
}
