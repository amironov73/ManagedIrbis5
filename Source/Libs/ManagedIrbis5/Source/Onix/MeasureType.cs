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

/* MeasureType.cs -- код, обозначающий физическую величину
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace ManagedIrbis.Onix
{
    /// <summary>
    /// Код, обозначающий физическую величину.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class MeasureType
    {
        #region Constants

        /// <summary>
        /// Высота.
        /// </summary>
        public const string Height = "01";

        /// <summary>
        /// Ширина.
        /// </summary>
        public const string Width = "02";

        /// <summary>
        /// Толщина.
        /// </summary>
        public const string Depth = "03";

        /// <summary>
        /// Вес.
        /// </summary>
        public const string Weight = "08";

        /// <summary>
        /// Диаметр тубы или цилиндра.
        /// </summary>
        public const string Diameter = "12";

        #endregion
    }
}
