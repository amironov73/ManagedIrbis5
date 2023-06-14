// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* ModelType.cs -- известные типы моделей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.StableDiffusion.CivitAI;

/// <summary>
/// Известные типы моделей.
/// </summary>
[PublicAPI]
public static class ModelType
{
    #region Constants

    /// <summary>
    /// Чекпоинт.
    /// </summary>
    public const string Checkpoint = "Checkpoint";

    /// <summary>
    /// Текстовая инверсия.
    /// </summary>
    public const string TextualInversion = "TextualInversion";

    /// <summary>
    /// Гиперсеть.
    /// </summary>
    public const string Hypernetwork = "Hypernetwork";

    /// <summary>
    /// Градиент.
    /// </summary>
    public const string AestheticGradient = "AestheticGradient";

    /// <summary>
    /// LoRA.
    /// </summary>
    public const string Lora = "LORA";

    /// <summary>
    /// Control.Net.
    /// </summary>
    public const string ControlNet = "Controlnet";

    /// <summary>
    /// Позы.
    /// </summary>
    public const string Poses = "Poses";

    #endregion
}
