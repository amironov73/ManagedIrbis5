// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TextBoxExtensions.cs -- методы расширения для TextBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="TextBox"/>.
/// </summary>
public static class TextBoxExtensions
{
    #region Public methods

    /// <summary>
    /// Включение режима приёма клавиши <c>Enter</c>.
    /// </summary>
    public static TTextBox AcceptsReturn<TTextBox>
        (
            this TTextBox textBox,
            bool accepts = true
        )
        where TTextBox: TextBox
    {
        Sure.NotNull (textBox);

        textBox.AcceptsReturn = accepts;

        return textBox;
    }

    /// <summary>
    /// Включение режима преобразования регистра символов.
    /// </summary>
    public static TTextBox CharacterCasing<TTextBox>
        (
            this TTextBox textBox,
            CharacterCasing casing = System.Windows.Forms.CharacterCasing.Upper
        )
        where TTextBox: TextBox
    {
        Sure.NotNull (textBox);

        textBox.CharacterCasing = casing;

        return textBox;
    }

    /// <summary>
    /// Включение многострочного режима.
    /// </summary>
    public static TTextBox Multiline<TTextBox>
        (
            this TTextBox textBox,
            bool multiline = true
        )
        where TTextBox: TextBox
    {
        Sure.NotNull (textBox);

        textBox.Multiline = multiline;

        return textBox;
    }

    /// <summary>
    /// Включение режима ввода пароля.
    /// </summary>
    public static TTextBox PasswordChar<TTextBox>
        (
            this TTextBox textBox,
            char passwordChar = '*'
        )
        where TTextBox: TextBox
    {
        Sure.NotNull (textBox);

        textBox.PasswordChar = passwordChar;

        return textBox;
    }

    /// <summary>
    /// Включение текста-подсказки.
    /// </summary>
    public static TTextBox PlaceholderText<TTextBox>
        (
            this TTextBox textBox,
            string? placeholder
        )
        where TTextBox: TextBox
    {
        Sure.NotNull (textBox);

        textBox.PlaceholderText = placeholder;

        return textBox;
    }

    /// <summary>
    /// Включение режима "только для чтения".
    /// </summary>
    public static TTextBox ReadOnly<TTextBox>
        (
            this TTextBox textBox,
            bool readOnly = true
        )
        where TTextBox: TextBox
    {
        Sure.NotNull (textBox);

        textBox.ReadOnly = readOnly;

        return textBox;
    }

    /// <summary>
    /// Включение использования системного символа
    /// для ввода пароля.
    /// </summary>
    public static TTextBox UseSystemPasswordChar<TTextBox>
        (
            this TTextBox textBox,
            bool useSystem = true
        )
        where TTextBox: TextBox
    {
        Sure.NotNull (textBox);

        textBox.UseSystemPasswordChar = useSystem;

        return textBox;
    }

    #endregion
}
