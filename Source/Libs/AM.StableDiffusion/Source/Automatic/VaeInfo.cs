// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* VaeInfo.cs -- информация о VAE
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

using Newtonsoft.Json.Linq;

#endregion

namespace AM.StableDiffusion.Automatic;

/// <summary>
/// Информация о VAE.
/// </summary>
[PublicAPI]
public sealed class VaeInfo
    : JObject
{
    // пока пустое тело класса
}
