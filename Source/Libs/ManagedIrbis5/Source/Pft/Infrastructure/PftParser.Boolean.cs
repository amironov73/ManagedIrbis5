// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftParser.Boolean.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    using Ast;

    partial class PftParser
    {
        private PftComparison ParseComparison()
        {
            var result = new PftComparison(Tokens.Current);

            var leftTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _comparisonStop
                )
                .ThrowIfNull("leftTokens");
            result.LeftOperand = NestedContext
                (
                    leftTokens,
                    ParseComparisonItem
                );

            result.Operation = Tokens.Current.Text;
            Tokens.RequireNext();

            result.RightOperand = ParseComparisonItem();

            return result;
        }

        private PftNode ParseComparisonItem()
        {
            var position = Tokens.SavePosition();

            PftNode result;

            try
            {
                result = ParseArithmetic();
                if (!Tokens.IsEof
                    || ReferenceEquals(result, null))
                {
                    // Non-arithmetic comparison detected

                    throw new PftSyntaxException();
                }
            }
            catch
            {
                // This is intentional behavior
                // If we can't compare as arithmetic
                // then we must compare as text

                Tokens.RestorePosition(position);
                result = ParseNext();

                if (!Tokens.IsEof)
                {
                    var node = new PftNode();
                    node.Children.Add(result);
                    result = node;

                    while (!Tokens.IsEof)
                    {
                        node = ParseNext();
                        result.Children.Add(node);
                    }
                }
            }

            return result;
        }

        private PftCondition _ParseCondition()
        {
            var token = Tokens.Current;

            PftCondition result;

            if (token.Kind == PftTokenKind.P)
            {
                result = ParseP();
            }
            else if (token.Kind == PftTokenKind.A)
            {
                result = ParseA();
            }
            else if (token.Kind == PftTokenKind.Not)
            {
                var not = new PftConditionNot(token);
                Tokens.RequireNext();
                not.InnerCondition = ParseCondition();
                result = not;
            }
            else if (token.Kind == PftTokenKind.Have)
            {
                result = ParseHave();
            }
            else if (token.Kind == PftTokenKind.Empty)
            {
                result = ParseEmpty();
            }
            else if (token.Kind == PftTokenKind.Blank)
            {
                result = ParseBlank();
            }
            else if (token.Kind == PftTokenKind.True)
            {
                result = ParseTrue();
            }
            else if (token.Kind == PftTokenKind.False)
            {
                result = ParseFalse();
            }
            else if (token.Kind == PftTokenKind.LeftParenthesis)
            {
                var parenthesis
                    = new PftConditionParenthesis(token);
                var saved = Tokens.SavePosition();
                Tokens.RequireNext();
                var innerTokens = Tokens.Segment
                    (
                        _parenthesisOpen,
                        _parenthesisClose,
                        _parenthesisStop
                    )
                    .ThrowIfNull("innerTokens");

                if (Tokens.CountRemainingTokens() == 0
                    && Tokens.Current.Kind == PftTokenKind.RightParenthesis)
                {
                    parenthesis.InnerCondition = (PftCondition?) NestedContext
                        (
                            innerTokens,
                            ParseCondition
                        );
                    Tokens.Current.MustBe(PftTokenKind.RightParenthesis);
                    Tokens.MoveNext();
                    result = parenthesis;
                }
                else
                {
                    Tokens.RestorePosition(saved);
                    var comparison = ParseComparison();
                    result = comparison;
                }
            }
            else if (token.Kind == PftTokenKind.All)
            {
                var all = ParseAll();
                result = all;
            }
            else if (token.Kind == PftTokenKind.Any)
            {
                var any = ParseAny();
                result = any;
            }
            else
            {
                var comparison = ParseComparison();
                result = comparison;
            }

            return result;
        }

        private PftCondition ParseCondition()
        {
            PftCondition? result;

            var conditionTokens = Tokens.Segment
                (
                    _parenthesisPairs,
                    _parenthesisOpen,
                    _parenthesisClose,
                    _andStop
                );
            if (ReferenceEquals(conditionTokens, null))
            {
                result = _ParseCondition();

                return result;
            }

            result = (PftCondition?) NestedContext
                (
                    conditionTokens,
                    _ParseCondition
                );

            while (!Tokens.IsEof)
            {
                var token = Tokens.Current;

                if (token.Kind == PftTokenKind.And
                    || token.Kind == PftTokenKind.Or)
                {
                    var andOr = new PftConditionAndOr(token)
                    {
                        LeftOperand = result,
                        Operation = token.Text
                    };
                    Tokens.RequireNext();

                    PftCondition? right;

                    conditionTokens = Tokens.Segment
                        (
                            _parenthesisOpen,
                            _parenthesisClose,
                            _andStop
                        );
                    if (ReferenceEquals(conditionTokens, null))
                    {
                        right = _ParseCondition();
                    }
                    else
                    {
                        right = (PftCondition?) NestedContext
                            (
                                conditionTokens,
                                _ParseCondition
                            );
                    }

                    andOr.RightOperand = right;
                    result = andOr;
                }
            }

            return result.ThrowIfNull("result");
        }

        //=================================================

        private PftCondition ParseFalse()
        {
            return MoveNext(new PftFalse(Tokens.Current));
        }

        //=================================================

        private PftNode ParseIf()
        {
            var result
                = new PftConditionalStatement(Tokens.Current);
            Tokens.RequireNext();

            var conditionTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _thenStop
                )
                .ThrowIfNull("conditionTokens");
            Tokens.Current.MustBe(PftTokenKind.Then);

            var condition = (PftCondition?) NestedContext
                (
                    conditionTokens,
                    ParseCondition
                );

            result.Condition = condition;

            Tokens.RequireNext();

            var thenTokens = Tokens.Segment
                (
                    _ifOpen,
                    _ifClose,
                    _elseStop
                )
                .ThrowIfNull("thenTokens");
            NestedContext
                (
                    result.ThenBranch,
                    thenTokens
                );

            if (Tokens.Current.Kind == PftTokenKind.Else)
            {
                Tokens.RequireNext();

                var elseTokens = Tokens.Segment
                    (
                        _ifOpen,
                        _ifClose,
                        _fiStop
                    )
                    .ThrowIfNull("elseTokens");
                NestedContext
                    (
                        result.ElseBranch,
                        elseTokens
                    );
            }

            Tokens.Current.MustBe(PftTokenKind.Fi);
            Tokens.MoveNext();

            return result;
        }

        //=================================================

        private PftCondition ParseTrue()
        {
            return MoveNext(new PftTrue(Tokens.Current));
        }
    }
}
