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

using System;
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
            ParseState state
        )
    {
        if (!TryParse (state, out var temporary))
        {
            return Result<TResult>.Failure();
        }

        return new Result<TResult> (temporary);
    }

    /// <summary>
    /// Разбор входного
    /// </summary>
    public TResult ParseOrThrow
        (
            ParseState state
        )
    {
        if (!TryParse (state, out var temporary))
        {
            throw new SyntaxException();
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
    /// "Нужное после ненужного".
    /// </summary>
    public static Parser<TResult> After<TAfter, TResult>
        (
            this Parser<TResult> parser,
            Parser<TAfter> other
        )
        where TAfter: class
        where TResult: class
    {
        return new AfterParser<TAfter, TResult> (parser, other);
    }

    /// <summary>
    /// "Нужное перед ненужным".
    /// </summary>
    public static Parser<TResult> Before<TBefore, TResult>
        (
            this Parser<TResult> parser,
            Parser<TBefore> other
        )
        where TBefore: class
        where TResult: class
    {
        return new BeforeParser<TBefore, TResult> (parser, other);
    }

    /// <summary>
    /// "Нужное между ненужным".
    /// </summary>
    public static Parser<TResult> Between<TBefore, TResult, TAfter>
        (
            this Parser<TResult> parser,
            Parser<TBefore> before,
            Parser<TAfter> after
        )
        where TBefore: class
        where TResult: class
        where TAfter: class
    {
        return new BetweenParser<TBefore, TResult, TAfter> (before, parser, after);
    }

    /// <summary>
    /// Цепочка из двух парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Func<T1, T2, TResult> function
        )
        where T1: class
        where T2: class
        where TResult: class
    {
        return new ChainParser<T1, T2, TResult> (first, second, function);
    }

    /// <summary>
    /// Цепочка из трех парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Func<T1, T2, T3, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, TResult> (first, second,
            third, function);
    }

    /// <summary>
    /// Цепочка из четырех парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Func<T1, T2, T3, T4, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, TResult> (first, second,
            third, fourth, function);
    }

    /// <summary>
    /// Цепочка из пяти парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, TResult> (first, second,
            third, fourth, fifth, function);
    }

    /// <summary>
    /// Цепочка из шести парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, TResult> (first,
            second, third, fourth, fifth, sixth, function);
    }

    /// <summary>
    /// Цепочка из семи парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where T7: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, TResult> (first,
            second, third, fourth, fifth, sixth, seventh, function);
    }

    /// <summary>
    /// Цепочка из восьми парсеров.
    /// </summary>
    public static Parser<TResult> Chain<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eight,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function
        )
        where T1: class
        where T2: class
        where T3: class
        where T4: class
        where T5: class
        where T6: class
        where T7: class
        where T8: class
        where TResult: class
    {
        return new ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, TResult> (first,
            second, third, fourth, fifth, sixth, seventh, eight, function);
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

    /// <summary>
    /// Идентификатор.
    /// </summary>
    public static readonly IdentifierParser Identifier = new ();

    /// <summary>
    /// Литерал.
    /// </summary>
    public static readonly LiteralParser Literal = new ();

    /// <summary>
    /// Преобразование разобранного значения.
    /// </summary>
    public static Parser<TResult> Map<TIntermediate, TResult>
        (
            this Parser<TIntermediate> parser,
            Func<TIntermediate, TResult> function
        )
        where TIntermediate: class
        where TResult: class
    {
        return new MapParser<TIntermediate, TResult> (parser, function);
    }

    /// <summary>
    /// Опциональный парсер.
    /// </summary>
    public static Parser<TResult> Optional<TResult>
        (
            this Parser<TResult> parser
        )
        where TResult: class
    {
        return new OptionalParser<TResult> (parser);
    }

    /// <summary>
    /// Терм.
    /// </summary>
    public static TermParser Term (params string[] terms) => new (terms);

    /// <summary>
    /// Зарезервированное слово.
    /// </summary>
    public static ReservedWordParser Reserved (string word) => new (word);

    /// <summary>
    /// Немедленный возврат значения.
    /// </summary>
    public static Parser<TResult> Return<TResult> (TResult result)
        where TResult: class
        => new ReturnParser<TResult> (result);

    /// <summary>
    /// Трассировка парсера.
    /// </summary>
    public static TraceParser<TResult> Trace<TResult>
        (
            this Parser<TResult> parser,
            string? message = null
        )
        where TResult: class
    {
        return new TraceParser<TResult> (parser, message);
    }

    #endregion
}
