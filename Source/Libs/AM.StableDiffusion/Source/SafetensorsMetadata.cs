// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* SafetensorsMetadata.cs -- метаданные safetensors
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text.Json.Serialization;

using AM.Json;

using JetBrains.Annotations;

using SixLabors.ImageSharp;

#endregion

namespace AM.StableDiffusion;

/*

 Пример метаданных LORA:

{
  "ss_output_name": "engineering_cards",
  "ss_sd_model_name": "v1-5-pruned-emaonly.safetensors",
  "ss_network_module": "networks.lora",
  "ss_total_batch_size": "1",
  "ss_resolution": "(512, 512)",
  "ss_optimizer": "bitsandbytes.optim.adamw.AdamW8bit",
  "ss_lr_scheduler": "cosine_with_restarts",
  "ss_clip_skip": "None",
  "ss_network_dim": "32",
  "ss_network_alpha": "32",
  "ss_epoch": "9",
  "ss_num_epochs": "10",
  "ss_steps": "2340",
  "ss_max_train_steps": "2600",
  "ss_learning_rate": "0.000001",
  "ss_text_encoder_lr": "5e-7",
  "ss_unet_lr": "0.000001",
  "ss_shuffle_caption": "False",
  "ss_keep_tokens": "0",
  "ss_flip_aug": "False",
  "ss_noise_offset": "0.1",
  "ss_adaptive_noise_scale": "None",
  "ss_min_snr_gamma": "None",
  "ss_training_started_at": "2023-06-01T21:17:57.620Z",
  "ss_training_finished_at": "2023-06-01T21:28:44.302Z",
  "sshs_model_hash": "924512c70d5930b80c73d6ff171c009f49600b246f2069a7a22ad93fb91e454d"
}

 */

/// <summary>
/// Метаданные safetensors.
/// </summary>
[PublicAPI]
public sealed class SafetensorsMetadata
{
    #region Properties

    /// <summary>
    /// Имя модели, например, лоры, под которым она сохранена на диск.
    /// Например, <c>"engineering_cards"</c>.
    /// </summary>
    [JsonPropertyName ("ss_output_name")]
    public string? OutputName { get; set; }

    /// <summary>
    /// Имя базовой модели.
    /// Например, <c>"v1-5-pruned-emaonly.safetensors"</c>.
    /// </summary>
    [JsonPropertyName ("ss_sd_model_name")]
    public string? ModelName { get; set; }

    /// <summary>
    /// Вид модели, например, <c>"networks.lora"</c>.
    /// </summary>
    [JsonPropertyName ("ss_network_module")]
    public string? Kind { get; set; }

    /// <summary>
    /// Общий размер пакета, чаще всего <c>1</c>.
    /// </summary>
    [JsonPropertyName ("ss_total_batch_size")]
    [JsonConverter (typeof (IntegerConverter))]
    public int TotalBatchSize { get; set; }

    /// <summary>
    /// Разрешение, например,<c>"(512, 512)"</c>
    /// </summary>
    [JsonPropertyName ("ss_resolution")]
    [JsonConverter (typeof (SizeConverter))]
    public Size Resolution { get; set; }

    /// <summary>
    /// Использованный оптимизатор.
    /// Например, <c>"bitsandbytes.optim.adamw.AdamW8bit"</c>.
    /// </summary>
    [JsonPropertyName ("ss_optimizer")]
    public string? Optimizer { get; set; }

    /// <summary>
    /// Использованный планировщик.
    /// Например, <c>"cosine_with_restarts"</c>.
    /// </summary>
    [JsonPropertyName ("ss_lr_scheduler")]
    public string? Scheduler { get; set; }

    /// <summary>
    /// Хеш-сумма.
    /// Например, <c>"924512c70d5930b80c73d6ff171c009f49600b246f2069a7a22ad93fb91e454d"</c>.
    /// </summary>
    [JsonPropertyName ("sshs_model_hash")]
    public string? Hash { get; set; }

    /// <summary>
    /// Clip skip, например, <c>"None"</c>.
    /// </summary>
    [JsonPropertyName ("ss_clip_skip")]
    public string? ClipSkip { get; set; }

    /// <summary>
    /// Разрешение гиперсети, например, <c>32</c>.
    /// </summary>
    [JsonPropertyName ("ss_network_dim")]
    [JsonConverter (typeof (IntegerConverter))]
    public int Dimension { get; set; }

