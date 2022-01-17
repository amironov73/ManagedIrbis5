// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo

/* ReadOnlyStream.cs -- обертка, не позволяющая записывать в поток
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

#endregion

#nullable enable

namespace AM.IO;

/// <summary>
/// Обертка, не позволяющая записывать в поток.
/// </summary>
public sealed class ReadOnlyStream
    : Stream
{
    #region Properties

    /// <summary>
    /// Обертываемый поток.
    /// </summary>
    public Stream Inner { get; }

    /// <summary>
    /// "Тихий режим", когда не возбуждаются исключения.
    /// </summary>
    public bool SilentMode { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ReadOnlyStream
        (
            Stream inner,
            bool silentMode = true
        )
    {
        Sure.NotNull (inner);

        Inner = inner;
        SilentMode = silentMode;
    }

    #endregion

    #region Stream members

    /// <inheritdoc cref="Stream.Flush"/>
    public override void Flush()
    {
        if (!SilentMode)
        {
            throw new NotSupportedException (nameof (Flush));
        }
    }

    /// <inheritdoc cref="Stream.Read(byte[],int,int)"/>
    public override int Read
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        return Inner.Read (buffer, offset, count);
    }

    /// <inheritdoc cref="Stream.Seek"/>
    public override long Seek
        (
            long offset,
            SeekOrigin origin
        )
    {
        return Inner.Seek (offset, origin);
    }

    /// <inheritdoc cref="Stream.SetLength"/>
    public override void SetLength
        (
            long value
        )
    {
        if (!SilentMode)
        {
            throw new NotSupportedException (nameof (SetLength));
        }
    }

    /// <inheritdoc cref="Stream.Write(byte[],int,int)"/>
    public override void Write
        (
            byte[] buffer,
            int offset,
            int count
        )
    {
        if (!SilentMode)
        {
            throw new NotSupportedException (nameof (Write));
        }
    }

    /// <inheritdoc cref="Stream.CanRead"/>
    public override bool CanRead => Inner.CanRead;

    /// <inheritdoc cref="Stream.CanSeek"/>
    public override bool CanSeek => Inner.CanSeek;

    /// <inheritdoc cref="Stream.CanWrite"/>
    public override bool CanWrite => false;

    /// <inheritdoc cref="Stream.Length"/>
    public override long Length => Inner.Length;

    /// <inheritdoc cref="Stream.Position"/>
    public override long Position
    {
        get => Inner.Position;
        set => Inner.Position = value;

    }

    #endregion
}
