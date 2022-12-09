// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TreeUtility.cs -- работа с TRE-файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Trees;

/// <summary>
/// Работа с TRE-файлами.
/// </summary>
public static class TreeUtility
{
    #region Public methods

    /// <summary>
    /// Чтение локального TRE-файла.
    /// </summary>
    public static TreeFile ReadLocalFile
        (
            string fileName,
            Encoding? encoding = default
        )
    {
        Sure.FileExists (fileName);

        encoding ??= IrbisEncoding.Ansi;

        using var reader = new StreamReader (fileName, encoding);
        var result = TreeFile.ParseStream (reader);
        result.FileName = Path.GetFileName (fileName);

        return result;
    }

    /// <summary>
    /// Сохранение дерева в локальный TRE-файл.
    /// </summary>
    public static void SaveToLocalFile
        (
            this TreeFile tree,
            string fileName,
            Encoding? encoding = default
        )
    {
        Sure.NotNull (tree);
        Sure.NotNullNorEmpty (fileName);

        encoding ??= IrbisEncoding.Ansi;

        using var writer = new StreamWriter (fileName, false, encoding);
        tree.Save (writer);
    }

    /// <summary>
    /// Преобразование узла с подузлами в массив элементов меню.
    /// </summary>
    public static MenuEntry[] ToMenu
        (
            this TreeLine line
        )
    {
        Sure.NotNull (line);

        var result = new List<MenuEntry>
        {
            new()
            {
                Code = line.Prefix,
                Comment = line.Suffix
            }
        };

        foreach (var child in line.Children)
        {
            result.AddRange (child.ToMenu());
        }

        return result.ToArray();
    }

    /// <summary>
    /// Преобразование TRE- в MNU-файл.
    /// </summary>
    public static MenuFile ToMenu
        (
            this TreeFile file
        )
    {
        Sure.NotNull (file);

        var result = new MenuFile();

        foreach (var root in file.Roots)
        {
            result.Entries.AddRange (root.ToMenu());
        }

        return result;
    }

    /// <summary>
    /// Проход по дереву, начиная с указанного узла.
    /// </summary>
    public static void Walk
        (
            this TreeLine line,
            Action<TreeLine> action
        )
    {
        Sure.NotNull (line);
        Sure.NotNull (action);

        action (line);
        foreach (var child in line.Children)
        {
            child.Walk (action);
        }
    }

    /// <summary>
    /// Проход по дереву, начиная с корня.
    /// </summary>
    public static void Walk
        (
            this TreeFile file,
            Action<TreeLine> action
        )
    {
        Sure.NotNull (file);
        Sure.NotNull (action);

        foreach (var child in file.Roots)
        {
            child.Walk (action);
        }
    }

    #endregion
}
