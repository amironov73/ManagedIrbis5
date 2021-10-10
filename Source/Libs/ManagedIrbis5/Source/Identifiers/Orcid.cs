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

/* Orcid.cs -- Open Researcher and Contributor ID
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // ORCID
    // Для научных исследователей зарезервирован блок ISNI
    // идентификаторов, который также называется ORCID
    // (Открытый идентификатор исследователя и автора исследований)
    // и координируется отдельной организацией.
    // Исследователи вправе создать и получить собственный
    // номер ORCID. Действия двух организаций тщательно координируются.
    //
    // См. https://ru.wikipedia.org/wiki/%D0%9C%D0%B5%D0%B6%D0%B4%D1%83%D0%BD%D0%B0%D1%80%D0%BE%D0%B4%D0%BD%D1%8B%D0%B9_%D0%B8%D0%B4%D0%B5%D0%BD%D1%82%D0%B8%D1%84%D0%B8%D0%BA%D0%B0%D1%82%D0%BE%D1%80_%D1%81%D1%82%D0%B0%D0%BD%D0%B4%D0%B0%D1%80%D1%82%D0%BD%D1%8B%D1%85_%D0%BD%D0%B0%D0%B8%D0%BC%D0%B5%D0%BD%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B9
    // https://en.wikipedia.org/wiki/ORCID
    // http://orcid.org/
    // https://github.com/ORCID/ORCID-Source
    //

    /// <summary>
    /// Open Researcher and Contributor ID.
    /// </summary>
    public sealed class Orcid
    {
        #region Properties

        /// <summary>
        /// Identifier.
        /// </summary>
        public string? Identifier { get; set; }

        #endregion

    } // class Orcid

} // namespace ManagedIrbis.Identifiers
