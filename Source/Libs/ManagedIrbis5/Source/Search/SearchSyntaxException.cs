// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* SearchSyntaxException.cs -- синтаксическая ошибка в поисковом запросе
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Синтаксическая ошибка в поисковом запросе.
    /// </summary>
    public sealed class SearchSyntaxException
        : SearchException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchSyntaxException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchSyntaxException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchSyntaxException
            (
                string message,
                Exception innerException
            )
            : base
            (
                message,
                innerException
            )
        {
        }

        #endregion

    } // class SearchSyntaxException

} // namespace ManagedIrbis
