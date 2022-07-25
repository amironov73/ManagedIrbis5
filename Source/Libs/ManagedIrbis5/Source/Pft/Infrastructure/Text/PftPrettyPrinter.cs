// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftPrettyPrinter.cs -- красивый вывод PFT-скриптов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.Text;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Text;

/// <summary>
/// Красивый вывод PFT-скриптов.
/// </summary>
public sealed class PftPrettyPrinter
    : IDisposable
{
    #region Properties

    /// <summary>
    /// Номер текущей колонки.
    /// </summary>
    public int Column { get; private set; }

    /// <summary>
    /// Ещё не было вывода текста?
    /// </summary>
    public bool IsEmpty => _writer.GetStringBuilder().Length != 0;

    /// <summary>
    /// Ширина отступа.
    /// </summary>
    public int IndentWidth { get; set; }

    /// <summary>
    /// Последний выведенный символ.
    /// </summary>
    public char LastCharacter => _writer.GetStringBuilder().GetLastChar();

    /// <summary>
    /// Nesting level.
    /// </summary>
    public int Level { get; private set; }

    /// <summary>
    /// RightBorder.
    /// </summary>
    public int RightBorder { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Constructor.
    /// </summary>
    public PftPrettyPrinter()
    {
        _writer = new StringWriter();
        RightBorder = 78;
        IndentWidth = 2;
    }

    #endregion

    #region Private members

    private readonly StringWriter _writer;

    private void _RecalculateColumn()
    {
        var builder = _writer.GetStringBuilder();

        var pos = builder.Length - 1;
        while (pos >= 0)
        {
            var chr = builder[pos];
            if (chr == '\r' || chr == '\n')
            {
                break;
            }

            pos--;
        }

        Column = builder.Length - pos - 1;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Decrease the <see cref="Level"/>.
    /// </summary>
    public PftPrettyPrinter DecreaseLevel()
    {
        Level--;
        if (Level < 0)
        {
            throw new IrbisException();
        }

        return this;
    }

    ///// <summary>
    ///// Eat the last character.
    ///// </summary>
    //public bool EatLastCharacter()
    //{
    //    StringBuilder builder = _writer.GetStringBuilder();

    //    bool result = false;
    //    while (builder.Length > 0 && !result)
    //    {
    //        char chr = builder[builder.Length - 1];
    //        if (chr != '\n' && chr != '\r')
    //        {
    //            LastCharacter = builder.Length > 1
    //                ? builder[builder.Length - 2]
    //                : '\0';
    //            result = true;
    //        }
    //        builder.Length--;
    //    }

    //    return result;
    //}

    ///// <summary>
    ///// Eat the trailing comma.
    ///// </summary>
    //public bool EatComma()
    //{
    //    StringBuilder builder = _writer.GetStringBuilder();

    //    bool result = false, flag = false;
    //    while (builder.Length > 0 && !result)
    //    {
    //        char chr = builder[builder.Length - 1];
    //        if (chr == ',')
    //        {
    //            LastCharacter = builder.Length > 1
    //                ? builder[builder.Length - 2]
    //                : '\0';

    //            builder.Length--;
    //            flag = true;
    //        }
    //        else
    //        {
    //            result = true;
    //        }
    //    }

    //    if (flag)
    //    {
    //        _RecalculateColumn();
    //    }

    //    return result;
    //}

    /// <summary>
    /// Eat trailing new line.
    /// </summary>
    public bool EatNewLine()
    {
        var builder = _writer.GetStringBuilder();

        bool result = false, flag = false;
        while (builder.Length > 0 && !result)
        {
            var chr = builder[^1];
            if (chr == '\n' || chr == '\r')
            {
                builder.Length--;
                flag = true;
            }
            else
            {
                result = true;
            }
        }

        if (flag)
        {
            _RecalculateColumn();
        }

        return result;
    }

    /// <summary>
    /// Eat trailing whitespace.
    /// </summary>
    public bool EatWhitespace()
    {
        var builder = _writer.GetStringBuilder();

        bool result = false, flag = false;
        while (builder.Length > 0 && !result)
        {
            var chr = builder[^1];
            if (char.IsWhiteSpace (chr))
            {
                builder.Length--;
                flag = true;
            }
            else
            {
                result = true;
            }
        }

        if (flag)
        {
            _RecalculateColumn();
        }

        return result;
    }

    /// <summary>
    /// Get current line.
    /// </summary>
    public string GetCurrentLine()
    {
        var builder = _writer.GetStringBuilder();

        if (builder.Length == 0)
        {
            return string.Empty;
        }

        var index = builder.Length - 1;
        while (index >= 0)
        {
            var chr = builder[index];
            if (chr == '\r' || chr == '\n')
            {
                index++;
                break;
            }

            index--;
        }

        if (index < 0)
        {
            index = 0;
        }

        var length = builder.Length - index;

        return builder.ToString (index, length);
    }

    /// <summary>
    /// Increase the <see cref="Level"/>.
    /// </summary>
    public PftPrettyPrinter IncreaseLevel()
    {
        Level++;

        return this;
    }

    /// <summary>
    /// Add one space if needed.
    /// </summary>
    public PftPrettyPrinter SingleSpace()
    {
        var chr = LastCharacter;
        if (chr != ' ' && chr != '\n' && chr != '\r'
            && chr != '(' && chr != '\0')
        {
            Write (' ');
        }

        return this;
    }

    /// <summary>
    /// Write the text.
    /// </summary>
    public PftPrettyPrinter Write
        (
            char chr
        )
    {
        _writer.Write (chr);
        if (chr == '\r' || chr == '\n')
        {
            Column = 0;
        }
        else
        {
            Column++;
        }

        return this;
    }

    /// <summary>
    /// Write the text.
    /// </summary>
    public PftPrettyPrinter Write
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            foreach (var c in text)
            {
                Write (c);
            }
        }

        return this;
    }

    /// <summary>
    /// Write the object.
    /// </summary>
    public PftPrettyPrinter Write
        (
            object? obj
        )
    {
        if (!ReferenceEquals (obj, null))
        {
            var text = obj.ToString();
            Write (text);
        }

        return this;
    }

    /// <summary>
    /// Write the formatted text.
    /// </summary>
    public PftPrettyPrinter Write
        (
            string format,
            params object[] args
        )
    {
        var text = string.Format (format, args);
        Write (text);

        return this;
    }

    /// <summary>
    /// Write the indent.
    /// </summary>
    public PftPrettyPrinter WriteIndent()
    {
        for (var i = 0; i < Level; i++)
        {
            for (var j = 0; j < IndentWidth; j++)
            {
                Write (' ');
            }
        }

        return this;
    }

    /// <summary>
    /// Write indent if needed.
    /// </summary>
    public PftPrettyPrinter WriteIndentIfNeeded()
    {
        var delta = IndentWidth * Level - Column;
        while (delta > 0)
        {
            Write (' ');
            delta--;
        }

        return this;
    }

    /// <summary>
    /// Write newline.
    /// </summary>
    public PftPrettyPrinter WriteLine()
    {
        Write (Environment.NewLine);

        return this;
    }

    /// <summary>
    /// Write the text.
    /// </summary>
    public PftPrettyPrinter WriteLine
        (
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            Write (text);
        }

        Write (Environment.NewLine);

        return this;
    }

    /// <summary>
    /// Write the object.
    /// </summary>
    public PftPrettyPrinter WriteLine
        (
            object? obj
        )
    {
        if (!ReferenceEquals (obj, null))
        {
            var text = obj.ToString();
            WriteLine (text);
        }

        return this;
    }

    /// <summary>
    /// Write the formatted text.
    /// </summary>
    public PftPrettyPrinter WriteLine
        (
            string format,
            params object[] args
        )
    {
        var text = string.Format (format, args);
        WriteLine (text);

        return this;
    }

    /// <summary>
    /// Write new line if needed.
    /// </summary>
    public PftPrettyPrinter WriteLineIfNeeded()
    {
        if (Column > RightBorder)
        {
            WriteLine();
        }

        return this;
    }

    /// <summary>
    /// Write nodes.
    /// </summary>
    public PftPrettyPrinter WriteNodes
        (
            IList<PftNode> nodes
        )
    {
        foreach (var node in nodes)
        {
            WriteIndentIfNeeded();
            node.PrettyPrint (this);
            WriteLineIfNeeded();
        }

        return this;
    }

    /// <summary>
    /// Write nodes.
    /// </summary>
    public PftPrettyPrinter WriteNodes
        (
            string delimiter,
            IList<PftNode> nodes
        )
    {
        var first = true;
        foreach (var node in nodes)
        {
            if (!first)
            {
                Write (delimiter);
            }

            WriteIndentIfNeeded();
            node.PrettyPrint (this);
            WriteLineIfNeeded();
            first = false;
        }

        return this;
    }

    #endregion

    #region IDisposable members

    /// <inheritdoc cref="IDisposable.Dispose" />
    public void Dispose()
    {
        if (!ReferenceEquals (_writer, null))
        {
            _writer.Dispose();
        }
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        if (Level != 0)
        {
            Magna.Logger.LogError
                (
                    nameof (PftPrettyPrinter) + "::" + nameof (ToString)
                    + ": level={Level}",
                    Level
                );
        }

        return _writer.ToString();
    }

    #endregion
}
