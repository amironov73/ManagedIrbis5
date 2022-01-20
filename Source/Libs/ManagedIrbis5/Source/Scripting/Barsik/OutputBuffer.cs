// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConvertIfStatementToConditionalTernaryExpression
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* OutputBuffer.cs -- выходной буфер для отслеживания удвоенных точек и прочего
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting.Barsik;

/// <summary>
/// Выходной буфер для отслеживания двойных точек и прочих радостей
/// </summary>
internal sealed class OutputBuffer
{
    #region Properties

    /// <summary>
    /// Выставляем поток наружу.
    /// </summary>
    public TextWriter Writer => _writer;

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public OutputBuffer()
    {
        _builder = new StringBuilder();
        _writer = new StringWriter (_builder);
    }

    #endregion

    #region Private members

    /// <summary>
    /// Сюда сохраняем.
    /// </summary>
    private readonly StringBuilder _builder;

    /// <summary>
    /// Для интерфейса потока.
    /// </summary>
    private readonly StringWriter _writer;

    #endregion

    #region Public methods

    /// <summary>
    /// Очистка выходного потока.
    /// </summary>
    public void Clear()
    {
        _builder.Clear();
    }

    /// <summary>
    /// Подсчет пробельных символов в конце.
    /// </summary>
    public int CountLastWhitespace()
    {
        var result = 0;
        for (var position = _builder.Length - 1; position >= 0; position--)
        {
            if (!char.IsWhiteSpace (_builder[position]))
            {
                break;
            }

            ++result;
        }

        return result;
    }

    /// <summary>
    /// Поедание последних пробелов.
    /// </summary>
    public void EatLastWhitespace()
    {
        var length = _builder.Length;
        while (length > 0)
        {
            if (!char.IsWhiteSpace (_builder[length - 1]))
            {
                break;
            }

            --length;
        }

        if (length != _builder.Length)
        {
            _builder.Length = length;
        }
    }

    /// <summary>
    /// Последний непробельный символ.
    /// </summary>
    public char GetLastChar()
    {
        for (var position = _builder.Length - 1; position >= 0; position--)
        {
            var chr = _builder[position];
            if (!char.IsWhiteSpace (chr))
            {
                return chr;
            }
        }

        return '\0';
    }

    /// <inheritdoc cref="TextWriter.Write(bool)"/>
    public void Write (bool value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(char[])"/>
    public void Write (char[]? buffer)
    {
        _writer.Write (buffer);
    }

    /// <inheritdoc cref="TextWriter.Write(decimal)"/>
    public void Write (decimal value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(double)"/>
    public void Write (double value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(int)"/>
    public void Write (int value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(long)"/>
    public void Write (long value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(object)"/>
    public void Write (object? value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(float)"/>
    public void Write (float value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(string,object[])"/>
    public void Write (string format, params object?[] arg)
    {
        _writer.Write (format, arg);
    }

    /// <inheritdoc cref="TextWriter.Write(uint)"/>
    public void Write (uint value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(ulong)"/>
    public void Write (ulong value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine()"/>
    public void WriteLine()
    {
        _writer.WriteLine();
    }

    /// <inheritdoc cref="TextWriter.WriteLine(bool)"/>
    public void WriteLine (bool value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(char)"/>
    public void WriteLine (char value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(char[])"/>
    public void WriteLine (char[]? buffer)
    {
        _writer.WriteLine (buffer);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(char[],int,int)"/>
    public void WriteLine (char[] buffer, int index, int count)
    {
        _writer.WriteLine (buffer, index, count);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(decimal)"/>
    public void WriteLine (decimal value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(double)"/>
    public void WriteLine (double value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(int)"/>
    public void WriteLine (int value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(long)"/>
    public void WriteLine (long value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(object)"/>
    public void WriteLine (object? value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(float)"/>
    public void WriteLine (float value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(string)"/>
    public void WriteLine (string? value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(string,object[])"/>
    public void WriteLine (string format, params object?[] arg)
    {
        _writer.WriteLine (format, arg);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(uint)"/>
    public void WriteLine (uint value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(ulong)"/>
    public void WriteLine (ulong value)
    {
        _writer.WriteLine (value);
    }

    /// <inheritdoc cref="TextWriter.Write(char)"/>
    public void Write (char value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.Write(char[],int,int)"/>
    public void Write (char[] buffer, int index, int count)
    {
        _writer.Write (buffer, index, count);
    }

    /// <inheritdoc cref="Write(bool)"/>
    public void Write (ReadOnlySpan<char> buffer)
    {
        _writer.Write (buffer);
    }

    /// <inheritdoc cref="TextWriter.Write(string)"/>
    public void Write (string? value)
    {
        _writer.Write (value);
    }

    /// <inheritdoc cref="TextWriter.WriteLine(bool)"/>
    public void WriteLine (ReadOnlySpan<char> buffer)
    {
        _writer.WriteLine (buffer);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="object.ToString"/>
    public override string ToString()
    {
        return _builder.ToString();
    }

    #endregion
}
