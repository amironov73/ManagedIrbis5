// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedType.Global

/* SearchException.cs -- исключение, специфичное для поисковой инфраструктуры
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace ManagedIrbis
{
    /// <summary>
    /// Исключение, специфичное для поисковой инфраструктуры.
    /// </summary>
    public class SearchException
        : IrbisException
    {
        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchException()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchException
            (
                string message
            )
            : base(message)
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SearchException
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

    } // class SearchException

} // namespace ManagedIrbis
