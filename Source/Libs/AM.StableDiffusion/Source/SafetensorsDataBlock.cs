// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* SafetensorsDataBlock.cs -- блок данныых safetensors
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.StableDiffusion;

/// <summary>
/// Блок данных safetensors.
/// </summary>
public sealed class SafetensorsDataBlock
{
    #region Properties

    /// <summary>
    /// Имя блока.
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Тип блока.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Форма блока.
    /// </summary>
    public int[]? Shape { get; set; }

    /// <summary>
    /// Смещения.
    /// </summary>
    public int[]? Offsets { get; set; }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString() => $"{Name}: {Type}";

    #endregion
}
