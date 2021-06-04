// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* StatDefinition.cs -- parameters for stat command
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Signature for Stat command.
    /// </summary>
    public sealed class StatDefinition
    {
        #region Nested classes

        /// <summary>
        /// Sort method.
        /// </summary>
        public enum SortMethod
        {
            /// <summary>
            /// Don't sort.
            /// </summary>
            None = 0,

            /// <summary>
            /// Ascending sort.
            /// </summary>
            Ascending = 1,

            /// <summary>
            /// Descending sort.
            /// </summary>
            Descending = 2
        }

        /// <summary>
        /// Stat item.
        /// </summary>
        public sealed class Item
        {
            #region Properties

            /// <summary>
            /// Field (possibly with subfield) specification.
            /// </summary>
            public string? Field { get; set; }

            /// <summary>
            /// Maximum length of the value (truncation).
            /// </summary>
            public int Length { get; set; }

            /// <summary>
            /// Count of items to take.
            /// </summary>
            public int Count { get; set; }

            /// <summary>
            /// How to sort result.
            /// </summary>
            public SortMethod Sort { get; set; }

            #endregion

            #region Object members

            /// <inheritdoc cref="object.ToString"/>
            public override string ToString() => $"{Field},{Length},{Count},{(int)Sort}";

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// Database name.
        /// </summary>
        public string? DatabaseName { get; set; }

        /// <summary>
        /// Items.
        /// </summary>
        public List<Item> Items { get; } = new();

        /// <summary>
        /// Search query specification.
        /// </summary>
        public string? SearchQuery { get; set; }

        /// <summary>
        /// Minimal MFN.
        /// </summary>
        public int MinMfn { get; set; }

        /// <summary>
        /// Maximal MFN.
        /// </summary>
        public int MaxMfn { get; set; }

        /// <summary>
        /// Optional query for sequential search.
        /// </summary>
        public string? SequentialQuery { get; set; }

        /// <summary>
        /// List of MFN.
        /// </summary>
        public List<int> MfnList { get; } = new();

        #endregion

    } // class StatDefinition

} // namespace ManagedIrbis
