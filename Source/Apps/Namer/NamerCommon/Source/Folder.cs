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
    [Reactive]
    public NamePair[]? Files { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public Folder()
    {
        DirectoryName = Directory.GetCurrentDirectory();
        Files = Array.Empty<NamePair>();
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
        Files = files.ToArray();
    }

    #endregion

    #region Private members

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
    /// Отметка всех файлов.
    /// </summary>
    public void CheckAll()
    {
        if (Files is not null)
        {
            foreach (var item in Files)
            {
                item.IsChecked = true;
            }
        }
    }

    /// <summary>
    /// Снятие отметок со всех файлов.
    /// </summary>
    public void CheckNone()
    {
        if (Files is not null)
        {
            foreach (var item in Files)
            {
                item.IsChecked = false;
            }
        }
    }

    /// <summary>
    /// Изменение отметки на противоположную.
    /// </summary>
    public void CheckReverse()
    {
        if (Files is not null)
        {
            foreach (var item in Files)
            {
                item.IsChecked = !item.IsChecked;
            }
        }
    }
    
    /// <summary>
    /// Проверка имен.
    /// </summary>
    public bool CheckNames()
    {
        if (Files is null
            || string.IsNullOrEmpty (DirectoryName))
        {
            return false;
        }

        var result = true;
        foreach (var pair in Files)
        {
            pair.ErrorMessage = null;
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
                    static (x, p) => x.Old != p.Old && (x.New == p.New || x.Old == x.New)
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
        Files = Files?.Where (item => !item.IsChecked).ToArray();
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

        if (Files is null
            || string.IsNullOrEmpty (DirectoryName))
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

            if (!action (this, pair, argument))
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

        if (Files is not null)
        {
            foreach (var file in Files)
            {
                if (errorsOnly && string.IsNullOrEmpty (file.ErrorMessage))
                {
                    continue;
                }

                AnsiConsole.Write ("  ");
                file.Render();
            }
        }
        
        AnsiConsole.WriteLine();
    }

    #endregion
}
