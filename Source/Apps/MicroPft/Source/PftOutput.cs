// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* PftOutput.cs -- принимает вывод из PFT-формата
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

#endregion

#nullable enable

namespace MicroPft;

/// <summary>
/// Принимает вывод из PFT-формата.
/// </summary>
internal sealed class PftOutput
{
    #region Properties

    /// <summary>
    /// Последний выведенный символ.
    /// </summary>
    public char LastCharacter { get; private set; }

    /// <summary>
    /// Текущий номер колонки (отсчет от 0).
    /// </summary>
    public int ColumnNumber { get; private set; }

    #endregion

    #region Private members

    private readonly StringBuilder _builder = new ();

    #endregion

    #region Public methods

    /// <summary>
    /// Сброс к начальному состоянию.
    /// </summary>
    public void Reset()
    {
        _builder.Clear();
        ColumnNumber = 0;
    }

    /// <summary>
    /// Вывод повторяющегося символа.
    /// </summary>
    public void Write
        (
            char chr,
            int count = 1
        )
    {
        _builder.EnsureCapacity (_builder.Length + count);
        if (count > 0)
        {
            LastCharacter = chr;
        }

        while (--count >= 0)
        {
            _builder.Append (chr);
            ColumnNumber = chr == '\n' ? 0 : ColumnNumber + 1;
        }
    }

    /// <summary>
    /// Вывод текста.
    /// </summary>
    public void Write
        (
            ReadOnlySpan<char> text
        )
    {
        foreach (var chr in text)
        {
            LastCharacter = chr;
            _builder.Append (chr);
            ColumnNumber = (chr == '\n') ? 0 : ColumnNumber + 1;
        }
    }

    #endregion

    #region Object members

    public override string ToString()
    {
        return _builder.ToString();
    }

    #endregion
}
