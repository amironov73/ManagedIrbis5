// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PooledMemoryStreamOptions.cs -- опции по умолчанию для PooledMemoryStreamManager
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.Buffers;

/// <summary>
/// Опции по умолчанию для <see cref="PooledMemoryStreamManager"/>.
/// </summary>
public class PooledMemoryStreamOptions
{
    #region Properties

    /// <summary>
    /// Минимальная длина массива байт, занимаемого из пула.
    /// </summary>
    public int MinimumSegmentSize { get; set; } = 4096;

    /// <summary>
    /// Максимальная длина массива байт, занимаемого из пула.
    /// </summary>
    public int MaximumSegmentSize { get; set; } = 81920;

    #endregion
}
