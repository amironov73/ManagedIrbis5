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

namespace ManagedIrbis.PftLite;

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
    /// Нужно ли переводить символы в верхний регистр.
    /// </summary>
    public bool UpperMode { get; set; }

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
        UpperMode = false;
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
        if (UpperMode)
        {
            chr = char.ToUpperInvariant (chr);
        }

        _builder.EnsureCapacity (_builder.Length + count);
        if (count > 0)
        {
            LastCharacter = chr;
        }

        while (--count >= 0)
        {
            _builder.Append (chr);
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
            var chr2 = UpperMode ? char.ToUpperInvariant (chr) : chr;
            LastCharacter = chr2;
            _builder.Append (chr2);
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
