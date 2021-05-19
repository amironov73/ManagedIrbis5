// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Unix.cs -- возня вокруг регистрозависимости имен файлов в системе UNIX
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM
{
    /// <summary>
    /// Возня вокру регистрозависимости имен файлов
    /// в системе UNIX.
    /// </summary>
    public static class Unix
    {
        #region Public methods

        /// <summary>
        /// Директория с похожим именем существует?
        /// </summary>
        public static bool DirectoryExist (string directoryName) =>
            FindDirectory (directoryName) is not null;

        /// <summary>
        /// Файл с похожим именем существует?
        /// </summary>
        public static bool FileExist (string fileName) =>
            FindFile (fileName) is not null;

        /// <summary>
        /// Поиск директории с подходящим именем.
        /// </summary>
        public static string? FindDirectory
            (
                string directoryName
            )
        {
            if (OperatingSystem.IsWindows())
            {
                return directoryName;
            }

            if (Directory.Exists(directoryName))
            {
                return directoryName;
            }

            var directory = Path.GetDirectoryName(directoryName) ?? ".";
            foreach (var candidate in Directory.EnumerateDirectories(directory, "*"))
            {
                if (candidate.SameString(directoryName))
                {
                    return candidate;
                }
            }

            return null;

        } // method FindDirectory

        /// <summary>
        /// Поиск директории с подходящим именем.
        /// Если директория не найдена, бросается исключение.
        /// </summary>
        public static string FindDirectoryOrThrow
            (
                string directoryName
            )
        {
            var result = FindDirectory(directoryName);
            if (string.IsNullOrEmpty(result))
            {
                throw new DirectoryNotFoundException(directoryName);
            }

            return result;

        } // method FindDirectoryOrThrow

        /// <summary>
        /// Поиск файла с подходящим именем.
        /// </summary>
        public static string? FindFile
            (
                string fileName
            )
        {
            if (OperatingSystem.IsWindows())
            {
                return fileName;
            }

            if (File.Exists(fileName))
            {
                return fileName;
            }

            var directory = Path.GetDirectoryName(fileName) ?? ".";
            foreach (var candidate in Directory.EnumerateFiles(directory, "*"))
            {
                if (candidate.SameString(fileName))
                {
                    return candidate;
                }
            }

            return null;

        } // method FindFile

        /// <summary>
        /// Поиск файла с подходящим именем.
        /// Если файл не найден, бросается исключение.
        /// </summary>
        public static string FindFileOrThrow
            (
                string fileName
            )
        {
            var result = FindFile(fileName);
            if (string.IsNullOrEmpty(result))
            {
                throw new FileNotFoundException(fileName);
            }

            return result;

        } // method FindFileOrThrow

        /// <summary>
        /// Открытие файла для чтения.
        /// </summary>
        public static FileStream OpenRead
            (
                string fileName
            )
        {
            var found = FindFileOrThrow(fileName);

            return File.OpenRead(found);

        } // method OpenRead

        /// <summary>
        /// Открытие файла для чтения/записи.
        /// </summary>
        public static FileStream OpenWrite
            (
                string fileName
            )
        {
            var found = FindFileOrThrow(fileName);

            return File.OpenWrite(found);

        } // method OpenWrite

        #endregion


    } // class Unix

} // namespace AM
