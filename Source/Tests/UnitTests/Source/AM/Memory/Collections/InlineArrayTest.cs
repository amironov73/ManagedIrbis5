// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using AM.Memory.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

namespace UnitTests.AM.Memory.Collections.Specialized;

[TestClass]
public sealed class InlineArrayTest
{
    [TestMethod]
    [Description ("Массив из двух элементов")]
    public void InlineArray2_Test()
    {
        InlineArray2<string> array = default;
        array[0] = "Hello";
        array[1] = "World";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
    }

    [TestMethod]
    [Description ("Массив из трех элементов")]
    public void InlineArray3_Test()
    {
        InlineArray3<string> array = default;
        array[0] = "Hello";
        array[1] = "World";
        array[2] = "Again";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");
        Assert.AreEqual (array[2], "Again");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
        Assert.AreEqual (span[2], "Again");
    }

    [TestMethod]
    [Description ("Массив из четырех элементов")]
    public void InlineArray4_Test()
    {
        InlineArray4<string> array = default;
        array[0] = "Hello";
        array[1] = "World";
        array[2] = "Alpha";
        array[3] = "Beta";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");
        Assert.AreEqual (array[2], "Alpha");
        Assert.AreEqual (array[3], "Beta");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
        Assert.AreEqual (span[2], "Alpha");
        Assert.AreEqual (span[3], "Beta");
    }

    [TestMethod]
    [Description ("Массив из пяти элементов")]
    public void InlineArray5_Test()
    {
        InlineArray5<string> array = default;
        array[0] = "Hello";
        array[1] = "World";
        array[2] = "Alpha";
        array[3] = "Beta";
        array[4] = "Gamma";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");
        Assert.AreEqual (array[2], "Alpha");
        Assert.AreEqual (array[3], "Beta");
        Assert.AreEqual (array[4], "Gamma");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
        Assert.AreEqual (span[2], "Alpha");
        Assert.AreEqual (span[3], "Beta");
        Assert.AreEqual (span[4], "Gamma");
    }

    [TestMethod]
    [Description ("Массив из шести элементов")]
    public void InlineArray6_Test()
    {
        InlineArray6<string> array = default;
        array[0] = "Hello";
        array[1] = "World";
        array[2] = "Alpha";
        array[3] = "Beta";
        array[4] = "Gamma";
        array[5] = "Delta";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");
        Assert.AreEqual (array[2], "Alpha");
        Assert.AreEqual (array[3], "Beta");
        Assert.AreEqual (array[4], "Gamma");
        Assert.AreEqual (array[5], "Delta");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
        Assert.AreEqual (span[2], "Alpha");
        Assert.AreEqual (span[3], "Beta");
        Assert.AreEqual (span[4], "Gamma");
        Assert.AreEqual (span[5], "Delta");
    }

    [TestMethod]
    [Description ("Массив из семи элементов")]
    public void InlineArray7_Test()
    {
        InlineArray7<string> array = default;
        array[0] = "Hello";
        array[1] = "World";
        array[2] = "Alpha";
        array[3] = "Beta";
        array[4] = "Gamma";
        array[5] = "Delta";
        array[6] = "Epsilon";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");
        Assert.AreEqual (array[2], "Alpha");
        Assert.AreEqual (array[3], "Beta");
        Assert.AreEqual (array[4], "Gamma");
        Assert.AreEqual (array[5], "Delta");
        Assert.AreEqual (array[6], "Epsilon");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
        Assert.AreEqual (span[2], "Alpha");
        Assert.AreEqual (span[3], "Beta");
        Assert.AreEqual (span[4], "Gamma");
        Assert.AreEqual (span[5], "Delta");
        Assert.AreEqual (span[6], "Epsilon");
    }

    [TestMethod]
    [Description ("Массив из восьми элементов")]
    public void InlineArray8_Test()
    {
        InlineArray8<string> array = default;
        array[0] = "Hello";
        array[1] = "World";
        array[2] = "Alpha";
        array[3] = "Beta";
        array[4] = "Gamma";
        array[5] = "Delta";
        array[6] = "Epsilon";
        array[7] = "Zeta";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");
        Assert.AreEqual (array[2], "Alpha");
        Assert.AreEqual (array[3], "Beta");
        Assert.AreEqual (array[4], "Gamma");
        Assert.AreEqual (array[5], "Delta");
        Assert.AreEqual (array[6], "Epsilon");
        Assert.AreEqual (array[7], "Zeta");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
        Assert.AreEqual (span[2], "Alpha");
        Assert.AreEqual (span[3], "Beta");
        Assert.AreEqual (span[4], "Gamma");
        Assert.AreEqual (span[5], "Delta");
        Assert.AreEqual (span[6], "Epsilon");
        Assert.AreEqual (span[7], "Zeta");
    }

    [TestMethod]
    [Description ("Массив из девяти элементов")]
    public void InlineArray9_Test()
    {
        InlineArray9<string> array = default;
        array[0] = "Hello";
        array[1] = "World";
        array[2] = "Alpha";
        array[3] = "Beta";
        array[4] = "Gamma";
        array[5] = "Delta";
        array[6] = "Epsilon";
        array[7] = "Zeta";
        array[8] = "Eta";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");
        Assert.AreEqual (array[2], "Alpha");
        Assert.AreEqual (array[3], "Beta");
        Assert.AreEqual (array[4], "Gamma");
        Assert.AreEqual (array[5], "Delta");
        Assert.AreEqual (array[6], "Epsilon");
        Assert.AreEqual (array[7], "Zeta");
        Assert.AreEqual (array[8], "Eta");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
        Assert.AreEqual (span[2], "Alpha");
        Assert.AreEqual (span[3], "Beta");
        Assert.AreEqual (span[4], "Gamma");
        Assert.AreEqual (span[5], "Delta");
        Assert.AreEqual (span[6], "Epsilon");
        Assert.AreEqual (span[7], "Zeta");
        Assert.AreEqual (span[8], "Eta");
    }

    [TestMethod]
    [Description ("Массив из десяти элементов")]
    public void InlineArray10_Test()
    {
        InlineArray10<string> array = default;
        array[0] = "Hello";
        array[1] = "World";
        array[2] = "Alpha";
        array[3] = "Beta";
        array[4] = "Gamma";
        array[5] = "Delta";
        array[6] = "Epsilon";
        array[7] = "Zeta";
        array[8] = "Eta";
        array[9] = "Theta";

        Assert.AreEqual (array[0], "Hello");
        Assert.AreEqual (array[1], "World");
        Assert.AreEqual (array[2], "Alpha");
        Assert.AreEqual (array[3], "Beta");
        Assert.AreEqual (array[4], "Gamma");
        Assert.AreEqual (array[5], "Delta");
        Assert.AreEqual (array[6], "Epsilon");
        Assert.AreEqual (array[7], "Zeta");
        Assert.AreEqual (array[8], "Eta");
        Assert.AreEqual (array[9], "Theta");

        var span = array.AsSpan();
        Assert.AreEqual (span[0], "Hello");
        Assert.AreEqual (span[1], "World");
        Assert.AreEqual (span[2], "Alpha");
        Assert.AreEqual (span[3], "Beta");
        Assert.AreEqual (span[4], "Gamma");
        Assert.AreEqual (span[5], "Delta");
        Assert.AreEqual (span[6], "Epsilon");
        Assert.AreEqual (span[7], "Zeta");
        Assert.AreEqual (span[8], "Eta");
        Assert.AreEqual (span[9], "Theta");
    }
}
