// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Bagel.cs -- результат "самоварного поиска"
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

#endregion

#nullable enable

using System;
using System.Collections.Generic;

using AM;

namespace ManagedIrbis.Searching;

/// <summary>
/// Результат "самоварного поиска"
/// </summary>
public sealed class Bagel
{
    #region Nested classes

    /// <summary>
    /// Умеет сравнивать результаты поиска.
    /// </summary>
    private sealed class BagelComparer
        : IComparer<Bagel>
    {
        #region IComparer<T> members

        /// <inheritdoc cref="IComparer{T}.Compare"/>
        public int Compare
            (
                Bagel? x,
                Bagel? y
            )
        {
            return Math.Sign ((x?.Rating ?? 0) - (y?.Rating ?? 0));
        }

        #endregion
    }

    #endregion

    #region Properties

    /// <summary>
    /// Обратное сравнение результатов по рейтингу.
    /// </summary>
    public static readonly IComparer<Bagel> ReverseComparer
        = new ReverseComparer<Bagel>(new BagelComparer());

    /// <summary>
    /// Расформатированное библиографическое описание.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Найденная библиографическая запись.
    /// </summary>
    public Record? Record { get; set; }

    /// <summary>
    /// Рейтинг найденной записи.
    /// </summary>
    public double Rating { get; set; }

    #endregion
}
