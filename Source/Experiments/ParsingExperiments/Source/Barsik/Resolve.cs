// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global

/* Resolve.cs -- полезные методы для Pidgin
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directive

using System;

using Pidgin;
using Pidgin.Comment;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#endregion

#nullable enable

namespace ParsingExperiments.Barsik;

/// <summary>
/// Полезные методы для Pidgin.
/// </summary>
static class Resolve
{
    public static readonly Parser<char, Unit> BlockComment = CommentParser.SkipBlockComment
        (
            String ("/*"),
            String ("*/")
        );

    public static readonly Parser<char, Unit> LineComment = CommentParser.SkipLineComment
        (
            String ("//")
        );

    public static readonly Parser<char, Unit> Skip = Try (BlockComment)
        .Or (Try (LineComment))
        .Or (Whitespace.IgnoreResult())
        .SkipMany();

    public static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        Try (token).Between (Skip);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

}
