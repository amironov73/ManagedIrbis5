// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedMember.Global

/* FulltextIndex.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Fulltext
{
    /// <summary>
    /// Поисковые индексы встроенной базы данных TEXT.
    /// </summary>
    public static class FulltextIndexes
    {
        #region Constants

        /// <summary>
        ///
        /// </summary>
        public const string Text = "TXT=";

        /// <summary>
        ///
        /// </summary>
        public const string BeginText = "TXT1=";

        /// <summary>
        ///
        /// </summary>
        public const string ContinueText = "TXT2=";

        /// <summary>
        ///
        /// </summary>
        public const string Guid = "GUID=";

        #endregion
    }
}
