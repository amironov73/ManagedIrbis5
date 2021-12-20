// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable InconsistentNaming

using System;
using System.Collections.Generic;
using System.Globalization;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

namespace ParsingExperiments;

static class PidginExperimentThree
{
    sealed class Context
    {
        public readonly Dictionary<string, double> Variables = new ();
    }

    class Node
    {
        public virtual double Compute (Context context) => 0.0;
    }

    sealed class LiteralNode : Node
    {
        private readonly double _value;

        public LiteralNode(double value)
        {
            _value = value;
        }

        public override double Compute (Context context)
        {
            return _value;
        }

        public override string ToString()
        {
            return _value.ToString (CultureInfo.InvariantCulture);
        }
    }

    // скобки
    sealed class GroupNode : Node
    {
        private readonly Node _inner;

        public GroupNode(Node inner)
        {
            _inner = inner;
        }

        public override double Compute (Context context)
        {
            return _inner.Compute (context);
        }

        public override string ToString()
        {
            return $"( {_inner} )";
        }
    }

    sealed class VariableNode : Node
    {
        private readonly string _name;

        public VariableNode(string name)
        {
            _name = name;
        }

        public override double Compute (Context context)
        {
            context.Variables.TryGetValue (_name, out var value);

            return value;
        }

        public override string ToString()
        {
            return _name;
        }
    }

    sealed class BinaryNode : Node
    {
        private readonly string _operation;
        private readonly Node _left;
        private readonly Node _right;

        public BinaryNode (string operation, Node left, Node right)
        {
            _operation = operation;
            _left = left;
            _right = right;
        }

        public override double Compute (Context context)
        {
            return _operation switch
            {
                "+" => _left.Compute (context) + _right.Compute (context),
                "-" => _left.Compute (context) - _right.Compute (context),
                "*" => _left.Compute (context) * _right.Compute (context),
                "/" => _left.Compute (context) / _right.Compute (context),
                _ => throw new Exception()
            };
        }

        public override string ToString()
        {
            return $"({_left} {_operation} {_right})";
        }
    }

    sealed class PostfixNode : Node
    {
        private readonly string _operation;
        private readonly Node _node;

        public PostfixNode(string operation, Node node)
        {
            _operation = operation;
            _node = node;
        }

        public override double Compute (Context context)
        {
            var value = _node.Compute (context);

            return _operation switch
            {
                "++" => value + 1.0,
                "--" => value - 1.0,
                _ => Double.NaN
            };
        }

        public override string ToString()
        {
            return $"{_node} {_operation}";
        }
    }

    class StatementNode
    {
        public virtual void Execute (Context context)
        {
        }
    }

    sealed class AssignmentNode : StatementNode
    {
        public AssignmentNode (string name, Node node)
        {
            _name = name;
            _node = node;
        }

        private readonly string _name;
        private readonly Node _node;

        public override void Execute (Context context)
        {
            var value = _node.Compute (context);
            context.Variables[_name] = value;
            Console.WriteLine ($"{_name} <- {value}");
        }

        public override string ToString()
        {
            return $"{_name} = {_node}";
        }
    }

    private static Parser<char, T> Tok<T> (Parser<char, T> token) =>
        token.Between (SkipWhitespaces);

    private static Parser<char, string> Tok (string token) =>
        Tok (String (token));

    private static readonly Parser<char, string> Id =
        Letter.Then (LetterOrDigit.ManyString(), (_1, _2) => _1 + _2);

    private static readonly Parser<char, Node> Var =
        Tok (Id).Select<Node> (name => new VariableNode (name));

    private static readonly Parser<char, Node> Literal =
        Tok (Real).Select<Node> (s => new LiteralNode (s));

    private static Parser<char, Func<Node, Node, Node>> Binary (string op) =>
        Tok (String (op)).Select<Func<Node, Node, Node>>
            (
                opType => (left, right) => new BinaryNode (opType, left, right)
            );

    private static Parser<char, Func<Node, Node>> Postfix (string op) =>
        Tok (String (op)).Select<Func<Node, Node>>
            (
                opType => node => new PostfixNode (opType, node)
            );

    private static readonly OperatorTableRow<char, Node> Plus  = Operator.InfixL (Binary ("+"));
    private static readonly OperatorTableRow<char, Node> Minus = Operator.InfixL (Binary ("-"));
    private static readonly OperatorTableRow<char, Node> Star  = Operator.InfixL (Binary ("*"));
    private static readonly OperatorTableRow<char, Node> Slash = Operator.InfixL (Binary ("/"));

    private static readonly OperatorTableRow<char, Node> PlusPlus   = Operator.Postfix (Postfix ("?"));
    private static readonly OperatorTableRow<char, Node> MinusMinus = Operator.Postfix (Postfix ("!"));

    // скобки
    private static readonly Parser<char, Node> Group =
        from open in Tok ("(")
        from inner in Rec (() => Expr!)
        from close in Tok (")")
        select (Node) new GroupNode (inner);

    private static readonly Parser<char, Node> Expr = ExpressionParser.Build
        (
            OneOf (Literal, Var, Group),
            new []
            {
                PlusPlus.And (MinusMinus),
                Star.And (Slash),
                Plus.And (Minus)
            }
        );

    private static readonly Parser<char, StatementNode> Assignment =
        from name in Tok (Id)
        from eq in Char ('=').Between (SkipWhitespaces)
        from expr in Expr
        select (StatementNode) new AssignmentNode (name, expr);

    private static readonly Parser<char, IEnumerable<StatementNode>> Program =
        Assignment.SeparatedAndOptionallyTerminated (SkipWhitespaces)
            .Then (End, (_1, _) => _1);

    private static void ParseAndExecute (string sourceCode)
    {
        Console.WriteLine (sourceCode);
        Console.WriteLine (new string ('.', 8));
        var context = new Context();
        try
        {
            var program = Program.ParseOrThrow (sourceCode);
            foreach (var statement in program)
            {
                Console.WriteLine (statement);
                statement.Execute (context);
                Console.WriteLine (new string ('-', 8));
            }

            Console.WriteLine (new string ('=', 8));
            foreach (var variable in context.Variables)
            {
                Console.WriteLine ($"{variable.Key} = {variable.Value}");
            }

            Console.WriteLine (new string ('=', 8));
        }
        catch (Exception exception)
        {
            Console.WriteLine (exception.Message);
        }
    }

    public static void Assignments()
    {
        ParseAndExecute ("a=1 b=2 c=a+b");
        ParseAndExecute ("c = a + b");
        ParseAndExecute ("a = (1)");
        ParseAndExecute ("a = (1 + 2)");
        ParseAndExecute ("a = (1 + 2) * 3");
        ParseAndExecute ("a = (1 + 2) * (3 + 4)");
    }
}
