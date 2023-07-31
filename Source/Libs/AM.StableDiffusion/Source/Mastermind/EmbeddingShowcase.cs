// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EmbeddingInfo.cs -- витрина текстовой инверсии
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using AM.Collections;

using DynamicData;

using JetBrains.Annotations;

using Microsoft.Extensions.Logging;

using SixLabors.ImageSharp;

#endregion

#nullable enable

namespace AM.StableDiffusion.Mastermind;

/// <summary>
/// Витрина текстовой инверсии.
/// </summary>
[PublicAPI]
public sealed class EmbeddingShowcase
    : ShowcaseBase
{
    #region Properties

    /// <summary>
    /// Предназначено для негативного промпта?
    /// </summary>
    public bool IsNegative { get; set; }

    /// <summary>
    /// Базовая модель, например, "SD 1.5".
    /// </summary>
    public string? BaseModel { get; set; }

    /// <summary>
    /// Число шагов тренировки, например, 2000.
    /// </summary>
    public int? TrainingSteps { get; set; }

    /// <summary>
    /// Число эпох тренировки, например, 100.
    /// </summary>
    public int? TrainingEpochs { get; set; }

    /// <summary>
    /// Clip skip, например, 2.
    /// </summary>
    public int? ClipSkip { get; set; }

    /// <summary>
    /// Слова активации, например,
    /// ["AM110_GRACIE", "ANNA_NETREBKO" }.
    /// </summary>
    public string[]? TriggerWords { get; set; }

    #endregion
}
