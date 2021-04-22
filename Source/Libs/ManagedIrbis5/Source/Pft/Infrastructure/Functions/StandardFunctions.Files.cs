// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StandardFunctions.Files.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    static partial class StandardFunctions
    {
        #region Private members

        private static readonly Dictionary<string, FileObject> _files
            = new (StringComparer.CurrentCultureIgnoreCase);

        //================================================================
        // INTERNAL METHODS
        //================================================================

        internal static void CloseFile
            (
                string fileName
            )
        {
            var fullPath = GetFullPath(fileName);

            if (_files.TryGetValue(fullPath, out var file))
            {
                file.Dispose();
                _files.Remove(fullPath);
            }
        }

        internal static string GetFullPath
            (
                string fileName
            )
        {
            var result = Path.GetFullPath(fileName);

            return result;
        }

        internal static bool HaveOpenFile
            (
                string? fileName
            )
        {
            if (string.IsNullOrEmpty(fileName))
            {
                return false;
            }

            var result = _files.ContainsKey(fileName);

            return result;
        }

        private static string OpenInternal
            (
                string fileName,
                bool write,
                bool append
            )
        {
            var fullPath = GetFullPath(fileName);
            if (HaveOpenFile(fileName))
            {
                return string.Empty;
            }

            var file = new FileObject(fullPath, write, append);
            _files.Add(fullPath, file);

            return fullPath;
        }

        internal static string OpenAppend
            (
                string fileName
            )
        {
            var result = OpenInternal(fileName, true, true);

            return result;
        }

        internal static string OpenRead
            (
                string fileName
            )
        {
            var result = OpenInternal(fileName, false, false);

            return result;
        }

        internal static string OpenWrite
            (
                string fileName
            )
        {
            var result = OpenInternal(fileName, true, false);

            return result;
        }

        internal static string ReadAll
            (
                string fileName
            )
        {
            var fullPath = GetFullPath(fileName);
            if (_files.TryGetValue(fullPath, out var file))
            {
                var result = file.ReadAll();

                return result;
            }

            return string.Empty;
        }

        internal static string ReadLine
            (
                string fileName
            )
        {
            var fullPath = GetFullPath(fileName);
            if (_files.TryGetValue(fullPath, out var file))
            {
                var result = file.ReadLine();

                return result ?? string.Empty;
            }

            return string.Empty;
        }

        internal static void Write
            (
                string fileName,
                string text
            )
        {
            var fullPath = GetFullPath(fileName);
            if (_files.TryGetValue(fullPath, out var file))
            {
                file.Write(text);
            }
        }

        internal static void WriteLine
            (
                string fileName,
                string text
            )
        {
            var fullPath = GetFullPath(fileName);
            if (_files.TryGetValue(fullPath, out var file))
            {
                file.WriteLine(text);
            }
        }

        //================================================================
        // FILE ORIENTED FUNCTIONS
        //================================================================

        private static void Close(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                CloseFile(fileName);
            }
        }

        private static void IsOpen(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            var output = HaveOpenFile(fileName)
                ? "1"
                : "0";
            context.Write(node, output);
        }

        private static void OpenAppend(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                var output = OpenAppend(fileName);
                context.Write(node, output);
            }
        }

        private static void OpenRead(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                var output = OpenRead(fileName);
                context.Write(node, output);
            }
        }

        private static void OpenWrite(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                var output = OpenWrite(fileName);
                context.Write(node, output);
            }
        }

        private static void ReadAll(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                var output = ReadAll(fileName);
                context.Write(node, output);
            }
        }

        private static void ReadLine(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                var output = ReadLine(fileName);
                context.Write(node, output);
            }
        }

        private static void Write(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            var text = context.GetStringArgument(arguments, 1);
            if (!string.IsNullOrEmpty(fileName)
                && !ReferenceEquals(text, null))
            {
                Write(fileName, text);
            }
        }

        private static void WriteLine(PftContext context, PftNode node, PftNode[] arguments)
        {
            var fileName = context.GetStringArgument(arguments, 0);
            var text = context.GetStringArgument(arguments, 1);
            if (!string.IsNullOrEmpty(fileName)
                && !ReferenceEquals(text, null))
            {
                WriteLine(fileName, text);
            }
        }

        #endregion
    }
}
