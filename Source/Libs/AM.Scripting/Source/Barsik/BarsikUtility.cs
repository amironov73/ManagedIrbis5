// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* BarsikUtility.cs -- полезные методы для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;
using System.Collections;
using System.Globalization;
using System.IO;

#endregion

#nullable enable

namespace AM.Scripting.Barsik;

/// <summary>
/// Полезные методы для Барсика.
/// </summary>
public static class BarsikUtility
{
    #region Properties

    /// <summary>
    /// Ключевые слова Барсика.
    /// </summary>
    public static string[] Keywords { get; } =
    {
        "and", "catch", "else", "false", "finally", "for", "foreach",
        "func", "if", "in", "new", "null", "or", "print", "println",
        "return", "throw", "true", "try", "using", "while"
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Вывод на печать произвольного объекта.
    /// </summary>
    public static void PrintObject
        (
            TextWriter output,
            object? value
        )
    {
        if (value is null)
        {
            output.Write ("(null)");
            return;
        }

        if (value is bool b)
        {
            output.Write (b ? "true" : "false");
            return;
        }

        if (value is string)
        {
            output.Write (value);
            return;
        }

        var type = value.GetType();
        if (type.IsPrimitive)
        {
            if (value is IFormattable formattable)
            {
                output.Write (formattable.ToString (null, CultureInfo.InvariantCulture));
            }
            else
            {
                output.Write (value);
            }

            return;
        }

        switch (value)
        {
            case IDictionary dictionary:
                PrintDictionary (output, dictionary);
                break;

            case IEnumerable sequence:
                PrintSequence (output, sequence);
                break;

            case IFormattable formattable:
                output.Write (formattable.ToString (null, CultureInfo.InvariantCulture));
                break;

            default:
                output.Write (value);
                break;
        }
    }

    /// <summary>
    /// Вывод на печать словаря.
    /// </summary>
    public static void PrintDictionary
        (
            TextWriter output,
            IDictionary? dictionary
        )
    {
        if (dictionary is null)
        {
            output.Write ("(null)");
            return;
        }

        output.Write ("{");

        var first = true;
        foreach (DictionaryEntry entry in dictionary)
        {
            if (!first)
            {
                output.Write (", ");
            }

            PrintObject (output, entry.Key);
            output.Write (": ");
            PrintObject (output, entry.Value);

            first = false;
        }

        output.Write ("}");
    }

    /// <summary>
    /// Вывод на печать последовательности.
    /// </summary>
    public static void PrintSequence
        (
            TextWriter output,
            IEnumerable? sequence
        )
    {
        if (sequence is null)
        {
            output.Write ("(null)");
            return;
        }

        if (sequence is IDictionary dictionary)
        {
            PrintDictionary (output, dictionary);
            return;
        }

        if (sequence is string)
        {
            output.Write (sequence);
            return;
        }

        var first = true;
        output.Write ("[");
        foreach (var item in sequence)
        {
            if (!first)
            {
                output.Write (", ");
            }

            PrintObject (output, item);
            first = false;
        }

        output.Write ("]");
    }

    /// <summary>
    /// Преобразование любого значения в логическое.
    /// </summary>
    public static bool ToBoolean
        (
            object? value
        )
    {
        return value switch
        {
            null => false,
            true => true,
            false => false,
            "true" or "True" => true,
            "false" or "False" => false,
            string text => !string.IsNullOrEmpty (text),
            sbyte sb => sb != 0,
            byte b => b != 0,
            short i16 => i16 != 0,
            int i32 => i32 != 0,
            long i64 => i64 != 0,
            float f32 => f32 != 0.0f,
            double d64 => d64 != 0.0,
            decimal d => d != 0.0m,
            IList list => list.Count != 0,
            IDictionary dictionary => dictionary.Count != 0,
            _ => true
        };
    }

    /// <summary>
    /// Вычисление длины объекта.
    /// </summary>
    public static int GetLength
        (
            object? value
        )
    {
        return value switch
        {
            null => 0,
            string text => text.Length,
            IList list => list.Count,
            IDictionary dictionary => dictionary.Count,
            _ => 1
        };
    }

    #endregion
}
