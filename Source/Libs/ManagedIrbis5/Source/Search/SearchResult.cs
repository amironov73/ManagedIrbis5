// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* SearchResult.cs -- результат ранее произведенного поиска
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Результат ранее произведенного поиска.
    /// </summary>
    public sealed class SearchResult
    {
        #region Properties

        /// <summary>
        /// Count of records found.
        /// </summary>
        public int FoundCount { get; set; }

        /// <summary>
        /// Search query text.
        /// </summary>
        public string? Query { get; set; }

        #endregion

    } // class SearchResult

} // namespace ManagedIrbis
