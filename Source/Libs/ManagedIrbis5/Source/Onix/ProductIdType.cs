// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ProductIdType.cs -- код вида международного стандартного номера издания или его эквивалента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Код вида международного стандартного номера издания или его эквивалента.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class ProductIdType
    {
        #region Constants

        /// <summary>
        /// Издательский номер.
        /// </summary>
        public const string PublishingNumber = "01";

        /// <summary>
        /// 10-значный ISBN.
        /// </summary>
        public const string Isbn10 = "02";

        /// <summary>
        /// GTIN-13 (13-значный номер EAN).
        /// </summary>
        public const string Gtin13 = "03";

        /// <summary>
        /// 10-значный ISMN.
        /// </summary>
        public const string Ismn10 = "05";

        /// <summary>
        /// DOI (идентификатор цифрового объекта).
        /// </summary>
        public const string Doi = "06";

        /// <summary>
        /// 13-значный ISBN.
        /// </summary>
        public const string Isbn13 = "15";

        /// <summary>
        /// Номер государственной регистрации обязательного экземпляра.
        /// </summary>
        public const string Ngroe = "17";

        /// <summary>
        /// 13-значный ISBN издателя-партнера.
        /// </summary>
        public const string PartnerIsbn13 = "24";

        /// <summary>
        /// 13-значный ISMN.
        /// </summary>
        public const string Ismn13 = "25";

        /// <summary>
        /// ISBN-A (номер ISBN в системе DOI).
        /// </summary>
        public const string IsbnA = "26";

        /// <summary>
        /// JP e-code (идентификатор электронной публикации).
        /// </summary>
        public const string JpEcode = "27";

        #endregion
    }
}
