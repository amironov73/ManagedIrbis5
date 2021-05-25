// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ExemplarInfoComparer.cs -- сравниватель экземпляров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Сравниватель экземпляров.
    /// </summary>
    public static class ExemplarInfoComparer
    {
        #region Nested classes

        class ByDescriptionComparer
            : IComparer<ExemplarInfo>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    ExemplarInfo? x,
                    ExemplarInfo? y
                )
            {
                return NumberText.Compare
                    (
                        x.ThrowIfNull().Description,
                        y.ThrowIfNull().Description
                    );
            }
        }

        class ByNumberComparer
            : IComparer<ExemplarInfo>
        {
            /// <inheritdoc cref="IComparer{T}.Compare" />
            public int Compare
                (
                    ExemplarInfo? x,
                    ExemplarInfo? y
                )
            {
                return NumberText.Compare
                    (
                        x.ThrowIfNull().Number,
                        y.ThrowIfNull().Number
                    );
            }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Compare <see cref="ExemplarInfo"/>
        /// by <see cref="ExemplarInfo.Description"/> field.
        /// </summary>
        public static IComparer<ExemplarInfo> ByDescription()
        {
            return new ByDescriptionComparer();
        }

        /// <summary>
        /// Compare <see cref="ExemplarInfo"/>
        /// by <see cref="ExemplarInfo.Number"/> field.
        /// </summary>
        public static IComparer<ExemplarInfo> ByNumber()
        {
            return new ByNumberComparer();
        }

        #endregion

    } // class ExemplarInfoComparer

} // namespace ManagedIrbis.Fields
