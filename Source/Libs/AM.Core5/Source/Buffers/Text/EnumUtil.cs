// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo

/* FastNumberWriter.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

#endregion

#nullable enable

namespace AM.Buffers.Text;

internal static class EnumUtil<T>
    where T: notnull
{
    const string InvalidName = "$";

    static readonly Dictionary<T, string> _names;
    static readonly Dictionary<T, byte[]> _utf8Names;

    static EnumUtil()
    {
        var enumNames = Enum.GetNames (typeof (T));
        var values = Enum.GetValues (typeof (T));
        _names = new Dictionary<T, string> (enumNames.Length);
        _utf8Names = new Dictionary<T, byte[]> (enumNames.Length);
        for (var i = 0; i < enumNames.Length; i++)
        {
            var key = (T) values.GetValue (i).ThrowIfNull();
            if (_names.ContainsKey (key))
            {
                // already registered = invalid.
                _names[key] = InvalidName;
                _utf8Names[key] = Array.Empty<byte>(); // byte[0] == Invalid.
            }
            else
            {
                _names.Add (key, enumNames[i]);
                _utf8Names.Add (key, Encoding.UTF8.GetBytes (enumNames[i]));
            }
        }
    }

    public static bool TryFormatUtf16
        (
            T value,
            Span<char> dest,
            out int written,
            ReadOnlySpan<char> _
        )
    {
        if (!_names.TryGetValue (value, out var v) || v == InvalidName)
        {
            v = value.ToString(); // T is Enum, not null always
        }

        written = v?.Length ?? 0;
        return v.AsSpan().TryCopyTo (dest);
    }

    public static bool TryFormatUtf8
        (
            T value,
            Span<byte> dest,
            out int written,
            StandardFormat _
        )
    {
        if (!_utf8Names.TryGetValue (value, out var v) || v.Length == 0)
        {
            v = Encoding.UTF8.GetBytes (value.ToString()!);
        }

        written = v.Length;
        return v.AsSpan().TryCopyTo (dest);
    }
}
