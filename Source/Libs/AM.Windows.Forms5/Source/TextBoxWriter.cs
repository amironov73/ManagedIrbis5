// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnassignedField.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* TextBoxWriter.cs -- TextWriter, пишущий в TextBox
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows.Forms;

#endregion

#nullable enable

namespace AM.Windows.Forms;

/// <summary>
/// <see cref="T:System.IO.TextWriter"/>, пишущий в
/// <see cref="T:System.Windows.Forms.TextBox"/>.
/// </summary>
public class TextBoxWriter
    : TextWriter
{
    #region Events

    /// <summary>
    /// Возникает при прокрутке содержимого.
    /// </summary>
    public event EventHandler? Scroll;

    #endregion

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether auto scroll
    /// is enabled.
    /// </summary>
    [DefaultValue (false)]
    public bool AutoScroll { get; set; }

    /// <summary>
    /// Gets the text box.
    /// </summary>
    /// <value>The text box.</value>
    public TextBoxBase TextBox { get; }

    #endregion

    #region Construction

    /// <summary>
    /// Initializes a new instance of the
    /// <see cref="TextBoxWriter"/> class.
    /// </summary>
    /// <param name="textBox">The text box.</param>
    public TextBoxWriter
        (
            TextBoxBase textBox
        )
    {
        Sure.NotNull (textBox);

        TextBox = textBox;
        TextBox.Disposed += _textBox_Disposed;
    }

    #endregion

    #region Private members

    private bool _disposed;
    private bool _textBoxDisposed;

    private void _CheckDisposed()
    {
        if (_disposed || _textBoxDisposed)
        {
            throw new ObjectDisposedException (nameof (TextBoxWriter));
        }
    }

    private void _textBox_Disposed
        (
            object? sender,
            EventArgs e
        )
    {
        _textBoxDisposed = true;
    }

    /// <summary>
    /// Called when scrolling occurs.
    /// </summary>
    protected virtual void OnScroll()
    {
        var handler = Scroll;
        handler?.Invoke (this, EventArgs.Empty);
        TextBox.SelectionStart = TextBox.TextLength;
        TextBox.SelectionLength = 0;
    }

    #endregion

    #region TextWriter members

    /// <inheritdoc cref="TextWriter.Dispose(bool)"/>
    protected override void Dispose
        (
            bool disposing
        )
    {
        base.Dispose (disposing);
        TextBox.Disposed -= _textBox_Disposed;
        _disposed = true;
    }

    /// <inheritdoc cref="TextWriter.Encoding"/>
    public override Encoding Encoding => Encoding.Unicode;

    /// <summary>
    /// Writes a character to the text stream.
    /// </summary>
    public override void Write
        (
            char value
        )
    {
        _CheckDisposed();
        TextBox.AppendText (new string (value, 1));
        OnScroll();
    }

    /// <summary>
    /// Writes a string to the text stream.
    /// </summary>
    /// <param name="value">The string to write.</param>
    public override void Write
        (
            string? value
        )
    {
        if (!string.IsNullOrEmpty (value))
        {
            _CheckDisposed();
            TextBox.AppendText (value);
            OnScroll();
        }
    }

    /// <summary>
    /// Writes a line terminator to the text stream.
    /// </summary>
    public override void WriteLine()
    {
        _CheckDisposed();
        TextBox.AppendText (Environment.NewLine);
        OnScroll();
    }

    /// <summary>
    /// Writes a string followed by a line terminator to the text stream.
    /// </summary>
    public override void WriteLine
        (
            string? value
        )
    {
        if (!string.IsNullOrEmpty (value))
        {
            _CheckDisposed();
            TextBox.AppendText (value + Environment.NewLine);
            OnScroll();
        }
    }

    #endregion
}
