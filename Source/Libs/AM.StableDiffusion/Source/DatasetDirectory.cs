// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* DatasetDirectory.cs -- директория (папка) с датасетом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using AM.Json;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion;

/// <summary>
/// Директория (папка) с датасетом, использованным для тренировки лоры.
/// </summary>
[PublicAPI]
public sealed class DatasetDirectory
{
    #region Property

    /// <summary>
    /// Имя директории.
    /// </summary>
    [JsonPropertyName ("name")]
    public string? Name { get; set; }

    /// <summary>
    /// Заданное количество повторений.
    /// </summary>
    [JsonPropertyName ("n_repeats")]
    public int Repeats { get; set; }

    /// <summary>
    /// Найденное количество избражений.
    /// </summary>
    [JsonPropertyName ("img_count")]
    public int ImageCount { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonUtility.SerializeShort (this);

    #endregion
}
