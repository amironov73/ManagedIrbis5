// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* CreatorInfo.cs -- информация о создателе контента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Информация о создателе контента
/// </summary>
[PublicAPI]
public sealed class CreatorInfo
{
    #region Properties

    /// <summary>
    /// Логин.
    /// </summary>
    [JsonProperty ("username")]
    public string? UserName { get; set; }

    /// <summary>
    /// Число загруженных моделей.
    /// </summary>
    [JsonProperty ("modelCount")]
    public int ModelCount { get; set; }

    /// <summary>
    /// Ссылка.
    /// </summary>
    [JsonProperty ("link")]
    public string? Link { get; set; }

    /// <summary>
    /// Аватар
    /// </summary>
    [JsonProperty ("image")]
    public string? Image { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => UserName.ToVisibleString();

    #endregion
}
