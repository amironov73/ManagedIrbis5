// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* FullTextResult.cs -- результат полнотекстового поиска
 * Ars Magna project, http://arsmagna.ru
 */

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
        public void Decode
            (
                Response response
            )
        {
            Pages = FoundPages.Decode (response);
        }

        #endregion
    }
}
