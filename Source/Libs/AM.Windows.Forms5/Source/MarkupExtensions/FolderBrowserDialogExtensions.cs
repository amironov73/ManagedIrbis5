// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FolderBrowserDialogExtensions.cs -- методы расширения для FolderBrowserDialog
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="FolderBrowserDialog"/>
/// </summary>
public static class FolderBrowserDialogExtensions
{
    #region Public methods

    /// <summary>
    /// Описание диалога для пользователя.
    /// </summary>
    public static FolderBrowserDialog Description
        (
            this FolderBrowserDialog dialog,
            string description
        )
    {
        Sure.NotNull (dialog);
        Sure.NotNullNorEmpty (description);

        dialog.Description = description;

        return dialog;
    }

    /// <summary>
    /// Установка корневой директории для диалога.
    /// </summary>
    public static FolderBrowserDialog RootFolder
        (
            this FolderBrowserDialog dialog,
            Environment.SpecialFolder rootFolder
        )
    {
        Sure.NotNull (dialog);
        Sure.Defined (rootFolder);

        dialog.RootFolder = rootFolder;

        return dialog;
    }

    /// <summary>
    /// Выбор директории в диалоге.
    /// </summary>
    public static FolderBrowserDialog SelectedPath
        (
            this FolderBrowserDialog dialog,
            string selectedPath
        )
    {
        Sure.NotNull (dialog);
        Sure.NotNullNorEmpty (selectedPath);

        dialog.SelectedPath = selectedPath;

        return dialog;
    }

    /// <summary>
    /// Включение кнопки "Новая папка".
    /// </summary>
    public static FolderBrowserDialog ShowNewFolderButton
        (
            this FolderBrowserDialog dialog,
            bool newFolderButton = true
        )
    {
        Sure.NotNull (dialog);

        dialog.ShowNewFolderButton = newFolderButton;

        return dialog;
    }

    /// <summary>
    /// Использовать описание в качестве заголовка диалога?
    /// </summary>
    public static FolderBrowserDialog UseDescriptionForTitle
        (
            this FolderBrowserDialog dialog,
            bool descriptionForTitle = true
        )
    {
        Sure.NotNull (dialog);

        dialog.UseDescriptionForTitle = descriptionForTitle;

        return dialog;
    }

    #endregion
}
