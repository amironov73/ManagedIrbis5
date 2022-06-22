// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* OpenFileDialogExtensions.cs -- методы расширения для OpenFileDialog
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="OpenFileDialog"/>
/// </summary>
public static class OpenFileDialogExtensions
{
    #region Public methods

    /// <summary>
    /// Разрешение множественного выбора файлов.
    /// </summary>
    public static OpenFileDialog Multiselect
        (
            this OpenFileDialog dialog,
            bool multiselect = true
        )
    {
        Sure.NotNull (dialog);

        dialog.Multiselect = multiselect;

        return dialog;
    }

    /// <summary>
    /// Переключатель "Только для чтения" включен?
    /// </summary>
    /// <param name="dialog"></param>
    /// <param name="readOnlyChecked"></param>
    /// <returns></returns>
    public static OpenFileDialog ReadOnlyChecked
        (
            this OpenFileDialog dialog,
            bool readOnlyChecked = true
        )
    {
        Sure.NotNull (dialog);

        dialog.ReadOnlyChecked = readOnlyChecked;

        return dialog;
    }

    /// <summary>
    /// Показывать файлы, доступные только для чтения?
    /// </summary>
    public static OpenFileDialog ShowReadOnly
        (
            this OpenFileDialog dialog,
            bool showReadOnly = true
        )
    {
        Sure.NotNull (dialog);

        dialog.ShowReadOnly = showReadOnly;

        return dialog;
    }

    #endregion
}
