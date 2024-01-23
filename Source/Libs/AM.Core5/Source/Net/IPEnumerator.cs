// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo

/* IPEnumerator.cs -- перечисляет IP-адреса
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

#endregion

namespace AM.Net;

/// <summary>
/// Перечисляет IP-адреса.
/// </summary>
internal sealed class IPEnumerator
    : IEnumerator<IPAddress>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public IPEnumerator
        (
            IPAddress begin,
            IPAddress end
        )
    {
        _begin = begin;
        _end = end;
        _current = null!;
    }

    #endregion

    #region Private members

    private readonly IPAddress _begin;
    private readonly IPAddress _end;
    private IPAddress? _current;

    private static void Advance
        (
            byte[] bytes
        )
    {
        var position = bytes.Length - 1;
        while (position >= 0)
        {
            if (bytes[position] != 255)
            {
                bytes[position]++;
                return;
            }

            bytes[position] = 0;
            position--;
        }
    }

    #endregion

    #region IEnumerator members

    /// <inheritdoc cref="IEnumerator.MoveNext"/>
    public bool MoveNext()
    {
        if (_current is null)
        {
            _current = _begin;
        }
        else
        {
            var bytes = _current.GetAddressBytes();
            Advance (bytes);
            _current = new IPAddress (bytes);
        }

        var current = _current.GetAddressBytes();
        return Bits.LessOrEqual (_end.GetAddressBytes(), current);
    }

    /// <inheritdoc cref="IEnumerator.Reset"/>
    public void Reset()
    {
        _current = null!;
    }

    object IEnumerator.Current => Current;

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        // пустое тело метода
    }

    /// <inheritdoc cref="IEnumerator{T}.Current"/>
    public IPAddress Current => _current!;

    #endregion
}
