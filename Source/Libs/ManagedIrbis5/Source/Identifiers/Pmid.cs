// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* Pmid.cs -- PubMed Identifier.
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // См. https://ru.wikipedia.org/wiki/PMID
    //
    // PMID (от англ. PubMed Identifier, PubMed ID) — уникальный
    // идентификационный номер, присваиваемый каждой публикации,
    // описание, аннотация или полный текст которой хранятся
    // в базе данных PubMed.
    //
    // Допускается одновременный поиск нескольких публикаций
    // по их PMID (с уточняющим тегом [pmid] или без него),
    // введённым в поисковую строку PubMed через пробел — например,
    // 7170002 16381840.
    //
    // При одновременном поиске названий публикаций и других терминов
    // использование стандартных поисковых тегов PubMed обязательно:
    // lipman[au] 16381840[pmid].
    //
    // Идентификаторы PMID, присвоенные публикациям в PubMed,
    // впоследствии никогда не меняются и не используются повторно.
    //

    /// <summary>
    /// PubMed Identifier.
    /// </summary>
    public sealed class Pmid
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string? Identifier { get; set; }

        #endregion

    } // class Pmid

} // namespace ManagedIrbis.Identifiers
