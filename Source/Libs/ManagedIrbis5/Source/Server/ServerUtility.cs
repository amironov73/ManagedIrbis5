// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* ServerUtility.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    ///
    /// </summary>
    public static class ServerUtility
    {
        #region Constants

        /// <summary>
        /// Inclusion begin sign.
        /// </summary>
        public const char InclusionStart = '\x1C';

        /// <summary>
        /// Inclusion end sign.
        /// </summary>
        public const char InclusionEnd = '\x1D';

        #endregion

        #region Public methods

        /// <summary>
        /// Expand inclusion.
        /// </summary>
        public static string ExpandInclusion
            (
                string text,
                string extension,
                string[] pathArray
            )
        {
            Sure.NotNull(text, nameof(text));
            Sure.NotNull(extension, nameof(extension));
            Sure.NotNull(pathArray, nameof(pathArray));

            if (!text.Contains(InclusionStart))
            {
                return text;
            }

            if (pathArray.Length == 0)
            {
                throw new IrbisException();
            }

            StringBuilder result = new StringBuilder(text.Length * 2);
            TextNavigator navigator = new TextNavigator(text);

            string fileName = navigator.ReadUntil(InclusionEnd).ToString();
            while (!navigator.IsEOF)
            {
                var prefix = navigator.ReadUntil(InclusionStart).ToString();
                result.Append(prefix);
                if (navigator.ReadChar() != InclusionStart)
                {
                    break;
                }

                if (ReferenceEquals(fileName, null)
                    || fileName.Length == 0
                    || navigator.ReadChar() != InclusionEnd)
                {
                    break;
                }

                string fullPath = FindFileOnPath
                    (
                        fileName,
                        extension,
                        pathArray
                    );
                string fileContent = File.ReadAllText
                    (
                        fullPath,
                        IrbisEncoding.Ansi
                    );
                fileContent = ExpandInclusion
                    (
                        fileContent,
                        extension,
                        pathArray
                    );
                result.Append(fileContent);
            }

            string remaining = navigator.GetRemainingText().ToString();
            result.Append(remaining);

            return result.ToString();
        }

        /// <summary>
        /// Find file on path.
        /// </summary>
        public static string FindFileOnPath
            (
                string fileName,
                string extension,
                string[] pathArray
            )
        {
            Sure.NotNullNorEmpty(fileName, nameof(fileName));
            Sure.NotNull(extension, nameof(extension));
            Sure.NotNull(pathArray, nameof(pathArray));

            if (!fileName.Contains('.'))
            {
                if (!extension.StartsWith("."))
                {
                    fileName += '.';
                }
                fileName += extension;
            }

            foreach (string path in pathArray)
            {
                string fullPath = Path.Combine(path, fileName);
                if (File.Exists(fullPath))
                {
                    return fullPath;
                }
            }

            throw new IrbisException();
        }

        #endregion

    } // class ServerUtility

} // namespace ManagedIrbis.Server
