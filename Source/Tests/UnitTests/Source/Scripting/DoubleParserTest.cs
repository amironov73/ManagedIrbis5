// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Scripting;

using Pidgin;
using static Pidgin.Parser<char>;

#nullable enable

namespace UnitTests.Scripting;

[TestClass]
public sealed class DoubleParserTest
{
    [TestMethod]
    [Description ("Число с дробной частью, но без экспоненты")]
    public void DoubleParser_Parse_1()
    {
        var parser = Resolve.Double.Before (End);
        var actual = parser.ParseOrThrow ("123.4");
        Assert.AreEqual (123.4, actual);

        actual = parser.ParseOrThrow ("+123.4");
        Assert.AreEqual (123.4, actual);

        actual = parser.ParseOrThrow ("-123.4");
        Assert.AreEqual (-123.4, actual);
    }

    [TestMethod]
    [Description ("Число с экспонентой, но без дробной части")]
    public void DoubleParser_Parse_2()
    {
        var parser = Resolve.Double.Before (End);
        var actual = parser.ParseOrThrow ("123e2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("123E2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("123e+2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("123E+2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("+123e+2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("+123E+2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("123e-2");
        Assert.AreEqual (1.23, actual);

        actual = parser.ParseOrThrow ("123E-2");
        Assert.AreEqual (1.23, actual);

        actual = parser.ParseOrThrow ("-123e-2");
        Assert.AreEqual (-1.23, actual);

        actual = parser.ParseOrThrow ("-123E-2");
        Assert.AreEqual (-1.23, actual);
    }

    [TestMethod]
    [Description ("Число с дробной частью и экспонентой одновременно")]
    public void DoubleParser_Parse_3()
    {
        var parser = Resolve.Double.Before (End);
        var actual = parser.ParseOrThrow ("1.23e2");
        Assert.AreEqual (123.0, actual);

        actual = parser.ParseOrThrow ("1.23E2");
        Assert.AreEqual (123.0, actual);

        actual = parser.ParseOrThrow ("1.23e+2");
        Assert.AreEqual (123.0, actual);

        actual = parser.ParseOrThrow ("1.23E+2");
        Assert.AreEqual (123.0, actual);

        actual = parser.ParseOrThrow ("1.23e-2");
        Assert.AreEqual (0.0123, actual);

        actual = parser.ParseOrThrow ("1.23E-2");
        Assert.AreEqual (0.0123, actual);

        actual = parser.ParseOrThrow ("-1.23e2");
        Assert.AreEqual (-123.0, actual);

        actual = parser.ParseOrThrow ("-1.23E2");
        Assert.AreEqual (-123.0, actual);

        actual = parser.ParseOrThrow ("-1.23e+2");
        Assert.AreEqual (-123.0, actual);

        actual = parser.ParseOrThrow ("-1.23E+2");
        Assert.AreEqual (-123.0, actual);

        actual = parser.ParseOrThrow ("-1.23e-2");
        Assert.AreEqual (-0.0123, actual);

        actual = parser.ParseOrThrow ("-1.23E-2");
        Assert.AreEqual (-0.0123, actual);
    }

    [TestMethod]
    [Description ("Число с пустой целой частью")]
    public void DoubleParser_Parse_4()
    {
        var parser = Resolve.Double.Before (End);
        var actual = parser.ParseOrThrow (".123");
        Assert.AreEqual (0.123, actual);

        actual = parser.ParseOrThrow ("+.123");
        Assert.AreEqual (0.123, actual);

        actual = parser.ParseOrThrow ("-.123");
        Assert.AreEqual (-0.123, actual);

        actual = parser.ParseOrThrow (".123e2");
        Assert.AreEqual (12.3, actual);

        actual = parser.ParseOrThrow (".123E2");
        Assert.AreEqual (12.3, actual);

        actual = parser.ParseOrThrow ("-.123e2");
        Assert.AreEqual (-12.3, actual);

        actual = parser.ParseOrThrow ("-.123E2");
        Assert.AreEqual (-12.3, actual);

        actual = parser.ParseOrThrow (".123e-2");
        Assert.AreEqual (0.00123, actual);

        actual = parser.ParseOrThrow (".123E-2");
        Assert.AreEqual (0.00123, actual);

        actual = parser.ParseOrThrow ("-.123e-2");
        Assert.AreEqual (-0.00123, actual);

        actual = parser.ParseOrThrow ("-.123E-2");
        Assert.AreEqual (-0.00123, actual);
    }

    [TestMethod]
    [Description ("Число с пустой дробной частью")]
    public void DoubleParser_Parse_5()
    {
        var parser = Resolve.Double.Before (End);
        var actual = parser.ParseOrThrow ("123.");
        Assert.AreEqual (123.0, actual);

        actual = parser.ParseOrThrow ("+123.");
        Assert.AreEqual (123.0, actual);

        actual = parser.ParseOrThrow ("-123.");
        Assert.AreEqual (-123.0, actual);

        actual = parser.ParseOrThrow ("123.e2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("123.E2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("+123.e+2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("+123.E+2");
        Assert.AreEqual (12300.0, actual);

        actual = parser.ParseOrThrow ("+123.e-2");
        Assert.AreEqual (1.23, actual);

        actual = parser.ParseOrThrow ("+123.E-2");
        Assert.AreEqual (1.23, actual);

        actual = parser.ParseOrThrow ("-123.");
        Assert.AreEqual (-123.0, actual);

        actual = parser.ParseOrThrow ("-123.e2");
        Assert.AreEqual (-12300.0, actual);

        actual = parser.ParseOrThrow ("-123.E2");
        Assert.AreEqual (-12300.0, actual);

        actual = parser.ParseOrThrow ("-123.e+2");
        Assert.AreEqual (-12300.0, actual);

        actual = parser.ParseOrThrow ("-123.E+2");
        Assert.AreEqual (-12300.0, actual);

        actual = parser.ParseOrThrow ("-123.e-2");
        Assert.AreEqual (-1.2300, actual);

        actual = parser.ParseOrThrow ("-123.E-2");
        Assert.AreEqual (-1.23, actual);
    }

    [TestMethod]
    [Description ("Просто целое число")]
    public void DoubleParser_Parse_6()
    {
        var parser = Resolve.Double.Before (End);
        var actual = parser.Parse ("123");
        Assert.IsFalse (actual.Success);

        actual = parser.Parse ("-123");
        Assert.IsFalse (actual.Success);
    }

    [TestMethod]
    [Description ("Пустая строка")]
    public void DoubleParser_Parse_7()
    {
        var parser = Resolve.Double.Before (End);
        var actual = parser.Parse (string.Empty);
        Assert.IsFalse (actual.Success);
    }

    [TestMethod]
    [Description ("Мусор во входных данных")]
    public void DoubleParser_Parse_8()
    {
        var parser = Resolve.Double.Before (End);
        var actual = parser.Parse ("hello");
        Assert.IsFalse (actual.Success);

        actual = parser.Parse (" 123");
        Assert.IsFalse (actual.Success);

        actual = parser.Parse ("123 ");
        Assert.IsFalse (actual.Success);

        actual = parser.Parse ("123,4");
        Assert.IsFalse (actual.Success);

        actual = parser.Parse ("++123.4");
        Assert.IsFalse (actual.Success);

        actual = parser.Parse ("--123.4");
        Assert.IsFalse (actual.Success);
    }
}
