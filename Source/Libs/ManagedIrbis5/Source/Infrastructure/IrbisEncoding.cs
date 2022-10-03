// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* IrbisEncoding.cs -- работа с кодировками, применяемыми в ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;

using AM;

using Microsoft.Extensions.Logging;

using CM = System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure;

/// <summary>
/// Работа с кодировками, применяемыми в ИРБИС64.
/// </summary>
public static class IrbisEncoding
{
    #region Properties

    /// <summary>
    /// Однобайтовая кодировка по умолчанию.
    /// Как правило, кодовая страница 1251.
    /// </summary>
    public static Encoding Ansi => _ansi;

    /// <summary>
    /// Однобайтовая OEM-кодировка.
    /// Как правило, кодовая страница 866.
    /// </summary>
    public static Encoding Oem => _oem;

    /// <summary>
    /// Кодировка UTF-8 без BOM.
    /// </summary>
    public static Encoding Utf8 => _utf8;

    #endregion

    #region Construction

    static IrbisEncoding()
    {
        Utility.RegisterEncodingProviders();
        _ansi = Utility.Windows1251;
        _oem = Utility.Cp866;
        _utf8 = new UTF8Encoding
            (
                encoderShouldEmitUTF8Identifier: false,
                throwOnInvalidBytes: true
            );
    }

    #endregion

    #region Private members

    private static Encoding _ansi, _oem, _utf8;

