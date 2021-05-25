// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* CatalogingRules.cs -- коды правил каталогизации
 * Ars Magna project, http://arsmagna.ru
 */

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Коды правил каталогизации в соответствии с 919g.mnu.
    /// </summary>
    public static class CatalogingRules
    {
        #region Constants

        /// <summary>
        /// Anglo-American cataloguing rules.
        /// </summary>
        public const string AACR2 = "AACR2";

        /// <summary>
        /// Library of Congress (USA).
        /// </summary>
        public const string BDRB = "BDRB";

        /// <summary>
        /// Российские "Правила составления библиографического описания".
        /// </summary>
        public const string PSBO = "PSBO";

        /// <summary>
        /// Российские "Правила каталогизации" (РПК).
        /// (Москва, 2005).
        /// </summary>
        public const string RCR = "RCR";

        #endregion

    } // class CataloguingRules

} // namespace ManagedIrbis.Fields
