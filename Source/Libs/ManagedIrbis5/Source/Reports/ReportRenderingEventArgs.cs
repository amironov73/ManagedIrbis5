// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* ReportRenderingEventArgs.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace ManagedIrbis.Reports
{
    /// <summary>
    ///
    /// </summary>
    public sealed class ReportRenderingEventArgs
        : EventArgs
    {
        #region Properties

        /// <summary>
        /// Context.
        /// </summary>
        public ReportContext Context { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReportRenderingEventArgs (ReportContext context) =>
            Context = context;

        #endregion

    } // class ReportRenderingEventArgs

} // namespace ManagedIrbis.Reports
