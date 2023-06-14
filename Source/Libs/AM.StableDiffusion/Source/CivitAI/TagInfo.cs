// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* TagInfo.cs -- информация о метке
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Информация о метке.
/// </summary>
public sealed class TagInfo
{
    #region Properties

    /// <summary>
    /// Наименование метки.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Количество моделей с данной меткой.
    /// </summary>
    [JsonProperty ("modelCount")]
    public int ModelCount { get; set; }

    /// <summary>
    /// Ссылка для получения всех моделей с данной меткой.
    /// </summary>
    [JsonProperty ("link")]
    public string? Link { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}: {ModelCount}: {Link}";

    #endregion
}
