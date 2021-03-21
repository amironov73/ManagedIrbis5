// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ProgramCache.cs -- simple cache for PFT scripts.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    /// <summary>
    /// Simple cache for PFT scripts.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ProgramCache
    {
        #region Properties

        /// <summary>
        /// Registry.
        /// </summary>
        static Dictionary<string, PftProgram> Registry
        {
            get;
            set;
        }

        #endregion

        #region Construction

        static ProgramCache()
        {
            Registry = new Dictionary<string, PftProgram>();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Add program for the sourceText.
        /// </summary>
        public static void AddProgram
            (
                string sourceText,
                PftProgram program
            )
        {
            lock (Registry)
            {
                Registry[sourceText] = program;
            }
        }

        /// <summary>
        /// Clear the cache.
        /// </summary>
        public static void Clear()
        {
            lock (Registry)
            {
                Registry.Clear();
            }
        }

        /// <summary>
        /// Get program for the text.
        /// </summary>
        public static PftProgram? GetProgram
            (
                string? sourceText
            )
        {
            if (string.IsNullOrEmpty(sourceText))
            {
                return null;
            }

            lock (Registry)
            {
                Registry.TryGetValue(sourceText, out PftProgram? result);

                return result;
            }
        }

        #endregion
    }
}
