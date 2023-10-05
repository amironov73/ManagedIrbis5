// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ButtonedTextBox.cs -- текстовый бокс, снабженный кнопкой
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Текстовый бокс, снабженный кнопкой.
/// </summary>
[PublicAPI]
public class ButtonedTextBox
    : TextBox
{
    #region Properties

    /// <summary>
    /// Кнопки.
    /// </summary>
    public IReadOnlyList<Button> Buttons => _buttonList;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ButtonedTextBox()
    {
        _buttonPanel = new StackPanel
        {
            Orientation = Orientation.Horizontal
        };
        _buttonList = new List<Button>();
        InnerRightContent = _buttonPanel;
    }

    #endregion

    #region Private members

    private readonly StackPanel _buttonPanel;
    private readonly List<Button> _buttonList;

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление кнопки.
    /// </summary>
    public Button AddButton
        (
            Button button
        )
    {
        Sure.NotNull (button);

        _buttonPanel.Children.Add (button);
        _buttonList.Add (button);

        return button;
    }

    /// <summary>
    /// Добавление кнопки.
    /// </summary>
    public Button AddButton (object content) =>
        AddButton (new Button { Content = content });

    /// <summary>
    /// Добавление кнопки.
    /// </summary>
    public Button AddButton() => AddButton (new Button());

    /// <summary>
    /// Добавление кнопки, очищающей текст.
    /// </summary>
    public ButtonedTextBox AddClearButton()
    {
        AddButton ("x").Click += (_, _) => Clear();
        return this;
    }

    /// <summary>
    /// Добавление кнопки, копирующей текст в буфер обмена.
    /// </summary>
    public ButtonedTextBox AddCopyButton()
    {
        AddButton ("Copy").Click += (_, _) => Copy();
        return this;
    }

    /// <summary>
    /// Добавление кнопки, вставляющей текст из буфера обмена.
    /// </summary>
    public ButtonedTextBox AddPasteButton()
    {
        AddButton ("Paste").Click += (_, _) => Paste();
        return this;
    }

    #endregion

    #region StyledElement members

    /// <inheritdoc cref="StyledElement.StyleKeyOverride"/>
    protected override Type StyleKeyOverride => typeof (TextBox);

    #endregion
}
