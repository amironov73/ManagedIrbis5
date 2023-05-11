// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* MapParser.cs -- парсер, преобразующий результат
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsing;

/// <summary>
/// Парсер, преобразующий результат.
/// </summary>
[PublicAPI]
public sealed class MapParser<TIntermediate, TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public MapParser
        (
            IParser<TIntermediate> parser,
            Func<TIntermediate, TResult> function
        )
    {
        _parser = parser;
        _function = function;
    }

    #endregion

    #region Private members

    private readonly IParser<TIntermediate> _parser;
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
        // не выполняем отладочных вызовов, чтобы не засорять логи
        // предполагаем, что все нужные отладочные вызовы будут
        // сделаны вложенным парсером

        result = default!;
        if (!_parser.TryParse (state, out var temporary))
        {
            return false;
        }

        result = _function (temporary);

        return true;
    }

    #endregion
}
