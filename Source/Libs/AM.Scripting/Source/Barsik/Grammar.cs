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

using AM.Text;

using Sprache;

#endregion

#nullable enable

namespace AM.Scripting.Barsik
{
    /// <summary>
    /// Грамматика Барсика.
    /// </summary>
    static class Grammar
    {
        /// <summary>
        /// Разбор текста программы.
        /// </summary>
        public static ProgramNode ParseProgram
            (
                string sourceCode
            )
        {
            Sure.NotNull (sourceCode);

            sourceCode = RemoveComments (sourceCode);

            return Program.End().Parse (sourceCode);
        }

        /// <summary>
        /// Замена в исходном тексте комментариев в стиле C/C++ на пробелы.
        /// </summary>
        private static string RemoveComments
            (
                string sourceCode
            )
        {
            if (!sourceCode.Contains ("//") && !sourceCode.Contains ("/*"))
            {
                return sourceCode;
            }

            var builder = StringBuilderPool.Shared.Get();
            builder.EnsureCapacity (sourceCode.Length);

            var navigator = new ValueTextNavigator (sourceCode);
            while (!navigator.IsEOF)
            {
                var line = navigator.ReadUntil ('/');
                builder.Append (line);
                if (navigator.IsEOF)
                {
                    break;
                }

                var first = navigator.ReadChar();
                var second = navigator.ReadChar();
                if (second == '/')
                {
                    navigator.ReadLine();
                    builder.AppendLine();
                }
                else if (second == '*')
                {
                    var commented = navigator.ReadToString ("*/");
                    builder.Append ("  "); // заменяем "/*"
                    foreach (var c1 in commented)
                    {
                        var c2 = c1 switch
                        {
                            '\r' => '\r',
                            '\n' => '\n',
                            '\t' => '\t',
                            _ => ' '
                        };
                        builder.Append (c2);
                    }

                    builder.Append ("  "); // заменяем "*/"
                }
                else
                {
                    builder.Append (first);
                }
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return (builder);

            return result;
        }

        private static readonly Parser<string> DirectiveCode =
            from hash in Parse.Char ('#')
            from code in Parse.Chars ("lru").Once().Text()
            select code;

        private static readonly Parser<string> DirectiveArgument =
            Parse.AnyChar.Until (Parse.LineEnd).Text();

        private static readonly Parser<Directive> Directive =
            from code in DirectiveCode
            from ws in Parse.WhiteSpace.AtLeastOnce()
            from argument in DirectiveArgument
            select new Directive (code, argument);

        private static readonly Parser<IEnumerable<Directive>> Directives =
            Directive.Token().Many();

        private static readonly Parser<ConstantNode> NullLiteral =
            from _ in Parse.String ("null")
            select new ConstantNode (null);

        private static readonly Parser<ConstantNode> BoolLiteral =
            from text in Parse.String ("true").Or (Parse.String ("false")).Text()
            select new ConstantNode (text == "true");

        private static readonly Parser<ConstantNode> CharLiteral =
            from open in Parse.Char ('\'')
            from symbol in Parse.CharExcept ('\'')
            from close in Parse.Char ('\'')
            select new ConstantNode (symbol);

        private static readonly Parser<ConstantNode> StringLiteral =
            from open in Parse.Char ('"')
            from text in Parse.CharExcept ('"').Many().Text()
            from close in Parse.Char ('"')
            select new ConstantNode (text);

        private static readonly Parser<ConstantNode> DoubleLiteral =
            from whole in Parse.Number
            from dot in Parse.Char ('.')
            from frac in Parse.Number
            select new ConstantNode (double.Parse (whole + '.' + frac, CultureInfo.InvariantCulture));

        private static readonly Parser<ConstantNode> Int32Literal =
            from number in Parse.Number
            select new ConstantNode (int.Parse (number, CultureInfo.InvariantCulture));

        private static readonly Parser<AtomNode> Constant =
            NullLiteral.Or (CharLiteral).Or (BoolLiteral)
                .Or (DoubleLiteral).Or (StringLiteral).Or (Int32Literal);

        private static readonly Parser<KeyValueNode> KeyAndValue =
            from key in Constant
            from colon in Parse.Char (':').Token()
            from value in Constant
            select new KeyValueNode (key, value);

        private static readonly Parser<string> Identifier =
            Parse.Identifier (Parse.Letter.Or (Parse.Char ('_')),
                Parse.LetterOrDigit.Or (Parse.Char ('_')));

        private static readonly Parser<AtomNode> Variable =
            from identifier in Identifier.Text()
            select new VariableNode (identifier);

        private static readonly Parser<AtomNode> Negation =
            from minus in Parse.Char ('-')
            from inner in Atom
            select new NegationNode (inner);

        private static readonly Parser<AtomNode> Parenthesis =
            from open in Parse.Char ('(').Token()
            from inner in Parse.Ref (() => ArithmeticExpression)
            from close in Parse.Char (')').Token()
            select new ParenthesisNode (inner);

        private static readonly Parser<AtomNode> FunctionCall =
            from name in Identifier
            from open in Parse.Char ('(').Token()
            from args in Parse.Ref (() => Atom).DelimitedBy (Parse.Char (',').Token()).Optional()
            from close in Parse.Char (')')
            select new FreeCallNode (name, args.GetOrDefault());

        private static readonly Parser<AtomNode> Property =
            from objectName in Identifier
            from dot in Parse.Char ('.')
            from propertyName in Identifier
            select new PropertyNode (objectName, propertyName);

        private static readonly Parser<AtomNode> MethodCall =
            from objectName in Identifier
            from dot in Parse.Char ('.')
            from memberName in Identifier
            from open in Parse.Char ('(').Token()
            from args in Parse.Ref (() => Atom).DelimitedBy (Parse.Char (',').Token()).Optional()
            from close in Parse.Char (')').Token()
            select new MethodNode (objectName, memberName, args.GetOrDefault());

        private static readonly Parser<AtomNode> List =
            from open in Parse.Char ('[').Token()
            from items in Parse.Ref (() => Atom).DelimitedBy (Parse.Char (',').Token()).Optional()
            from close in Parse.Char (']').Token()
            select new ListNode (items.GetOrDefault());

        private static readonly Parser<DictionaryNode> Dictionary =
            from open in Parse.Char ('{').Token()
            from items in KeyAndValue.DelimitedBy (Parse.Char (',').Token()).Optional()
            from close in Parse.Char ('}').Token()
            select new DictionaryNode (items.GetOrDefault());

        private static readonly Parser<AtomNode> Index =
            from objectName in Identifier
            from open in Parse.Char ('[').Token()
            from index in Parse.Ref (() => Atom)
            from close in Parse.Char (']').Token()
            select new IndexNode (objectName, index);

        private static readonly Parser<AtomNode> New =
            from _ in Parse.String ("new").Token()
            from typeName in Identifier
            from open in Parse.Char ('(').Token()
            from args in Parse.Ref (() => Atom).DelimitedBy (Parse.Char (',').Token()).Optional()
            from close in Parse.Char (')').Token()
            select new NewNode (typeName, args.GetOrDefault());

        private static readonly Parser<string> AndOr =
            Parse.String ("and").Token().Or (Parse.String ("or").Token()).Text();

        private static readonly Parser<AtomNode> Comparison =
            from left in Parse.Ref (() => Atom)
            from op in Compare
            from right in Parse.Ref (() => Atom)
            select new BinaryNode (left, right, op);

        private static readonly Parser<AtomNode> Condition =
            from condition in Parse.ChainOperator
                (
                    AndOr.Token(),
                    Comparison,
                    (op, left, right) =>
                        new ConditionNode (left, right, op)
                )
            select condition;

        private static readonly Parser<AtomNode> Atom =
            MethodCall.Or (New). Or (Parenthesis).Or (Dictionary).Or (List)
                .Or (Index).Or (Property).Or (Constant)
                .Or (FunctionCall).Or (Variable).Or (Negation);

        private static readonly Parser<string> Compare =
            Parse.String ("<=").Token()
                .Or (Parse.String ("<").Token())
                .Or (Parse.String (">=").Token())
                .Or (Parse.String (">").Token())
                .Or (Parse.String ("==").Token())
                .Or (Parse.String ("!=").Token()).Text();

        private static readonly Parser<AtomNode> Multiplication =
            from node in Parse.ChainOperator
                (
                    (Parse.String ("*").Or (Parse.String ("/"))
                        .Or (Parse.String ("%"))).Token().Text(),
                    Atom,
                    (op, left, right) => new BinaryNode (left, right, op)
                )
            select node;

        private static readonly Parser<AtomNode> Addition =
            from node in Parse.ChainOperator
                (
                    (Parse.String ("+").Or (Parse.String ("-"))).Token().Text(),
                    Multiplication,
                    (op, left, right) =>
                        new BinaryNode (left, right, op)
                )
            select node;

        private static readonly Parser<AtomNode> ArithmeticExpression =
            from atom in Addition.XOr (Atom)
            select atom;

        private static readonly Parser<string> _Member =
            from dot in Parse.Char ('.')
            from memberName in Identifier.Text()
            select memberName;

        private static readonly Parser<AtomNode> _Index =
            from open in Parse.Char ('[').Token()
            from index in Atom
            from close in Parse.Char (']').Token()
            select index;

        private static readonly Parser<TargetNode> Target =
            from variable in Identifier.Text()
            from member in _Member.Optional()
            from index in _Index.Optional()
            select new TargetNode (variable, member.GetOrDefault(), index.GetOrDefault());

        private static readonly Parser<StatementNode> Assignment =
            from variable in Target
            from eq in Parse.Char ('=').Token()
            from expression in ArithmeticExpression
            select new AssignmentNode (variable, expression);

        private static readonly Parser<StatementNode> NotAssignment =
            from expression in MethodCall.Or (FunctionCall)
            select new NotAssignmentNode (expression);

        private static readonly Parser<StatementNode> Return =
            from _ in Parse.String ("return").Token()
            from expression in Atom.Optional()
            select new ReturnNode (expression.GetOrDefault());

        private static readonly Parser<StatementNode> Print =
            from print in (Parse.String ("println").Or (Parse.String ("print"))).Token().Text()
            from variables in Atom.DelimitedBy (Parse.Char (',').Token()).Optional()
            select new PrintNode (variables.GetOrDefault(), print == "println");

        private static readonly Parser<IEnumerable<StatementNode>> Else =
            from _ in Parse.String ("else")
            from open in Parse.Char ('{').Token()
            from statements in Parse.Ref (() => Block)
            from close in Parse.Char ('}').Token()
            select statements;

        private static readonly Parser<StatementNode> If =
            from _ in Parse.String ("if")
            from open1 in Parse.Char ('(').Token()
            from condition in Condition
            from close1 in Parse.Char (')').Token()
            from open2 in Parse.Char ('{').Token()
            from thenBlock in Parse.Ref (() => Block)
            from close2 in Parse.Char ('}').Token()
            from elseBlock in Else.Optional()
            select new IfNode (condition, thenBlock, elseBlock.GetOrDefault());

        private static readonly Parser<StatementNode> While =
            from _ in Parse.String ("while")
            from open1 in Parse.Char ('(').Token()
            from condition in Condition
            from close1 in Parse.Char (')').Token()
            from open2 in Parse.Char ('{').Token()
            from statements in Block
            from close2 in Parse.Char ('}').Token()
            select new WhileNode (condition, statements);

        private static readonly Parser<StatementNode> ForEach =
            from _1 in Parse.String ("foreach")
            from open1 in Parse.Char ('(').Token()
            from variableName in Identifier
            from _2 in Parse.String ("in").Token()
            from enumerable in Atom
            from close1 in Parse.Char (')').Token()
            from open2 in Parse.Char ('{').Token()
            from statements in Block
            from close2 in Parse.Char ('}').Token()
            select new ForEachNode (variableName, enumerable, statements);

        private static readonly Parser<StatementNode> For =
            from _1 in Parse.String ("for")
            from open1 in Parse.Char ('(').Token()
            from init in Assignment
            from _2 in Parse.Char (';').Token()
            from condition in Condition
            from _3 in Parse.Char (';').Token()
            from step in Assignment
            from close1 in Parse.Char (')').Token()
            from open2 in Parse.Char ('{').Token()
            from statements in Block
            from close2 in Parse.Char ('}').Token()
            select new ForNode (init, condition, step, statements);

        // определение функции
        private static readonly Parser<DefinitionNode> Definition =
            from _ in Parse.String ("func").Token()
            from name in Identifier
            from open1 in Parse.Char ('(').Token()
            from args in Identifier.DelimitedBy (Parse.Char (',').Token()).Optional()
            from close1 in Parse.Char (')').Token()
            from open2 in Parse.Char ('{')
            from body in Parse.Ref (() => Block)
            from close2 in Parse.Char ('}')
            select new DefinitionNode (name, args.GetOrDefault(), body);

        // костыль
        private static readonly Parser<StatementNode> Nop =
            from _ in Parse.Chars (" \t").Until (Parse.LineEnd)
            select new StatementNode();

        private static readonly Parser<StatementNode> NoSemicolon =
            from statement in Nop.Or (Definition).Or (ForEach).Or (For).Or (While).Or (If)
            select statement;

        private static readonly Parser<StatementNode> RequireSemicolon =
            from statement in Print.Or (Return).Or (Assignment).Or (NotAssignment)
            from semicolon in Parse.Char (';').Token()
            select statement;

        private static readonly Parser<StatementNode> Statement =
            from statement in NoSemicolon.Or (RequireSemicolon)
            select statement;

        private static readonly Parser<IEnumerable<StatementNode>> Block =
            from statements in Statement.Many()
            select statements;

        private static readonly Parser<ProgramNode> Program =
            from directives in Directives.Optional()
            from block in Block
            select new ProgramNode (directives.GetOrDefault(), block);
    }
}
