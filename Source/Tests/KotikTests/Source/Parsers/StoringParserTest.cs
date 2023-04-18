// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Globalization;

using AM;
using AM.Kotik;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace KotikTests;

[TestClass]
public sealed class StoringParserTest
    : CommonParserTest
{
    [TestMethod]
    [Description ("Успешный простой разбор целого")]
    public void StoringParser_Parse_1()
    {
        var value = new int [1];
        var state = _GetState ("123");
        var parser = new StoringParser<int>(value);
        parser.ParseOrThrow (state);
        Assert.IsNotNull (value);
        Assert.AreEqual (123, value[0]);
    }

    [TestMethod]
    [Description ("Неуспешный простой разбор целого")]
    public void StoringParser_Parse_2()
    {
        var value = new int[1];
        var state = _GetState ("aga");
        var parser = new StoringParser<int>(value);
        var result = parser.Parse (state);
        Assert.IsFalse (result.IsSuccess);
    }

    [TestMethod]
    [Description ("Успешный разбор целого между двух скобок")]
    public void StoringParser_Parse_3()
    {
        var value = new int[1];
        var state = _GetState ("(123)");
        var parser = new StoringParser<int> (value).RoundBrackets ();
        parser.ParseOrThrow (state);
        Assert.AreEqual (123, value[0]);
    }

    [TestMethod]
    [Description ("Неуспешный разбор целого между двух скобок")]
    public void StoringParser_Parse_4()
    {
        var value = new int[1];
        var state = _GetState ("(ugu)");
        var parser = new StoringParser<int> (value).RoundBrackets ();
        var result = parser.Parse (state);
        Assert.IsFalse (result.IsSuccess);
    }

    [TestMethod]
    [Description ("Успешный простой разбор числа с плавающей точкой")]
    public void StoringParser_Parse_5()
    {
        var value = new [] { 0.0 };
        var state = _GetState ("123.45");
        var parser = new StoringParser<double> (value)
        {
            FormatProvider = CultureInfo.InvariantCulture
        };
        parser.ParseOrThrow (state);
        Assert.IsNotNull (value);
        Assert.AreEqual (123.45, value[0]);
    }

    [TestMethod]
    [Description ("Более сложный случай разбора")]
    public void StoringParser_Parse_6()
    {
        var invariantCulture = CultureInfo.InvariantCulture;
        var first = new [] { 0.0 };
        var second = new[] { 0.0 };
        var state = _GetState ("http: 1.2 + 3.4");
        var parser = Parser.Sequence
            (
                Parser.Identifier.Assert (x => x.IsOneOf ("http", "https")),
                Parser.Term (":"),
                new StoringParser<double> (first) { FormatProvider = invariantCulture },
                Parser.Term ("+", "-"),
                new StoringParser<double> (second) { FormatProvider = invariantCulture }
            );
        parser.ParseOrThrow (state);
        Assert.AreEqual (1.2, first[0]);
        Assert.AreEqual (3.4, second[0]);
    }
}
