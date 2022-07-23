// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* HeaderParser.cs -- парсер заголовка запроса/ответа
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

using AM;

using Microsoft.Extensions.Logging;

#endregion

#nullable enable

namespace ManagedIrbis.Proxying;

/// <summary>
/// Простой парсер заголовка запроса/ответа,
/// разбирающий его на строки.
/// </summary>
internal sealed class HeaderParser
{
    #region Private members

    private readonly byte[] _data;

    private int _offset;

    #endregion

    #region Public methods

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HeaderParser
        (
            byte[] data
        )
    {
        Sure.NotNull (data);

        _data = data;
        _offset = 0;
    }

    public string NextString()
    {
        int i;
        for (i = _offset; i < _data.Length; i++)
        {
            if (_data[i] == 0x0A)
            {
                break;
            }
        }

        var result = Encoding.Default.GetString
            (
                _data,
                _offset,
                i - _offset
            );

        _offset = i + 1;
        if (_offset > _data.Length)
        {
            _offset = _data.Length;
        }

        return result;
    }

    public byte[] NextBytes()
    {
        var remainder = _data.Length - _offset;
        var result = new byte[remainder];
        Array.Copy (_data, _offset, result, 0, remainder);

        return result;
    }

    public string[]? SplitLines
        (
            byte[] buffer,
            bool utf
        )
    {
        string[]? result = null;
        try
        {
            var encoding = utf
                ? Encoding.UTF8
                : Encoding.Default;
            var text = encoding.GetString (buffer);
            text = text.Replace ("\r\n", "\r");
            result = text.Split (new[] { '\r', '\n' });
        }
        catch (Exception exception)
        {
            Magna.Logger.LogError
                (
                    exception,
                    nameof (HeaderParser) + "::" + nameof (SplitLines)
                );
        }

        return result;
    }

    #endregion
}
