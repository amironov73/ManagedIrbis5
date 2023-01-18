// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable ArgumentsStyleLiteral
// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StaticMemberInitializerReferesToMemberBelow

/* UnaryParser.cs -- парсер для "нанизываемых" унарных операций
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;

#endregion

#nullable enable

namespace AM.Kotik;

/// <summary>
/// Парсер для "нанизываемых" унарных операций.
/// </summary>
public sealed class UnaryParser<TResult>
    : Parser<TResult>
    where TResult: class
{
    #region Construciton

    /// <summary>
    /// Конструктор.
    /// </summary>
    public UnaryParser
        (
            Parser<TResult> root,
            HalfParser<TResult>[] allowed
        )
    {
        _root = root;
        _allowed = allowed;
    }

    #endregion

    #region Private members

    private readonly Parser<TResult> _root;
    private readonly HalfParser<TResult>[] _allowed;

    #endregion

    #region Parser<TResult> members

    /// <inheritdoc cref="Parser{TResult}.TryParse"/>
    public override bool TryParse
        (
            ParseState state,
            [MaybeNullWhen (false)] out TResult result
        )
    {
        using var level = state.Enter (this);
        result = default;
        DebugHook (state);

        var total = false;
        if (_root.TryParse (state, out var finalResult))
        {
            while (true)
            {
                var flag = false;
                foreach (var one in _allowed)
                {
                    if (one.Parser.TryParse (state, out var temporary))
                    {
                        finalResult = one.Applier (finalResult, temporary);
                        flag = true;
                        total = true;
                        break;
                    }
                }

                if (!flag)
                {
                    break;
                }
            }

            result = finalResult;
        }

        return DebugSuccess (state, total);
    }

    #endregion
}
