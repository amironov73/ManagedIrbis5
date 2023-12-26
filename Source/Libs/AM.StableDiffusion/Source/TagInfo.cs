// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable MemberCanBeProtected.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global

/* TagInfo.cs -- информация о теге
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Информация о теге (элементе описания картинки).
/// </summary>
[PublicAPI]
public sealed class TagInfo
{
    #region Properties

    /// <summary>
    /// Содержимое тега - некий (произвольный в общем случае) текст.
    /// </summary>
    [JsonPropertyName ("title")]
    public string? Title { get; set; }

    /// <summary>
    /// Количество использований тега.
    /// Применяется далеко не всегда.
    /// </summary>
    [JsonPropertyName ("count")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Count { get; set; }

    /// <summary>
    /// Состояние тега (например, ошибка или не активен).
    /// Применяется далеко не всегда.
    /// </summary>
    [JsonPropertyName ("state")]
    [JsonIgnore (Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? State { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => Count != 0
            ? $"{Title}: {Count}"
            : Title.ToVisibleString();

    #endregion
}
