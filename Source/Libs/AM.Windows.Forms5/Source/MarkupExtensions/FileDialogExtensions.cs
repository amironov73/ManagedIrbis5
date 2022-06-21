// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FileDialogExtensions.cs -- методы расширения для FileDialog
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.MarkupExtensions;

/// <summary>
/// Методы расширения для <see cref="FileDialog"/>.
/// </summary>
public static class FileDialogExtensions
{
    #region Public methods

    /// <summary>
    /// Проверять, что файл существует?
    /// </summary>
    public static TFileDialog CheckFileExists<TFileDialog>
        (
            this TFileDialog dialog,
            bool checkExists = true
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);

        dialog.CheckFileExists = checkExists;

        return dialog;
    }

    /// <summary>
    /// Проверять, что путь существует?
    /// </summary>
    public static TFileDialog CheckPathExists<TFileDialog>
        (
            this TFileDialog dialog,
            bool checkExists = true
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);

        dialog.CheckPathExists = checkExists;

        return dialog;
    }

    /// <summary>
    /// Дополнительные места для показа пользователю.
    /// </summary>
    public static TFileDialog CustomPlaces<TFileDialog>
        (
            this TFileDialog dialog,
            params string[] places
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);
        Sure.NotNull (places);

        foreach (var place in places)
        {
            if (!string.IsNullOrEmpty (place))
            {
                dialog.CustomPlaces.Add (place);
            }
        }

        return dialog;
    }

    /// <summary>
    /// Дополнительные места для показа пользователю.
    /// </summary>
    public static TFileDialog CustomPlaces<TFileDialog>
        (
            this TFileDialog dialog,
            params Guid[] places
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);

        foreach (var place in places)
        {
            dialog.CustomPlaces.Add (place);
        }

        return dialog;
    }

    /// <summary>
    /// Дополнительные места для показа пользователю.
    /// </summary>
    public static TFileDialog CustomPlaces<TFileDialog>
        (
            this TFileDialog dialog,
            params FileDialogCustomPlace[] places
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);
        Sure.NotNull (places);

        foreach (var place in places)
        {
            dialog.CustomPlaces.Add(place);
        }

        return dialog;
    }

    /// <summary>
    /// Задание расширения по умолчанию.
    /// </summary>
    public static TFileDialog DefaultExt<TFileDialog>
        (
            this TFileDialog dialog,
            string defaultExt
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);
        Sure.NotNullNorEmpty (defaultExt);

        dialog.DefaultExt = defaultExt;

        return dialog;
    }

    /// <summary>
    /// Установка выбранного файла.
    /// </summary>
    public static TFileDialog FileName<TFileDialog>
        (
            this TFileDialog dialog,
            string fileName
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);
        Sure.NotNullNorEmpty (fileName);

        dialog.FileName = fileName;

        return dialog;
    }

    /// <summary>
    /// Задание фильтра для диалога.
    /// </summary>
    public static TFileDialog Filter<TFileDialog>
        (
            this TFileDialog dialog,
            string filter
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);
        Sure.NotNullNorEmpty (filter);

        dialog.Filter = filter;

        return dialog;
    }

    /// <summary>
    /// Задание начальной директории для диалога.
    /// </summary>
    public static TFileDialog InitialDirectory<TFileDialog>
        (
            this TFileDialog dialog,
            string initialPath
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);

        if (!string.IsNullOrEmpty (initialPath))
        {
            dialog.InitialDirectory = initialPath;
        }

        return dialog;
    }

    /// <summary>
    /// Добавление обработчика события выбора файла пользователем.
    /// </summary>
    public static TFileDialog OnFileOk<TFileDialog>
        (
            this TFileDialog dialog,
            CancelEventHandler handler
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);

        dialog.FileOk += handler;

        return dialog;
    }

    /// <summary>
    /// Восстанавливать директорию после демонстрации диалога?
    /// </summary>
    /// <param name="dialog"></param>
    /// <param name="restore"></param>
    /// <typeparam name="TFileDialog"></typeparam>
    /// <returns></returns>
    public static TFileDialog RestoreDirectory<TFileDialog>
        (
            this TFileDialog dialog,
            bool restore = true
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);

        dialog.RestoreDirectory = restore;

        return dialog;
    }

    /// <summary>
    /// Включение подсказки.
    /// </summary>
    public static TFileDialog ShowHelp<TFileDialog>
        (
            this TFileDialog dialog,
            bool showHelp = true
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);

        dialog.ShowHelp = showHelp;

        return dialog;
    }

    /// <summary>
    /// Включение поддержки расширений с несколькими точками.
    /// </summary>
    public static TFileDialog SupportMultiDottedExtensions<TFileDialog>
        (
            this TFileDialog dialog,
            bool multiDot = true
        )
        where TFileDialog : FileDialog
    {
        Sure.NotNull (dialog);

        dialog.SupportMultiDottedExtensions = multiDot;

        return dialog;
    }

    /// <summary>
    /// Задание заголовка для окна диалога.
    /// </summary>
    public static TFileDialog Title<TFileDialog>
        (
            this TFileDialog dialog,
            string title
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);
        Sure.NotNullNorEmpty (title);

        dialog.Title = title;

        return dialog;
    }

    /// <summary>
    /// Валидировать имена?
    /// </summary>
    public static TFileDialog ValidateNames<TFileDialog>
        (
            this TFileDialog dialog,
            bool validate = true
        )
        where TFileDialog: FileDialog
    {
        Sure.NotNull (dialog);

        dialog.ValidateNames = validate;

        return dialog;
    }

    #endregion
}
