// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Local
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Local
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ReturnTypeCanBeNotNullable
// ReSharper disable StringLiteralTypo

using System;
using System.Collections.Generic;
using System.Linq;

using Pidgin;
using Pidgin.Expression;

using static Pidgin.Parser;
using static Pidgin.Parser<char>;

#nullable enable

namespace ParsingExperiments;

static class PidginExperimentThirteen
{
    sealed class Context
    {
    }

    class AstNode
    {
    }

    class CallNode
    {
        public string Name { get; }
        public IEnumerable<int>? Arguments { get; }

        public CallNode(string name, IEnumerable<int>? arguments)
        {
            Name = name;
            Arguments = arguments;
        }
    }

    abstract class SubNode
    {
        /// <summary>
        /// Вычисление значения.
        /// </summary>
        public abstract dynamic? Compute (Context context);

        /// <summary>
        /// Присвоение значения.
        /// </summary>
        public abstract void Assign (Context context, dynamic? value);
    }

    sealed class PropertySubNode
        : SubNode
    {
        private readonly SubNode _left;
        private readonly string _name;

        public PropertySubNode (SubNode left, string name)
        {
            _left = left;
            _name = name;
        }

        public override dynamic? Compute (Context context)
        {
            throw new NotImplementedException();
        }

        public override void Assign (Context context, dynamic? value)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"({_left}).{_name}";
        }
    }

    sealed class IndexSubNode
        : SubNode
    {
        private readonly SubNode _left;
        private readonly int _right;

        public IndexSubNode (SubNode left, int right)
        {
            _left = left;
            _right = right;
        }

        public override dynamic? Compute (Context context)
        {
            throw new NotImplementedException();
        }

        public override void Assign (Context context, dynamic? value)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"({_left}) [{_right}]";
        }
    }

    sealed class MethodSubNode
        : SubNode
    {
        private readonly SubNode _left;
        private readonly string _name;
        private readonly int[] _arguments;

        public MethodSubNode (SubNode left, string name, IEnumerable<int>? arguments)
        {
            _left = left;
            _name = name;
            _arguments = Array.Empty<int>();
            if (arguments is not null)
            {
                _arguments = arguments.ToArray();
            }
        }

        public override dynamic? Compute (Context context)
        {
            throw new NotImplementedException();
        }

        public override void Assign (Context context, dynamic? value)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            var args = string.Join (", ", _arguments);
            return $"{_name} (({_left}) ({args}))";
        }
    }

    sealed class RootSubNode
        : SubNode
    {
        private readonly string _name;

        public RootSubNode(string name)
        {
            _name = name;
        }

        public override dynamic? Compute (Context context)
        {
            throw new NotImplementedException();
        }

        public override void Assign (Context context, dynamic? value)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return $"root {_name}";
        }
    }

    sealed class TargetNode
        : AstNode
    {
        private readonly SubNode _top;

        public TargetNode(SubNode top)
        {
            _top = top;
        }

        public override string ToString()
        {
            return _top.ToString()!;
        }
    }

    sealed class AssignmentNode
        : AstNode
    {
        private readonly TargetNode _target;
        private readonly int _value;

        public AssignmentNode(TargetNode target, int value)
        {
            _target = target;
            _value = value;
        }

        public override string ToString()
        {
            return $"{_target} = {_value}";
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

    private static readonly Parser<char, SubNode> _Root = Identifier
        .Select<SubNode> (v => new RootSubNode (v));

    private static readonly Parser<char, string> _Property =
        Tok ('.').Then (Identifier);

    private static readonly Parser<char, int> _Index =
        Tok (Num).Between (Tok ('['), Tok (']'));

    private static readonly Parser<char, CallNode> _Call = Map
        (
            (_, name, _, args, _) => new CallNode (name, args.GetValueOrDefault()),
            Tok ('.'),
            Tok (Identifier),
            Tok ('('),
            Try (Num).Separated (Tok (',')).Optional(),
            Tok (')')
        );


    private static Parser<char, Func<SubNode, SubNode>> Property (Parser<char, string> op) =>
        op.Select<Func<SubNode, SubNode>> (name => node => new PropertySubNode (node, name));

    private static Parser<char, Func<SubNode, SubNode>> Index (Parser<char, int> op) =>
        op.Select<Func<SubNode, SubNode>> (index => node => new IndexSubNode (node, index));

    private static Parser<char, Func<SubNode, SubNode>> Call (Parser<char, CallNode> op) =>
        op.Select<Func<SubNode, SubNode>> (call => node => new MethodSubNode (node, call.Name, call.Arguments));

    private static readonly Parser<char, SubNode> Target = ExpressionParser.Build
        (
            OneOf
                (
                    _Root
                ),
            new []
            {
                Operator.PostfixChainable
                    (
                        Try (Call (_Call)),
                        Try (Property (_Property)),
                        Try (Index (_Index))
                    )
            }
        );

    private static readonly Parser<char, AstNode> Assignment = Map
        (
            (target, _, value) => (AstNode) new AssignmentNode (new TargetNode (target), value),
            Tok (Target),
            Tok ('='),
            Tok (Num)
        );


    // блок стейтментов
    private static readonly Parser<char, IEnumerable<AstNode>> Block =
        Tok (Assignment).SeparatedAndOptionallyTerminated (Tok (';'));

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

    public static void Targets()
    {
        ParseAndPrint ("a = 1;");
        ParseAndPrint ("a.b = 1;");
        ParseAndPrint ("a.b.c = 1;");

        ParseAndPrint ("a[1] = 1;");
        ParseAndPrint ("a[1][2] = 1;");
        ParseAndPrint ("a[1][2][3] = 1;");

        ParseAndPrint ("a.b() = 1;");
        ParseAndPrint ("a.b().c() = 1;");
        ParseAndPrint ("a.b(1) = 1;");
        ParseAndPrint ("a.b(1, 2) = 1;");
        ParseAndPrint ("a.b(1).c(2, 3) = 1;");

        ParseAndPrint ("a.first[1].second[2] = 1;");
        ParseAndPrint ("a.first(1, 2).second[3] = 1;");
    }

}
