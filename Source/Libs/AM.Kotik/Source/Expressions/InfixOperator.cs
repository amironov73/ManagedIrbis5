// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global

/* InfixOperator.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
///
/// </summary>
public sealed class InfixOperator<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public InfixOperator
        (
            Parser<TResult> item,
            Parser<string> operations,
            [MaybeNullWhen (false)] Func<TResult, string, TResult, TResult> function,
            BinaryOperatorType operatorType
        )
    {
        _itemParser = item;
        _operationParser = operations;
        _function = function;
        _operatorType = operatorType;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _itemParser;
    private readonly Parser<string> _operationParser;
    private readonly Func<TResult, string, TResult, TResult> _function;
    private readonly BinaryOperatorType _operatorType;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        result = default;
        if (!state.HasCurrent)
        {
            return false;
        }

        var location = state.Location;
        if (!_itemParser.TryParse (state, out var left))
        {
            state.Location = location;
            return false;
        }

        if (!_operationParser.TryParse (state, out var code))
        {
            // если не удалось распарсить операцию,
            // выдаем только левый операнд
            result = left;
            return true;
        }

        if (!_itemParser.TryParse (state, out var right))
        {
            state.Location = location;
            return false;
        }

        // продвижение state выполняют встроенные парсеры
        var temporary = _function (left, code, right);

        if (_operatorType == BinaryOperatorType.NonAssociative)
        {
            result = temporary;
            return true;
        }

        while (state.HasCurrent)
        {
            var location2 = state.Location;
            if (!_operationParser.TryParse (state, out code))
            {
                state.Location = location2;
                break;
            }

            if (!_itemParser.TryParse (state, out right))
            {
                state.Location = location;
                return false;
            }

            temporary = _operatorType switch
            {
                BinaryOperatorType.LeftAssociative => _function (temporary, code, right),
                BinaryOperatorType.RightAssociative => _function (right, code, temporary),
                _ => throw new InvalidOperationException()
            };
        }

        result = temporary;

        return true;
    }

    #endregion
}
