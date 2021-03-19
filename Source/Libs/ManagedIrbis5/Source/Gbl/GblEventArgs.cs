// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* GblEventArgs.cs -- event arguments for GlobalCorrecor
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Gbl
{
    /// <summary>
    /// Event arguments for <see cref="GlobalCorrector"/>.
    /// </summary>
    public sealed class GblEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// <see cref="GlobalCorrector"/>
        /// </summary>
        public GlobalCorrector Corrector { get; set; }

        /// <summary>
        /// Whether the execution canceled.
        /// </summary>
        public bool Cancel { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public GblEventArgs
            (
                GlobalCorrector corrector
            )
        {
            Corrector = corrector;
        }

        #endregion
    }
}
