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

/* Isni.cs -- International Standard Name Identifier
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Identifiers
{
    //
    // Международный идентификатор стандартных наименований
    // Международный стандартный идентификатор имени
    // (англ. International Standard Name Identifier,
    // сокращённо — англ. ISNI) — метод для уникальной идентификации
    // создателей и издателей таких видов медиа-контента, как книги,
    // телевизионные передачи, газетные статьи и др.
    // Такой идентификатор состоит из 16 цифр, разделенных на четыре блока.
    //
    // Стандарт был разработан под эгидой международной организации
    // по стандартизации (ISO), как проект международного стандарта 27729.
    // Действительный стандарт был опубликован 15 марта 2012 года.
    // За разработку стандарта отвечает технический комитет ISO 46,
    // подкомитет 9 (TC 46/SC 9).
    //
    // ISNI может быть использован для разрешения неоднозначности имён,
    // которые в противном случае могли быть перепутаны, и связывает
    // данные об именах, которые используются во всех отраслях
    // промышленности средств массовой информации.
    //
    // На территории России ISNI присваивает Российское авторское общество
    // КОПИРУС, которое является членом международного агентства ISNI-IA.
    //
    // См. https://ru.wikipedia.org/wiki/%D0%9C%D0%B5%D0%B6%D0%B4%D1%83%D0%BD%D0%B0%D1%80%D0%BE%D0%B4%D0%BD%D1%8B%D0%B9_%D0%B8%D0%B4%D0%B5%D0%BD%D1%82%D0%B8%D1%84%D0%B8%D0%BA%D0%B0%D1%82%D0%BE%D1%80_%D1%81%D1%82%D0%B0%D0%BD%D0%B4%D0%B0%D1%80%D1%82%D0%BD%D1%8B%D1%85_%D0%BD%D0%B0%D0%B8%D0%BC%D0%B5%D0%BD%D0%BE%D0%B2%D0%B0%D0%BD%D0%B8%D0%B9
    // https://en.wikipedia.org/wiki/International_Standard_Name_Identifier
    //
    // См. https://en.wikipedia.org/wiki/International_Standard_Name_Identifier
    //
    // См. https://support.orcid.org/knowledgebase/articles/116780-structure-of-the-orcid-identifier
    //

    /// <summary>
    /// International Standard Name Identifier.
    /// </summary>
    public static class Isni
    {
        #region Public methods

        ///<summary>
        /// Генерация контрольной цифры согласно ISO 7064 11,2.
        /// </summary>
        public static string GenerateCheckDigit
            (
                string baseDigits
            )
        {
            Sure.NotNullNorEmpty (baseDigits);

            var total = 0;
            foreach (var c in baseDigits)
            {
                var digit = c - '0';
                total = (total + digit) * 2;
            }

            var remainder = total % 11;
            var result = (12 - remainder) % 11;

            return result == 10 ? "X" : result.ToInvariantString();
        }

        #endregion

    }
}
