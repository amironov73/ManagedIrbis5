// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

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
        public static CultureInfo AmericanEnglish => new (CultureCode.AmericanEnglish);

        /// <summary>
        /// Русская культура (Русская-в-России!).
        /// </summary>
        public static CultureInfo Russian => new (CultureCode.Russian);

        #endregion
    }
}
