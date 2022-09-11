// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local

/*
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics;
using System.IO;

using AM;

using PdfSharpCore.Pdf.Internal;

#endregion

#nullable enable

namespace PdfSharpCore.Pdf.Content;

/// <summary>
/// Represents a writer for generation of PDF streams.
/// </summary>
internal class ContentWriter
{
    public ContentWriter (Stream contentStream)
    {
        _stream = contentStream;
#if DEBUG

        //layout = PdfWriterLayout.Verbose;
#endif
    }

    public void Close (bool closeUnderlyingStream)
    {
        if (_stream != null && closeUnderlyingStream)
        {
            _stream.Dispose();
            _stream = null;
        }
    }

    public void Close()
    {
        Close (true);
    }

    public int Position => (int) (_stream?.Position ?? 0);

    //public PdfWriterLayout Layout
    //{
    //  get { return layout; }
    //  set { layout = value; }
    //}
    //PdfWriterLayout layout;

    //public PdfWriterOptions Options
    //{
    //  get { return options; }
    //  set { options = value; }
    //}
    //PdfWriterOptions options;

    // -----------------------------------------------------------

    /// <summary>
    /// Writes the specified value to the PDF stream.
    /// </summary>
    public void Write (bool value)
    {
        //WriteSeparator(CharCat.Character);
        //WriteRaw(value ? bool.TrueString : bool.FalseString);
        //lastCat = CharCat.Character;
    }

    public void WriteRaw (string rawString)
    {
        if (string.IsNullOrEmpty (rawString))
        {
            return;
        }

        //AppendBlank(rawString[0]);
        var bytes = PdfEncoders.RawEncoding.GetBytes (rawString);
        var stream = _stream.ThrowIfNull();
        stream.ThrowIfNull().Write (bytes, 0, bytes.Length);
        _lastCat = GetCategory ((char) bytes[^1]);
    }

    public void WriteLineRaw (string rawString)
    {
        if (string.IsNullOrEmpty (rawString))
        {
            return;
        }

        //AppendBlank(rawString[0]);
        var bytes = PdfEncoders.RawEncoding.GetBytes (rawString);
        var stream = _stream.ThrowIfNull();
        stream.Write (bytes, 0, bytes.Length);
        stream.Write (new [] { (byte)'\n' }, 0, 1);
        _lastCat = GetCategory ((char) bytes[^1]);
    }

    public void WriteRaw (char ch)
    {
        Debug.Assert (ch < 256, "Raw character greater than 255 detected.");
        var stream = _stream.ThrowIfNull();
        stream.WriteByte ((byte)ch);
        _lastCat = GetCategory (ch);
    }

    /// <summary>
    /// Gets or sets the indentation for a new indentation level.
    /// </summary>
    protected internal int Indent { get; set; } = 2;

    protected int _writeIndent;

    /// <summary>
    /// Increases indent level.
    /// </summary>
    private void IncreaseIndent()
    {
        _writeIndent += Indent;
    }

    /// <summary>
    /// Decreases indent level.
    /// </summary>
    private void DecreaseIndent()
    {
        _writeIndent -= Indent;
    }

    /// <summary>
    /// Gets an indent string of current indent.
    /// </summary>
    private string IndentBlanks => new string (' ', _writeIndent);

    private void WriteIndent()
    {
        WriteRaw (IndentBlanks);
    }

    private void WriteSeparator
        (
            CharCat cat,
            char ch = '\0'
        )
    {
        cat.NotUsed();
        ch.NotUsed();

        switch (_lastCat)
        {
            //case CharCat.NewLine:
            //  if (this.layout == PdfWriterLayout.Verbose)
            //    WriteIndent();
            //  break;

            case CharCat.Delimiter:
                break;

            //case CharCat.Character:
            //  if (this.layout == PdfWriterLayout.Verbose)
            //  {
            //    //if (cat == CharCat.Character || ch == '/')
            //    this.stream.WriteByte((byte)' ');
            //  }
            //  else
            //  {
            //    if (cat == CharCat.Character)
            //      this.stream.WriteByte((byte)' ');
            //  }
            //  break;
        }
    }

    public void NewLine()
    {
        if (_lastCat != CharCat.NewLine)
        {
            WriteRaw ('\n');
        }
    }

    private CharCat GetCategory (char ch)
    {
        ch.NotUsed();

        //if (Lexer.IsDelimiter(ch))
        //  return CharCat.Delimiter;
        //if (ch == Chars.LF)
        //  return CharCat.NewLine;
        return CharCat.Character;
    }

    private enum CharCat
    {
        NewLine,
        Character,
        Delimiter,
    };

    private CharCat _lastCat;

    /// <summary>
    /// Gets the underlying stream.
    /// </summary>
    internal Stream Stream => _stream.ThrowIfNull();

    private Stream? _stream;
}
