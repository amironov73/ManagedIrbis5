// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* RelevanceCoefficent.cs -- коэффициент релевантности
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.Json.Serialization;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Searching;

/// <summary>
/// Коэффициент релевантности.
/// </summary>
public sealed class RelevanceCoefficient
{
    #region Properties

    /// <summary>
    /// Перечень полей, для которых действует коэффициент.
    /// </summary>
    [JsonPropertyName ("fields")]
    public IList<int> Fields { get; set; } = new List<int>();

    /// <summary>
    /// Значение коэффициента.
    /// </summary>
    [JsonPropertyName ("value")]
    public double Value { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RelevanceCoefficient
        (
            double value
        )
    {
        Sure.NonNegative (value);

        Value = value;
    }

    #endregion
}
