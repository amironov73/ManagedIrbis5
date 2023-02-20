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
    // where T : Enum
{
    const string InvalidName = "$";

    static readonly Dictionary<T, string> names;
    static readonly Dictionary<T, byte[]> utf8names;

    static EnumUtil()
    {
        var enumNames = Enum.GetNames(typeof(T));
        var values = Enum.GetValues(typeof(T));
        names = new Dictionary<T, string>(enumNames.Length);
        utf8names = new Dictionary<T, byte[]>(enumNames.Length);
        for (int i = 0; i < enumNames.Length; i++)
        {
            if (names.ContainsKey((T)values.GetValue(i)))
            {
                // already registered = invalid.
                names[(T)values.GetValue(i)] = InvalidName;
                utf8names[(T)values.GetValue(i)] = Array.Empty<byte>(); // byte[0] == Invalid.
            }
            else
            {
                names.Add((T)values.GetValue(i), enumNames[i]);
                utf8names.Add((T)values.GetValue(i), Encoding.UTF8.GetBytes(enumNames[i]));
            }
        }
    }

    public static bool TryFormatUtf16(T value, Span<char> dest, out int written, ReadOnlySpan<char> _)
    {
        if (!names.TryGetValue(value, out var v) || v == InvalidName)
        {
            v = value!.ToString(); // T is Enum, not null always
        }

        written = v.Length;
        return v.AsSpan().TryCopyTo(dest);
    }

    public static bool TryFormatUtf8(T value, Span<byte> dest, out int written, StandardFormat _)
    {
        if (!utf8names.TryGetValue(value, out var v) || v.Length == 0)
        {
            v = Encoding.UTF8.GetBytes(value!.ToString());
        }

        written = v.Length;
        return v.AsSpan().TryCopyTo(dest);
    }
}
