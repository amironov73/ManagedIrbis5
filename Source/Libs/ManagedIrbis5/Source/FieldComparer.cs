// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* FieldComparer.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    ///
    /// </summary>
    public static class FieldComparer
    {
        #region Nested classes

        class ByTagComparer
            : Comparer<Field>
        {
            /// <inheritdoc cref="Comparer{T}.Compare" />
            public override int Compare
                (
                    Field? left,
                    Field? right
                )
            {
                return (left?.Tag ?? 0) - (right?.Tag ?? 0);
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="Field"/> by
        /// <see cref="Field.Tag"/>.
        /// </summary>
        public static Comparer<Field> ByTag()
        {
            return new ByTagComparer();
        }

        #endregion
    }
}
