// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* InvokeReader.cs -- текстовый поток, умеющий перенаправлять вызовы в UI-thread
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.IO;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms.IO;

/// <summary>
/// Текстовый поток, умеющий перенаправлять вызовы в UI-thread.
/// </summary>
public sealed class InvokeReader
    : TextReader
{
    #region Properties

    /// <summary>
    /// Оборачиваемый поток.
    /// </summary>
    public TextReader InnerReader { get; }

    /// <summary>
    /// Контрол, созданный в UI-thread
    /// </summary>
    public Control Control { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор
    /// </summary>
    public InvokeReader
        (
            TextReader innerReader,
            Control control
        )
    {
        Sure.NotNull (innerReader);
        Sure.NotNull (control);

        InnerReader = innerReader;
        Control = control;
    }

    #endregion

    #region TextReader members

    /// <inheritdoc cref="TextReader.Peek"/>
    public override int Peek()
    {
        if (Control.InvokeRequired)
        {
            return Control.Invoke (() => InnerReader.Peek());
        }

        return InnerReader.Peek();
    }

    /// <inheritdoc cref="TextReader.Read()"/>
    public override int Read()
    {
        if (Control.InvokeRequired)
        {
            return Control.Invoke (() => InnerReader.Read());
        }

        return InnerReader.Read();
    }

    /// <inheritdoc cref="TextReader.Read(char[],int,int)"/>
    public override int Read
        (
            char[] buffer,
            int index,
            int count
        )
    {
        if (Control.InvokeRequired)
        {
            return Control.Invoke (() => InnerReader.Read (buffer, index, count));
        }

        return InnerReader.Read (buffer, index, count);
    }

    /// <inheritdoc cref="TextReader.ReadLine"/>
    public override string? ReadLine()
    {
        if (Control.InvokeRequired)
        {
            return Control.Invoke (() => InnerReader.ReadLine());
        }

        return InnerReader.ReadLine();
    }

    /// <inheritdoc cref="TextReader.ReadToEnd"/>
    public override string ReadToEnd()
    {
        if (Control.InvokeRequired)
        {
            return Control.Invoke (() => InnerReader.ReadToEnd());
        }

        return InnerReader.ReadToEnd();
    }

    /// <inheritdoc cref="TextReader.ReadBlock(char[],int,int)"/>
    public override int ReadBlock
        (
            char[] buffer,
            int index,
            int count
        )
    {
        if (Control.InvokeRequired)
        {
            return Control.Invoke (() => InnerReader.ReadBlock (buffer, index, count));
        }

        return InnerReader.ReadBlock (buffer, index, count);
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
