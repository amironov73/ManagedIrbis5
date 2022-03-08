// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

using Pidgin;

#nullable enable

namespace ParsingExperiments;

/// <summary>
/// Превью-парсер.
/// Умеет запускать после первого парсера второй, "заглядывающий" вперед.
/// Если второй парсер не сработал, то и первый считается провалившимся.
/// </summary>
/// <typeparam name="TToken"></typeparam>
/// <typeparam name="TResult"></typeparam>
sealed class PreviewParser<TToken, TResult>
    : Parser<TToken, TResult>
{
    public PreviewParser
        (
            Parser<TToken, TResult> first,
            Parser<TToken, Unit> second
        )
    {
        _first = first;
        _second = second;
    }

    private readonly Parser<TToken, TResult> _first;
    private readonly Parser<TToken, Unit> _second;

    public override bool TryParse
        (
            ref ParseState<TToken> state,
            ref PooledList<Expected<TToken>> expecteds,
            out TResult result
        )
    {
        var success1 = _first.TryParse (ref state, ref expecteds, out var result1);
        if (!success1)
        {
            state.Rewind();
            result = default!;
            return false;
        }

        state.PushBookmark();
        var success2 = _second.TryParse (ref state, ref expecteds, out var _);
        state.Rewind();
        if (!success2)
        {
            result = default!;
            return false;
        }

        result = result1!;

        return true;
    }
}
