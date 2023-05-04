// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* InfixOperator.cs -- инфиксный парсер
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Purr.Expressions;

using JetBrains.Annotations;

#endregion

#nullable enable

namespace AM.Purr.Parsers;

/// <summary>
/// Инфиксный парсер для разбора бинарных операций.
/// </summary>
[PublicAPI]
public sealed class InfixParser<TResult>
    : Parser<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public InfixParser
        (
            IParser<TResult> itemParser,
            IParser<string> operationsParser,
            Func<TResult, string, TResult, TResult> function,
            InfixOperatorKind operatorKind
        )
    {
        Sure.NotNull (itemParser);
        Sure.NotNull (operationsParser);
        Sure.NotNull (function);
        Sure.Defined (operatorKind);

        _itemParser = itemParser;
        _operationParser = operationsParser;
        _function = function;
        _operatorKind = operatorKind;
    }

    #endregion

    #region Private members

    private readonly IParser<TResult> _itemParser;
    private readonly IParser<string> _operationParser;
    private readonly Func<TResult, string, TResult, TResult> _function;
    private readonly InfixOperatorKind _operatorKind;

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
            // такая вот у нас защита от рекурсии :)
            result = left;
            return true;
        }

        if (!_itemParser.TryParse(state, out var right))
        {
            state.Location = location;
            return false;
        }

        List<ValueTuple<TResult, string>>? list = null;
        var temporary = left;
        if (_operatorKind is InfixOperatorKind.RightAssociative)
        {
            list = new ()
            {
                new ValueTuple<TResult, string>(right, code)
            };
        }
        else
        {
            temporary = _function (left, code, right);
            if (_operatorKind == InfixOperatorKind.NonAssociative)
            {
                result = temporary;
                return true;
            }
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

            switch (_operatorKind)
            {
                case InfixOperatorKind.LeftAssociative:
                    temporary = _function (temporary, code, right);
                    break;

                case InfixOperatorKind.RightAssociative:
                    list!.Add (new ValueTuple<TResult, string> (right, code));
                    break;

                default:
                    throw new InvalidOperationException();
            }
        }

        if (_operatorKind is InfixOperatorKind.RightAssociative)
        {
            for (var i = list!.Count - 1; i >= 0; i--)
            {
                temporary = _function (temporary, list[i].Item2, list[i].Item1);
            }
        }

        result = temporary;

        return true;
    }

    #endregion

    #region Object members

    /// <inheritdoc cref="Parser{TResult}.ToString"/>
    public override string ToString()
        => $"InfixParser: operation={_operationParser}, item=({_itemParser})";

    #endregion
}
