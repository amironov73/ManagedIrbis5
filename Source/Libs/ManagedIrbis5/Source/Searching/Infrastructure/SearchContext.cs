// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

/* SearchContext.cs -- контекст для поиска
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using ManagedIrbis.Client;

#endregion

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Контекст для поиска.
    /// </summary>
    public sealed class SearchContext
    {
        #region Properties

        /// <summary>
        /// Search manager.
        /// </summary>
        public SearchManager Manager { get; }

        /// <summary>
        /// Providr.
        /// </summary>
        public ISyncProvider Provider { get; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchContext
            (
                SearchManager manager,
                ISyncProvider provider
            )
        {
            Manager = manager;
            Provider = provider;
        }

        #endregion

    } // class SearchContext

} // namespace ManagedIrbis
