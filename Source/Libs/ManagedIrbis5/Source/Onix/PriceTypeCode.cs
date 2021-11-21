// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* PriceTypeCode.cs -- код вида цены
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Reflection;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Код вида цены.
    /// </summary>
    public static class PriceTypeCode
    {
        #region Constants

        /// <summary>
        /// Рекомендуемая розничная цена без учета налогов.
        /// </summary>
        public const string RecommendedRetailPriceExcludingTaxes = "01";

        /// <summary>
        /// Рекомендуемая розничная цена с учетом налогов.
        /// </summary>
        public const string RecommendedRetailPriceIncludingTaxes = "02";

        /// <summary>
        /// Оптовая цена без учета налогов.
        /// </summary>
        public const string WholesalePriceExcludingTaxes = "05";

        /// <summary>
        /// Оптовая цена с учетом налогов.
        /// </summary>
        public const string WhilesalePriceIncludingTaxes = "06";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значений констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (PriceTypeCode));
        }

        #endregion
    }
}
