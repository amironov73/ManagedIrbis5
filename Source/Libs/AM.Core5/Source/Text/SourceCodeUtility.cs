// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* SourceCodeUtility.cs -- утилиты для работы исходным кодом C#
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Globalization;
using System.Text;

#endregion

#nullable enable

namespace AM.Text;

/// <summary>
/// Утилиты для работы с исходным кодом C#.
/// </summary>
public static class SourceCodeUtility
{
    #region Public methods

    /// <summary>
    /// Convert byte value to C# source code.
    /// </summary>
    public static string ToSourceCode
        (
            byte value
        )
    {
        return "0x" + value.ToString
            (
                "X2",
                CultureInfo.InvariantCulture
            );
    }

    /// <summary>
    /// Convert array of bytes to C# source code.
    /// </summary>
    public static string ToSourceCode
        (
            byte[] array
        )
    {
        Sure.NotNull (array);

        var result = new StringBuilder ("{");
        for (var i = 0; i < array.Length; i++)
        {
            if (i != 0)
            {
                result.Append (", ");
                if (i % 10 == 0)
                {
                    result.AppendLine();
                    result.Append ("  ");
                }
            }

            result.AppendFormat
                (
                    CultureInfo.InvariantCulture,
                    "0x{0:X2}",
                    array[i]
                );
        }

        result.Append ('}');

        return result.ToString();
    }

    /// <summary>
    /// Encode one character.
    /// </summary>
    public static string EncodeCharacter
        (
            char value
        )
    {
        switch (value)
        {
            case '\a': return "\\a";
            case '\b': return "\\b";
            case '\f': return "\\f";
            case '\n': return "\\n";
            case '\r': return "\\r";
            case '\t': return "\\t";
            case '\v': return "\\v";
            case '\\': return "\\\\";
            case '\'': return "\\'";
            case '\"': return "\\\"";
        }

        if (value < ' ')
        {
            return string.Format
                (
                    CultureInfo.InvariantCulture,
                    "\\x{0:X2}",
                    (int) value
                );
        }

        return value.ToString();
    }

    /// <summary>
    /// Convert the character to C# source code.
    /// </summary>
    public static string ToSourceCode
        (
            char value
        )
    {
        return "'" + EncodeCharacter (value) + "'";
    }

    /// <summary>
    /// Convert array of characters to C# source code.
    /// </summary>
    public static string ToSourceCode
        (
            char[] array
        )
    {
        Sure.NotNull (array);

        var result = new StringBuilder ("{");
        for (var i = 0; i < array.Length; i++)
        {
            if (i != 0)
            {
                result.Append (", ");
                if (i % 10 == 0)
                {
                    result.AppendLine();
                    result.Append ("  ");
                }
            }

            result.Append
                (
                    "'" + EncodeCharacter (array[i]) + "'"
                );
        }

        result.Append ('}');

        return result.ToString();
    }

    /// <summary>
    /// Convert array of 32-bit integers to C# source code.
    /// </summary>
    public static string ToSourceCode
        (
            int[] array
        )
    {
        Sure.NotNull (array);

        var result = new StringBuilder ("{");
        for (var i = 0; i < array.Length; i++)
        {
            if (i != 0)
            {
                result.Append (", ");
                if (i % 10 == 0)
                {
                    result.AppendLine();
                    result.Append ("  ");
                }
            }

            result.Append
                (
                    array[i].ToString
                        (
                            CultureInfo.InvariantCulture
                        )
                );
        }

        result.Append ('}');

        return result.ToString();
    }

    #endregion
}
