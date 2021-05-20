// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* Unix.cs -- возня вокруг регистрозависимости имен файлов в системе UNIX
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using AM.Text;

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
        #region Private members

        private static string? _Correct
            (
                string parent,
                string child
            )
        {
            var combined = Path.Combine(parent, child);
            if (Directory.Exists(combined))
            {
                return child;
            }

            var options = new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive
            };
            foreach (var candidate in
                Directory.EnumerateDirectories(parent, child, options))
            {
                if (candidate.SameString(child))
                {
                    return candidate;
                }
            }

            return null;
        }

        private static string? _FixDirectory
            (
                string directory
            )
        {
            if (Directory.Exists(directory))
            {
                return directory;
            }

            directory = directory.ConvertSlashes();
            var rooted = Path.IsPathRooted(directory);
            if (rooted)
            {
                directory = directory.Substring(1);
            }

            var trailed = Path.EndsInDirectorySeparator(directory);
            if (trailed)
            {
                directory = directory.Substring(0, directory.Length - 1);
            }

            var parts = directory.Split(Path.DirectorySeparatorChar);
            var builder = StringBuilderPool.Shared.Get();
            if (rooted)
            {
                builder.Append(Path.DirectorySeparatorChar);
            }

            var first = true;
            foreach (var child in parts)
            {
                var parent = builder.ToString();
                var corrected = _Correct(parent, child);
                if (corrected is null)
                {
                    return null;
                }

                if (!first)
                {
                    builder.Append(Path.DirectorySeparatorChar);
                }

                builder.Append(corrected);
                first = false;
            }

            if (trailed)
            {
                builder.Append(Path.DirectorySeparatorChar);
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return(builder);

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Добавление строк в указанный файл.
        /// </summary>
        public static void AppendAllLines
            (
                string path,
                IEnumerable<string> lines,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var found = FindFileOrThrow(path);

            File.AppendAllLines(found, lines, encoding);

        } // method AppendAllLines

        /// <summary>
        /// Добавление текста в указанный файл.
        /// </summary>
        public static void AppendAllText
            (
                string path,
                string text,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var found = FindFileOrThrow(path);

            File.AppendAllText(found, text, encoding);

        } // method AppendAllText

        /// <summary>
        /// Создает <see cref="StreamWriter"/> для добавления текста.
        /// </summary>
        public static StreamWriter AppendText
            (
                string path,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var found = FindDirectoryOrThrow(path);

            return new StreamWriter(found, true, encoding);

        } // method AppendText

        /// <summary>
        /// Удаление файла с указанным именем.
        /// </summary>
        public static void DeleteFile
            (
                string path
            )
        {
            var found = FindFile(path);
            if (found is not null)
            {
                File.Delete(found);
            }

        } // method DeleteFile

        /// <summary>
        /// Директория с похожим именем существует?
        /// </summary>
        public static bool DirectoryExists (string directoryName) =>
            FindDirectory (directoryName) is not null;

        /// <summary>
        /// Файл с похожим именем существует?
        /// </summary>
        public static bool FileExists (string fileName) =>
            FindFile (fileName) is not null;

        /// <summary>
        /// Поиск директории с подходящим именем.
        /// </summary>
        public static string? FindDirectory
            (
                string directoryName
            )
        {
            if (Directory.Exists(directoryName))
            {
                return directoryName;
            }

            if (OperatingSystem.IsWindows())
            {
                return directoryName;
            }

            var parent = Path.GetDirectoryName(directoryName) ?? ".";
            parent = _FixDirectory(parent);
            if (string.IsNullOrEmpty(parent))
            {
                return null;
            }

            var options = new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive
            };
            var child = Path.GetFileName(directoryName);
            foreach (var candidate in
                Directory.EnumerateDirectories(parent, child, options))
            {
                if (candidate.SameString(child))
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
            if (File.Exists(fileName))
            {
                return fileName;
            }

            if (OperatingSystem.IsWindows())
            {
                return fileName;
            }

            var parent = Path.GetDirectoryName(fileName) ?? ".";
            parent = _FixDirectory(parent);
            if (string.IsNullOrEmpty(parent))
            {
                return null;
            }

            var options = new EnumerationOptions
            {
                MatchCasing = MatchCasing.CaseInsensitive
            };
            var child = Path.GetFileName(fileName);
            foreach (var candidate in
                Directory.EnumerateFiles(parent, child, options))
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
        /// Открытие указанного файла.
        /// </summary>
        public static FileStream OpenFile
            (
                string path,
                FileMode mode
            )
        {
            var found = FindFileOrThrow(path);

            return File.Open(found, mode);

        } // method OpenFile

        /// <summary>
        /// Открытие указанного файла.
        /// </summary>
        public static FileStream OpenFile
            (
                string path,
                FileMode mode,
                FileAccess access
            )
        {
            var found = FindFileOrThrow(path);

            return File.Open(found, mode, access);

        } // method OpenFile

        /// <summary>
        /// Открытие указанного файла.
        /// </summary>
        public static FileStream OpenFile
            (
                string path,
                FileMode mode,
                FileAccess access,
                FileShare share
            )
        {
            var found = FindFileOrThrow(path);

            return File.Open(found, mode, access, share);

        } // method OpenFile

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
        /// Открытие текстового файла для чтения.
        /// </summary>
        public static StreamReader OpenText
            (
                string path
            )
        {
            var found = FindFileOrThrow(path);

            return File.OpenText(found);

        } // method OpenText

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

        /// <summary>
        /// Чтение всех данных из указанного файла.
        /// </summary>
        public static byte[] ReadAllBytes
            (
                string path
            )
        {
            var found = FindFileOrThrow(path);

            return File.ReadAllBytes(found);

        } // method ReadAllBytes

        /// <summary>
        /// Чтение всех строк из указанного файла.
        /// </summary>
        public static string[] ReadAllLines
            (
                string path,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var found = FindFileOrThrow(path);

            return File.ReadAllLines(found, encoding);

        } // method ReadAllLines

        /// <summary>
        /// Чтение всего текста из указанного файла.
        /// </summary>
        public static string ReadAllText
            (
                string path,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var found = FindFileOrThrow(path);

            return File.ReadAllText(found, encoding);

        } // method ReadAllText

        /// <summary>
        /// Построчное чтение строк из указанного файла.
        /// </summary>
        public static IEnumerable<string> ReadLines
            (
                string path,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var found = FindDirectoryOrThrow(path);

            return File.ReadLines(found, encoding);

        } // method ReadLines

        /// <summary>
        /// Запись данных в указанный файл.
        /// </summary>
        public static void WriteAllBytes
            (
                string path,
                byte[] bytes
            )
        {
            var found = FindFileOrThrow(path);

            File.WriteAllBytes(found, bytes);

        } // method WriteAllBytes

        /// <summary>
        /// Запись строк в указанный файл.
        /// </summary>
        public static void WriteAllLines
            (
                string path,
                IEnumerable<string> lines,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var found = FindFileOrThrow(path);

            File.WriteAllLines(found, lines, encoding);

        } // method WriteAllLines

        /// <summary>
        /// Запись текста в указанный файл.
        /// </summary>
        public static void WriteAllText
            (
                string path,
                string text,
                Encoding? encoding = default
            )
        {
            encoding ??= Encoding.UTF8;
            var found = FindFileOrThrow(path);

            File.WriteAllText(found, text, encoding);

        } // method WriteAllText

        #endregion


    } // class Unix

} // namespace AM
