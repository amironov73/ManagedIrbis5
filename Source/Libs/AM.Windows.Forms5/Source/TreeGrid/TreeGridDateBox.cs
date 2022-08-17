// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

/* TreeGridDateBox.cs -- редактор для грида, позволяющий вводить дату
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Редактор для грида, позволяющий вводить дату.
/// </summary>
public class TreeGridDateBox
    : TreeGridEditor
{
    #region Properties

    /// <summary>
    /// Собственно редактор даты.
    /// </summary>
    public DateTimePicker DateTimePicker { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public TreeGridDateBox()
    {
        DateTimePicker = new DateTimePicker();
    }

    #endregion

    #region TreeGridEditor members

    /// <inheritdoc cref="TreeGridEditor.Control"/>
    public override Control Control => DateTimePicker;

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
        // пустое тело метода
    }

    #endregion
}