    /// <summary>
    /// Альфа, например, <c>32</c>.
    /// </summary>
    [JsonPropertyName ("ss_network_alpha")]
    [JsonConverter (typeof (DoubleConverter))]
    public double Alpha { get; set; }

    /// <summary>
    /// Номер эпохи, например, <c>9</c>.
    /// </summary>
    [JsonPropertyName ("ss_epoch")]
    [JsonConverter (typeof (IntegerConverter))]
    public int Epoch { get; set; }

    /// <summary>
    /// Запланированное количество эпох, например, <c>10</c>.
    /// </summary>
    [JsonPropertyName ("ss_num_epochs")]
    [JsonConverter (typeof (IntegerConverter))]
    public int NumEpochs { get; set; }

    /// <summary>
    /// Количество пройденных шагов, например, <c>2340</c>.
    /// </summary>
    [JsonPropertyName ("ss_steps")]
    [JsonConverter (typeof (IntegerConverter))]
    public int Steps { get; set; }

    /// <summary>
    /// Запланированное количество шагов, например, <c>2600</c>.
    /// </summary>
    [JsonPropertyName ("ss_max_train_steps")]
    [JsonConverter (typeof (IntegerConverter))]
    public int MaxTrainSteps { get; set; }

    /// <summary>
    /// Скорость обучения, например, <c>0.000001</c>.
    /// </summary>
    [JsonPropertyName ("ss_learning_rate")]
    [JsonConverter (typeof (DoubleConverter))]
    public double LearningRate { get; set; }

    /// <summary>
    /// Скорость обучения текстового энкодера.
    /// Например, <c>5e-7</c>.
    /// </summary>
    [JsonPropertyName ("ss_text_encoder_lr")]
    [JsonConverter (typeof (DoubleConverter))]
    public double TextEncoderLearningRate { get; set; }

    /// <summary>
    /// Скорость обучения Unet.
    /// Например, <c>0.000001</c>.
    /// </summary>
    [JsonPropertyName ("ss_unet_lr")]
    [JsonConverter (typeof (DoubleConverter))]
    public double UnetEncoderLearningRate { get; set; }

    /// <summary>
    /// Смещение шума.
    /// Например, <c>0.1</c>.
    /// </summary>
    [JsonPropertyName ("ss_noise_offset")]
    [JsonConverter (typeof (DoubleConverter))]
    public double NoiseOffset { get; set; }

    /// <summary>
    /// Дата начала тренировки.
    /// Например, <c>"2023-06-01T21:17:57.620Z"</c>.
    /// </summary>
    [JsonPropertyName ("ss_training_started_at")]
    public string? TrainingStarted { get; set; }

    /// <summary>
    /// Дата окончания тренировки.
    /// Например, <c>"2023-06-01T21:28:44.302Z"</c>.
    /// </summary>
    [JsonPropertyName ("ss_training_finished_at")]
    public string? TraniningFinished { get; set; }

    /// <summary>
    /// Директории с данными, использовавшиеся для обучения.
    /// </summary>
    [JsonPropertyName ("ss_dataset_dirs")]
    [JsonConverter (typeof (DatasetDirectoryConverter))]
    public DatasetDirectory[]? DatasetDirectories { get; set; }

    /// <summary>
    /// Частоты тегов.
    /// </summary>
    [JsonPropertyName ("ss_tag_frequency")]
    [JsonConverter (typeof (TagFrequencyConverter))]
    public Dictionary<string, Dictionary<string, int>>? TagFrequency { get; set; }

    /// <summary>
    /// Корзины разрешены?
    /// </summary>
    [JsonPropertyName ("ss_enable_bucket")]
    [JsonConverter (typeof (BooleanConverter))]
    public bool EnableBucket { get; set; }

    /// <summary>
    /// Mixed precision.
    /// </summary>
    [JsonPropertyName ("ss_mixed_precision")]
    public string? MixedPrecision { get; set; }

    /// <summary>
    /// Количество шагов разогрева.
    /// </summary>
    [JsonPropertyName ("ss_lr_warmup_steps")]
    [JsonConverter (typeof (IntegerConverter))]
    public int WarmupSteps { get; set; }

    /// <summary>
    /// Вторая версия?
    /// </summary>
    [JsonPropertyName ("ss_v2")]
    [JsonConverter (typeof (BooleanConverter))]
    public bool Version2 { get; set; }



    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonUtility.SerializeIndented (this);

    #endregion
}
