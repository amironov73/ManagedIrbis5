// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* SearchLogicType.cs -- какие логические операции могут использоваться
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis
{
    /// <summary>
    /// Какие логические операторы могут использоваться
    /// для данного вида поиска.
    /// </summary>
    public enum SearchLogicType
    {
        /// <summary>
        /// только логика ИЛИ
        /// </summary>
        Or = 0,

        /// <summary>
        /// логика ИЛИ и И
        /// </summary>
        OrAnd = 1,

        /// <summary>
        /// логика ИЛИ, И, НЕТ (по умолчанию)
        /// </summary>
        OrAndNot = 2,

        /// <summary>
        /// логика ИЛИ, И, НЕТ, И (в поле)
        /// </summary>
        OrAndNotField = 3,

        /// <summary>
        /// логика ИЛИ, И, НЕТ, И (в поле), И (фраза)
        /// </summary>
        OrAndNotPhrase = 4

    } // enum SearchLogicType

} // namespace ManagedIrbis
