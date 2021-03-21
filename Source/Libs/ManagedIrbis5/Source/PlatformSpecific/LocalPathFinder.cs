// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* LocalPathFinder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

#endregion

#nullable enable

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    ///
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class LocalPathFinder
        : PathFinder
    {
        #region Properties

        /// <summary>
        /// Extensions.
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// Separator.
        /// </summary>
        public string[] Separators { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalPathFinder()
        {
            Separators = new [] {";"};
            Extensions = new[] {".dll", ".exe"};
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalPathFinder
            (
                string[] separators,
                string[] extensions
            )
        {
            Separators = separators;
            Extensions = extensions;
        }

        #endregion

        #region Private members

        #endregion

        #region PathFinder members

        /// <inheritdoc cref="PathFinder.FindFile" />
        public override string? FindFile
            (
                string path,
                string fileName
            )
        {
            var extension = Path.GetExtension(path);
            bool haveExtension = !string.IsNullOrEmpty(extension);

            string[] elements = path.Split
                (
                    Separators,
                    StringSplitOptions.RemoveEmptyEntries
                );

            foreach (string element in elements)
            {
                if (haveExtension)
                {
                    string fullPath = Path.Combine
                        (
                            element,
                            fileName
                        );
                    if (File.Exists(fullPath))
                    {
                        return fullPath;
                    }
                }
                else
                {
                    foreach (string ext in Extensions)
                    {
                        string fullPath = Path.Combine
                            (
                                element,
                                fileName + ext
                            );
                        if (File.Exists(fullPath))
                        {
                            return fullPath;
                        }
                    }
                }
            }

            return null;
        }

        #endregion
    }
}
