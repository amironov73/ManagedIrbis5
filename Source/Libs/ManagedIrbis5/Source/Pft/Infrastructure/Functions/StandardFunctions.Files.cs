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

        private static readonly Dictionary<string, FileObject> Files
            = new Dictionary<string, FileObject>
                (
                    StringComparer.CurrentCultureIgnoreCase
                );

        //================================================================
        // INTERNAL METHODS
        //================================================================

        internal static void CloseFile
            (
                string fileName
            )
        {
            string fullPath = GetFullPath(fileName);

            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
            {
                file.Dispose();
                Files.Remove(fullPath);
            }
        }

        internal static string GetFullPath
            (
                string fileName
            )
        {
            string result = Path.GetFullPath(fileName);

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

            bool result = Files.ContainsKey(fileName);

            return result;
        }

        private static string OpenInternal
            (
                string fileName,
                bool write,
                bool append
            )
        {
            string fullPath = GetFullPath(fileName);
            if (HaveOpenFile(fileName))
            {
                return string.Empty;
            }

            FileObject file = new FileObject(fullPath, write, append);
            Files.Add(fullPath, file);

            return fullPath;
        }

        internal static string OpenAppend
            (
                string fileName
            )
        {
            string result = OpenInternal(fileName, true, true);

            return result;
        }

        internal static string OpenRead
            (
                string fileName
            )
        {
            string result = OpenInternal(fileName, false, false);

            return result;
        }

        internal static string OpenWrite
            (
                string fileName
            )
        {
            string result = OpenInternal(fileName, true, false);

            return result;
        }

        internal static string ReadAll
            (
                string fileName
            )
        {
            string fullPath = GetFullPath(fileName);
            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
            {
                string result = file.ReadAll();

                return result;
            }

            return string.Empty;
        }

        internal static string ReadLine
            (
                string fileName
            )
        {
            string fullPath = GetFullPath(fileName);
            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
            {
                string result = file.ReadLine();

                return result;
            }

            return string.Empty;
        }

        internal static void Write
            (
                string fileName,
                string text
            )
        {
            string fullPath = GetFullPath(fileName);
            FileObject file;
            if (Files.TryGetValue(fullPath, out file))
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
            string fullPath = GetFullPath(fileName);
            if (Files.TryGetValue(fullPath, out FileObject? file))
            {
                file.WriteLine(text);
            }
        }

        //================================================================
        // FILE ORIENTED FUNCTIONS
        //================================================================

        private static void Close(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                CloseFile(fileName);
            }
        }

        private static void IsOpen(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            string output = HaveOpenFile(fileName)
                ? "1"
                : "0";
            context.Write(node, output);
        }

        private static void OpenAppend(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = OpenAppend(fileName);
                context.Write(node, output);
            }
        }

        private static void OpenRead(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = OpenRead(fileName);
                context.Write(node, output);
            }
        }

        private static void OpenWrite(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = OpenWrite(fileName);
                context.Write(node, output);
            }
        }

        private static void ReadAll(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = ReadAll(fileName);
                context.Write(node, output);
            }
        }

        private static void ReadLine(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            if (!string.IsNullOrEmpty(fileName))
            {
                string output = ReadLine(fileName);
                context.Write(node, output);
            }
        }

        private static void Write(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            string text = context.GetStringArgument(arguments, 1);
            if (!string.IsNullOrEmpty(fileName)
                && !ReferenceEquals(text, null))
            {
                Write(fileName, text);
            }
        }

        private static void WriteLine(PftContext context, PftNode node, PftNode[] arguments)
        {
            string fileName = context.GetStringArgument(arguments, 0);
            string text = context.GetStringArgument(arguments, 1);
            if (!string.IsNullOrEmpty(fileName)
                && !ReferenceEquals(text, null))
            {
                WriteLine(fileName, text);
            }
        }

        #endregion
    }
}
