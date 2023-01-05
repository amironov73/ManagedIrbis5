// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Reporting.Data;

/// <summary>
/// Specifies the total type.
/// </summary>
public enum TotalType
{
    /// <summary>
    /// The total returns sum of values.
    /// </summary>
    Sum,

    /// <summary>
    /// The total returns minimal value.
    /// </summary>
    Min,

    /// <summary>
    /// The total returns maximal value.
    /// </summary>
    Max,

    /// <summary>
    /// The total returns average value.
    /// </summary>
    Avg,

    /// <summary>
    /// The total returns number of values.
    /// </summary>
    Count,


    /// <summary>
    /// The total returns number of distinct values.
    /// </summary>
    CountDistinct
}
