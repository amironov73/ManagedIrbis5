// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PftParser.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;

using AM;
using AM.Collections;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure
{
    using Ast;

    // ReSharper disable InconsistentNaming

    /// <summary>
    /// Parser for PFT language.
    /// </summary>

    public sealed partial class PftParser
    {
        #region Properties

        /// <summary>
        /// Token list.
        /// </summary>
        public PftTokenList Tokens { get; private set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        // ReSharper disable once NotNullMemberIsNotInitialized
        public PftParser
            (
                PftTokenList tokens
            )
        {
            Tokens = tokens;

            _procedures = new PftProcedureManager();
            CreateTokenMap();
        }

        #endregion

        #region Private members

        //================================================================
        // Parsing
        //================================================================

        private PftA ParseA()
        {
            var result = new PftA(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext(PftTokenKind.V);
            var field = ParseField();
            if (field.Command != 'v' && field.Command != 'g')
            {
                Magna.Error
                    (
                        "PftParser::ParseA: "
                        + "bad field specified"
                    );

                throw new PftSyntaxException(Tokens.Current);
            }

            result.Field = field;
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            return MoveNext(result);
        }

        //=================================================

        private PftAll ParseAll()
        {
            var result = new PftAll(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.MoveNext();

            var conditionTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
                .ThrowIfNull("conditionTokens");
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            var condition = (PftCondition?) NestedContext
                (
                    conditionTokens,
                    ParseCondition
                );

            result.InnerCondition = condition;

            return MoveNext(result);
        }

        //=================================================

        private PftAny ParseAny()
        {
            var result = new PftAny(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.MoveNext();

            var conditionTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
                .ThrowIfNull("conditionTokens");
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            var condition = (PftCondition?)NestedContext
                (
                    conditionTokens,
                    ParseCondition
                );

            result.InnerCondition = condition;

            return MoveNext(result);
        }

        //=================================================

        private PftAssignment ParseAssignment
            (
                PftAssignment result,
                PftTokenList tokens
            )
        {
            var saveList = Tokens;
            Tokens = tokens;
            var position = Tokens.SavePosition();

            try
            {
                PftNode node;
                try
                {
                    node = ParseArithmetic();
                    result.Children.Add(node);
                    if (!Tokens.IsEof)
                    {
                        Magna.Error
                            (
                                "PftParser::ParseAssignment: "
                                + "garbage detected"
                            );

                        throw new PftSyntaxException();
                    }

                    result.IsNumeric = true;
                }
                catch
                {
                    // This is intentional behavior
                    // If we can't assign as arithmetic
                    // then we must assign as plain text

                    result.Children.Clear();
                    Tokens.RestorePosition(position);
                    while (!Tokens.IsEof)
                    {
                        node = ParseNext();
                        result.Children.Add(node);
                    }
                }
            }
            finally
            {
                Tokens = saveList;
            }

            return result;
        }

        //=================================================

        private PftNode ParseAt()
        {
            return MoveNext(new PftInclude(Tokens.Current));
        }

        //=================================================

        private PftNode ParseBackslash()
        {
            return MoveNext(new PftBackslash(Tokens.Current));
        }

        //=================================================

        private PftNode ParseBang()
        {
            return MoveNext(new PftBang(Tokens.Current));
        }

        //=================================================

        private PftBlank ParseBlank()
        {
            var result = new PftBlank(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseBreak()
        {
            return MoveNext(new PftBreak(Tokens.Current));
        }

        //=================================================

        private PftNode ParseC()
        {
            return MoveNext(new PftC(Tokens.Current));
        }

        //=================================================

        private PftNode ParseCodeBlock()
        {
            return MoveNext(new PftCodeBlock(Tokens.Current));
        }

        //=================================================

        private PftNode ParseComma()
        {
            return MoveNext(new PftComma(Tokens.Current));
        }

        //=================================================

        private PftNode ParseComment()
        {
            return MoveNext(new PftComment(Tokens.Current));
        }

        //=================================================

        private PftNode ParseConditionalLiteral()
        {
            return MoveNext
                (
                    new PftConditionalLiteral(Tokens.Current, false)
                );
        }

        //=================================================

        private PftNode ParseCsEval()
        {
            PftNode result = new PftCsEval(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseEat()
        {
            var result = new PftEat(Tokens.Current);
            Tokens.MoveNext();

            var ok = false;
            while (!Tokens.IsEof)
            {
                if (Tokens.Current.Kind == PftTokenKind.EatClose)
                {
                    ok = true;
                    Tokens.MoveNext();
                    break;
                }

                var node = ParseNext();
                result.Children.Add(node);
            }

            if (!ok)
            {
                Magna.Error
                    (
                        "PftParser::ParseEat: "
                        + "syntax error"
                    );

                throw new PftSyntaxException(Tokens);
            }

            return result;
        }


        //=================================================

        private PftEmpty ParseEmpty()
        {
            var result = new PftEmpty(Tokens.Current);
            ParseCall(result);
            return result;
        }


        //=================================================

        private PftNode ParseEval()
        {
            PftNode result = new PftEval(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseF()
        {
            var result = new PftF(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            result.Argument1 = ParseArithmetic
                (
                    PftTokenKind.Comma,
                    PftTokenKind.RightParenthesis
                );
            if (Tokens.IsEof)
            {
                Magna.Error
                    (
                        "PftParser::ParseF: "
                        + "unexpected end of stream"
                    );

                throw new PftSyntaxException(Tokens);
            }

            if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
            {
                if (Tokens.Current.Kind != PftTokenKind.Comma)
                {
                    Magna.Error
                        (
                            "PftParser::ParseF: "
                            + "syntax error"
                        );

                    throw new PftSyntaxException(Tokens.Current);
                }

                Tokens.RequireNext();
                result.Argument2 = ParseNumber();
                if (Tokens.IsEof)
                {
                    Magna.Error
                        (
                            "PftParser::ParseF: "
                            + "unexpected end of stream"
                        );

                    throw new PftSyntaxException(Tokens);
                }

                if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
                {
                    if (Tokens.Current.Kind != PftTokenKind.Comma)
                    {
                        Magna.Error
                            (
                                "PftParser::ParseF: "
                                + "syntax error"
                            );

                        throw new PftSyntaxException(Tokens.Current);
                    }

                    Tokens.RequireNext();
                    result.Argument3 = ParseNumber();
                }
            }
            if (Tokens.IsEof)
            {
                Magna.Error
                    (
                        "PftParser::ParseF: "
                        + "unexpected end of stream"
                    );

                throw new PftSyntaxException(Tokens);
            }

            if (Tokens.Current.Kind != PftTokenKind.RightParenthesis)
            {
                Magna.Error
                    (
                        "PftParser::ParseF: "
                        + "syntax error"
                    );

                throw new PftSyntaxException(Tokens.Current);
            }

            Tokens.MoveNext();

            return result;
        }

        //=================================================

        private PftNode ParseF2()
        {
            var result = new PftFmt(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            result.Number = ParseArithmetic
                (
                    PftTokenKind.Comma
                );
            if (Tokens.IsEof
                || Tokens.Current.Kind != PftTokenKind.Comma
               )
            {
                Magna.Error
                    (
                        "PftParser::ParseF2: "
                        + "syntax error"
                    );

                throw new PftSyntaxException(Tokens);
            }

            Tokens.RequireNext();
            var formatTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
                .ThrowIfNull("formatTokens");
            NestedContext
                (
                    result.Format,
                    formatTokens
                );
            Tokens.MoveNext();

            return result;
        }

        //=================================================

        /// <summary>
        /// For loop.
        /// </summary>
        /// <example>
        /// for $x=0; $x &lt; 10; $x = $x+1;
        /// do
        ///     $x, ') ',
        ///     'Прикольно же!'
        ///     #
        /// end
        /// </example>
        private PftNode ParseFor()
        {
            var result = new PftFor(Tokens.Current);

            var saveLoop = _inLoop;

            try
            {
                _inLoop = true;

                Tokens.RequireNext();
                var initTokens = Tokens.Segment
                    (
                        _semicolonStop
                    )
                    .ThrowIfNull("initTokens");
                initTokens.Add(PftTokenKind.Semicolon);
                Tokens.Current.MustBe(PftTokenKind.Semicolon);
                NestedContext(result.Initialization, initTokens);
                Tokens.RequireNext();
                var conditionTokens = Tokens.Segment
                    (
                        _semicolonStop
                    )
                    .ThrowIfNull("conditionTokens");
                Tokens.Current.MustBe(PftTokenKind.Semicolon);
                result.Condition = (PftCondition?) NestedContext
                    (
                        conditionTokens,
                        ParseCondition
                    );
                Tokens.RequireNext();
                var loopTokens = Tokens.Segment
                    (
                        _doStop
                    )
                    .ThrowIfNull("loopTokens");
                Tokens.Current.MustBe(PftTokenKind.Do);
                NestedContext(result.Loop, loopTokens);
                Tokens.RequireNext();
                var bodyTokens = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _loopStop
                    )
                    .ThrowIfNull("bodyTokens");
                Tokens.Current.MustBe(PftTokenKind.End);
                NestedContext(result.Body, bodyTokens);
                Tokens.MoveNext();
            }
            finally
            {
                _inLoop = saveLoop;
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// ForEach loop.
        /// </summary>
        /// <example>
        /// foreach $x in v200^a,v200^e,'Hello'
        /// do
        ///     $x
        ///     #
        /// end
        /// </example>
        private PftNode ParseForEach()
        {
            var result = new PftForEach(Tokens.Current);

            var saveLoop = _inLoop;

            try
            {
                _inLoop = true;

                Tokens.RequireNext();
                result.Variable = (PftVariableReference)ParseVariableReference();
                Tokens.Current.MustBe(PftTokenKind.In);
                Tokens.RequireNext();
                var sequenceTokens = Tokens.Segment
                    (
                        _doStop
                    )
                    .ThrowIfNull("sequenceTokens");
                Tokens.Current.MustBe(PftTokenKind.Do);
                NestedContext
                    (
                        result.Sequence,
                        sequenceTokens
                    );
                Tokens.RequireNext();
                var bodyTokens = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _loopStop
                    )
                    .ThrowIfNull("bodyTokens");
                Tokens.Current.MustBe(PftTokenKind.End);
                NestedContext
                    (
                        result.Body,
                        bodyTokens
                    );
                Tokens.MoveNext();
            }
            finally
            {
                _inLoop = saveLoop;
            }

            return result;
        }

        //=================================================

        /// <summary>
        /// from ... select...
        /// </summary>
        /// <example>
        /// from $x in (v300/)
        /// where $x:'a'
        /// select 'Comment: ', $x
        /// order $x
        /// end
        /// </example>
        private PftNode ParseFrom()
        {
            var result = new PftFrom(Tokens.Current);

            var saveLoop = _inLoop;

            try
            {
                _inLoop = true;

                // Variable reference
                Tokens.RequireNext(PftTokenKind.Variable);
                result.Variable = (PftVariableReference)ParseVariableReference();

                // In clause
                Tokens.Current.MustBe(PftTokenKind.In);
                Tokens.RequireNext();
                var sourceTokens = Tokens.Segment(_whereStop)
                    .ThrowIfNull("sourceTokens");
                NestedContext
                    (
                        result.Source,
                        sourceTokens
                    );

                // Where clause (optional)
                if (Tokens.Current.Kind == PftTokenKind.Where)
                {
                    Tokens.RequireNext();
                    var whereTokens = Tokens.Segment(_selectStop)
                        .ThrowIfNull("whereTokens");
                    result.Where = (PftCondition?) NestedContext
                        (
                            whereTokens,
                            ParseCondition
                        );
                }

                // Select clause
                Tokens.Current.MustBe(PftTokenKind.Select);
                Tokens.RequireNext();
                var selectTokens = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _orderStop
                    )
                    .ThrowIfNull("selectTokens");
                NestedContext
                    (
                        result.Select,
                        selectTokens
                    );

                // Order clause (optional)
                if (Tokens.Current.Kind == PftTokenKind.Order)
                {
                    Tokens.RequireNext();
                    var orderTokens = Tokens.Segment
                        (
                            _loopOpen,
                            _loopClose,
                            _loopStop
                        )
                        .ThrowIfNull("orderTokens");
                    NestedContext
                        (
                            result.Order,
                            orderTokens
                        );
                }

                Tokens.Current.MustBe(PftTokenKind.End);
                Tokens.MoveNext();
            }
            finally
            {
                _inLoop = saveLoop;
            }

            return result;
        }

        //=================================================

        private void _HandleArguments
            (
                PftFunctionCall call,
                PftTokenList tokens
            )
        {
            var saveTokens = Tokens;
            Tokens = tokens;
            var savePosition = Tokens.SavePosition();

            try
            {
                PftNode superNode = ParseArithmetic();

                if (ReferenceEquals(superNode, null))
                {
                    try
                    {
                        superNode = ParseCondition();
                    }
                    catch
                    {
                        // This is intentional behavior

                        superNode = new PftNode();
                        Tokens.RestorePosition(savePosition);
                        while (!Tokens.IsEof)
                        {
                            var node = ParseNext();
                            superNode.Children.Add(node);
                        }
                    }
                }

                if (superNode.Children.Count == 1)
                {
                    var tempNode = superNode.Children[0];
                    superNode.Children.RemoveAt(0);
                    superNode = tempNode;
                }

                call.Arguments.Add(superNode);
            }
            finally
            {
                Tokens = saveTokens;
            }
        }

        private PftNode ParseFunctionCall()
        {
            var result = new PftFunctionCall(Tokens.Current);

            Tokens.RequireNext();

            var token = Tokens.Current;
            token.MustBe(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();

            var innerTokens = Tokens.Segment
                (
                    _parenthesisOpen,
                    _parenthesisClose,
                    _parenthesisStop
                )
                .ThrowIfNull("innerTokens");

            if (innerTokens.IsEof)
            {
                Tokens.MoveNext();
            }
            else
            {
                while (!innerTokens.IsEof)
                {
                    var argumentTokens = innerTokens.Segment
                        (
                            _parenthesisOpen,
                            _parenthesisClose,
                            _semicolonStop
                        );
                    if (ReferenceEquals(argumentTokens, null))
                    {
                        _HandleArguments(result, innerTokens);
                        Tokens.MoveNext();
                        break;
                    }
                    _HandleArguments(result, argumentTokens);
                    innerTokens.MoveNext();
                }
            }

            return result;
        }

        //=================================================

        private PftNode ParseGraveAccent()
        {
            return MoveNext(new PftGraveAccent(Tokens.Current));
        }

        //=================================================

        private PftNode ParseGroup()
        {
            var result = new PftGroup(Tokens.Current);

            if (_inGroup)
            {
                Magna.Error
                    (
                        "PftParser::ParseGroup: "
                        + "nested group detected"
                    );

                throw new PftSyntaxException("no nested group enabled");
            }

            try
            {
                _inGroup = true;

                ParseCall2(result);
            }
            finally
            {
                _inGroup = false;
            }

            return result;
        }

        //=================================================

        private PftNode ParseHash()
        {
            return MoveNext(new PftHash(Tokens.Current));
        }

        //=================================================

        private PftHave ParseHave()
        {
            var result = new PftHave(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            if (Tokens.Current.Kind == PftTokenKind.Variable)
            {
                var variable
                    = (PftVariableReference)ParseVariableReference();
                result.Variable = variable;
            }
            else if (Tokens.Current.Kind == PftTokenKind.Identifier)
            {
                result.Identifier = Tokens.Current.Text;
                Tokens.MoveNext();
            }
            else
            {
                Magna.Error
                    (
                        "PftParser::ParseHave: "
                        + "syntax error"
                    );

                throw new PftSyntaxException(Tokens);
            }

            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            return MoveNext(result);

        }

        //=================================================

        private PftNode ParseL()
        {
            var result = new PftL(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseLocal()
        {
            var result = new PftLocal(Tokens.Current);

            Tokens.MoveNext();
            var localTokens = Tokens.Segment(_doStop)
                .ThrowIfNull("localTokens");
            Tokens.MoveNext();

            var bodyTokens = Tokens.Segment
                (
                    _loopOpen,
                    _loopClose,
                    _loopStop
                )
                .ThrowIfNull("bodyTokens");

            while (!localTokens.IsEof)
            {
                var token = localTokens.Current;
                switch (token.Kind)
                {
                    case PftTokenKind.Variable:
                        if (token.Text is not null)
                        {
                            result.Names.Add(token.Text);
                        }
                        break;

                    case PftTokenKind.Comma:
                        break;

                    default:
                        Magna.Error
                            (
                                "PftParser::ParseLocal: "
                                + "unexpected token="
                                + token.Kind
                            );

                        throw new PftSyntaxException(token);
                }

                localTokens.MoveNext();
            }

            NestedContext
                (
                    (NonNullCollection<PftNode>)result.Children,
                    bodyTokens
                );

            return MoveNext(result);
        }

        //=================================================

        private PftNode ParseMfn()
        {
            return MoveNext(new PftMfn(Tokens.Current));
        }

        //=================================================

        private PftNode ParseMpl()
        {
            return MoveNext(new PftMode(Tokens.Current));
        }

        //=================================================

        private PftNode ParseNested()
        {
            var result = new PftNested(Tokens.Current);
            Tokens.RequireNext();

            var tokens = Tokens.Segment
                (
                    _curlyOpen,
                    _curlyClose,
                    _curlyStop
                )
                .ThrowIfNull("tokens");

            NestedContext
                (
                    (NonNullCollection<PftNode>)result.Children,
                    tokens
                );

            Tokens.MoveNext();

            return result;
        }

        //=================================================

        private PftNode ParseNl()
        {
            return MoveNext(new PftNl(Tokens.Current));
        }

        //=================================================

        private PftNumeric ParseNumber()
        {
            return MoveNext(new PftNumericLiteral(Tokens.Current));
        }

        //=================================================

        private PftP ParseP()
        {
            var result = new PftP(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext(PftTokenKind.V);

            var field = ParseField();
            if (ReferenceEquals(field, null))
            {
                Magna.Error
                    (
                        "PftParser::ParseP: "
                        + "field not set"
                    );

                throw new PftSyntaxException(Tokens.Current);
            }

            result.Field = field;
            Tokens.Current.MustBe(PftTokenKind.RightParenthesis);

            return MoveNext(result);
        }

        //=================================================

        private PftNode ParsePercent()
        {
            return MoveNext(new PftPercent(Tokens.Current));
        }

        //=================================================

        private PftNode ParseProc()
        {
            var result
                = new PftProcedureDefinition(Tokens.Current);

            if (_inProcedure)
            {
                Magna.Error
                    (
                        "PftParser::ParseProc: "
                        + "nested procedure detected"
                    );

                throw new PftSyntaxException("no nested proc allowed");
            }

            if (_inLoop)
            {
                Magna.Error
                    (
                        "PftParser::ParseProc: "
                        + "procedure inside loop detected"
                    );

                throw new PftSyntaxException("no proc in loop allowed");
            }

            if (_inGroup)
            {
                Magna.Error
                    (
                        "PftParser::ParseProc: "
                        + "procedure inside group detected"
                    );

                throw new PftSyntaxException("no proc in group allowed");
            }

            try
            {
                _inProcedure = true;

                var procedure = new PftProcedure();
                result.Procedure = procedure;

                Tokens.RequireNext(PftTokenKind.Identifier);
                procedure.Name = Tokens.Current.Text;
                Tokens.RequireNext(PftTokenKind.Do);
                Tokens.RequireNext();

                var name = procedure.Name
                    .ThrowIfNull("procedure.Name");
                if (name.IsOneOf(PftUtility.GetReservedWords()))
                {
                    Magna.Error
                        (
                            "PftParser::ParseProc: "
                            + "reserved word="
                            + name.ToVisibleString()
                        );

                    throw new PftSyntaxException
                        (
                            "reserved word: " + name
                        );
                }
                if (PftFunctionManager.BuiltinFunctions.HaveFunction(name)
                   || PftFunctionManager.UserFunctions.HaveFunction(name)
                   )
                {
                    Magna.Error
                        (
                            "PftParser::ParseProc: "
                            + "already have function: "
                            + name.ToVisibleString()
                        );

                    throw new PftSyntaxException
                        (
                            "already have function: " + name
                        );
                }

                if (!ReferenceEquals(_procedures.FindProcedure(name), null))
                {
                    Magna.Error
                        (
                            "PftParser::ParseProc: "
                            + "already have procedure: "
                            + name.ToVisibleString()
                        );

                    throw new PftSyntaxException
                        (
                            "already have procedure: " + name
                        );
                }

                _procedures.Registry.Add
                    (
                        name,
                        procedure
                    );

                var bodyList = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _procedureStop
                    )
                    .ThrowIfNull("bodyList");
                Tokens.Current.MustBe(PftTokenKind.End);
                NestedContext
                    (
                        procedure.Body,
                        bodyList
                    );
                Tokens.MoveNext();

            }
            finally
            {
                _inProcedure = false;
            }

            return result;
        }

        //=================================================

        private PftNode ParseQuestion()
        {
            return MoveNext(new PftQuestion(Tokens.Current));
        }

        //=================================================

        private PftNode ParseRef()
        {
            var result = new PftRef(Tokens.Current);

            Tokens.RequireNext(PftTokenKind.LeftParenthesis);
            Tokens.RequireNext();
            result.Mfn = ParseArithmetic(PftTokenKind.Comma);
            Tokens.Current.MustBe(PftTokenKind.Comma);
            Tokens.RequireNext();

            var pseudo = new PftNode();
            var saveInGroup = _inGroup;
            // there can be nested group inside the ref format
            _inGroup = false;
            try
            {
                ParseCall3(pseudo);
            }
            finally
            {
                _inGroup = saveInGroup;
            }

            var tempArray = pseudo.Children.ToArray();
            pseudo.Children.Clear();
            result.Format.AddRange(tempArray);

            return result;
        }

        //=================================================

        private PftNode ParseRepeatableLiteral()
        {
            return MoveNext(new PftRepeatableLiteral(Tokens.Current, false));
        }

        //=================================================

        private PftNode ParseRsum()
        {
            PftNode result = new PftRsum(Tokens.Current);
            return ParseCall(result);
        }

        //=================================================

        private PftNode ParseS()
        {
            PftNode result = new PftS(Tokens.Current);
            ParseCall(result);
            return result;
        }

        //=================================================

        private PftNode ParseSemicolon()
        {
            return MoveNext(new PftSemicolon(Tokens.Current));
        }

        //=================================================

        private PftNode ParseSlash()
        {
            return MoveNext(new PftSlash(Tokens.Current));
        }

        //=================================================

        private PftNode ParseUnconditionalLiteral()
        {
            return MoveNext(new PftUnconditionalLiteral(Tokens.Current));
        }

        //=================================================

        private PftNode ParseUnifor()
        {
            PftNode result = new PftUnifor(Tokens.Current);
            return ParseCall(result);
        }

        //=================================================

        private PftNode ParseVal()
        {
            PftNode result = new PftVal(Tokens.Current);
            return ParseCall(result);
        }

        //=================================================

        private PftNode ParseVariable()
        {
            var firstToken = Tokens.Current;

            var index = ParseIndex();

            if (Tokens.Peek() == PftTokenKind.Equals)
            {
                if (_inAssignment)
                {
                    Magna.Error
                        (
                            "PftParser::ParseVariable: "
                            + "nested assignment detected"
                        );

                    throw new PftSyntaxException("nested assignment");
                }

                try
                {
                    _inAssignment = true;

                    var result = new PftAssignment(firstToken)
                    {
                        Index = index
                    };
                    Tokens.RequireNext(PftTokenKind.Equals);
                    Tokens.RequireNext();

                    var tokens = Tokens.Segment
                        (
                            _parenthesisOpen,
                            _parenthesisClose,
                            _semicolonStop
                        );
                    if (ReferenceEquals(tokens, null))
                    {
                        Magna.Error
                            (
                                "PftParser::ParseVariable: "
                                + "unclosed assignment"
                            );

                        throw new PftSyntaxException(Tokens);
                    }

                    Tokens.Current.MustBe(PftTokenKind.Semicolon);
                    Tokens.MoveNext();

                    result = ParseAssignment(result, tokens);

                    return result;
                }
                finally
                {
                    _inAssignment = false;
                }
            }

            PftNode reference = new PftVariableReference(firstToken)
            {
                Index = index,
                SubFieldCode = ParseSubField()
            };

            return MoveNext(reference);
        }

        //=================================================

        private PftNode ParseVariableReference()
        {
            var result
                = new PftVariableReference(Tokens.Current)
                {
                    Index = ParseIndex(),
                    SubFieldCode = ParseSubField()
                };

            return MoveNext(result);
        }

        //=================================================

        private PftNode ParseVerbatim()
        {
            return MoveNext(new PftVerbatim(Tokens.Current));
        }

        //=================================================

        /// <summary>
        /// While loop.
        /// </summary>
        /// <example>
        /// $x=0;
        /// while $x &lt; 10
        /// do
        ///     $x, ') ',
        ///     'Прикольно же!'
        ///     #
        ///     $x=$x+1;
        /// end
        /// </example>
        private PftNode ParseWhile()
        {
            var result = new PftWhile(Tokens.Current);

            var saveLoop = _inLoop;

            try
            {
                _inLoop = true;

                Tokens.RequireNext();
                var conditionTokens = Tokens.Segment
                    (
                        _doStop
                    )
                    .ThrowIfNull("conditionTokens");
                Tokens.Current.MustBe(PftTokenKind.Do);
                result.Condition = (PftCondition?) NestedContext
                    (
                        conditionTokens,
                        ParseCondition
                    );
                Tokens.RequireNext();
                var bodyTokens = Tokens.Segment
                    (
                        _loopOpen,
                        _loopClose,
                        _loopStop
                    )
                    .ThrowIfNull("bodyTokens");
                Tokens.Current.MustBe(PftTokenKind.End);
                NestedContext
                    (
                        result.Body,
                        bodyTokens
                    );
                Tokens.MoveNext();
            }
            finally
            {
                _inLoop = saveLoop;
            }

            return result;
        }

        //=================================================

        private PftNode ParseWith()
        {
            var result = new PftWith(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.Variable);

            result.Variable = new PftVariableReference(Tokens.Current);
            Tokens.RequireNext(PftTokenKind.In);
            Tokens.RequireNext();

            var fieldTokens = Tokens.Segment
                (
                    _doStop
                )
                .ThrowIfNull("fieldTokens");
            Tokens.Current.MustBe(PftTokenKind.Do);
            Tokens.RequireNext();
            var bodyTokens = Tokens.Segment
                (
                    _loopOpen,
                    _loopClose,
                    _loopStop
                )
                .ThrowIfNull("bodyTokens");

            while (!fieldTokens.IsEof)
            {
                var token = fieldTokens.Current;
                switch (token.Kind)
                {
                    case PftTokenKind.V:
                        var specification
                            = new FieldSpecification();
                        if (!specification.ParseShort
                            (
                                token.Text
                                    .ThrowIfNull("token.Text")
                            ))
                        {
                            Magna.Error
                                (
                                    "PftParser::ParseWith: "
                                    + "field not specified"
                                );

                            throw new PftSyntaxException(token);
                        }

                        result.Fields.Add(specification);
                        break;

                    case PftTokenKind.Comma:
                        break;

                    default:
                        Magna.Error
                            (
                                "PftParser::ParseWith: "
                                + "unexpected token="
                                + token.Kind
                            );

                        throw new PftSyntaxException(token);
                }

                fieldTokens.MoveNext();
            }

            NestedContext
                (
                    result.Body,
                    bodyTokens
                );

            Tokens.MoveNext();

            return result;
        }

        //=================================================

        private PftNode ParseX()
        {
            return MoveNext(new PftX(Tokens.Current));
        }

        //================================================================
        // Other routines
        //================================================================

        private PftProgram ParseProgram()
        {
            var result = new PftProgram();

            while (!Tokens.IsEof)
            {
                var node = ParseNext();
                result.Children.Add(node);
            }

            return result;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Parse the tokens.
        /// </summary>
        public PftProgram Parse()
        {
            PftProgram result;

            try
            {
                result = ParseProgram();
                result.Procedures = _procedures;
            }
            catch (Exception exception)
            {
                if (!ReferenceEquals(Tokens.Current, null))
                {
                    var tokenText = "Current token: " + Tokens.Current;

                    var pftException = new PftException
                        (
                            tokenText,
                            exception
                        );
                    throw pftException;
                }

                throw;
            }

            return result;
        }

        #endregion
    }
}
