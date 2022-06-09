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

#endregion

#nullable enable

namespace AM.Scripting;

/// <summary>
/// Полезные методы для Барсика.
/// </summary>
public static class BarsikUtility
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

    #endregion
}
