// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* EmbeddingInfo.cs -- витрина модели
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
/// Витрина модели.
/// </summary>
[PublicAPI]
public class CheckpointShowcase
    : ShowcaseBase
{
    #region Properties

    /// <summary>
    /// Базовая модель, например, "SD 1.5".
    /// </summary>
    public string? BaseModel { get; set; }

    #endregion
}
