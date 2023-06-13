// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable AccessToDisposedClosure
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* MemoryResponse.cs -- ответ на запрос о памяти
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#endregion

#nullable enable

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Ответ на запрос о памяти.
/// </summary>
[PublicAPI]
public sealed class MemoryResponse
{
    #region Properties

    /// <summary>
    /// Информация об ОЗУ.
    /// </summary>
    [JsonProperty ("ram")]
    public JObject? Ram { get; set; }

    /// <summary>
    /// Информация о CUDA.
    /// </summary>
    [JsonProperty ("cuda")]
    public JObject? Cuda { get; set; }

    #endregion
}
