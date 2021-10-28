// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* FileRenamer.cs -- переименовыватель файлов для исправления косяков в нумерации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

using AM.Text.Output;

#endregion

#nullable enable

namespace AM.IO
{
    /// <summary>
    /// Переименовыватель файлов для исправления косяков в нумерации.
    /// </summary>
    public sealed class FileRenumber
    {
        #region Nested classes

        /// <summary>
        /// Пара имен: старое и новое.
        /// </summary>
        public record Bunch (string OldName, string NewName)
        {
            private string FullNewName
            {
                get
                {
                    var result = NewName;
                    var directory = Path.GetDirectoryName (OldName);
                    if (directory is not null)
                    {
                        result = Path.Combine (directory, result);
                    }

                    var extension = Path.GetExtension (OldName);
                    if (!string.IsNullOrEmpty (extension))
                    {
                        result += extension;
                    }

                    return result;
                }

            } // method GetFullNewName

            /// <summary>
            /// Проверка возможности переименования.
            /// </summary>
            public bool Check
                (
                    AbstractOutput? output = null
                )
            {
                var newName = FullNewName;

                if (!File.Exists (OldName))
                {
                    output?.WriteLine ($"File {OldName} doesn't exist");

                    return false;
                }

                if (File.Exists (newName) || Directory.Exists (newName))
                {
                    output?.WriteLine ($"File {newName} already exists");

                    return false;
                }

                return true;

            } // method Check

            /// <summary>
            /// Переименование файла.
            /// </summary>
            public void Rename
                (
                    AbstractOutput? output = null
                )
            {
                var fullName = FullNewName;

                output?.WriteLine ($"{OldName} -> {fullName}");
                File.Move (OldName, fullName);

            } // method Rename

        } // record Bunch

        #endregion

        #region Properties

        /// <summary>
        /// Номер цифровой группы, подлежащей перенумерации.
        /// Нумерация с нуля.
        /// </summary>
        public int GroupNumber { get; set; }

        /// <summary>
        /// Ширина (для дополнения нулями слева).
        /// Ноль означает автоматическое определение ширины.
        /// </summary>
        public int GroupWidth { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Генерация списка для переименования.
        /// </summary>
        public List<Bunch> GenerateNames
            (
                IList<string> existingFiles
            )
        {
            var regex = new Regex (@"\d+");
            var result = new List<Bunch>();
            var maxValue = 0;
            var sourceFiles = new List<string>();

            foreach (var fullName in existingFiles)
            {
                var fileName = Path.GetFileNameWithoutExtension (fullName);
                if (string.IsNullOrEmpty (fileName))
                {
                    // это очень странный файл, не будем с ним связываться
                    continue;
                }

                var group = regex.Matches (fileName)!.SafeAt (GroupNumber)?.Value;
                if (string.IsNullOrEmpty (group))
                {
                    // в имени файла нет цифр, пропускаем
                    continue;
                }

                var value = group.SafeToInt32 (-1);
                if (value < 0)
                {
                    // странно, но это не удалось сконвертировать в число
                    continue;
                }

                if (maxValue < value)
                {
                    maxValue = value;
                }

                sourceFiles.Add (fullName);

            } // foreach

            var pattern = "0";
            while (maxValue >= 10)
            {
                pattern += "0";
                maxValue /= 10;
            }

            if (GroupWidth > 1)
            {
                pattern = new string ('0', GroupWidth);
            }

            if (pattern.Length == 1)
            {
                // нет смысла переименовывать файлы, им не нужны ведущие нули
                return result;
            }

            foreach (var sourceFile in sourceFiles)
            {
                var oldName = Path.GetFileNameWithoutExtension (sourceFile);
                var match = regex.Matches (oldName)!.SafeAt (GroupNumber)!;
                var value = int.Parse  (match.Value, CultureInfo.InvariantCulture);
                var newName = oldName.Substring (0, match.Index)
                    + value.ToString (pattern, CultureInfo.InvariantCulture)
                    + oldName.Substring (match.Index + match.Length);
                if (string.CompareOrdinal (oldName, newName) != 0)
                {
                    var bunch = new Bunch (sourceFile, newName);
                    result.Add (bunch);
                }
            }

            return result;

        } // method GenerateNames

        /// <summary>
        /// Проверка имен.
        /// </summary>
        public bool CheckNames
            (
                IEnumerable<Bunch> fileList,
                AbstractOutput? output = null
            )
        {
            foreach (var bunch in fileList)
            {
                if (!bunch.Check (output))
                {
                    return false;
                }
            }

            return true;

        } // method CheckNames

        /// <summary>
        /// Переименование файлов.
        /// </summary>
        public void Rename
            (
                IEnumerable<Bunch> fileList,
                AbstractOutput? output = null
            )
        {
            foreach (var bunch in fileList)
            {
                bunch.Rename (output);
            }

        } // method Rename

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString() => $"Number: {GroupNumber}, Width: {GroupWidth}";

        #endregion

    } // class FileRenames

} // namespace AM.IO
