// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* CultureCode.cs -- коды поддерживаемых культур
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

#endregion

#nullable enable

namespace AM.Globalization
{
    /// <summary>
    /// Коды поддерживаемых культур.
    /// </summary>
    public static class CultureCode
    {
        #region Constants

        /// <summary>
        /// American English.
        /// </summary>
        public const string AmericanEnglish = "en-US";

        /// <summary>
        /// Russian in Russia.
        /// </summary>
        public const string Russian = "ru-RU";

        #endregion
    }
}
