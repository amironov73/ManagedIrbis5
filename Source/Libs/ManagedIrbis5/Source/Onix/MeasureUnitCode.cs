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

/* MeasureUnitCode.cs -- единица измерения физической величины
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Единица измерения физической величины.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MeasureUnitCode
    {
        #region Constants

        /// <summary>
        /// Сантиметр.
        /// </summary>
        public const string Centimeter = "cm";

        /// <summary>
        /// Миллиметр.
        /// </summary>
        public const string Millimeter = "mm";

        /// <summary>
        /// Грамм.
        /// </summary>
        public const string Gramm = "gr";

        /// <summary>
        /// Килограмм.
        /// </summary>
        public const string Weight = "kg";

        #endregion
    }
}
