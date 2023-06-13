// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ScriptArgument.cs -- аргумент скрипта
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Аргумент скрипта.
/// </summary>
[PublicAPI]
public sealed class ScriptArgument
{
    #region Properties

    /// <summary>
    /// Метка (наименование) аргумента.
    /// </summary>
    [JsonProperty ("label")]
    public string? Label { get; set; }

    /// <summary>
    /// Значение аргумента.
    /// </summary>
    [JsonProperty ("value", NullValueHandling = NullValueHandling.Ignore)]
    public object? Value { get; set; }

    /// <summary>
    /// Минимально возможное значение аргумента (если есть).
    /// </summary>
    [JsonProperty ("minimum", NullValueHandling = NullValueHandling.Ignore)]
    public object? Minimum { get; set; }

    /// <summary>
    /// Максимально возможное значение аргумента (если есть).
    /// </summary>
    [JsonProperty ("maximum", NullValueHandling = NullValueHandling.Ignore)]
    public object? Maximum { get; set; }

    /// <summary>
    /// Шаг, с которым может изменяться значение аргумента (если есть).
    /// </summary>
    [JsonProperty ("step", NullValueHandling = NullValueHandling.Ignore)]
    public object? Step { get; set; }

    /// <summary>
    /// Возможные варианты выбора (если есть).
    /// </summary>
    [JsonProperty ("choices", NullValueHandling = NullValueHandling.Ignore)]
    public string[]? Choices { get; set; }

    #endregion
}
