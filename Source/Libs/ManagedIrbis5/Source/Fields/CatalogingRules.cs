// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* CatalogingRules.cs -- коды правил каталогизации
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;

using AM.Reflection;

#endregion

#nullable enable

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
        [Description ("Anglo-American cataloguing rules")]
        public const string AACR2 = "AACR2";

        /// <summary>
        /// Library of Congress (USA).
        /// </summary>
        [Description ("Library of Congress (USA)")]
        public const string BDRB = "BDRB";

        /// <summary>
        /// Российские "Правила составления библиографического описания".
        /// </summary>
        [Description ("Российские Правила составления библиографического описания")]
        public const string PSBO = "PSBO";

        /// <summary>
        /// Российские "Правила каталогизации" (РПК).
        /// (Москва, 2005).
        /// </summary>
        [Description ("Российские Правила каталогизации")]
        public const string RCR = "RCR";

        #endregion

        #region Public methods

        /// <summary>
        /// Получение массива значение констант.
        /// </summary>
        public static string[] ListValues()
        {
            return ReflectionUtility.ListConstantValues<string> (typeof (CatalogingRules));
        }

        /// <summary>
        /// Получение словаря "код" - "значение".
        /// </summary>
        public static Dictionary<string, string> ListValuesWithDescriptions()
        {
            return ReflectionUtility.ListConstantValuesWithDescriptions (typeof (CatalogingRules));
        }

        #endregion
    }
}
