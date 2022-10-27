// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* LogTextOutput.cs -- устройство вывода в текстовое поле
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text.Output;

using Avalonia.Controls;
using Avalonia.Threading;

#endregion

#nullable enable

namespace AM.Avalonia.Controls;

/// <summary>
/// Устройство вывода в текстовое поле.
/// </summary>
public sealed class TextBoxOutput
    : AbstractOutput
{
    #region Properties

    /// <summary>
    /// Текстбокс, в который направляется выводимый текст.
    /// </summary>
    public TextBox TextBox { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextBoxOutput
        (
            TextBox textBox
        )
    {
        Sure.NotNull (textBox);

        TextBox = textBox;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Добавление текста в конец текстбокса.
    /// </summary>
    /// <param name="text"></param>
    public void AppendText
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            Dispatcher.UIThread.Post (() => TextBox.Text += text);
        }
    }

    #endregion

    #region AbstractOutput members

    /// <summary>
    /// Флаг: был ли вывод с помощью WriteError.
    /// </summary>
    public override bool HaveError { get; set; }

    /// <summary>
    /// Очищает вывод, например, окно.
    /// Надо переопределить в потомке.
    /// </summary>
    public override AbstractOutput Clear()
    {
        HaveError = false;
        Dispatcher.UIThread.Post (() => TextBox.Text = string.Empty);

        return this;
    }

    /// <summary>
    /// Конфигурирование объекта.
    /// Надо переопределить в потомке.
    /// </summary>
    public override AbstractOutput Configure
        (
            string configuration
        )
    {
        // TODO: implement

        return this;
    }

    /// <summary>
    /// Простой вывод строки.
    /// Нужно переопределить в потомке.
    /// </summary>
    public override AbstractOutput Write
        (
            string text
        )
    {
        AppendText (text);

        return this;
    }

    /// <summary>
    /// Выводит ошибку. Например, красным цветом.
    /// Надо переопределить в потомке.
    /// </summary>
    public override AbstractOutput WriteError
        (
            string text
        )
    {
        HaveError = true;
        AppendText (text);

        return this;
    }

    #endregion
}
