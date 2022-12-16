// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable LocalizableElement
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UseNameofExpression

/*
 * Ars Magna project, http://arsmagna.ru
 */

namespace AM.HtmlTags;

using System;

internal static class StringExtensions
{
    public static bool EqualsIgnoreCase (this string thisString, string otherString) =>
        thisString.Equals (otherString, StringComparison.OrdinalIgnoreCase);

    public static bool IsEmpty (this string stringValue) => string.IsNullOrEmpty (stringValue);

    public static bool IsNotEmpty (this string stringValue) => !string.IsNullOrEmpty (stringValue);
}
