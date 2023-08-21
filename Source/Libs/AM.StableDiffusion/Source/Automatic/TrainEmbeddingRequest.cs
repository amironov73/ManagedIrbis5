// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* TrainEmbeddingRequest.cs -- запрос на тренировку текстовой инверсии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;

#endregion

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Запрос на тренировку текстовой инверсии.
/// </summary>
[PublicAPI]
public class TrainEmbeddingRequest
{
    #region Properties

    /// <summary>
    /// Имя текстовой инверсии, подлежащей тренировке.
    /// </summary>
    [JsonProperty ("embedding_name")]
    public string? Name { get; set; }

    /// <summary>
    /// Скорость обучения.
    /// </summary>
    [JsonProperty ("learn_rate", NullValueHandling = NullValueHandling.Ignore)]
    public string? LearningRate { get; set; }

    /// <summary>
    /// Количество изображений в одной связке.
    /// </summary>
    [JsonProperty ("batch_size", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int BatchSize { get; set; }

    /// <summary>
    /// Количество шагов накопления градиента.
    /// </summary>
    [JsonProperty ("gradient_step", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int GradientStep { get; set; }

    /// <summary>
    /// Имя папки с данными для обучения.
    /// </summary>
    [JsonProperty ("data_root", NullValueHandling = NullValueHandling.Ignore)]
    public string? DataRoot { get; set; }

    /// <summary>
    /// Папка для хранения логов.
    /// </summary>
    [JsonProperty ("log_directory", NullValueHandling = NullValueHandling.Ignore)]
    public string? LogDirectory { get; set; }

    /// <summary>
    /// Ширина изображения.
    /// </summary>
    [JsonProperty ("training_width", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Width { get; set; }

    /// <summary>
    /// Высота изображения.
    /// </summary>
    [JsonProperty ("training_height", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int Height { get; set; }

    /// <summary>
    /// Количество шагов обучения.
    /// </summary>
    [JsonProperty ("steps", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int MaxStep { get; set; }

    /// <summary>
    /// Режим отсечения.
    /// </summary>
    [JsonProperty ("clip_grad_mode")]
    public string? GradientClipping { get; set; }

    /// <summary>
    /// Перемешивать токены в запросе?
    /// </summary>
    [JsonProperty ("shuffle_tags", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool ShuffleTags { get; set; }

    /// <summary>
    /// Отбрасывать некоторые токены?
    /// </summary>
    [JsonProperty ("tag_drop_out", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool TagDropOut { get; set; }

    /// <summary>
    /// Метод латентного семплирования.
    /// </summary>
    [JsonProperty ("latent_sampling_method", NullValueHandling = NullValueHandling.Ignore)]
    public string? LatentSamplingMethod { get; set; }

    /// <summary>
    /// Создавать пробное изображение через указанное количество шагов.
    /// </summary>
    [JsonProperty ("create_image_every", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int CreateImageEvery { get; set; }

    /// <summary>
    /// Сохранять файл инверсии через указанное количество шагов.
    /// </summary>
    [JsonProperty ("save_embedding_every", DefaultValueHandling = DefaultValueHandling.Ignore)]
    public int SaveEmbeddingEvery { get; set; }

    /// <summary>
    /// Имя файла с шаблоном запроса.
    /// </summary>
    [JsonProperty ("template_filename")]
    public string? TemplateFileName { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Формирование запроса на обучение текстовой инверсии.
    /// из командной строки.
    /// </summary>
    public TrainEmbeddingRequest FromCommandLine
        (
            string[] args
        )
    {
        var result = new TrainEmbeddingRequest
        {
            LearningRate = "0.005",
            GradientStep = 1,
            BatchSize = 1,
            LogDirectory = "textual_inversion",
            Width = 512,
            Height = 512,
            GradientClipping = "disabled",
            LatentSamplingMethod = "deterministic",
            CreateImageEvery = 100,
            SaveEmbeddingEvery = 100,
            TemplateFileName = "style_filewords.txt",
        };

        for (var i = 0; i < args.Length; i++)
        {
            var arg = args[i];
            switch (arg)
            {
                case "--name":
                    result.Name = args[++i];
                    break;

                case "--rate":
                    result.LearningRate = args[++i];
                    break;

                case "--batch":
                    result.BatchSize = args[++i].SafeToInt32 (1);
                    break;

                case "--gradient":
                    result.GradientStep = args[++i].SafeToInt32 (1);
                    break;

                case "--data":
                case "--root":
                    result.DataRoot = args[++i];
                    break;

                case "--log":
                    result.LogDirectory = args[++i];
                    break;

                case "--width":
                    result.Width = args[++i].SafeToInt32 (512);
                    break;

                case "--height":
                    result.Height = args[++i].SafeToInt32 (512);
                    break;

                case "--step":
                case "--steps":
                case "--max-step":
                    result.MaxStep = args[++i].SafeToInt32 (100);
                    break;

                case "--clip":
                case "--clipping":
                    result.GradientClipping = args[++i];
                    break;

                case "--shuffle":
                    result.ShuffleTags = true;
                    break;

                case "--drop":
                    result.TagDropOut = true;
                    break;

                case "--latent-sampling":
                    result.LatentSamplingMethod = args[++i];
                    break;

                case "--image-every":
                    result.CreateImageEvery = args[++i].SafeToInt32 (100);
                    break;

                case "--save-every":
                    result.SaveEmbeddingEvery = args[++i].SafeToInt32 (100);
                    break;

                case "--template":
                    result.TemplateFileName = args[++i];
                    break;
            }
        }

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => JsonConvert.SerializeObject (this);

    #endregion
}
