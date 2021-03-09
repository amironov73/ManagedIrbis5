// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* TreeUtility.cs -- работа с TRE-файлами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;

#endregion

#nullable enable

namespace ManagedIrbis.Trees
{
    /// <summary>
    /// Работа с TRE-файлами.
    /// </summary>
    public static class TreeUtility
    {
        #region Public methods

        /// <summary>
        /// Read local file.
        /// </summary>
        public static TreeFile ReadLocalFile
            (
                string fileName,
                Encoding? encoding = default
            )
        {
            encoding ??= IrbisEncoding.Ansi;

            using var reader = new StreamReader(fileName, encoding);
            var result = TreeFile.ParseStream(reader);
            result.FileName = Path.GetFileName(fileName);

            return result;
        } // method ReadLocalFile

        /// <summary>
        /// Save to local file.
        /// </summary>
        public static void SaveToLocalFile
            (
                this TreeFile file,
                string fileName,
                Encoding? encoding = default
            )
        {
            encoding ??= IrbisEncoding.Ansi;

            using var writer = new StreamWriter(fileName, false, encoding);
            file.Save(writer);
        } // method SaveToLocalFile

        /// <summary>
        /// Convert to array of menu items.
        /// </summary>
        public static MenuEntry[] ToMenu
            (
                this TreeLine line
            )
        {
            var result = new List<MenuEntry>
            {
                new MenuEntry
                {
                    Code = line.Prefix,
                    Comment = line.Suffix
                }
            };

            foreach (var child in line.Children)
            {
                result.AddRange(child.ToMenu());
            }

            return result.ToArray();
        } // method ToMenu

        /// <summary>
        /// Convert tree to menu.
        /// </summary>
        public static MenuFile ToMenu
            (
                this TreeFile file
            )
        {
            var result = new MenuFile();

            foreach (var root in file.Roots)
            {
                result.Entries.AddRange(root.ToMenu());
            }

            return result;
        } // method ToMenu

        /// <summary>
        /// Walk over the tree.
        /// </summary>
        public static void Walk
            (
                this TreeLine line,
                Action<TreeLine> action
            )
        {
            action(line);
            foreach (var child in line.Children)
            {
                child.Walk(action);
            }
        } // method Walk

        /// <summary>
        /// Walk over the tree.
        /// </summary>
        public static void Walk
            (
                this TreeFile file,
                Action<TreeLine> action
            )
        {
            foreach (var child in file.Roots)
            {
                child.Walk(action);
            }
        } // method Walk

        #endregion

    } // class TreeUtility

} // namespace ManagedIrbis.Trees
