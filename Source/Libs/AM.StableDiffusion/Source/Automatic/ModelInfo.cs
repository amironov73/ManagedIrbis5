// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ModelInfo.cs -- информация о модели, например, о LoRA.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Text.Json.Serialization;

using JetBrains.Annotations;

#endregion

namespace AM.StableDiffusion.Automatic;

/*
   Пример файла для LoRA:

   {
    "description": "Aida (aka Aida A, Aida B, Hristina, Kristina, Kristy, Tina) is a Russian nude model in 2007-2022 (born in 1989)",
    "notes": "Retrain with resolution 768x768",
    "sd version": "SD1",
    "activation text": "Aida girl",
    "preferred weight": 0.8,
    "extensions": {
        "sd_civitai_helper": {
            "version": "1.8.1",
            "last_update": 1703241810,
            "skeleton_file": true
        }
      }
    }

 */

/// <summary>
/// Информация о модели, например, о LoRA.
/// Хранится в файле <code>model.json</code>.
/// </summary>
[PublicAPI]
public sealed class ModelInfo
{
    #region Properties

    /// <summary>
    /// Описание в произвольной форме.
    /// </summary>
    /// <example>
    /// Aida (aka Aida A, Aida B, Hristina, Kristina, Kristy, Tina) is a Russian nude model in 2007-2022 (born in 1989)
    /// </example>
    [JsonPropertyName ("description")]
    public string? Description { get; set; }

    /// <summary>
    /// Опциональные примечания в произвольной форме.
    /// </summary>
    /// <example>
    /// Retrain with resolution 768x768.
    /// </example>
    [JsonPropertyName ("notes")]
    public string? Notes { get; set; }

    /// <summary>
    /// Версия Stable Diffusion: "SD1", "SD2" или "SDXL".
    /// </summary>
    [JsonPropertyName ("sd version")]
    public string? StableDiffusionVersion { get; set; }

    /// <summary>
    /// Текст активации, например, "Aida girl".
    /// </summary>
    [JsonPropertyName ("activation text")]
    public string? ActivationText { get; set; }

    /// <summary>
    /// Предпочтительный вес, например, <code>0.8</code>.
    /// </summary>
    [JsonPropertyName ("preferred weight")]
    public float PreferredWeight { get; set; }

    #endregion
}
