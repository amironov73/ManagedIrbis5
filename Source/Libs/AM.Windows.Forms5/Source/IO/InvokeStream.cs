// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InvokeStream.cs -- поток, умеющий перенаправлять вызовы в UI-thread
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.IO;

/// <summary>
/// Поток, умеющий перенаправлять вызовы в UI-thread.
/// </summary>
public sealed class InvokeStream
    : Stream
{
    #region Properties

    /// <summary>
    /// Оборачиваемый поток.
    /// </summary>
    public Stream InnerStream { get; }

    /// <summary>
    /// Контрол, созданный в UI-thread
    /// </summary>
    public Control Control { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public InvokeStream
        (
            Stream innerStream,
            Control control
        )
    {
        Sure.NotNull (innerStream);
        Sure.NotNull (control);

        InnerStream = innerStream;
        Control = control;
    }

    #endregion

    #region Stream members

    /// <inheritdoc cref="Stream.Flush"/>
    public override void Flush()
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (InnerStream.Flush);
        }
        else
        {
            InnerStream.Flush();
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
        if (Control.InvokeRequired)
        {
            return Control.Invoke (() => InnerStream.Read (buffer, offset, count));
        }

        return InnerStream.Read (buffer, offset, count);
    }

    /// <inheritdoc cref="Stream.Seek"/>
    public override long Seek
        (
            long offset,
            SeekOrigin origin
        )
    {
        if (Control.InvokeRequired)
        {
            return Control.Invoke (() => InnerStream.Seek (offset, origin));
        }

        return InnerStream.Seek (offset, origin);
    }

    /// <inheritdoc cref="Stream.SetLength"/>
    public override void SetLength (long value)
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerStream.SetLength (value));
        }
        else
        {
            InnerStream.SetLength (value);
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
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerStream.Write (buffer, offset, count));
        }
        else
        {
            InnerStream.Write (buffer, offset, count);
        }
    }

    /// <inheritdoc cref="Stream.CanRead"/>
    public override bool CanRead
    {
        get
        {
            if (Control.InvokeRequired)
            {
                return Control.Invoke (() => InnerStream.CanRead);
            }

            return InnerStream.CanRead;
        }
    }

    /// <inheritdoc cref="Stream.CanSeek"/>
    public override bool CanSeek
    {
        get
        {
            if (Control.InvokeRequired)
            {
                return Control.Invoke (() => InnerStream.CanSeek);
            }

            return InnerStream.CanSeek;
        }
    }

    /// <inheritdoc cref="Stream.CanWrite"/>
    public override bool CanWrite
    {
        get
        {
            if (Control.InvokeRequired)
            {
                return Control.Invoke (() => InnerStream.CanWrite);
            }

            return InnerStream.CanWrite;
        }
    }

    /// <inheritdoc cref="Stream.Length"/>
    public override long Length
    {
        get
        {
            if (Control.InvokeRequired)
            {
                return Control.Invoke (() => InnerStream.Length);
            }

            return InnerStream.Length;
        }
    }

    /// <inheritdoc cref="Stream.Position"/>
    public override long Position
    {
        get
        {
            if (Control.InvokeRequired)
            {
                return Control.Invoke (() => InnerStream.Position);
            }

            return InnerStream.Position;
        }
        set
        {
            if (Control.InvokeRequired)
            {
                Control.Invoke (() => InnerStream.Position = value);
            }

            InnerStream.Position = value;
        }
    }

    #endregion
}
