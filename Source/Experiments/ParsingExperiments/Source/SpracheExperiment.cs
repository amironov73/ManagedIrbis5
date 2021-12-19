// ReSharper disable CheckNamespace

using System;
using System.Linq;

using Sprache;

using static Sprache.Parse;

namespace ParsingExperiments;

static class SpracheExperiment
{
    class Statement
    {
        public Statement
            (
                string identifier,
                string expression
            )
        {
            _identifier = identifier;
            _expression = expression;
        }

        private readonly string _identifier;
        private readonly string _expression;

        public override string ToString()
        {
            return $"{_identifier} = {_expression}";
        }
    }

    private static void NoSemi
        (
            string sourceCode
        )
    {
        var identifier = Identifier (Letter, LetterOrDigit);
        var add = Chars ("+-").Token();
        var equ = Char ('=').Token();
        var expression = ChainOperator
            (
                add,
                Number,
                (op, left, right) => left + op + right
            );
        var statement = from id in identifier
            from eq in equ
            from expr in expression
            select new Statement (id, expr);
        var program = statement.DelimitedBy (WhiteSpace.AtLeastOnce()).End();

        var parsed = program.Parse (sourceCode).ToArray();
        foreach (var stmt in parsed)
        {
            Console.WriteLine (stmt);
        }

        Console.WriteLine (new string ('-', 10));
    }

    public static void NoSemicolons()
    {
        NoSemi ("x = 1 y = 2");
        NoSemi ("x = 1 + 2\r\ny = 2 + 3 + 4");
    }
}
