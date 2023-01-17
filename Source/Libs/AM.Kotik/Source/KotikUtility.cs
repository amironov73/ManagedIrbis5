// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global

/* BarsikUtility.cs -- полезные методы для Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Полезные методы для Барсика.
/// </summary>
public static class KotikUtility
{
    #region Properties

    /// <summary>
    /// Ключевые слова Барсика.
    /// </summary>
    public static string[] ReservedWords { get; } =
    {
        "abstract", "and", "as", "async", "await", "base", "bool", "break",
        "byte", "case", "catch", "char", "checked", "class", "const",
        "continue", "decimal", "default", "delegate", "do", "double",
        "else", "enum", "event", "explicit", "extern", "false", "finally",
        "fixed", "float", "for", "foreach", "func", "goto", "if", "implicit",
        "in", "int", "interface", "internal", "is", "lock", "long",
        "namespace", "new", "null", "object", "operator", "or", "out",
        "override", "params", "private", "protected", "public", "readonly",
        "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc",
        "static", "string", "struct", "switch", "this", "throw", "true",
        "try", "typeof", "uint", "ulong", "unchecked", "unsafe", "ushort",
        "using", "virtual", "void", "volatile", "while", "with"
    };

    #endregion

    #region Public methods

    /// <summary>
    /// Проверка, не является ли указанный текст зарезервированным словом.
    /// </summary>
    public static bool IsReservedWord
        (
            string text
        )
    {
        Sure.NotNull (text);

        foreach (var word in ReservedWords)
        {
            if (string.CompareOrdinal (word, text) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Проверка, не является ли указанный текст зарезервированным словом.
    /// </summary>
    public static bool IsReservedWord
        (
            ReadOnlyMemory<char> text
        )
    {
        var textSpan = text.Span;
        foreach (var word in ReservedWords)
        {
            if (Utility.CompareSpans (word.AsSpan(), textSpan) == 0)
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// Вывод на печать Expando-объекта.
    /// </summary>
    public static void PrintExpando
        (
            TextWriter output,
            ExpandoObject? expando
        )
    {
        if (expando is null)
        {
            output.Write ("(null)");
            return;
        }

        var dictionary = (IDictionary<string, object?>)expando;
        output.Write ("{");

        var keys = dictionary.Keys;
        var first = true;
        foreach (var key in keys)
        {
            if (!first)
            {
                output.Write (", ");
            }

            PrintObject (output, key);
            output.Write (": ");
            PrintObject (output, dictionary[key]);

            first = false;
        }

        output.Write ("}");
    }

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

        if (value is ExpandoObject expando)
        {
            PrintExpando (output, expando);
            return;
        }

        if (value is Array array)
        {
            PrintArray (output, array);
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
    /// Вывод массива на печать.
    /// </summary>
    public static void PrintArray
        (
            TextWriter output,
            Array? array
        )
    {
        if (array is null)
        {
            output.Write ("(null)");
            return;
        }

        output.Write ("[");
        for (var i = 0; i < array.Length; i++)
        {
            if (i != 0)
            {
                output.Write (", ");
            }

            PrintObject (output, array.GetValue (i));
        }

        output.Write ("]");
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

    #endregion
}
