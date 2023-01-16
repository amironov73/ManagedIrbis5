// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MapParser.cs -- парсер, преобразующий результат
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер, преобразующий результат.
/// </summary>
public sealed class MapParser<TIntermediate, TResult>
    : Parser<TResult>
    where TIntermediate: class
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MapParser
        (
            Parser<TIntermediate> parser,
            Func<TIntermediate, TResult> function
        )
    {
        _parser = parser;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly Parser<TIntermediate> _parser;
    private readonly Func<TIntermediate, TResult> _function;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            out TResult result
        )
    {
        result = default!;
        DebugHook (state);
        if (!_parser.TryParse (state, out var temporary))
        {
            return DebugSuccess(state, false);
        }

        // продвижение state выполнил вложенный парсер
        result = _function (temporary);

        // TODO отобразить правильно
        return DebugSuccess(state, true);
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Parser{TResult}.ToString"/>
    public override string ToString() =>
        $"{GetType().Name}: {_parser}";

    #endregion
}
