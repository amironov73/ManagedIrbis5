// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* Parser.cs -- базовы класс для парсеров
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;

using AM.Results;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Базовый класс для парсеров.
/// </summary>
public abstract class Parser<TResult>
    where TResult: class
{
    #region Public methods

    /// <summary>
    /// Разбор входного потока (попытка).
    /// </summary>
    public abstract bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        );

    /// <summary>
    /// Разбор входного
    /// </summary>
    public Result<TResult> Parse
        (
            ParseState state,
            bool advance = false
        )
    {
        if (!TryParse (state, out var temporary))
        {
            return Result<TResult>.Failure();
        }

        if (advance)
        {
            state.Advance();
        }

        return new Result<TResult> (temporary);
    }

    /// <summary>
    /// Разбор входного
    /// </summary>
    public TResult ParseOrThrow
        (
            ParseState state,
            bool advance = false
        )
    {
        if (!TryParse (state, out var temporary))
        {
            throw new SyntaxException();
        }

        if (advance)
        {
            state.Advance();
        }

        return new Result<TResult> (temporary).Value;
    }

    #endregion
}

/// <summary>
/// Полезные методы расширения.
/// </summary>
public static class Parser
{
    #region Public methods

    /// <summary>
    /// "Нужное перед прочим".
    /// </summary>
    public static Parser<TResult> Before<TBefore, TResult>
        (
            this Parser<TResult> parser,
            Parser<TBefore> before
        )
        where TBefore: class
        where TResult: class
    {
        return new BeforeParser<TBefore, TResult> (parser, before);
    }

    /// <summary>
    /// Проверка успешного окончания разбора.
    /// </summary>
    public static Parser<TResult> End<TResult>
        (
            this Parser<TResult> parser
        )
        where TResult: class
    {
        return new BeforeParser<Unit, TResult> (parser, new EndParser());
    }

    #endregion
}
