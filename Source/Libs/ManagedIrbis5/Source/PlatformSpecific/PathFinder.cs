// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PathFinder.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.PlatformSpecific
{
    /// <summary>
    /// Finds a file on abstract path.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public abstract class PathFinder
    {
        #region Public methods

        /// <summary>
        /// Find file.
        /// </summary>
        public virtual string? FindFile
            (
                string path,
                string fileName
            )
        {
            return null;
        }

        #endregion
    }
}
