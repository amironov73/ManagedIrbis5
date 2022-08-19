// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* TextBoxTraceListener.cs -- trace listener that uses TextBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// <see cref="TraceListener"/> that uses <see cref="TextBox"/>
/// to write trace messages.
/// </summary>
public class TextBoxTraceListener
    : TraceListener
{
    #region Properties

    /// <summary>
    /// Gets the text box used to write trace messages.
    /// </summary>
    /// <value>The text box used.</value>
    public TextBox TextBox { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public TextBoxTraceListener
        (
            TextBox textBox
        )
    {
        Sure.NotNull (textBox);

        TextBox = textBox;
        TextBox.Disposed += _textBox_Disposed;
    }

    #endregion

    #region Private members

    private bool _disposed;

    /// <summary>
    /// Отрабатываем ситуацию, когда текстбокс, в который мы писали,
    /// неожиданно для нас был удален.
    /// </summary>
    private void _textBox_Disposed
        (
            object? sender,
            EventArgs e
        )
    {
        _disposed = true;
        Trace.Listeners.Remove (this);
    }

    #endregion

    #region TraceListener members

    /// <inheritdoc cref="TraceListener.Write(string?)"/>
    public override void Write
        (
            string? message
        )
    {
        if (!_disposed)
        {
            if (TextBox.InvokeRequired)
            {
                TextBox.Invoke ((MethodInvoker) delegate { Write (message); });
            }
            else
            {
                if (!string.IsNullOrEmpty (message))
                {
                    TextBox.AppendText (message);
                    TextBox.SelectionStart = TextBox.TextLength;
                }
            }
        }
    }

    /// <inheritdoc cref="TraceListener.WriteLine(string?)"/>
    public override void WriteLine
        (
            string? message
        )
    {
        if (!_disposed)
        {
            if (TextBox.InvokeRequired)
            {
                TextBox.Invoke ((MethodInvoker) delegate { WriteLine (message); });
            }
            else
            {
                TextBox.AppendText (message + Environment.NewLine);
                TextBox.SelectionStart = TextBox.TextLength;
            }
        }
    }

    /// <inheritdoc cref="TraceListener.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        base.Dispose (disposing);
        if (disposing && !_disposed)
        {
            _disposed = true;
            TextBox.Dispose();
        }
    }

    #endregion
}
