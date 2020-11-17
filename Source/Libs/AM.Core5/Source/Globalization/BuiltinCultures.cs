// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* BuiltinCultures.cs -- поддерживаемые встроенные культуры
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;

#endregion

#nullable enable

namespace AM.Globalization
{
    /// <summary>
    /// Поддерживаемые встроенные культуры.
    /// </summary>
    public static class BuiltinCultures
    {
        #region Properties

        /// <summary>
        /// American English.
        /// </summary>
        public static CultureInfo AmericanEnglish => new CultureInfo(CultureCode.AmericanEnglish);

        /// <summary>
        /// Russian culture (just russian, not ru-RU).
        /// </summary>
        public static CultureInfo Russian => new CultureInfo(CultureCode.Russian);

        #endregion
    }
}