    /// <summary>
    /// Таблица ручной перекодировки CP1251 &lt;-&gt; UTF8.
    /// </summary>
    private static readonly short[] _cp1251_unicode =
    {
        0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18,
        19, 20, 21, 22, 23, 24, 25, 26, 27, 28, 29, 30, 31, 32, 33, 34,
        35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50,
        51, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, 62, 63, 64, 65, 66,
        67, 68, 69, 70, 71, 72, 73, 74, 75, 76, 77, 78, 79, 80, 81, 82,
        83, 84, 85, 86, 87, 88, 89, 90, 91, 92, 93, 94, 95, 96, 97, 98,
        99, 100, 101, 102, 103, 104, 105, 106, 107, 108, 109, 110, 111,
        112, 113, 114, 115, 116, 117, 118, 119, 120, 121, 122, 123, 124,
        125, 126, 127, 1026, 1027, 8218, 1107, 8222, 8230, 8224, 8225,
        8364, 8240, 1033, 8249, 1034, 1036, 1035, 1039, 1106, 8216, 8217,
        8220, 8221, 8226, 8211, 8212, 152, 8482, 1113, 8250, 1114, 1116,
        1115, 1119, 160, 1038, 1118, 1032, 164, 1168, 166, 167, 1025, 169,
        1028, 171, 172, 173, 174, 1031, 176, 177, 1030, 1110, 1169, 181,
        182, 183, 1105, 8470, 1108, 187, 1112, 1029, 1109, 1111, 1040,
        1041, 1042, 1043, 1044, 1045, 1046, 1047, 1048, 1049, 1050, 1051,
        1052, 1053, 1054, 1055, 1056, 1057, 1058, 1059, 1060, 1061, 1062,
        1063, 1064, 1065, 1066, 1067, 1068, 1069, 1070, 1071, 1072, 1073,
        1074, 1075, 1076, 1077, 1078, 1079, 1080, 1081, 1082, 1083, 1084,
        1085, 1086, 1087, 1088, 1089, 1090, 1091, 1092, 1093, 1094, 1095,
        1096, 1097, 1098, 1099, 1100, 1101, 1102, 1103
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Get encoding by name.
    /// </summary>
    public static Encoding ByName
        (
            string? name
        )
    {
        if (string.IsNullOrEmpty (name))
        {
            // кодировка по умолчанию
            return Utf8;
        }

        if (name.SameString ("Ansi"))
        {
            return Ansi;
        }

        if (name.SameString ("Dos")
            || name.SameString ("MsDos")
            || name.SameString ("Oem"))
        {
            return Oem;
        }

        if (name.SameString ("Utf")
            || name.SameString ("Utf8")
            || name.SameString ("Utf-8"))
        {
            return Utf8;
        }

        var result = Encoding.GetEncoding (name);

        return result;
    }

    /// <summary>
    /// Get encoding from config file.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.ExcludeFromCodeCoverage]
    public static Encoding FromConfig
        (
            string key
        )
    {
        Sure.NotNullNorEmpty (key, nameof (key));

        var name = CM.AppSettings[key];
        var result = ByName (name);

        return result;
    }

    /// <summary>
    /// Relax UTF-8 decoder, do not throw exceptions
    /// on invalid bytes.
    /// </summary>
    public static void RelaxUtf8()
    {
        _utf8 = new UTF8Encoding
            (
                encoderShouldEmitUTF8Identifier: false,
                throwOnInvalidBytes: false
            );
    }

    /// <summary>
    /// Strong UTF-8 decoder, throw exceptions
    /// on invalid bytes.
    /// </summary>
    public static void StrongUtf8()
    {
        _utf8 = new UTF8Encoding
            (
                encoderShouldEmitUTF8Identifier: false,
                throwOnInvalidBytes: true
            );
    }

    /// <summary>
    /// Override default single-byte encoding.
    /// </summary>
    public static void SetAnsiEncoding
        (
            Encoding encoding
        )
    {
        if (!encoding.IsSingleByte)
        {
            Magna.Logger.LogError
                (
                    nameof (IrbisEncoding) + "::" + nameof (SetAnsiEncoding)
                    + ": not single-byte encoding"
                );

            throw new ArgumentOutOfRangeException (nameof (encoding));
        }

        _ansi = encoding;
    }

    /// <summary>
    /// Override OEM encoding.
    /// </summary>
    public static void SetOemEncoding
        (
            Encoding encoding
        )
    {
        if (!encoding.IsSingleByte)
        {
            Magna.Logger.LogError
                (
                    nameof (IrbisEncoding) + "::" + nameof (SetOemEncoding)
                    + ": not single-byte encoding"
                );

            throw new ArgumentOutOfRangeException (nameof (encoding));
        }

        _oem = encoding;
    }

    /// <summary>
    /// Ручная перекодировка CP1251 -&gt; UTF8.
    /// </summary>
    public static byte[] AnsiToUtf
        (
            ReadOnlySpan<byte> text
        )
    {
        var result = new MemoryStream();
        AnsiToUtf (result, text);

        return result.ToArray();
    }

    /// <summary>
    /// Ручная перекодировка CP1251 -&gt; UTF8.
    /// </summary>
    public static void AnsiToUtf
        (
            Stream output,
            ReadOnlySpan<byte> text
        )
    {
        Sure.NotNull (output);

        var length = text.Length;

        unchecked
        {
            for (var i = 0; i < length; i++)
            {
                var chr = _cp1251_unicode[text[i]];
                if (chr < 128)
                {
                    output.WriteByte ((byte) chr);
                }
                else
                {
                    output.WriteByte ((byte) ((chr >> 6) | 0xC0));
                    output.WriteByte ((byte) ((chr & 0x3Fu) | 0x80u));
                }
            }
        }
    }

    /// <summary>
    /// Ручная перекодировка UTF8 -&gt; CP1251.
    /// </summary>
    public static byte[] UtfToAnsi
        (
            ReadOnlySpan<byte> text
        )
    {
        var result = new MemoryStream();
        UtfToAnsi (result, text);

        return result.ToArray();
    }

    /// <summary>
    /// Ручная перекодировка UTF8 -&gt; CP1251.
    /// </summary>
    public static void UtfToAnsi
        (
            Stream output,
            ReadOnlySpan<byte> text
        )
    {
        Sure.NotNull (output);

        var length = text.Length;
        unchecked
        {
            var offset = 0;
            while (offset < length)
            {
                var first = text[offset++];
                if ((first & 0x80) == 0)
                {
                    output.WriteByte (first);
                }
                else
                {
                    var second = text[offset++];
                    if ((first & 0xF0) == 0xE0)
                    {
                        output.WriteByte ((byte) '?');
                        offset++;
                        continue;
                    }

                    if ((first & 0xF8) == 0xF0)
                    {
                        output.WriteByte ((byte) '?');
                        output.WriteByte ((byte) '?');
                        offset++;
                        offset++;
                        continue;
                    }

                    if ((first & 0xF8) == 0xF8)
                    {
                        output.WriteByte ((byte) '?');
                        output.WriteByte ((byte) '?');
                        output.WriteByte ((byte) '?');
                        offset++;
                        offset++;
                        offset++;
                        continue;
                    }

                    var unicode = (short) (((first & 0x1F) << 6) + (second & 0x3F));
                    var chr = (byte) '?';
                    for (var i = 0; i < 256; i++)
                    {
                        if (_cp1251_unicode[i] == unicode)
                        {
                            chr = (byte) i;
                            break;
                        }
                    }

                    output.WriteByte (chr);
                }
            }
        }
    }

    #endregion
}
