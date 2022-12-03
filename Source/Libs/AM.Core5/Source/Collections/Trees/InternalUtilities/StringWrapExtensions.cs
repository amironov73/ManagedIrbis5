// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable NonReadonlyMemberInGetHashCode
// ReSharper disable UnusedMember.Global

/* StringWrapExtensions.cs --
 * Ars Magna project, http://arsmagna.ru
 */

namespace TreeCollections;

internal static class StringUtilityExtensions
{
    public static string Wrap
        (
            this string originalText,
            string prefix,
            string postfix
        )
    {
        return prefix + originalText + postfix;
    }

    public static string Wrap
        (
            this string originalText, string wrapper
        )
    {
        return Wrap (originalText, wrapper, wrapper);
    }

    public static string WrapSingleQuotes
        (
            this string originalText
        )
    {
        return Wrap (originalText, "'");
    }

    public static string WrapDoubleQuotes
        (
            this string originalText
        )
    {
        return Wrap (originalText, @"""");
    }

    public static string WrapParentheses
        (
            this string originalText
        )
    {
        return Wrap (originalText, "(", ")");
    }

    public static string WrapSquareBrackets
        (
            this string originalText
        )
    {
        return Wrap (originalText, "[", "]");
    }

    public static string WrapCurlyBrackets
        (
            this string originalText
        )
    {
        return Wrap (originalText, "{", "}");
    }
}
