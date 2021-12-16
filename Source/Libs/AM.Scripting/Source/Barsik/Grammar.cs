// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* Grammar.cs -- грамматика Барсика
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.Globalization;

using Sprache;

using static Sprache.Parse;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Грамматика Барсика.
    /// </summary>
    static class Grammar
    {
        #region Public methods

        /// <summary>
        /// Разбор текста программы.
        /// </summary>
        public static ProgramNode ParseProgram
            (
                string sourceCode
            )
        {
            Sure.NotNull (sourceCode);

            sourceCode = BarsikUtility.RemoveComments (sourceCode);

            return Program.End().Parse (sourceCode);
        }

        /// <summary>
        /// Разбор текста программы.
        /// </summary>
        public static AtomNode? ParseExpression
            (
                string sourceCode
            )
        {
            Sure.NotNull (sourceCode);

            sourceCode = BarsikUtility.RemoveComments (sourceCode);

            return ArithmeticExpression.End().Parse (sourceCode);
        }

        #endregion

        #region Private methods

        #endregion

        #region Grammar itself

        private static readonly Parser<string> DirectiveCode =
            from hash in Char ('#')
            from code in Chars ("lru").Once().Text()
            select code;

        private static readonly Parser<string> DirectiveArgument =
            AnyChar.Until (LineEnd).Text();

        private static readonly Parser<Directive> Directive =
            from code in DirectiveCode
            from ws in WhiteSpace.AtLeastOnce()
            from argument in DirectiveArgument
            select new Directive (code, argument);

        private static readonly Parser<IEnumerable<Directive>> Directives =
            Directive.Token().Many();

        private static readonly Parser<ConstantNode> NullLiteral =
            from _ in String ("null")
            select new ConstantNode (null);

        private static readonly Parser<ConstantNode> BoolLiteral =
            from text in String ("true").Or (String ("false")).Text()
            select new ConstantNode (text == "true");

        private static readonly Parser<ConstantNode> CharLiteral =
            Resolve.EscapedLiteral('\'').Select
                (
                    text => new ConstantNode (Resolve.UnescapeText (text).FirstChar())
                );

        private static readonly Parser<ConstantNode> StringLiteral =
            Resolve.EscapedLiteral().Select
                (
                    text => new ConstantNode (Resolve.UnescapeText (text))
                );

        private static readonly Parser<ConstantNode> DoubleLiteral =
            from whole in Number
            from dot in Char ('.')
            from frac in Number
            select new ConstantNode (double.Parse (whole + '.' + frac, CultureInfo.InvariantCulture));

        private static readonly Parser<ConstantNode> Int32Literal =
            from number in Number
            select new ConstantNode (int.Parse (number, CultureInfo.InvariantCulture));

        private static readonly Parser<AtomNode> Constant =
            NullLiteral.Or (CharLiteral).Or (BoolLiteral)
                .Or (DoubleLiteral).Or (StringLiteral).Or (Int32Literal);

        private static readonly Parser<KeyValueNode> KeyAndValue =
            from key in Constant
            from colon in Char (':').Token()
            from value in Constant
            select new KeyValueNode (key, value);

        private static readonly Parser<string> Identifier =
            Parse.Identifier (Letter.Or (Char ('_')).Or (Char ('$')),
                LetterOrDigit.Or (Char ('_').Or (Char ('$'))));

        private static readonly Parser<AtomNode> Variable =
            from identifier in Identifier.Text()
            select new VariableNode (identifier);

        private static readonly Parser<AtomNode> Negation =
            from minus in Char ('-').Token()
            from inner in Atom
            select new NegationNode (inner);

        private static readonly Parser<AtomNode> Not =
            from minus in Char ('!').Token()
            from inner in Atom
            select new NotNode (inner);

        private static readonly Parser<AtomNode> Parenthesis =
            Ref (() => ArithmeticExpression).RoundBraces()
                .Select (s => new ParenthesisNode (s));

        private static readonly Parser<AtomNode> FunctionCall =
            from name in Identifier
            from args in Ref (() => Atom)
                .DelimitedBy (Char (',').Token()).Optional().RoundBraces()
            select new FreeCallNode (name, args.GetOrDefault());

        private static readonly Parser<AtomNode> Property =
            from objectName in Identifier
            from dot in Char ('.')
            from propertyName in Identifier
            select new PropertyNode (objectName, propertyName);

        private static readonly Parser<AtomNode> MethodCall =
            from objectName in Identifier
            from dot in Char ('.')
            from memberName in Identifier
            from args in Ref (() => Atom)
                .DelimitedBy (Char (',').Token()).Optional().RoundBraces()
            select new MethodNode (objectName, memberName, args.GetOrDefault());

        private static readonly Parser<AtomNode> List =
            from open in Char ('[').Token()
            from items in Ref (() => Atom)
                .DelimitedBy (Char (',').Token()).Optional()
            from close in Char (']').Token()
            select new ListNode (items.GetOrDefault());

        private static readonly Parser<DictionaryNode> Dictionary =
            from items in KeyAndValue
                .DelimitedBy (Char (',').Token()).Optional().CurlyBraces()
            select new DictionaryNode (items.GetOrDefault());

        private static readonly Parser<AtomNode> Index =
            from objectName in Identifier
            from open in Char ('[').Token()
            from index in Ref (() => Atom)
            from close in Char (']').Token()
            select new IndexNode (objectName, index);

        private static readonly Parser<AtomNode> New =
            from _ in String ("new").Token()
            from typeName in Identifier
            from args in Ref (() => Atom).DelimitedBy (Char (',').Token()).Optional().RoundBraces()
            select new NewNode (typeName, args.GetOrDefault());

        private static readonly Parser<string> AndOr =
            String ("and").Token().Or (String ("or").Token()).Text();

        private static readonly Parser<AtomNode> Comparison =
            from left in Ref (() => Atom)
            from op in Compare
            from right in Ref (() => Atom)
            select new BinaryNode (left, right, op);

        private static readonly Parser<AtomNode> Condition =
            from condition in ChainOperator
                (
                    AndOr.Token(),
                    Comparison.Or (Variable),
                    (op, left, right) =>
                        new ConditionNode (left, right, op)
                )
            select condition;

        private static readonly Parser<AtomNode> Increment =
            (from identifier in Identifier
            from suffix in (String ("++").Or (String ("--"))).Text().Token()
            select new IncrementNode (identifier, null, suffix))
            .Or
            (from prefix in (String ("++").Or (String ("--"))).Text().Token()
            from identifier in Identifier
            select new IncrementNode (identifier, prefix, null));

        // private static readonly Parser<AtomNode> Ternary =
        //     from condition in Condition
        //     from question in Parse.Char ('?').Token()
        //     from trueValue in Parse.Ref (() => Atom)
        //     from colon in Parse.Char (':').Token()
        //     from falseValue in Parse.Ref (() => Atom)
        //     select new TernaryNode (condition, trueValue, falseValue);

        private static readonly Parser<AtomNode> Atom =
            MethodCall.Or (New). Or (Parenthesis).Or (Dictionary).Or (List)
                .Or (Index).Or (Property).Or (Constant).Or (Increment)
                .Or (FunctionCall).Or (Variable).Or (Negation).Or (Not);

        private static readonly Parser<string> Compare =
            String ("<=").Token()
                .Or (String ("<").Token())
                .Or (String (">=").Token())
                .Or (String (">").Token())
                .Or (String ("==").Token())
                .Or (String ("!=").Token()).Text();

        private static readonly Parser<AtomNode> Multiplication =
            from node in ChainOperator
                (
                    (String ("*").Or (String ("/"))
                        .Or (String ("%"))).Token().Text(),
                    Atom,
                    (op, left, right) => new BinaryNode (left, right, op)
                )
            select node;

        private static readonly Parser<AtomNode> Addition =
            from node in ChainOperator
                (
                    (String ("+").Or (String ("-"))).Token().Text(),
                    Multiplication,
                    (op, left, right) =>
                        new BinaryNode (left, right, op)
                )
            select node;

        private static readonly Parser<AtomNode> ArithmeticExpression =
            from atom in Addition.XOr (Atom)
            select atom;

        private static readonly Parser<string> _Member =
            from dot in Char ('.')
            from memberName in Identifier.Text()
            select memberName;

        private static readonly Parser<AtomNode> _Index =
            from open in Char ('[').Token()
            from index in Atom
            from close in Char (']').Token()
            select index;

        private static readonly Parser<TargetNode> Target =
            from variable in Identifier.Text()
            from member in _Member.Optional()
            from index in _Index.Optional()
            select new TargetNode (variable, member.GetOrDefault(), index.GetOrDefault());

        private static readonly Parser<StatementNode> Assignment =
            from variable in Target
            from eq in Char ('=').Token()
            from expression in ArithmeticExpression
            select new AssignmentNode (variable, expression);

        private static readonly Parser<StatementNode> NotAssignment =
            from expression in MethodCall.Or (FunctionCall).Or (Increment)
            select new NotAssignmentNode (expression);

        private static readonly Parser<StatementNode> Return =
            from _ in String ("return").Token()
            from expression in Atom.Optional()
            select new ReturnNode (expression.GetOrDefault());

        private static readonly Parser<StatementNode> Print =
            from print in (String ("println").Or (String ("print"))).Token().Text()
            from variables in Atom.DelimitedBy (Char (',').Token()).Optional()
            select new PrintNode (variables.GetOrDefault(), print == "println");

        private static readonly Parser<IEnumerable<StatementNode>> Else =
            from _ in String ("else")
            from statements in Ref (() => Block).CurlyBraces()
            select statements;

        private static readonly Parser<IfNode> ElseIf =
            from _1 in String ("else").Token()
            from _2 in String ("if")
            from condition in Condition.RoundBraces()
            from statements in Block.CurlyBraces()
            select new IfNode (condition, statements, null, null);

        private static readonly Parser<StatementNode> If =
            from _ in String ("if")
            from condition in Condition.RoundBraces()
            from thenBlock in Ref (() => Block).CurlyBraces()
            from elseIf in ElseIf.Many().Optional()
            from elseBlock in Else.Optional()
            select new IfNode (condition, thenBlock, elseIf.GetOrDefault(), elseBlock.GetOrDefault());

        private static readonly Parser<StatementNode> While =
            from _ in String ("while")
            from condition in Condition.RoundBraces()
            from statements in Block.CurlyBraces()
            select new WhileNode (condition, statements);

        private static readonly Parser<StatementNode> ForEach =
            from _1 in String ("foreach")
            from open1 in Char ('(').Token()
            from variableName in Identifier
            from _2 in String ("in").Token()
            from enumerable in Atom
            from close1 in Char (')').Token()
            from statements in Block.CurlyBraces()
            select new ForEachNode (variableName, enumerable, statements);

        private static readonly Parser<StatementNode> For =
            from _1 in String ("for")
            from open1 in Char ('(').Token()
            from init in Assignment
            from _2 in Char (';').Token()
            from condition in Condition
            from _3 in Char (';').Token()
            from step in Assignment.Or (NotAssignment)
            from close1 in Char (')').Token()
            from statements in Block.CurlyBraces()
            from elseBody in Else.Optional()
            select new ForNode (init, condition, step, statements, elseBody.GetOrDefault());

        private static readonly Parser<CatchNode> Catch =
            from _ in String ("catch")
            from variable in Identifier.RoundBraces()
            from block in Block.CurlyBraces()
            select new CatchNode (variable, block);

        private static readonly Parser<IEnumerable<StatementNode>> Finally =
            from _ in String ("finally")
            from block in Block.CurlyBraces()
            select block;

        private static readonly Parser<StatementNode> TryCatchFinally =
            from _ in String ("try")
            from tryBlock in Block.CurlyBraces()
            from catchNode in Catch.Optional()
            from finallyBlock in Finally.Optional()
            select new TryNode (tryBlock, catchNode.GetOrDefault(), finallyBlock.GetOrDefault());

        // определение функции
        private static readonly Parser<DefinitionNode> Definition =
            from _ in String ("func").Token()
            from name in Identifier
            from args in Identifier.DelimitedBy (Char (',').Token()).Optional().RoundBraces()
            from body in Ref (() => Block).CurlyBraces()
            select new DefinitionNode (name, args.GetOrDefault(), body);

        // блок using
        private static readonly Parser<StatementNode> Using =
            from _ in String ("using").Token()
            from open in Char ('(').Token()
            from variable in Identifier
            from equal in Char ('=').Token()
            from initialization in Atom
            from close in Char (')').Token()
            from body in Block.CurlyBraces()
            select new UsingNode (variable, initialization, body);

        // оператор throw
        private static readonly Parser<StatementNode> Throw =
            from _ in String ("throw").Token()
            from operand in Atom
            select new ThrowNode (operand);

        // костыль
        private static readonly Parser<StatementNode> Nop =
            from _ in Chars (" \t").Until (LineEnd)
            select new StatementNode();

        private static readonly Parser<ExternalNode> External =
            from code in CharExcept ('}').Many().Text().CurlyBraces()
            select new ExternalNode (code);

        private static readonly Parser<StatementNode> NoSemicolon =
            from statement in Nop.Or (Definition).Or (ForEach).Or (For)
                .Or (While).Or (If).Or (TryCatchFinally).Or (External)
                .Or (Using)
            select statement;

        private static readonly Parser<StatementNode> RequireSemicolon =
            from statement in Print.Or (Return).Or (Throw).Or (Assignment).Or (NotAssignment)
            from semicolon in Char (';').Token()
            select statement;

        private static readonly Parser<StatementNode> Statement =
            (from statement in NoSemicolon.Or (RequireSemicolon)
            select statement).Positioned();

        private static readonly Parser<IEnumerable<StatementNode>> Block =
            from statements in Statement.Many()
            select statements;

        // собственно программа
        private static readonly Parser<ProgramNode> Program =
            from directives in Directives.Optional()
            from block in Block
            select new ProgramNode (directives.GetOrDefault(), block);

        #endregion
    }
}
