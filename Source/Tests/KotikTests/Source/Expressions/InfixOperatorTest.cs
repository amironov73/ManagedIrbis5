// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class InfixOperatorTest
    : CommonParserTest
{
    private static object IntegerArithmetic
        (
            object leftOperand,
            string operationCode,
            object rightOperand
        )
    {
        var left = (int) leftOperand;
        var right = (int) rightOperand;
        var result = operationCode switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => left / right,
            _ => throw new InvalidOperationException()
        };

        return result;
    }

    [TestMethod]
    [Description ("Сложение двух целых")]
    public void InfixOperator_Parse_1()
    {
        var state = _GetState ("123 + 456");
        var parser = new InfixOperator<object>
            (
                Parser.Literal,
                new[] { "+", "-" },
                IntegerArithmetic,
                BinaryOperatorType.LeftAssociative
            )
            .End();
        var result = (int) parser.ParseOrThrow (state);
        Assert.AreEqual (579, result);
    }

    [TestMethod]
    [Description ("Сложение трех целых")]
    public void InfixOperator_Parse_2()
    {
        var state = _GetState ("12 + 34 + 56");
        var parser = new InfixOperator<object>
            (
                Parser.Literal,
                new[] { "+", "-" },
                IntegerArithmetic,
                BinaryOperatorType.LeftAssociative
            )
            .End();
        var result = (int) parser.ParseOrThrow (state);
        Assert.AreEqual (102, result);
    }

    [TestMethod]
    [Description ("Сложение одного числа")]
    public void InfixOperator_Parse_3()
    {
        var state = _GetState ("123456");
        var parser = new InfixOperator<object>
            (
                Parser.Literal,
                new[] { "+", "-" },
                IntegerArithmetic,
                BinaryOperatorType.LeftAssociative
            )
            .End();
        var result = (int) parser.ParseOrThrow (state);
        Assert.AreEqual (123456, result);
    }

    [TestMethod]
    [Description ("Парсер берет только свое")]
    public void InfixOperator_Parse_4()
    {
        var state = _GetState ("12 * 3 + 45 * 6");
        var parser = new InfixOperator<object>
                (
                    Parser.Literal,
                    new[] { "*", "/" },
                    IntegerArithmetic,
                    BinaryOperatorType.LeftAssociative
                );
        var result = (int) parser.ParseOrThrow (state);
        Assert.AreEqual (36, result);
    }

    [TestMethod]
    [Description ("Парсер берет только свое")]
    public void InfixOperator_Parse_5()
    {
        var state = _GetState ("12 * 3 + 45 * 6");
        var multiplication = new InfixOperator<object>
                (
                    Parser.Literal,
                    new[] { "*", "/" },
                    IntegerArithmetic,
                    BinaryOperatorType.LeftAssociative
                );
        var addition = new InfixOperator<object>
                (
                    multiplication,
                    new[] { "+", "-" },
                    IntegerArithmetic,
                    BinaryOperatorType.LeftAssociative
                );
        var result = (int) addition.ParseOrThrow (state);
        Assert.AreEqual (306, result);
    }

    [TestMethod]
    [Description ("Только круглые скобки")]
    public void InfixOperator_Parse_6()
    {
        var state = _GetState ("(12 + 34)");
        var multiplication = new InfixOperator<object>
            (
                Parser.Literal,
                new[] { "*", "/" },
                IntegerArithmetic,
                BinaryOperatorType.LeftAssociative
            );
        var addition = new InfixOperator<object>
            (
                multiplication,
                new[] { "+", "-" },
                IntegerArithmetic,
                BinaryOperatorType.LeftAssociative
            );
        var parenthesis = addition.RoundBrackets().End();
        var value = (int) parenthesis.ParseOrThrow (state);
        Assert.AreEqual (46, value);
    }

    [Ignore]
    [TestMethod]
    [Description ("Круглые скобки в составе выражения")]
    public void InfixOperator_Parse_7()
    {
        var state = _GetState ("(12 + 34) * 5", true);
        var literal = Parser.Literal;
        var expr = new DynamicParser<object> (() => null!);
        var multiplication = new InfixOperator<object>
            (
                expr,
                new[] { "*", "/" },
                IntegerArithmetic,
                BinaryOperatorType.LeftAssociative
            );
        var addition = new InfixOperator<object>
            (
                multiplication,
                new[] { "+", "-" },
                IntegerArithmetic,
                BinaryOperatorType.LeftAssociative
            );
        var parenthesis = addition.RoundBrackets();
        var math = literal.Or (parenthesis);
        expr.Inner = () => math.Trace();

        var value = (int) math.ParseOrThrow (state);
        Assert.AreEqual (230, value);
    }
}
