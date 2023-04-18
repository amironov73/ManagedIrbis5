// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* StoringParser.cs -- парсит и запоминает парсуемое
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсит и запоминает парсуемое.
/// </summary>
public sealed class StoringParser<TResult>
    : Parser<Unit>
    where TResult: unmanaged, IParsable<TResult>
{
    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public StoringParser
        (
            Memory<TResult> memory
        )
    {
        _memory = memory;
    }

    #endregion

    #region Public members

    /// <summary>
    /// Где размещается результат.
    /// </summary>
    public ref TResult Result => ref _memory.Span[0];

    /// <summary>
    /// Провайдер формата.
    /// </summary>
    public IFormatProvider? FormatProvider { get; init; }

    #endregion

    #region Private members

    private readonly Memory<TResult> _memory;

    #endregion

    #region Parser<T> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen(false)] out Unit unit
        )
    {
        unit = default;
        DebugHook (state);
        if (!state.HasCurrent)
        {
            return DebugSuccess (state, false);
        }

        var current = state.Current;
        var text = current.Value;
        if (string.IsNullOrEmpty (text))
        {
            return DebugSuccess (state, false);
        }

        if (TResult.TryParse (text, FormatProvider, out var temporary))
        {
            Result = temporary;
            state.Advance();

            return DebugSuccess (state, true);
        }

        return DebugSuccess (state, false);
    }

    #endregion
}
