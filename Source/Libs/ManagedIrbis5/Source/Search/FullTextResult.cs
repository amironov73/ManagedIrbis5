// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FullTextResult.cs -- результат полнотекстового поиска
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Text;

using AM;

using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Результат полнотекстового поиска.
    /// </summary>
    public sealed class FullTextResult
    {
        #region Properties

        /// <summary>
        /// Найденные страницы
        /// </summary>
        public FoundPages[]? Pages { get; set; }

        /// <summary>
        /// Фасеты.
        /// </summary>
        public FoundFacet[]? Facets { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор ответа сервера.
        /// </summary>
        /// <param name="response"></param>
        public void Parse
            (
                Response response
            )
        {
            throw new NotImplementedException();
        }

        #endregion

    } // class FullTextResult

} // namespace ManagedIrbis
