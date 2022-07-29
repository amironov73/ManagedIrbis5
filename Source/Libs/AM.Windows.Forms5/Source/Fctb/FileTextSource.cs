// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* FileTextSource.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

#endregion

#nullable enable

namespace Fctb;

/// <summary>
/// This class contains the source text (chars and styles).
/// It stores a text lines, the manager of commands, undo/redo stack, styles.
/// </summary>
public class FileTextSource
    : TextSource
{
    #region Events

    /// <summary>
    /// Occurs when need to display line in the textbox
    /// </summary>
    public event EventHandler<LineNeededEventArgs>? LineNeeded;

    /// <summary>
    /// Occurs when need to save line in the file
    /// </summary>
    public event EventHandler<LinePushedEventArgs>? LinePushed;

    #endregion

    #region Propeties



    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public FileTextSource
        (
            SyntaxTextBox currentTextBox
        )
        : base (currentTextBox)
    {
        _timer.Interval = 10000;
        _timer.Tick += timer_Tick;
        _timer.Enabled = true;

        SaveEOL = Environment.NewLine;
    }

    #endregion

    #region Private members

    private List<int> _sourceFileLinePositions = new ();

    private FileStream _stream;

    private Encoding _encoding;

    private readonly System.Windows.Forms.Timer _timer = new ();

    private void UnloadUnusedLines()
    {
        const int margin = 2000;
        var iStartVisibleLine = CurrentTextBox.VisibleRange.Start.Line;
        var iFinishVisibleLine = CurrentTextBox.VisibleRange.End.Line;

        var count = 0;
        for (var i = 0; i < Count; i++)
        {
            if (base.lines[i] != null && !base.lines[i].IsChanged && Math.Abs (i - iFinishVisibleLine) > margin)
            {
                lines[i] = null;
                count++;
            }
        }
    }

    void timer_Tick
        (
            object? sender,
            EventArgs e
        )
    {
        _timer.Enabled = false;
        try
        {
            UnloadUnusedLines();
        }
        finally
        {
            _timer.Enabled = true;
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Открытие файла.
    /// </summary>
    public void OpenFile
        (
            string fileName,
            Encoding encoding
        )
    {
        Clear();

        if (_stream != null)
        {
            _stream.Dispose();
        }

        SaveEOL = Environment.NewLine;

        //read lines of file
        _stream = new FileStream (fileName, FileMode.Open);
        var length = _stream.Length;

        //read signature
        encoding = DefineEncoding (encoding, _stream);
        var shift = DefineShift (encoding);

        //first line
        _sourceFileLinePositions.Add ((int)_stream.Position);
        base.lines.Add (null);

        //other lines
        _sourceFileLinePositions.Capacity = (int)(length / 7 + 1000);

        //int prev = 0;
        //while(fs.Position < length)
        //{
        //    var b = fs.ReadByte();

        //    if (b == 10)// \n
        //    {
        //        sourceFileLinePositions.Add((int)(fs.Position) + shift);
        //        base.lines.Add(null);
        //    }else
        //    if (prev == 13)// \r (Mac format)
        //    {
        //        sourceFileLinePositions.Add((int)(fs.Position - 1) + shift);
        //        base.lines.Add(null);
        //        SaveEOL = "\r";
        //    }

        //    prev = b;
        //}

        //if (prev == 13)
        //{
        //    sourceFileLinePositions.Add((int)(fs.Position) + shift);
        //    base.lines.Add(null);
        //}

        var prev = 0;
        var prevPos = 0;
        var br = new BinaryReader (_stream, encoding);
        while (_stream.Position < length)
        {
            prevPos = (int)_stream.Position;
            var b = br.ReadChar();

            if (b == 10) // \n
            {
                _sourceFileLinePositions.Add ((int)_stream.Position);
                base.lines.Add (null);
            }
            else if (prev == 13) // \r (Mac format)
            {
                _sourceFileLinePositions.Add ((int)prevPos);
                base.lines.Add (null);
                SaveEOL = "\r";
            }

            prev = b;
        }

        if (prev == 13)
        {
            _sourceFileLinePositions.Add ((int)prevPos);
            base.lines.Add (null);
        }

        if (length > 2000000)
        {
            GC.Collect();
        }

        var temp = new Line[100];

        var c = base.lines.Count;
        base.lines.AddRange (temp);
        base.lines.TrimExcess();
        base.lines.RemoveRange (c, temp.Length);


        var temp2 = new int[100];
        c = base.lines.Count;
        _sourceFileLinePositions.AddRange (temp2);
        _sourceFileLinePositions.TrimExcess();
        _sourceFileLinePositions.RemoveRange (c, temp.Length);


        _encoding = encoding;

        OnLineInserted (0, Count);

        //load first lines for calc width of the text
        var linesCount = Math.Min (lines.Count, CurrentTextBox.ClientRectangle.Height / CurrentTextBox.CharHeight);
        for (var i = 0; i < linesCount; i++)
            LoadLineFromSourceFile (i);

        //
        NeedRecalc (new TextChangedEventArgs (0, linesCount - 1));
        if (CurrentTextBox.WordWrap)
        {
            OnRecalcWordWrap (new TextChangedEventArgs (0, linesCount - 1));
        }
    }

    #endregion

    private int DefineShift (Encoding enc)
    {
        if (enc.IsSingleByte)
        {
            return 0;
        }

        if (enc.HeaderName == "unicodeFFFE")
        {
            return 0; //UTF16 BE
        }

        if (enc.HeaderName == "utf-16")
        {
            return 1; //UTF16 LE
        }

        if (enc.HeaderName == "utf-32BE")
        {
            return 0; //UTF32 BE
        }

        return enc.HeaderName == "utf-32" ? 3 : 0;
    }

    private static Encoding DefineEncoding
        (
            Encoding encoding,
            FileStream stream
        )
    {
        var bytesPerSignature = 0;
        var signature = new byte[4];
        var c = stream.Read (signature, 0, 4);
        if (signature[0] == 0xFF && signature[1] == 0xFE && signature[2] == 0x00 && signature[3] == 0x00 && c >= 4)
        {
            encoding = Encoding.UTF32; //UTF32 LE
            bytesPerSignature = 4;
        }
        else if (signature[0] == 0x00 && signature[1] == 0x00 && signature[2] == 0xFE && signature[3] == 0xFF)
        {
            encoding = new UTF32Encoding (true, true); //UTF32 BE
            bytesPerSignature = 4;
        }
        else if (signature[0] == 0xEF && signature[1] == 0xBB && signature[2] == 0xBF)
        {
            encoding = Encoding.UTF8; //UTF8
            bytesPerSignature = 3;
        }
        else if (signature[0] == 0xFE && signature[1] == 0xFF)
        {
            encoding = Encoding.BigEndianUnicode; //UTF16 BE
            bytesPerSignature = 2;
        }
        else if (signature[0] == 0xFF && signature[1] == 0xFE)
        {
            encoding = Encoding.Unicode; //UTF16 LE
            bytesPerSignature = 2;
        }

        stream.Seek (bytesPerSignature, SeekOrigin.Begin);

        return encoding;
    }

    public void CloseFile()
    {
        if (_stream != null)
        {
            try
            {
                _stream.Dispose();
            }
            catch
            {
                ;
            }
        }

        _stream = null;
    }

    /// <summary>
    /// End Of Line characters used for saving
    /// </summary>
    public string SaveEOL { get; set; }

    public override void SaveToFile (string fileName, Encoding encoding)
    {
        //
        var newLinePos = new List<int> (Count);

        //create temp file
        var dir = Path.GetDirectoryName (fileName);
        var tempFileName = Path.Combine (dir, Path.GetFileNameWithoutExtension (fileName) + ".tmp");

        var sr = new StreamReader (_stream, _encoding);
        using (var tempFs = new FileStream (tempFileName, FileMode.Create))
        using (var sw = new StreamWriter (tempFs, encoding))
        {
            sw.Flush();

            for (var i = 0; i < Count; i++)
            {
                newLinePos.Add ((int)tempFs.Length);

                var sourceLine = ReadLine (sr, i); //read line from source file
                string line;

                var lineIsChanged = lines[i] != null && lines[i].IsChanged;

                if (lineIsChanged)
                {
                    line = lines[i].Text;
                }
                else
                {
                    line = sourceLine;
                }

                //call event handler
                if (LinePushed != null)
                {
                    var args = new LinePushedEventArgs (sourceLine, i, lineIsChanged ? line : null);
                    LinePushed (this, args);

                    if (args.SavedText != null)
                    {
                        line = args.SavedText;
                    }
                }

                //save line to file
                sw.Write (line);

                if (i < Count - 1)
                {
                    sw.Write (SaveEOL);
                }

                sw.Flush();
            }
        }

        //clear lines buffer
        for (var i = 0; i < Count; i++)
            lines[i] = null;

        //deattach from source file
        sr.Dispose();
        _stream.Dispose();

        //delete target file
        if (File.Exists (fileName))
        {
            File.Delete (fileName);
        }

        //rename temp file
        File.Move (tempFileName, fileName);

        //binding to new file
        _sourceFileLinePositions = newLinePos;
        _stream = new FileStream (fileName, FileMode.Open);
        this._encoding = encoding;
    }

    private string ReadLine (StreamReader sr, int i)
    {
        string line;
        var filePos = _sourceFileLinePositions[i];
        if (filePos < 0)
        {
            return "";
        }

        _stream.Seek (filePos, SeekOrigin.Begin);
        sr.DiscardBufferedData();
        line = sr.ReadLine();
        return line;
    }

    public override void ClearIsChanged()
    {
        foreach (var line in lines)
            if (line != null)
            {
                line.IsChanged = false;
            }
    }

    public override Line this [int i]
    {
        get
        {
            if (base.lines[i] != null)
            {
                return lines[i];
            }
            else
            {
                LoadLineFromSourceFile (i);
            }

            return lines[i];
        }
        set { throw new NotImplementedException(); }
    }

    private void LoadLineFromSourceFile (int i)
    {
        var line = CreateLine();
        _stream.Seek (_sourceFileLinePositions[i], SeekOrigin.Begin);
        var sr = new StreamReader (_stream, _encoding);

        var s = sr.ReadLine();
        if (s == null)
        {
            s = "";
        }

        //call event handler
        if (LineNeeded != null)
        {
            var args = new LineNeededEventArgs (s, i);
            LineNeeded (this, args);
            s = args.DisplayedLineText;
            if (s == null)
            {
                return;
            }
        }

        foreach (var c in s)
            line.Add (new Character (c));
        base.lines[i] = line;

        if (CurrentTextBox.WordWrap)
        {
            OnRecalcWordWrap (new TextChangedEventArgs (i, i));
        }
    }

    public override void InsertLine (int index, Line line)
    {
        _sourceFileLinePositions.Insert (index, -1);
        base.InsertLine (index, line);
    }

    public override void RemoveLine (int index, int count)
    {
        _sourceFileLinePositions.RemoveRange (index, count);
        base.RemoveLine (index, count);
    }

    public override void Clear()
    {
        base.Clear();
    }

    public override int GetLineLength (int index)
    {
        if (base.lines[index] == null)
        {
            return 0;
        }
        else
        {
            return base.lines[index].Count;
        }
    }

    /// <summary>
    ///
    /// </summary>
    public override bool LineHasFoldingStartMarker (int index)
    {
        if (lines[index] == null)
        {
            return false;
        }

        return !string.IsNullOrEmpty (lines[index].FoldingStartMarker);
    }

    /// <summary>
    ///
    /// </summary>
    public override bool LineHasFoldingEndMarker (int index)
    {
        if (lines[index] == null)
        {
            return false;
        }
        else
        {
            return !string.IsNullOrEmpty (lines[index].FoldingEndMarker);
        }
    }

    public override void Dispose()
    {
        if (_stream != null)
        {
            _stream.Dispose();
        }

        _timer.Dispose();
    }

    internal void UnloadLine (int iLine)
    {
        if (lines[iLine] != null && !lines[iLine].IsChanged)
        {
            lines[iLine] = null;
        }
    }
}

class CharReader : TextReader
{
    public override int Read()
    {
        return base.Read();
    }
}
