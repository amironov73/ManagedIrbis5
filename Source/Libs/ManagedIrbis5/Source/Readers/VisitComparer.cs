// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* VisitComparer.cs -- упорядочение информации о посещениях/выдачи
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;

#endregion

#nullable enable

namespace ManagedIrbis.Readers;

/// <summary>
/// Упорядочение информации о посещениях/выдачах.
/// </summary>
public static class VisitComparer
{
    #region Nested classes

    /// <summary>
    /// Упорядочение по дате выдачи.
    /// </summary>
    public sealed class ByDateGiven
        : IComparer<VisitInfo>
    {
        #region IComparer members

        /// <inheritdoc cref="IComparer{T}.Compare"/>
        public int Compare (VisitInfo? first, VisitInfo? second)
            => StringComparer.OrdinalIgnoreCase
                .Compare (first?.DateGiven, second?.DateGiven);

        #endregion
    }

    /// <summary>
    /// Обратное упорядочение по дате выдачи.
    /// </summary>
    public sealed class ByDateGivenReverse
        : IComparer<VisitInfo>
    {
        #region IComparer members

        /// <inheritdoc cref="IComparer{T}.Compare"/>
        public int Compare (VisitInfo? first, VisitInfo? second)
            => StringComparer.OrdinalIgnoreCase
                .Compare (second?.DateGiven, first?.DateGiven);

        #endregion
    }

    #endregion
}
