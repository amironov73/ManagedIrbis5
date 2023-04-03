// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.Kotik;
using AM.Kotik.Barsik;
using AM.Kotik.Barsik.Ast;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class ExpressionTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Единичный литерал")]
    public void Expression_Parse_Literal_1()
    {
        var state = _GetState ("1");
        var grammar = Grammar.CreateDefaultBarsikGrammar();
        grammar.Rebuild();
        var parser = grammar.Expression.End();
        var value = (ConstantNode) parser.ParseOrThrow (state);
        Assert.AreEqual (1, value.Value);
    }

    [TestMethod]
    [Description ("Единичная идентификатор")]
    public void Expression_Parse_Variable_1()
    {
        var state = _GetState ("x");
        var grammar = Grammar.CreateDefaultBarsikGrammar();
        grammar.Rebuild();
        var parser = grammar.Expression.End();
        var value = (VariableNode) parser.ParseOrThrow (state);
        Assert.AreEqual ("x", value.Name);
    }

    [TestMethod]
    [Description ("Инфиксная операция, два литерала")]
    public void Expression_Parse_Infix_Literal_1()
    {
        var state = _GetState ("1 + 2");
        var grammar = Grammar.CreateDefaultBarsikGrammar();
        grammar.Rebuild();
        var parser = grammar.Expression.End();
        var value = (BinaryNode) parser.ParseOrThrow (state);
        Assert.AreEqual (1, ((ConstantNode) value.left).Value);
        Assert.AreEqual ("+", value.operation);
        Assert.AreEqual (2, ((ConstantNode) value.right).Value);
    }

    [TestMethod]
    [Description ("Инфиксная операция, два идентификатора")]
    public void Expression_Parse_Infix_Variable_1()
    {
        var state = _GetState ("x + y");
        var grammar = Grammar.CreateDefaultBarsikGrammar();
        grammar.Rebuild();
        var parser = grammar.Expression.End();
        var value = (BinaryNode) parser.ParseOrThrow (state);
        Assert.AreEqual ("x", ((VariableNode) value.left).Name);
        Assert.AreEqual ("+", value.operation);
        Assert.AreEqual ("y", ((VariableNode) value.right).Name);
    }

    [TestMethod]
    [Description ("Круглые скобки, внутри литерал")]
    public void Expression_Parse_Brackets_Literal_1()
    {
        var state = _GetState ("(1)");
        var grammar = Grammar.CreateDefaultBarsikGrammar();
        grammar.Rebuild();
        var parser = grammar.Expression.End();
        var value = (ConstantNode) parser.ParseOrThrow (state);
        Assert.AreEqual (1, value.Value);
    }

    [Ignore]
    [TestMethod]
    [Description ("Круглые скобки, внутри идентификатор")]
    public void Expression_Parse_Brackets_Identifier_1()
    {
        // BUG матчится cast вместо переменной
        var state = _GetState ("(x)");
        // state.DebugOutput = System.Console.Out;
        var grammar = Grammar.CreateDefaultBarsikGrammar();
        grammar.Rebuild();
        var parser = grammar.Expression.End();
        var value = (VariableNode) parser.ParseOrThrow (state);
        Assert.AreEqual ("x", value.Name);
    }


}
