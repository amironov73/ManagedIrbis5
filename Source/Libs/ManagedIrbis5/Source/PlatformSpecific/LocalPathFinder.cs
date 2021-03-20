﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

/* LocalPathFinder.cs -- 
 * Ars Magna project, http://arsmagna.ru
 * -------------------------------------------------------
 * Status: poor
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

using CodeJam;

using JetBrains.Annotations;

using MoonSharp.Interpreter;

#endregion

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    [MoonSharpUserData]
    [ExcludeFromCodeCoverage]
    public class LocalPathFinder
        : PathFinder
    {
        #region Properties

        /// <summary>
        /// Extensions.
        /// </summary>
        [NotNull]
        public string[] Extensions
        {
            get { return _extensions; }
            set
            {
                Code.NotNull(value, "value");

                _extensions = value;
            }
        }

        /// <summary>
        /// Separator.
        /// </summary>
        [NotNull]
        public string[] Separators
        {
            get { return _separators; }
            set
            {
                Code.NotNull(value, "value");

                _separators = value;
            }
        }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalPathFinder()
        {
            _separators = new [] {";"};
            _extensions = new[] {".dll", ".exe"};
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public LocalPathFinder
            (
                [NotNull] string[] separators,
                [NotNull] string[] extensions
            )
        {
            Code.NotNull(separators, "separators");
            Code.NotNull(extensions, "extensions");

            _separators = separators;
            _extensions = extensions;
        }

        #endregion

        #region Private members

        private string[] _separators;

        private string[] _extensions;

        #endregion

        #region PathFinder members

        /// <inheritdoc cref="PathFinder.FindFile" />
        public override string FindFile
            (
                string path,
                string fileName
            )
        {
            Code.NotNull(path, "path");
            Code.NotNull(fileName, "fileName");

            string extension = Path.GetExtension(path);
            bool haveExtension = !string.IsNullOrEmpty(extension);

#if PocketPC || WINMOBILE

            string[] elements = AM.StringUtility.SplitString
                (
                    path,
                    Separators
                );

#else

            string[] elements = path.Split
                (
                    Separators,
                    StringSplitOptions.RemoveEmptyEntries
                );

#endif

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
