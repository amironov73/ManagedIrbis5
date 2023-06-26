// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* GenerationData.cs -- информация о генерации изображения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Text;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.StableDiffusion.PromptEngineering;

/// <summary>
/// Информация о генерации изображения.
/// </summary>
[PublicAPI]
public sealed class GenerationData
{
    #region Properties

    /// <summary>
    /// Позитивный промпт.
    /// </summary>
    public string? Prompt { get; set; }

    /// <summary>
    /// Негативный промпт.
    /// </summary>
    public string? NegativePrompt { get; set; }

    /// <summary>
    /// Наименование модели, использованной для генерации.
    /// </summary>
    public string? ModelName { get; set; }

    /// <summary>
    /// Хеш-сумма модели, использованной для генерации.
    /// </summary>
    public string? ModelHash { get; set; }

    /// <summary>
    /// Семплер.
    /// </summary>
    public string? Sampler { get; set; }

    /// <summary>
    /// Количество шагов.
    /// </summary>
    public string? Steps { get; set; }

    /// <summary>
    /// Сила промпта.
    /// </summary>
    public string? CfgScale { get; set; }

    /// <summary>
    /// Начальное значение.
    /// </summary>
    public string? Seed { get; set; }

    /// <summary>
    /// Размер изображения в пикселах.
    /// </summary>
    public string? Size { get; set; }

    /// <summary>
    /// Другие параметры, если имеются.
    /// </summary>
    public Dictionary<string, string>? Other { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Разбор пар "ключ-значение"
    /// </summary>
    public static GenerationData Parse
        (
            IDictionary<string, string> dictionary
        )
    {
        Sure.NotNull (dictionary);

        var result = new GenerationData();
        foreach (var pair in dictionary)
        {
            switch (pair.Key)
            {
                case "":
                case "Prompt":
                    result.Prompt = pair.Value;
                    break;

                case "Negative Prompt":
                    result.NegativePrompt = pair.Value;
                    break;

                case "Model":
                    result.ModelName = pair.Value;
                    break;

                case "Model hash":
                    result.ModelHash = pair.Value;
                    break;

                case "Sampler":
                    result.Sampler = pair.Value;
                    break;

                case "Steps":
                    result.Steps = pair.Value;
                    break;

                case "Seed":
                    result.Seed = pair.Value;
                    break;

                case "CFG scale":
                    result.CfgScale = pair.Value;
                    break;

                case "Size":
                    result.Size = pair.Value;
                    break;

                default:
                    result.Other ??= new ();
                    result.Other.Add (pair.Key, pair.Value);
                    break;
            }
        }

        return result;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        var builder = new StringBuilder();

        if (!string.IsNullOrEmpty (Prompt))
        {
            builder.AppendLine (Prompt);
        }

        if (!string.IsNullOrEmpty (NegativePrompt))
        {
            builder.AppendLine (NegativePrompt);
        }

        if (!string.IsNullOrEmpty (ModelName))
        {
            builder.Append ($"Model: {ModelName}, ");
        }

        if (!string.IsNullOrEmpty (ModelHash))
        {
            builder.Append ($"Model: {ModelHash}, ");
        }

        return builder.ToString();
    }

    #endregion
}
