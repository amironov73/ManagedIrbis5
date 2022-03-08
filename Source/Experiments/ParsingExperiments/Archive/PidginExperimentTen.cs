// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

using System;
using System.Collections.Generic;
using System.Linq;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

static class PidginExperimentTen
{
    class AstNode
    {
    }

    sealed class VariableNode
        : AstNode
    {
        public string Name { get; }

        public VariableNode (string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return $"variable ({Name})";
        }
    }

    sealed class NumberNode
        : AstNode
    {
        public int Value { get; }

        public NumberNode (int value)
        {
            Value = value;
        }

        public override string ToString()
        {
            return $"number ({Value})";
        }
    }

    sealed class IndexNode
        : AstNode
    {
        public AstNode Var { get; }
        public AstNode Index { get; }

        public IndexNode (AstNode @var, AstNode index)
        {
            Var = var;
            Index = index;
        }

        public override string ToString()
        {
            return $"index ({Var} {Index})";
        }
    }

    sealed class AssignmentNode
        : AstNode
    {
        public string Name { get; }
        public AstNode Value { get; }

        public AssignmentNode (string name, AstNode value)
        {
            Name = name;
            Value = value;
        }

        public override string ToString()
        {
            return $"{Name} = {Value}";
        }
    }

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (SkipWhitespaces);

    public static Parser<char, char> Tok (char token) => Tok (Char (token));

    public static Parser<char, string> Tok (string token) => Tok (String (token));

    private static readonly Parser<char, string> Identifier = Map
        (
            (first, rest) => first + rest,
            Letter,
            LetterOrDigit.ManyString()
        );

    private static readonly Parser<char, AstNode> Constant =
        Tok (Num).Select<AstNode> (v => new NumberNode (v));

    private static readonly Parser<char, AstNode> Variable =
        Tok (Identifier).Select<AstNode> (v => new VariableNode (v));

    private static readonly Parser<char, AstNode> Indexing = Map
        (
            (_, n, _) => (AstNode) new NumberNode (n),
            Tok ('['),
            Tok (Num),
            Tok (']')
        );

    // ReSharper disable RedundantSuppressNullableWarningExpression
    private static readonly Parser<char, AstNode> Assignment = Map
        (
            (name, _, value) => (AstNode) new AssignmentNode (name, value),
            Tok (Identifier),
            Tok ('='),
            Tok (Rec (() => Expr!))
        );
    // ReSharper restore RedundantSuppressNullableWarningExpression

    private static Parser<char, Func<AstNode, AstNode>> Postfix (Parser<char, AstNode> op) =>
        op.Select<Func<AstNode, AstNode>> (type => v => new IndexNode (v, type));


    private static readonly Parser<char, AstNode> Expr = ExpressionParser.Build
        (
            OneOf
                (
                    Try (Constant),
                    Try (Variable)
                ),
            new []
            {
                //Operator.Postfix (Postfix (Indexing))
                Operator.PostfixChainable (Postfix (Indexing))
            }
        );

    // блок стейтментов
    private static readonly Parser<char, IEnumerable<AstNode>> Block =
        Assignment.SeparatedAndOptionallyTerminated (Tok (';'));

    private static void ParseAndPrint (string sourceCode)
    {
        Console.WriteLine (sourceCode);
        Console.WriteLine (new string ('.', 60));
        try
        {
            var statements = Block.Before (End).ParseOrThrow (sourceCode).ToArray();
            foreach (var statement in statements)
            {
                Console.WriteLine (statement);
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception);
        }

        Console.WriteLine (new string ('-', 60));
    }

    public static void IndexingAssignment()
    {
        ParseAndPrint ("hello = 1");
        ParseAndPrint ("hello = v");
        ParseAndPrint ("hello = v[1]");
        ParseAndPrint ("hello = v[1][2]");
        ParseAndPrint ("hello = v[1][2][3]");
    }
}
