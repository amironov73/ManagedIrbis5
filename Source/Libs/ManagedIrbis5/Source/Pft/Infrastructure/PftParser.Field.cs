﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftParser.Field.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.Collections;
using AM.IO;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    using Ast;

    partial class PftParser
    {
        private PftField ParseField()
        {
            List<PftNode> leftHand = new List<PftNode>();
            PftField result;
            PftNode node;
            PftRepeatableLiteral literal;
            PftToken token;

            // Gather left hand of the field: conditional literal and friends
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;
                if (token.Kind == PftTokenKind.RepeatableLiteral
                    || token.Kind == PftTokenKind.V)
                {
                    break;
                }

                node = Get(FieldMap, LeftHandItems1);
                if (!ReferenceEquals(node, null))
                {
                    leftHand.Add(node);
                }
            } // Tokens.IsEof

            // Gather left hand of the field: repeatable literal
            while (!Tokens.IsEof)
            {
                token = Tokens.Current;
                if (token.Kind == PftTokenKind.RepeatableLiteral)
                {
                    literal = new PftRepeatableLiteral(token, true);
                    leftHand.Add(literal);

                    if (Tokens.Peek() == PftTokenKind.Plus)
                    {
                        literal.Plus = true;
                        Tokens.MoveNext();
                    }

                    Tokens.MoveNext();
                    break;
                }

                node = Get(FieldMap, LeftHandItems2);
                if (ReferenceEquals(node, null))
                {
                    break;
                }
                leftHand.Add(node);
            } // Tokens.IsEof

            // Orphaned left hand?
            if (Tokens.IsEof)
            {
                result = new PftOrphan();
                result.LeftHand.AddRange(leftHand);
                goto DONE;
            }

            //
            // Parse field itself
            //

            token = Tokens.Current;
            //PftToken leadToken = token;

            // Orphaned?
            if (token.Kind != PftTokenKind.V)
            {
                result = new PftOrphan();
                result.LeftHand.AddRange(leftHand);
                goto DONE;
            }
            if (string.IsNullOrEmpty(token.Text))
            {
                Magna.Error
                    (
                        "PftParser::ParseField: "
                        + "empty field"
                    );

                throw new PftSyntaxException(token);
            }

            FieldSpecification specification
                = (FieldSpecification)token.UserData;
            if (ReferenceEquals(specification, null))
            {
                Magna.Error
                    (
                        "PftParser::ParseField: "
                        + "missing field specification"
                    );

                throw new PftSyntaxException(token);
            }

            // Check for command code
            switch (specification.Command)
            {
                case 'v':
                case 'V':
                    result = new PftV(token);
                    break;

                case 'd':
                case 'D':
                    result = new PftD(token);
                    break;

                case 'n':
                case 'N':
                    result = new PftN(token);
                    break;

                case 'g':
                case 'G':
                    result = new PftG(token);
                    break;

                default:
                    Magna.Error
                        (
                            "PftParser::ParseField: "
                            + "unexpected token="
                            + token.Kind
                        );

                    throw new PftSyntaxException(token);
            }

            result.LeftHand.AddRange(leftHand);
            result.Apply(specification);
            Tokens.MoveNext();

            // Gather right hand (for V and G commands only)
            if (result is PftV || result is PftG)
            {
                if (!Tokens.IsEof)
                {
                    bool plus = false;
                    token = Tokens.Current;
                    if (token.Kind == PftTokenKind.Plus)
                    {
                        plus = true;
                        Tokens.RequireNext();
                        token = Tokens.Current;
                    }
                    if (token.Kind == PftTokenKind.RepeatableLiteral)
                    {
                        literal = new PftRepeatableLiteral(token, false)
                        {
                            Plus = plus
                        };
                        result.RightHand.Add(literal);
                        Tokens.MoveNext();
                    }
                    else
                    {
                        if (plus)
                        {
                            Magna.Error
                                (
                                    "PftParser::ParseField: "
                                    + "PLUS not allowed here"
                                );

                            throw new PftSyntaxException(token);
                        }

                        node = Get(FieldMap, RightHandItems);
                        if (!ReferenceEquals(node, null))
                        {
                            result.RightHand.Add(node);
                        }
                    }
                } // Tokens.IsEof

                if (!Tokens.IsEof)
                {
                    token = Tokens.Current;
                    if (token.Kind == PftTokenKind.ConditionalLiteral)
                    {
                        node = new PftConditionalLiteral(token, true);
                        result.RightHand.Add(node);
                        Tokens.MoveNext();
                    }
                }
            } // result is PftV

            DONE: return result;
        }

        private PftNode ParseFieldAssignment()
        {
            PftToken headToken = Tokens.Current;
            PftField field = ParseField();
            PftNode result = field;

            if (!Tokens.IsEof
                && Tokens.Current.Kind == PftTokenKind.Equals)
            {
                Tokens.RequireNext();

                PftFieldAssignment assignment = new PftFieldAssignment(headToken)
                {
                    Field = field
                };

                PftTokenList tokens = Tokens.Segment(_semicolonStop)
                    .ThrowIfNull("tokens");
                NestedContext
                    (
                        assignment.Expression,
                        tokens
                    );
                Tokens.MoveNext();

                result = assignment;
            }

            return result;
        }
    }
}
