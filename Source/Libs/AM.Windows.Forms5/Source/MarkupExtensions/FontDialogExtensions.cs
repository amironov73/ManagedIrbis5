// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FontDialogExtensions.cs -- методы расширения для FontDialog
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Drawing;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="FontDialog"/>.
/// </summary>
public static class FontDialogExtensions
{
    #region Public methods

    /// <summary>
    /// Разрешение пользователю менять скрипт при выборе шрифта.
    /// </summary>
    public static TFontDialog AllowScriptChange<TFontDialog>
        (
            this TFontDialog dialog,
            bool allow = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.AllowScriptChange = allow;

        return dialog;
    }

    /// <summary>
    /// Разрешение показывать пользователю симуляцию вывода
    /// выбранным шрифтом.
    /// </summary>
    public static TFontDialog AllowSimulations<TFontDialog>
        (
            this TFontDialog dialog,
            bool allow = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.AllowSimulations = allow;

        return dialog;
    }

    /// <summary>
    /// Разрешение показывать пользователю векторные шрифты.
    /// </summary>
    public static TFontDialog AllowVectorFonts<TFontDialog>
        (
            this TFontDialog dialog,
            bool allow = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.AllowVectorFonts = allow;

        return dialog;
    }

    /// <summary>
    /// Разрешение показывать пользователю вертикальные шрифты.
    /// </summary>
    public static TFontDialog AllowVerticalFonts<TFontDialog>
        (
            this TFontDialog dialog,
            bool allow = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.AllowVerticalFonts = allow;

        return dialog;
    }

    /// <summary>
    /// Выбранный цвет текста.
    /// </summary>
    public static TFontDialog Color<TFontDialog>
        (
            this TFontDialog dialog,
            Color color
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.Color = color;

        return dialog;
    }

    /// <summary>
    /// Только шрифты с фиксированным шагом.
    /// </summary>
    public static TFontDialog FixedPitchOnly<TFontDialog>
        (
            this TFontDialog dialog,
            bool allow = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.FixedPitchOnly = allow;

        return dialog;
    }

    /// <summary>
    /// Задание пред-выбранного шрифта.
    /// </summary>
    public static TFontDialog Font<TFontDialog>
        (
            this TFontDialog dialog,
            Font font
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);
        Sure.NotNull (font);

        dialog.Font = font;

        return dialog;
    }

    /// <summary>
    /// Шрифт должен существовать?
    /// </summary>
    public static TFontDialog FontMustExist<TFontDialog>
        (
            this TFontDialog dialog,
            bool must = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.FontMustExist = must;

        return dialog;
    }

    /// <summary>
    /// Максимальный размер шрифта, который позволяется выбрать.
    /// </summary>
    public static TFontDialog MaxSize<TFontDialog>
        (
            this TFontDialog dialog,
            int maxSize
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);
        Sure.Positive (maxSize);

        dialog.MaxSize = maxSize;

        return dialog;
    }

    /// <summary>
    /// Минимальный размер шрифта, который позволяется выбрать.
    /// </summary>
    public static TFontDialog MinSize<TFontDialog>
        (
            this TFontDialog dialog,
            int minSize
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);
        Sure.Positive (minSize);

        dialog.MinSize = minSize;

        return dialog;
    }

    /// <summary>
    /// Позволяет ли диалоговое окно выбирать шрифты для всех наборов
    /// символов, отличных от OEM, и наборов символов Symbol,
    /// а также для набора символов ANSI.
    /// </summary>
    public static TFontDialog ScriptOnly<TFontDialog>
        (
            this TFontDialog dialog,
            bool allow = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.ScriptsOnly = allow;

        return dialog;
    }

    /// <summary>
    /// Показывать ли кнопку "Apply"?
    /// </summary>
    public static TFontDialog ShowApply<TFontDialog>
        (
            this TFontDialog dialog,
            bool show = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.ShowApply = show;

        return dialog;
    }

    /// <summary>
    /// Давать ли возможность выбора цвета?
    /// </summary>
    public static TFontDialog ShowColor<TFontDialog>
        (
            this TFontDialog dialog,
            bool show = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.ShowColor = show;

        return dialog;
    }

    /// <summary>
    /// Давать ли возможность выбора эффектов
    /// подчеркивания и перечеркивания?
    /// </summary>
    public static TFontDialog ShowEffects<TFontDialog>
        (
            this TFontDialog dialog,
            bool show = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.ShowEffects = show;

        return dialog;
    }

    /// <summary>
    /// Показывать кнопку "Help"?
    /// </summary>
    public static TFontDialog ShowHelp<TFontDialog>
        (
            this TFontDialog dialog,
            bool show = true
        )
        where TFontDialog: FontDialog
    {
        Sure.NotNull (dialog);

        dialog.ShowHelp = show;

        return dialog;
    }

    #endregion
}
