// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* FontWriter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;

using AM;

#endregion

#nullable enable

namespace PdfSharpCore.Fonts;

/// <summary>
/// Represents a writer for generation of font file streams.
/// </summary>
internal class FontWriter
{
    #region Construction

    /// <summary>
    /// Initializes a new instance of the <see cref="FontWriter"/> class.
    /// Data is written in Motorola format (big-endian).
    /// </summary>
    public FontWriter
        (
            Stream stream
        )
    {
        Sure.NotNull (stream);

        _stream = stream;
    }

    #endregion

    /// <summary>
    /// Closes the writer and, if specified, the underlying stream.
    /// </summary>
    public void Close (bool closeUnderlyingStream)
    {
        if (_stream != null && closeUnderlyingStream)
        {
            _stream.Dispose();
        }

        _stream = null;
    }

    /// <summary>
    /// Closes the writer and the underlying stream.
    /// </summary>
    public void Close()
    {
        Close (true);
    }

    /// <summary>
    /// Gets or sets the position within the stream.
    /// </summary>
    public int Position
    {
        get => (int) _stream!.Position;
        set => _stream!.Position = value;
    }

    /// <summary>
    /// Writes the specified value to the font stream.
    /// </summary>
    public void WriteByte (byte value)
    {
        _stream?.WriteByte (value);
    }

    /// <summary>
    /// Writes the specified value to the font stream.
    /// </summary>
    public void WriteByte (int value)
    {
        _stream?.WriteByte ((byte)value);
    }

    /// <summary>
    /// Writes the specified value to the font stream using big-endian.
    /// </summary>
    public void WriteShort (short value)
    {
        if (_stream is not null)
        {
            _stream.WriteByte ((byte)(value >> 8));
            _stream.WriteByte ((byte)value);
        }
    }

    /// <summary>
    /// Writes the specified value to the font stream using big-endian.
    /// </summary>
    public void WriteShort (int value)
    {
        WriteShort ((short)value);
    }

    /// <summary>
    /// Writes the specified value to the font stream using big-endian.
    /// </summary>
    public void WriteUShort (ushort value)
    {
        if (_stream is not null)
        {
            _stream.WriteByte ((byte)(value >> 8));
            _stream.WriteByte ((byte)value);
        }
    }

    /// <summary>
    /// Writes the specified value to the font stream using big-endian.
    /// </summary>
    public void WriteUShort (int value)
    {
        WriteUShort ((ushort)value);
    }

    /// <summary>
    /// Writes the specified value to the font stream using big-endian.
    /// </summary>
    public void WriteInt (int value)
    {
        if (_stream is not null)
        {
            _stream.WriteByte ((byte)(value >> 24));
            _stream.WriteByte ((byte)(value >> 16));
            _stream.WriteByte ((byte)(value >> 8));
            _stream.WriteByte ((byte)value);
        }
    }

    /// <summary>
    /// Writes the specified value to the font stream using big-endian.
    /// </summary>
    public void WriteUInt (uint value)
    {
        if (_stream is not null)
        {
            _stream.WriteByte ((byte)(value >> 24));
            _stream.WriteByte ((byte)(value >> 16));
            _stream.WriteByte ((byte)(value >> 8));
            _stream.WriteByte ((byte)value);
        }
    }

    //public short ReadFWord()
    //public ushort ReadUFWord()
    //public long ReadLongDate()
    //public string ReadString(int size)

    public void Write (byte[] buffer)
    {
        _stream?.Write (buffer, 0, buffer.Length);
    }

    public void Write (byte[] buffer, int offset, int count)
    {
        _stream?.Write (buffer, offset, count);
    }

    /// <summary>
    /// Gets the underlying stream.
    /// </summary>
    internal Stream Stream => _stream!;

    private Stream? _stream;
}
