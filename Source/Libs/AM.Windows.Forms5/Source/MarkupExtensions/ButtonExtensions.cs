// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ButtonExtensions.cs -- методы расширения для Button
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="Button"/>.
/// </summary>
public static class ButtonExtensions
{
    #region Public methods

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResult<TButton>
        (
            TButton button,
            DialogResult dialogResult
        )
        where TButton: Button
    {
        Sure.NotNull (button);
        Sure.Defined (dialogResult);

        button.DialogResult = dialogResult;

        return button;
    }

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResultCancel<TButton>
        (
            TButton button
        )
        where TButton: Button
    {
        Sure.NotNull (button);

        button.DialogResult = System.Windows.Forms.DialogResult.Cancel;

        return button;
    }

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResultNo<TButton>
        (
            TButton button
        )
        where TButton: Button
    {
        Sure.NotNull (button);

        button.DialogResult = System.Windows.Forms.DialogResult.No;

        return button;
    }

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResultOK<TButton>
        (
            TButton button
        )
        where TButton: Button
    {
        Sure.NotNull (button);

        button.DialogResult = System.Windows.Forms.DialogResult.OK;

        return button;
    }

    /// <summary>
    /// Задание диалогового результата для кнопки.
    /// </summary>
    public static TButton DialogResultYes<TButton>
        (
            TButton button
        )
        where TButton: Button
    {
        Sure.NotNull (button);

        button.DialogResult = System.Windows.Forms.DialogResult.Yes;

        return button;
    }

    #endregion
}
