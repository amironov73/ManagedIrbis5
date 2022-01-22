// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable VirtualMemberCallInConstructor

/* AutocompleteItem.cs -- элемент меню автокомплита
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// Элемент меню автокомплита.
/// </summary>
public class AutocompleteItem
{
    #region Properties

    /// <summary>
    /// Title for tooltip.
    /// </summary>
    /// <remarks>Return null for disable tooltip for this item</remarks>
    public virtual string? ToolTipTitle { get; set; }

    /// <summary>
    /// Tooltip text.
    /// </summary>
    /// <remarks>For display tooltip text, ToolTipTitle must be not null</remarks>
    public virtual string? ToolTipText { get; set; }

    /// <summary>
    /// Menu text. This text is displayed in the drop-down menu.
    /// </summary>
    public virtual string? MenuText { get; set; }

    /// <summary>
    /// Fore color of text of item
    /// </summary>
    public virtual Color ForeColor
    {
        get => Color.Transparent;
        set => throw new NotImplementedException ("Override this property to change color");
    }


    /// <summary>
    /// Цвет фона.
    /// </summary>
    public virtual Color BackColor
    {
        get => Color.Transparent;
        set => throw new NotImplementedException ("Override this property to change color");
    }

    /// <summary>
    /// Родительский элемент.
    /// </summary>
    public AutocompleteMenu? Parent { get; internal set; }

    /// <summary>
    /// Отображаемый текст.
    /// </summary>
    public string? Text { get; set; }

    /// <summary>
    /// Индекс иконки.
    /// </summary>
    public int ImageIndex { get; set; }

    /// <summary>
    /// Произвольные пользовательские данные.
    /// </summary>
    public object? Tag { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public AutocompleteItem()
    {
        ImageIndex = -1;
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AutocompleteItem
        (
            string? text,
            int imageIndex = -1,
            string? menuText = null,
            string? toolTipTitle = null,
            string? toolTipText = null
        )
    {
        Text = text;
        ImageIndex = imageIndex;
        MenuText = menuText;
        ToolTipTitle = toolTipTitle;
        ToolTipText = toolTipText;
    }

    #endregion

    #region Private members

    #endregion

    #region Public methods

    /// <summary>
    /// Returns text for inserting into Textbox
    /// </summary>
    public virtual string GetTextForReplace()
    {
        return Text ?? string.Empty;
    }

    /// <summary>
    /// Compares fragment text with this item
    /// </summary>
    public virtual CompareResult Compare
        (
            string fragmentText
        )
    {
        if (Text!.StartsWith (fragmentText, StringComparison.InvariantCultureIgnoreCase) &&
            Text != fragmentText)
        {
            return CompareResult.VisibleAndSelected;
        }

        return CompareResult.Hidden;
    }

    /// <summary>
    /// This method is called after item inserted into text
    /// </summary>
    public virtual void OnSelected
        (
            AutocompleteMenu popupMenu,
            SelectedEventArgs e
        )
    {
        // пустое тело метода
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return MenuText ?? Text ?? string.Empty;
    }

    #endregion
}
