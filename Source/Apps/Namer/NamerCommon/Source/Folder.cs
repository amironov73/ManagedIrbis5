// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable LocalizableElement

/* Folder.cs -- папка с файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;

using AM;
using AM.Collections;
using AM.IO;

using JetBrains.Annotations;

using ReactiveUI;
using ReactiveUI.Fody.Helpers;

using Spectre.Console;

#endregion

#nullable enable

namespace NamerCommon;

/// <summary>
/// Папка с файлами.
/// </summary>
[PublicAPI]
public class Folder
    : ReactiveObject
{
    #region Properties

    /// <summary>
    /// Имя директории.
    /// </summary>
    [Reactive]
    public string? DirectoryName { get; set; }

    /// <summary>
    /// Файлы.
    /// </summary>
    public ItemPropertyTrackingCollection<NamePair> Files { get; }

    /// <summary>
    /// Количество помеченных.
    /// </summary>
    [Reactive]
    public int CheckedCount { get; set; }

    /// <summary>
    /// Количество ошибок.
    /// </summary>
    [Reactive]
    public int ErrorCount { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Folder()
    {
        DirectoryName = string.Empty;
        Files = new();
        CreateProperties();
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public Folder
        (
            string directoryName,
            IEnumerable<NamePair> files
        )
    {
        Sure.DirectoryExists (directoryName);
        Sure.NotNull (files);

        DirectoryName = directoryName;
        Files = new (files);
        CreateProperties();
    }

    #endregion

    #region Private members

    private void CountElements()
    {
        ErrorCount = Files.Count (x => x.HasError);
        CheckedCount = Files.Count (x => x.IsChecked);
    }

    private void CreateProperties()
    {
        Files.CollectionChanged += (_, _) => CountElements();
        Files.ItemPropertyChanged += (_, _) => CountElements();
    }

    private static bool RenameImpl<T>
        (
            Folder folder,
            NamePair pair,
            T? arg
        )
    {
        var oldName = Path.Combine (folder.DirectoryName!, pair.Old);
        var newName = Path.Combine (folder.DirectoryName!, pair.New);

        return FileUtility.TryMove (oldName, newName);
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Присваивание перечня файлов.
    /// </summary>
    public void Assign
        (
            IEnumerable<NamePair> files
        )
    {
        Sure.NotNull (files);

        Files.Clear();
        foreach (var file in files)
        {
            Files.Add (file);
        }
    }

    /// <summary>
    /// Отметка всех файлов.
    /// </summary>
    public void CheckAll()
    {
        foreach (var item in Files)
        {
            item.IsChecked = true;
        }
    }

    /// <summary>
    /// Снятие отметок со всех файлов.
    /// </summary>
    public void CheckNone()
    {
        foreach (var item in Files)
        {
            item.IsChecked = false;
        }
    }

    /// <summary>
    /// Изменение отметки на противоположную.
    /// </summary>
    public void CheckReverse()
    {
        foreach (var item in Files)
        {
            item.IsChecked = !item.IsChecked;
        }
    }

    /// <summary>
    /// Проверка имен.
    /// </summary>
    public bool CheckNames()
    {
        if (string.IsNullOrEmpty (DirectoryName))
        {
            return false;
        }

        var result = true;
        foreach (var pair in Files)
        {
            pair.ErrorMessage = null;
            if (!pair.IsChecked)
            {
                // неотмеченные пары не берем в расчет
                continue;
            }

            if (pair.IsSame)
            {
                // переименования не требуется
                continue;
            }

            if (!pair.ValidateNewName())
            {
                pair.ErrorMessage = "Invalid name";
                continue;
            }

            var any = Files.Any
                (
                    pair,
                    static (x, p) => x.IsChecked
                        && x.Old != p.Old && (x.New == p.New || x.Old == x.New)
                );
            if (any)
            {
                pair.ErrorMessage = "Duplicate name";
                result = false;
                continue;
            }

            var newPath = Path.Combine (DirectoryName, pair.New);
            if (File.Exists (newPath) || Directory.Exists (newPath))
            {
                pair.ErrorMessage = "Already exist";
                result = false;
            }
        }

        return result;
    }

    /// <summary>
    /// Удаление из списка отмеченных элементов.
    /// </summary>
    public void ClearChecked()
    {
        var notChecked = Files.Where (x => !x.IsChecked).ToArray();
        Assign (notChecked);
    }

    /// <summary>
    /// Переименование.
    /// </summary>
    public bool Rename() => Rename<object> (RenameImpl, null);

    /// <summary>
    /// Переименование.
    /// </summary>
    public bool Rename<T>
        (
            Func<Folder, NamePair, T?, bool> action,
            T? argument
        )
    {
        if (!CheckNames())
        {
            return false;
        }

        if (string.IsNullOrEmpty (DirectoryName))
        {
            return false;
        }

        var result = true;
            foreach (var pair in Files)
            {
                if (!pair.IsChecked || pair.HasError)
                {
                    // пропускаем пары с ошибками и без отметок
                    continue;
                }

                if (pair.IsSame)
                {
                    // переименования не требуется
                    continue;
                }

                try
                {
                    if (!action (this, pair, argument))
                    {
                        result = false;
                        break;
                    }
                }
                catch (Exception exception)
                {
                    Debug.WriteLine (exception);
                    result = false;
                    break;
                }
            }

        return result;
    }

    /// <summary>
    /// Переименование.
    /// </summary>
    public async Task<bool> RenameAsync
        (
            Func<Folder, NamePair, double, Task<bool>> action
        )
    {
        if (!CheckNames())
        {
            return false;
        }

        if (string.IsNullOrEmpty (DirectoryName))
        {
            return false;
        }

        var result = true;
        var counter = 0;
        foreach (var pair in Files)
        {
            counter++;
            if (!pair.IsChecked || pair.HasError)
            {
                // пропускаем пары с ошибками и без отметок
                continue;
            }

            if (pair.IsSame)
            {
                // переименования не требуется
                continue;
            }

            var percentage = 100.0 * counter / Files.Count;
            if (!await action (this, pair, percentage))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// Вывод на консоль.
    /// </summary>
    public void Render
        (
            bool errorsOnly = false
        )
    {
        if (DirectoryName is not null)
        {
            AnsiConsole.Write (new Text (DirectoryName, new Style (Color.Aqua)));
            AnsiConsole.WriteLine();
        }

        foreach (var file in Files)
        {
            if (errorsOnly && string.IsNullOrEmpty (file.ErrorMessage))
            {
                continue;
            }

            AnsiConsole.Write ("  ");
            file.Render();
        }

        AnsiConsole.WriteLine();
    }

    #endregion
}
