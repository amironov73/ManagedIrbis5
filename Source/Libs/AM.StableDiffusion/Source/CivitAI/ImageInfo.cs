// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ImageInfo.cs -- информация о загруженном изображении
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using AM.Text;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Информация о загруженном изображении.
/// </summary>
public sealed class ImageInfo
{
    #region Properties

    /// <summary>
    /// Идентификатор изображения.
    /// </summary>
    [JsonProperty ("id")]
    public long Id { get; set; }

    /// <summary>
    /// Ссылка на изображение.
    /// </summary>
    [JsonProperty ("url")]
    public string? Url { get; set; }

    /// <summary>
    /// Хеш.
    /// </summary>
    [JsonProperty ("hash")]
    public string? Hash { get; set; }

    /// <summary>
    /// Ширина изображения в пикселах.
    /// </summary>
    [JsonProperty ("width")]
    public int Width { get; set; }

    /// <summary>
    /// Высота изображения в пикселах.
    /// </summary>
    [JsonProperty ("height")]
    public int Height { get; set; }

    /// <summary>
    /// Небезопасно для работы.
    /// </summary>
    [JsonProperty ("nsfw")]
    public string? NotSafe { get; set; }

    /// <summary>
    /// Уровень небезопасности.
    /// </summary>
    [JsonProperty ("nsfwLevel")]
    public string? Level { get; set; }

    /// <summary>
    /// Дата создания изображения.
    /// </summary>
    [JsonProperty ("createdAt")]
    public DateTime? Created { get; set; }

    /// <summary>
    /// Идентификатор поста, в котором опубликовано изображение.
    /// </summary>
    [JsonProperty ("postId")]
    public long PostId { get; set; }

    /// <summary>
    /// Статистика лайков/дизлайков.
    /// </summary>
    [JsonProperty ("stats")]
    public ImageStats? Stats { get; set; }

    /// <summary>
    /// Метаданные изображения.
    /// </summary>
    [JsonProperty ("meta")]
    public ImageMeta? Meta { get; set; }

    /// <summary>
    /// Пользователь, загрузивший изображение.
    /// </summary>
    [JsonProperty ("username")]
    public string? UserName { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = StringBuilderPool.Shared.Get();
        builder.Append ($"{Id}");

        if (Width != default || Height != default)
        {
            builder.Append ($" {Width} x {Height}");
        }

        if (!string.IsNullOrEmpty (NotSafe))
        {
            builder.Append ($" NSFW ({Level})");
        }

        if (!string.IsNullOrEmpty (Url))
        {
            builder.Append ($" {Url}");
        }

        return builder.ReturnShared();
    }

    #endregion
}
