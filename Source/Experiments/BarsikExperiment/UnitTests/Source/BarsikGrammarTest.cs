// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.Linq;

using Pidgin;

namespace BarsikTests;

[TestClass]
public sealed class BarsikGrammarTest
{
    private static T[] ParseMany<T>
        (
            string text,
            Parser<BarsikToken, T> parser
        )
    {
        var tokens = BarsikTokenizer.Tokenize (text);
        var result = parser.Many().Before (Parser<BarsikToken>.End)
            .ParseReadOnlyList (tokens);

        return result.Value.ToArray();
    }

    private void CheckConstantValue
        (
            AtomNode node,
            object value
        )
    {
        Assert.IsNotNull (node);
        Assert.AreEqual (typeof (ConstantNode), node.GetType());

        var constant = (ConstantNode) node;
        Assert.AreEqual (value, constant.Value);
    }

    [TestMethod]
    [Description ("Отдельные символы")]
    public void Grammar_Literal_1()
    {
        var parsed = ParseMany ("'a' 'b' 'c'", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], 'a');
        CheckConstantValue (parsed[1], 'b');
        CheckConstantValue (parsed[2], 'c');
    }

    [TestMethod]
    [Description ("Строки")]
    public void Grammar_Literal_2()
    {
        var parsed = ParseMany ("\"one\" \"two\" \"three\"", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], "one");
        CheckConstantValue (parsed[1], "two");
        CheckConstantValue (parsed[2], "three");
    }

    [TestMethod]
    [Description ("Целые 32-битные числа")]
    public void Grammar_Literal_3()
    {
        var parsed = ParseMany ("1 12 123", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], 1);
        CheckConstantValue (parsed[1], 12);
        CheckConstantValue (parsed[2], 123);
    }

    [TestMethod]
    [Description ("Целые 64-битные числа")]
    public void Grammar_Literal_4()
    {
        var parsed = ParseMany ("1L 12L 123L", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], 1L);
        CheckConstantValue (parsed[1], 12L);
        CheckConstantValue (parsed[2], 123L);
    }

    [TestMethod]
    [Description ("Целые 32-битные числа без знака")]
    public void Grammar_Literal_5()
    {
        var parsed = ParseMany ("1u 12u 123u", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], 1u);
        CheckConstantValue (parsed[1], 12u);
        CheckConstantValue (parsed[2], 123u);
    }

    [TestMethod]
    [Description ("Целые 64-битные числа без знака")]
    public void Grammar_Literal_6()
    {
        var parsed = ParseMany ("1ul 12ul 123ul", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], 1ul);
        CheckConstantValue (parsed[1], 12ul);
        CheckConstantValue (parsed[2], 123ul);
    }

    [TestMethod]
    [Description ("Числа с плавающей точкой одинарной точности")]
    public void Grammar_Literal_7()
    {
        var parsed = ParseMany ("1f 1.2f 1.23f", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], 1f);
        CheckConstantValue (parsed[1], 1.2f);
        CheckConstantValue (parsed[2], 1.23f);
    }

    [TestMethod]
    [Description ("Числа с плавающей точкой двойной точности")]
    public void Grammar_Literal_8()
    {
        var parsed = ParseMany ("1.0 1.2 1.23", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], 1.0);
        CheckConstantValue (parsed[1], 1.2);
        CheckConstantValue (parsed[2], 1.23);
    }

    [TestMethod]
    [Description ("Числа с фиксированной точкой")]
    public void Grammar_Literal_9()
    {
        var parsed = ParseMany ("1m 1.2m 1.23m", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], 1m);
        CheckConstantValue (parsed[1], 1.2m);
        CheckConstantValue (parsed[2], 1.23m);
    }

    [TestMethod]
    [Description ("Булевы значения")]
    public void Grammar_Literal_10()
    {
        var parsed = ParseMany ("true false true", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        CheckConstantValue (parsed[0], true);
        CheckConstantValue (parsed[1], false);
        CheckConstantValue (parsed[2], true);
    }

    [TestMethod]
    [Description ("null-литерал")]
    public void Grammar_Literal_11()
    {
        var parsed = ParseMany ("null null null", Grammar.Literal);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        Assert.IsNull (parsed[0].Value);
        Assert.IsNull (parsed[1].Value);
        Assert.IsNull (parsed[2].Value);
    }

    [TestMethod]
    [Description ("Идентификаторы")]
    public void Grammar_Identifier_1()
    {
        var parsed = ParseMany (" hello \t\nworld ", Grammar.Identifier);
        Assert.IsNotNull (parsed);
        Assert.AreEqual (2, parsed.Length);

        Assert.AreEqual ("hello", parsed[0]);
        Assert.AreEqual ("world", parsed[1]);
    }

    [TestMethod]
    [Description ("Термы")]
    public void Grammar_Term_1()
    {
        var parsed = ParseMany (" + ++ = +=", Grammar.Term ("+", "++", "=", "+=", "=="));
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        Assert.AreEqual ("++", parsed[0]);
        Assert.AreEqual ("+=", parsed[1]);
        Assert.AreEqual ("+=", parsed[2]);

        Assert.ThrowsException<InvalidOperationException>
            (
                () => ParseMany ("-", Grammar.Term ("+"))
            );
    }

    [TestMethod]
    [Description ("Зарезервированные слова")]
    public void Grammar_Keyword_1()
    {
        var parsed = ParseMany (" if this else ", Grammar.Keyword ("if", "this", "else"));
        Assert.IsNotNull (parsed);
        Assert.AreEqual (3, parsed.Length);

        Assert.AreEqual ("if", parsed[0]);
        Assert.AreEqual ("this", parsed[1]);
        Assert.AreEqual ("else", parsed[2]);
    }

}
