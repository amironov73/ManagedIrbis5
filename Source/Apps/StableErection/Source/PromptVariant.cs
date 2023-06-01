// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* VariantLine.cs -- строка с вариантом заполнения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace StableErection;

/// <summary>
/// Строка с вариантом заполнения элемента запроса.
/// </summary>
[PublicAPI]
public sealed class PromptVariant
{
    #region Properties

    /// <summary>
    /// Выбрано по умолчанию?
    /// </summary>
    [JsonPropertyName ("default")]
    public bool IsDefault { get; set; }

    /// <summary>
    /// Предлагаемое значение.
    /// </summary>
    [JsonPropertyName ("value")]
    public string? Value { get; set; }

    /// <summary>
    /// Описание в произвольной форме.
    /// </summary>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    #endregion
}
