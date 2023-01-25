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
using System.Collections.Generic;
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
            bool isPrefix,
            Parser<TResult> root,
            IList<Parser<Func<TResult, TResult>>> allowed
        )
    {
        Sure.NotNull (root);
        Sure.NotNull (allowed);

        _isPrefix = isPrefix;
        _root = root;
        _allowed = allowed;
    }

    #endregion

    #region Private members

    private readonly bool _isPrefix;
    private readonly Parser<TResult> _root;
    private readonly IList<Parser<Func<TResult, TResult>>> _allowed;

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

        Stack<Func<TResult, TResult>>? stack = _isPrefix ? new Stack<Func<TResult, TResult>>() : null;
        if (_isPrefix)
        {
            while (true)
            {
                var flag = false;
                foreach (var one in _allowed)
                {
                    if (one.TryParse (state, out var func))
                    {
                        stack!.Push (func);
                        flag = true;
                        break;
                    }
                }

                if (!flag)
                {
                    break;
                }
            }
        }

        if (_root.TryParse (state, out var finalResult))
        {
            total = true;

            if (!_isPrefix)
            {
                while (true)
                {
                    var flag = false;
                    foreach (var one in _allowed)
                    {
                        if (one.TryParse (state, out var func))
                        {
                            finalResult = func (finalResult);

                            flag = true;
                            break;
                        }
                    }

                    if (!flag)
                    {
                        break;
                    }
                }
            }

            if (_isPrefix)
            {
                while (stack!.TryPop (out var func))
                {
                    finalResult = func (finalResult);
                }
            }

            result = finalResult;
        }

        return DebugSuccess (state, total);
    }

    #endregion
}
