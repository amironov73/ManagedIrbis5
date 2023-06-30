// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* ChainParser.cs -- парсер для последовательностей
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер для последовательностей.
/// </summary>
[PublicAPI]
public sealed class ChainParser<TFirst, TSecond, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<TFirst> first,
            IParser<TSecond> second,
            Func<TFirst, TSecond, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<TFirst> _first;
    private readonly IParser<TSecond> _second;
    private readonly Func<TFirst, TSecond, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<TFirst, TSecond, TThird, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<TFirst> first,
            IParser<TSecond> second,
            IParser<TThird> third,
            Func<TFirst, TSecond, TThird, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<TFirst> _first;
    private readonly IParser<TSecond> _second;
    private readonly IParser<TThird> _third;
    private readonly Func<TFirst, TSecond, TThird, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            Func<T1, T2, T3, T4, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly Func<T1, T2, T3, T4, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, T5, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly IParser<T5> _fifth;
    private readonly Func<T1, T2, T3, T4, T5, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fifth.TryParse (state, out var fifth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth, fifth);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, T5, T6, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _sixth = sixth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly IParser<T5> _fifth;
    private readonly IParser<T6> _sixth;
    private readonly Func<T1, T2, T3, T4, T5, T6, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fifth.TryParse (state, out var fifth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_sixth.TryParse (state, out var sixth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth, fifth, sixth);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, T5, T6, T7, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _sixth = sixth;
        _seventh = seventh;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly IParser<T5> _fifth;
    private readonly IParser<T6> _sixth;
    private readonly IParser<T7> _seventh;
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fifth.TryParse (state, out var fifth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_sixth.TryParse (state, out var sixth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_seventh.TryParse (state, out var seventh))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth, fifth, sixth, seventh);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eighth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eighth);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _sixth = sixth;
        _seventh = seventh;
        _eighth = eighth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly IParser<T5> _fifth;
    private readonly IParser<T6> _sixth;
    private readonly IParser<T7> _seventh;
    private readonly IParser<T8> _eighth;
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fifth.TryParse (state, out var fifth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_sixth.TryParse (state, out var sixth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_seventh.TryParse (state, out var seventh))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_eighth.TryParse (state, out var eighth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth, fifth, sixth,
            seventh, eighth);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eighth,
            IParser<T9> nineth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eighth);
        Sure.NotNull (nineth);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _sixth = sixth;
        _seventh = seventh;
        _eighth = eighth;
        _nineth = nineth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly IParser<T5> _fifth;
    private readonly IParser<T6> _sixth;
    private readonly IParser<T7> _seventh;
    private readonly IParser<T8> _eighth;
    private readonly IParser<T9> _nineth;
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fifth.TryParse (state, out var fifth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_sixth.TryParse (state, out var sixth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_seventh.TryParse (state, out var seventh))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_eighth.TryParse (state, out var eighth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_nineth.TryParse (state, out var nineth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth, fifth, sixth,
            seventh, eighth, nineth);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eighth,
            IParser<T9> nineth,
            IParser<T10> tenth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eighth);
        Sure.NotNull (nineth);
        Sure.NotNull (tenth);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _sixth = sixth;
        _seventh = seventh;
        _eighth = eighth;
        _nineth = nineth;
        _tenth = tenth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly IParser<T5> _fifth;
    private readonly IParser<T6> _sixth;
    private readonly IParser<T7> _seventh;
    private readonly IParser<T8> _eighth;
    private readonly IParser<T9> _nineth;
    private readonly IParser<T10> _tenth;
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fifth.TryParse (state, out var fifth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_sixth.TryParse (state, out var sixth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_seventh.TryParse (state, out var seventh))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_eighth.TryParse (state, out var eighth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_nineth.TryParse (state, out var nineth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_tenth.TryParse (state, out var tenth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth, fifth, sixth,
            seventh, eighth, nineth, tenth);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eighth,
            IParser<T9> nineth,
            IParser<T10> tenth,
            IParser<T11> eleventh,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eighth);
        Sure.NotNull (nineth);
        Sure.NotNull (tenth);
        Sure.NotNull (eleventh);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _sixth = sixth;
        _seventh = seventh;
        _eighth = eighth;
        _nineth = nineth;
        _tenth = tenth;
        _eleventh = eleventh;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly IParser<T5> _fifth;
    private readonly IParser<T6> _sixth;
    private readonly IParser<T7> _seventh;
    private readonly IParser<T8> _eighth;
    private readonly IParser<T9> _nineth;
    private readonly IParser<T10> _tenth;
    private readonly IParser<T11> _eleventh;
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fifth.TryParse (state, out var fifth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_sixth.TryParse (state, out var sixth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_seventh.TryParse (state, out var seventh))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_eighth.TryParse (state, out var eighth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_nineth.TryParse (state, out var nineth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_tenth.TryParse (state, out var tenth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_eleventh.TryParse (state, out var eleventh))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth, fifth, sixth,
            seventh, eighth, nineth, tenth, eleventh);

        return DebugSuccess (state, true);
    }

    #endregion
}

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            IParser<T1> first,
            IParser<T2> second,
            IParser<T3> third,
            IParser<T4> fourth,
            IParser<T5> fifth,
            IParser<T6> sixth,
            IParser<T7> seventh,
            IParser<T8> eighth,
            IParser<T9> nineth,
            IParser<T10> tenth,
            IParser<T11> eleventh,
            IParser<T12> twelveth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);
        Sure.NotNull (third);
        Sure.NotNull (fourth);
        Sure.NotNull (fifth);
        Sure.NotNull (sixth);
        Sure.NotNull (seventh);
        Sure.NotNull (eighth);
        Sure.NotNull (nineth);
        Sure.NotNull (tenth);
        Sure.NotNull (eleventh);
        Sure.NotNull (twelveth);
        Sure.NotNull (function);

        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _sixth = sixth;
        _seventh = seventh;
        _eighth = eighth;
        _nineth = nineth;
        _tenth = tenth;
        _eleventh = eleventh;
        _twelveth = twelveth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<T1> _first;
    private readonly IParser<T2> _second;
    private readonly IParser<T3> _third;
    private readonly IParser<T4> _fourth;
    private readonly IParser<T5> _fifth;
    private readonly IParser<T6> _sixth;
    private readonly IParser<T7> _seventh;
    private readonly IParser<T8> _eighth;
    private readonly IParser<T9> _nineth;
    private readonly IParser<T10> _tenth;
    private readonly IParser<T11> _eleventh;
    private readonly IParser<T12> _twelveth;
    private readonly Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        using var _ = state.Enter (this);
        result = default!;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var location = state.Location;
        if (!_first.TryParse (state, out var first))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_second.TryParse (state, out var second))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_third.TryParse (state, out var third))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fourth.TryParse (state, out var fourth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_fifth.TryParse (state, out var fifth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_sixth.TryParse (state, out var sixth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_seventh.TryParse (state, out var seventh))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_eighth.TryParse (state, out var eighth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_nineth.TryParse (state, out var nineth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_tenth.TryParse (state, out var tenth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_eleventh.TryParse (state, out var eleventh))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        if (!_twelveth.TryParse (state, out var twelveth))
        {
            state.Location = location;
            return DebugSuccess (state, false);
        }

        result = _function (first, second, third, fourth, fifth, sixth,
            seventh, eighth, nineth, tenth, eleventh, twelveth);

        return DebugSuccess (state, true);
    }

    #endregion
}
