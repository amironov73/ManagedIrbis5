// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TypedValue.cs -- значение с типом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace RestfulIrbis.OpenLibrary;

/// <summary>
/// Значение с типом.
/// </summary>
[PublicAPI]
public sealed class TypedValue
{
    #region Properties

    /// <summary>
    /// Тип, например, <c>"/type/text"</c>.
    /// </summary>
    [JsonProperty ("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Собственно хранимое значение.
    /// </summary>
    [JsonProperty ("value")]
    public string? Value { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() =>
        JsonConvert.ToString (this);

    #endregion
}
