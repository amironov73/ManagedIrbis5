// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ModelInfo.cs -- информация о загруженной модели
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;

using Newtonsoft.Json;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Информация о загруженной на CivitAI модели.
/// </summary>
public sealed class ModelInfo
{
    #region Properties

    /// <summary>
    /// Идентификатор модели + версий.
    /// </summary>
    [JsonProperty ("id")]
    public int Id { get; set; }

    /// <summary>
    /// Наименование.
    /// </summary>
    [JsonProperty ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Описание модели в свободной форме (HTML).
    /// </summary>
    [JsonProperty ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Тип модели, см. <see cref="ModelType"/>.
    /// </summary>
    [JsonProperty ("type")]
    public string? Type { get; set; }

    /// <summary>
    /// Небезопасно для работы?
    /// </summary>
    [JsonProperty ("nsfw")]
    public  string? NotSafe { get; set; }

    /// <summary>
    /// Метки.
    /// </summary>
    [JsonProperty ("tags")]
    public string[]? Tags { get; set; }

    /// <summary>
    /// Режим: заархивирована, удалена и т. п.
    /// </summary>
    [JsonProperty ("mode")]
    public string? Mode { get; set; }

    /// <summary>
    /// Статистика по модели.
    /// </summary>
    [JsonProperty ("stats")]
    public ModelStats? Stats { get; set; }

    /// <summary>
    /// Создатель модели.
    /// </summary>
    [JsonProperty ("creator")]
    public CreatorInfo? Creator { get; set; }

    /// <summary>
    /// Версии модели.
    /// </summary>
    [JsonProperty ("modelVersions")]
    public ModelVersion[]? Versions { get; set; }

    /// <summary>
    /// Приложенные изображения.
    /// </summary>
    [JsonProperty ("images")]
    public ImageInfo[]? Images { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение указанной версии модели.
    /// Если указана версия <c>null</c>, то будет выдана
    /// версия с наибольшим номером, если таковая обнаружится.
    /// </summary>
    public ModelVersion? GetVersion
        (
            string? versionName
        )
    {
        var versions = Versions;
        if (versions is null || versions.Length == 0)
        {
            return null;
        }

        if (string.IsNullOrEmpty (versionName))
        {
            versionName = versions.Max (it => it.Name);
            if (string.IsNullOrEmpty (versionName))
            {
                return versions.First();
            }
        }

        if (string.IsNullOrEmpty (versionName))
        {
            return null;
        }

        var result = versions.FirstOrDefault
            (
                it => it.Name.SameString (versionName)
            );

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Id}: {Name}: {Type}";

    #endregion
}
