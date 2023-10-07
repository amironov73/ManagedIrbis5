// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* PooledMemoryStreamManager.cs -- менеджер потоков для создания PooledMemoryStream
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

namespace AM.Buffers;

/// <summary>
/// Менеджер потоков для создания <see cref="PooledMemoryStream"/>.
/// </summary>
public abstract class PooledMemoryStreamManager
{
    /// <summary>
    /// Общий экземпляр <see cref="PooledMemoryStreamManager"/>.
    /// </summary>
    public static PooledMemoryStreamManager Shared { get; } =
        new DefaultPooledMemoryStreamManager (null);

    /// <summary>
    /// Получает экземпляр <see cref="PooledMemoryStream"/>,
    /// буферы которого управляются данным менеджером.
    /// </summary>
    public PooledMemoryStream GetStream()
    {
        return new PooledMemoryStream (this);
    }

    /// <summary>
    /// Создает или занимает из пула экземпляр <see cref="BufferSegment"/>.
    /// </summary>
    /// <param name="length">Минимальная длина буфера.</param>
    /// <returns>Полученный экземпляр <see cref="BufferSegment"/>.</returns>
    protected abstract BufferSegment AllocateBufferSegment (int length);

    /// <summary>
    /// Освобождает или возвращает в пул экземпляр <see cref="BufferSegment"/>
    /// </summary>
    /// <param name="segment">Объект, подлежащий возврату или освобождению.
    /// </param>
    protected abstract void FreeBufferSegment (BufferSegment segment);

    internal BufferSegment Allocate
        (
            int length
        )
    {
        length = Math.Max (length, 0);

        return AllocateBufferSegment (length);
    }

    internal void Free
        (
            BufferSegment segment
        )
    {
        Sure.NotNull (segment);

        FreeBufferSegment (segment);
    }
}
