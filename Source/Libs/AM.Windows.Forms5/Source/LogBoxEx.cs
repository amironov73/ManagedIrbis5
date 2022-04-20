// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable LocalizableElement
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LogBoxEx.cs -- расширенный список лог-сообщений
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Drawing;
using System.Windows.Forms;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// Расширенный список лог-сообщений.
/// </summary>
public sealed partial class LogBoxEx
    : UserControl
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public LogBoxEx()
    {
        InitializeComponent();

        _listBox.MeasureItem += ListBox_MeasureItem;
        _listBox.DrawItem += ListBox_DrawItem;

        _clearButton.Click += ClearButton_Click;
        _copyButton.Click += CopyButton_Click;
        _saveButton.Click += SaveButton_Click;
     }

    #endregion

    #region Private members

    private void ClearButton_Click
        (
            object? sender,
            EventArgs e
        )
    {
        Clear();
    }

    private void CopyButton_Click
        (
            object? sender,
            EventArgs e
        )
    {
        CopyToClipboard();
    }

    private void SaveButton_Click
        (
            object? sender,
            EventArgs e
        )
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Отрисовка элемента.
    /// </summary>
    private void ListBox_DrawItem
        (
            object? sender,
            DrawItemEventArgs e
        )
    {
        // TODO выбрать фон
        e.DrawBackground();

        var index = e.Index;
        if (index < 0 || index >= _listBox.Items.Count)
        {
            return;
        }

        var line = (LogLine?) _listBox.Items[index];
        if (line is null)
        {
            return;
        }

        if ((e.State & DrawItemState.Focus) != 0)
        {
            e.DrawFocusRectangle();
        }

        var graphics = e.Graphics;
        var bounds = e.Bounds;
        var font = e.Font ?? _listBox.Font;
        var icon = line.Icon;

        if (icon is not null)
        {
            var iconWidth = icon.Width + 5;
            graphics.DrawIconUnstretched
                (
                    icon,
                    bounds
                );
            bounds = new Rectangle
                (
                    bounds.Left + iconWidth,
                    bounds.Top,
                    bounds.Width - iconWidth,
                    bounds.Height
                );
        }

        var foreColor = e.ForeColor;
        if (line.ForeColor.HasValue)
        {
            foreColor = line.ForeColor.Value;
        }
        else
        {
            // TODO транслировать уровень сообщения в цвет
        }

        using var brush = new SolidBrush (foreColor);

        if (line.Moment.HasValue)
        {
            var momentText = line.Moment.Value + ":";
            var momentWidth = (int) Math.Ceiling (graphics.MeasureString (momentText, font).Width + 5);
            graphics.DrawString
                (
                    momentText,
                    font,
                    brush,
                    bounds
                );
            bounds = new Rectangle
                (
                    bounds.Left + momentWidth,
                    bounds.Top,
                    bounds.Width - momentWidth,
                    bounds.Height
                );
        }

        graphics.DrawString
            (
                line.Message,
                font,
                brush,
                bounds
            );
    }

    /// <summary>
    /// Измерение элемента.
    /// </summary>
    private void ListBox_MeasureItem
        (
            object? sender,
            MeasureItemEventArgs e
        )
    {
        // TODO implement
        e.ItemHeight = _listBox.ItemHeight;
        e.ItemWidth = _listBox.ClientSize.Width;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление сообщения в начало списка.
    /// </summary>
    public void AddLine
        (
            string message,
            LogLevel level = LogLevel.Information,
            Icon? icon = null
        )
    {
        var line = new LogLine
        {
            Moment = DateTime.Now,
            Message = message,
            Level = level,
            Icon = icon
        };

        AddLine (line);
    }

    /// <summary>
    /// Добавление сообщения в начало списка.
    /// </summary>
    public void AddLine
        (
            LogLine line
        )
    {
        Sure.NotNull (line);

        _listBox.Items.Insert (0, line);
    }

    /// <summary>
    /// Удаление всех сообщений из списка.
    /// </summary>
    public void Clear()
    {
        _listBox.Items.Clear();
    }

    /// <summary>
    /// Копирование всех сообщений в буфер обмена
    /// в обратном хронологическом порядке.
    /// </summary>
    public void CopyToClipboard()
    {
        // TODO implement
    }

    /// <summary>
    /// Сохренение всех сообщений в указанный файл
    /// в обратном хронологическом порядке.
    /// </summary>
    public void SaveToFile
        (
            string fileName
        )
    {
        Sure.NotNull (fileName);

        // TODO implement
    }

    #endregion
}
