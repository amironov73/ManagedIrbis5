// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SiberianField.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

using ManagedIrbis.Workspace;

#endregion

#nullable enable

namespace ManagedIrbis.WinForms.Grid
{
    /// <summary>
    ///
    /// </summary>
    public class SiberianField
    {
        #region Properties

        /// <summary>
        /// Field tag.
        /// </summary>
        public int Tag { get; set; }

        /// <summary>
        /// Title.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        /// Repeat.
        /// </summary>
        public int Repeat { get; set; }

        /// <summary>
        /// Parent (first) field.
        /// </summary>
        public SiberianField? Parent { get; set; }

        /// <summary>
        /// Whether the field is repeatable?
        /// </summary>
        public bool Repeatable { get; set; }

        /// <summary>
        /// Value.
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// Original value.
        /// </summary>
        public string? OriginalValue { get; set; }

        /// <summary>
        /// Editing mode?
        /// </summary>
        public string? Mode { get; set; }

        /// <summary>
        /// Modified?
        /// </summary>
        public bool Modified { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Create <see cref="SiberianField"/> from
        /// <see cref="WorksheetItem"/>.
        /// </summary>
        public static SiberianField FromWorksheetItem
            (
                WorksheetItem item
            )
        {
            var result = new SiberianField
            {
                Tag = item.Tag?.ParseInt32() ?? 0,
                Title = item.Title,
                Repeatable = item.Repeatable
            };

            return result;
        }

        #endregion

    }
}
