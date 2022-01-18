// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InvokeWriter.cs -- текстовый поток, умеющий перенаправлять вызовы в UI-thread
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Text;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.IO;

/// <summary>
/// Текстовый поток, умеющий перенаправлять вызовы в UI-thread.
/// </summary>
public sealed class InvokeWriter
    : TextWriter
{
    #region Properties

    /// <summary>
    /// Оборачиваемый поток.
    /// </summary>
    public TextWriter InnerWriter { get; }

    /// <summary>
    /// Контрол, созданный в UI-thread
    /// </summary>
    public Control Control { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public InvokeWriter
        (
            TextWriter innerWriter,
            Control control
        )
    {
        Sure.NotNull (innerWriter);
        Sure.NotNull (control);

        InnerWriter = innerWriter;
        Control = control;
    }

    #endregion

    #region TextWriter members

    /// <inheritdoc cref="TextWriter.Encoding"/>
    public override Encoding Encoding
    {
        get
        {
            if (Control.InvokeRequired)
            {
                return Control.Invoke (() => InnerWriter.Encoding);
            }

            return InnerWriter.Encoding;
        }
    }

    /// <inheritdoc cref="TextWriter.Write(char)"/>
    public override void Write
        (
            char value
        )
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerWriter.Write (value));
        }
        else
        {
            InnerWriter.Write (value);
        }
    }

    /// <inheritdoc cref="TextWriter.Write(char[],int,int)"/>
    public override void Write
        (
            char[] buffer,
            int index,
            int count
        )
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerWriter.Write (buffer, index, count));
        }
        else
        {
            InnerWriter.Write (buffer, index, count);
        }
    }

    /// <inheritdoc cref="TextWriter.Write(string?)"/>
    public override void Write
        (
            string? value
        )
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerWriter.Write (value));
        }
        else
        {
            InnerWriter.Write (value);
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(char[],int,int)"/>
    public override void WriteLine
        (
            char[] buffer,
            int index,
            int count
        )
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerWriter.WriteLine (buffer, index, count));
        }
        else
        {
            InnerWriter.WriteLine (buffer, index, count);
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine()"/>
    public override void WriteLine()
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerWriter.WriteLine());
        }
        else
        {
            InnerWriter.WriteLine();
        }
    }

    /// <inheritdoc cref="TextWriter.WriteLine(string?)"/>
    public override void WriteLine
        (
            string? value
        )
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerWriter.WriteLine (value));
        }
        else
        {
            InnerWriter.WriteLine (value);
        }
    }

    /// <inheritdoc cref="TextWriter.Flush"/>
    public override void Flush()
    {
        if (Control.InvokeRequired)
        {
            Control.Invoke (() => InnerWriter.Flush());
        }
        else
        {
            InnerWriter.Flush();
        }
    }

    /// <inheritdoc cref="TextWriter.Close"/>
    public override void Close()
    {
        // освобождать обернутый поток должен его владелец
    }

    /// <inheritdoc cref="TextWriter.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        // освобождать обернутый поток должен его владелец
    }

    #endregion
}
