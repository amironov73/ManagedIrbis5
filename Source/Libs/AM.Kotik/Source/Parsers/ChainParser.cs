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

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер для последовательностей.
/// </summary>
public sealed class ChainParser<TFirst, TSecond, TResult>
    : Parser<TResult>
    where TFirst: class
    where TSecond: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<TFirst> first,
            Parser<TSecond> second,
            Func<TFirst, TSecond, TResult> function
        )
    {
        _first = first;
        _second = second;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly Parser<TFirst> _first;
    private readonly Parser<TSecond> _second;
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
    where TFirst: class
    where TSecond: class
    where TThird: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<TFirst> first,
            Parser<TSecond> second,
            Parser<TThird> third,
            Func<TFirst, TSecond, TThird, TResult> function
        )
    {
        _first = first;
        _second = second;
        _third = third;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly Parser<TFirst> _first;
    private readonly Parser<TSecond> _second;
    private readonly Parser<TThird> _third;
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
    where T1: class
    where T2: class
    where T3: class
    where T4: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Func<T1, T2, T3, T4, TResult> function
        )
    {
        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
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
    where T1: class
    where T2: class
    where T3: class
    where T4: class
    where T5: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Func<T1, T2, T3, T4, T5, TResult> function
        )
    {
        _first = first;
        _second = second;
        _third = third;
        _fourth = fourth;
        _fifth = fifth;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
    private readonly Parser<T5> _fifth;
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
    where T1: class
    where T2: class
    where T3: class
    where T4: class
    where T5: class
    where T6: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Func<T1, T2, T3, T4, T5, T6, TResult> function
        )
    {
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

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
    private readonly Parser<T5> _fifth;
    private readonly Parser<T6> _sixth;
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
    where T1: class
    where T2: class
    where T3: class
    where T4: class
    where T5: class
    where T6: class
    where T7: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
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
    {
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

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
    private readonly Parser<T5> _fifth;
    private readonly Parser<T6> _sixth;
    private readonly Parser<T7> _seventh;
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
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eighth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> function
        )
    {
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

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
    private readonly Parser<T5> _fifth;
    private readonly Parser<T6> _sixth;
    private readonly Parser<T7> _seventh;
    private readonly Parser<T8> _eighth;
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
    where T1: class
    where T2: class
    where T3: class
    where T4: class
    where T5: class
    where T6: class
    where T7: class
    where T8: class
    where T9: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eighth,
            Parser<T9> nineth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> function
        )
    {
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

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
    private readonly Parser<T5> _fifth;
    private readonly Parser<T6> _sixth;
    private readonly Parser<T7> _seventh;
    private readonly Parser<T8> _eighth;
    private readonly Parser<T9> _nineth;
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
    where T1: class
    where T2: class
    where T3: class
    where T4: class
    where T5: class
    where T6: class
    where T7: class
    where T8: class
    where T9: class
    where T10: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eighth,
            Parser<T9> nineth,
            Parser<T10> tenth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> function
        )
    {
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

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
    private readonly Parser<T5> _fifth;
    private readonly Parser<T6> _sixth;
    private readonly Parser<T7> _seventh;
    private readonly Parser<T8> _eighth;
    private readonly Parser<T9> _nineth;
    private readonly Parser<T10> _tenth;
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
    where T1: class
    where T2: class
    where T3: class
    where T4: class
    where T5: class
    where T6: class
    where T7: class
    where T8: class
    where T9: class
    where T10: class
    where T11: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eighth,
            Parser<T9> nineth,
            Parser<T10> tenth,
            Parser<T11> eleventh,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult> function
        )
    {
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

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
    private readonly Parser<T5> _fifth;
    private readonly Parser<T6> _sixth;
    private readonly Parser<T7> _seventh;
    private readonly Parser<T8> _eighth;
    private readonly Parser<T9> _nineth;
    private readonly Parser<T10> _tenth;
    private readonly Parser<T11> _eleventh;
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
    where T1: class
    where T2: class
    where T3: class
    where T4: class
    where T5: class
    where T6: class
    where T7: class
    where T8: class
    where T9: class
    where T10: class
    where T11: class
    where T12: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public ChainParser
        (
            Parser<T1> first,
            Parser<T2> second,
            Parser<T3> third,
            Parser<T4> fourth,
            Parser<T5> fifth,
            Parser<T6> sixth,
            Parser<T7> seventh,
            Parser<T8> eighth,
            Parser<T9> nineth,
            Parser<T10> tenth,
            Parser<T11> eleventh,
            Parser<T12> twelveth,
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult> function
        )
    {
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

    private readonly Parser<T1> _first;
    private readonly Parser<T2> _second;
    private readonly Parser<T3> _third;
    private readonly Parser<T4> _fourth;
    private readonly Parser<T5> _fifth;
    private readonly Parser<T6> _sixth;
    private readonly Parser<T7> _seventh;
    private readonly Parser<T8> _eighth;
    private readonly Parser<T9> _nineth;
    private readonly Parser<T10> _tenth;
    private readonly Parser<T11> _eleventh;
    private readonly Parser<T12> _twelveth;
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
