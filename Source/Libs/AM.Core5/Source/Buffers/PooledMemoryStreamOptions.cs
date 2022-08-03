// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PooledMemoryStreamOptions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#nullable enable

namespace AM.Buffers;

/// <summary>
/// The default <see cref="PooledMemoryStreamManager"/> options.
/// </summary>
public class PooledMemoryStreamOptions
{
    #region Properties

    /// <summary>
    /// The minimum length of byte array rented from the pool.
    /// </summary>
    public int MinimumSegmentSize { get; set; } = 4096;

    /// <summary>
    /// The maximum length of byte array rented from the pool.
    /// </summary>
    public int MaximumSegmentSize { get; set; } = 81920;

    #endregion
}
