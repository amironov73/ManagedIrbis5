// ReSharper disable CheckNamespace
// ReSharper disable InconsistentNaming
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable StringLiteralTypo

using System;
using System.Net;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Net;

#nullable enable

namespace UnitTests.AM.Net;

[TestClass]
public sealed class IPRangeTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void IPRange_Construction_1()
    {
        var begin = IPAddress.Parse ("127.0.0.1");
        var end = IPAddress.Parse ("127.0.0.255");
        var range = new IPRange (begin, end);
        Assert.AreEqual (begin, range.Begin);
        Assert.AreEqual (end, range.End);
    }

    [TestMethod]
    [Description ("Проверка, содержит ли диапазон указанный адрес")]
    public void IPRange_Contains_1()
    {
        var begin = IPAddress.Parse ("127.0.0.1");
        var end = IPAddress.Parse ("127.0.0.255");
        var range = new IPRange (begin, end);

        var address = IPAddress.Parse ("127.0.0.1");
        Assert.IsTrue (range.Contains (address));

        address = IPAddress.Parse ("127.0.0.2");
        Assert.IsTrue (range.Contains (address));

        address = IPAddress.Parse ("127.0.0.254");
        Assert.IsTrue (range.Contains (address));

        address = IPAddress.Parse ("127.0.0.255");
        Assert.IsTrue (range.Contains (address));

        address = IPAddress.Parse ("128.0.0.1");
        Assert.IsFalse (range.Contains (address));
    }

    [TestMethod]
    [Description ("Проверка, содержит ли диапазон указанный адрес")]
    public void IPRange_Contains_2()
    {
        var begin = IPAddress.Parse ("127.0.0.100");
        var end = IPAddress.Parse ("127.0.0.110");
        var range = new IPRange (begin, end);

        var address = IPAddress.Parse ("127.0.0.1");
        Assert.IsFalse (range.Contains (address));

        address = IPAddress.Parse ("127.0.0.100");
        Assert.IsTrue (range.Contains (address));

        address = IPAddress.Parse ("127.0.0.109");
        Assert.IsTrue (range.Contains (address));

        address = IPAddress.Parse ("127.0.0.110");
        Assert.IsTrue (range.Contains (address));

        address = IPAddress.Parse ("128.0.0.200");
        Assert.IsFalse (range.Contains (address));
    }

    [TestMethod]
    [Description ("Проверка, содержит ли диапазон указанный поддиапазон")]
    public void IPRange_Contains_3()
    {
        var begin = IPAddress.Parse ("127.0.0.1");
        var end = IPAddress.Parse ("127.0.0.255");
        var first = new IPRange (begin, end);

        begin = IPAddress.Parse ("127.0.0.100");
        end = IPAddress.Parse ("127.0.0.110");
        var second = new IPRange (begin, end);

        Assert.IsTrue (first.Contains (second));
        Assert.IsTrue (first.Contains (first));
        Assert.IsTrue (second.Contains (second));
        Assert.IsFalse (second.Contains (first));
    }

    [TestMethod]
    [Description ("Проверка, содержит ли диапазон указанный поддиапазон")]
    public void IPRange_Contains_4()
    {
        var begin = IPAddress.Parse ("127.0.0.100");
        var end = IPAddress.Parse ("127.0.0.110");
        var first = new IPRange (begin, end);

        begin = IPAddress.Parse ("127.0.0.1");
        end = IPAddress.Parse ("127.0.0.100");
        var second = new IPRange (begin, end);

        Assert.IsFalse (first.Contains (second));
    }

    [TestMethod]
    [Description ("Проверка, содержит ли диапазон указанный поддиапазон")]
    public void IPRange_Contains_5()
    {
        var begin = IPAddress.Parse ("127.0.0.100");
        var end = IPAddress.Parse ("127.0.0.110");
        var first = new IPRange (begin, end);

        begin = IPAddress.Parse ("127.0.0.1");
        end = IPAddress.Parse ("127.0.0.99");
        var second = new IPRange (begin, end);

        Assert.IsFalse (first.Contains (second));
    }

    [TestMethod]
    [Description ("Разбор текстовой спецификации диапазона")]
    public void IPRange_Parse_1()
    {
        var range = IPRange.Parse ("*");
        Assert.AreEqual (IPAddress.Any, range.Begin);
        Assert.AreEqual (IPAddress.Broadcast, range.End);

        range = IPRange.Parse (" * ");
        Assert.AreEqual (IPAddress.Any, range.Begin);
        Assert.AreEqual (IPAddress.Broadcast, range.End);
    }

    [TestMethod]
    [Description ("Разбор текстовой спецификации диапазона")]
    public void IPRange_Parse_2()
    {
        var range = IPRange.Parse ("127.0.0.1/24");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);

        range = IPRange.Parse (" 127.0.0.1/24 ");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);

        range = IPRange.Parse ("127.0.0.0/24");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);

        range = IPRange.Parse ("127.0.0.100/24");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);

        range = IPRange.Parse ("127.0.0.255/24");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);

        range = IPRange.Parse ("127.0.0.1/8");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.255.255.255"), range.End);
    }

    [TestMethod]
    [Description ("Разбор текстовой спецификации диапазона")]
    public void IPRange_Parse_3()
    {
        var range = IPRange.Parse ("127.0.0.1/32");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.End);

        range = IPRange.Parse ("127.0.0.1/31");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.End);

        range = IPRange.Parse ("127.0.0.1/30");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.3"), range.End);
    }

    [TestMethod]
    [Description ("Разбор текстовой спецификации диапазона")]
    public void IPRange_Parse_4()
    {
        Assert.ThrowsException<IndexOutOfRangeException>
            (
                () => IPRange.Parse ("127.0.0.1/33")
            );
    }

    [TestMethod]
    [Description ("Разбор текстовой спецификации диапазона")]
    public void IPRange_Parse_5()
    {
        var range = IPRange.Parse ("127.0.0.1");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.End);

        range = IPRange.Parse ("127.0.0.0");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.End);

        range = IPRange.Parse ("127.0.0.255");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);

        range = IPRange.Parse ("2130706433");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.End);

        range = IPRange.Parse ("2130706432");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.End);

        range = IPRange.Parse ("2130706687");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);
    }

    [TestMethod]
    [Description ("Разбор текстовой спецификации диапазона")]
    public void IPRange_Parse_6()
    {
        Assert.ThrowsException<FormatException>
            (
                () => IPRange.Parse ("127.0.0.266")
            );
    }

    [TestMethod]
    [Description ("Разбор текстовой спецификации диапазона")]
    public void IPRange_Parse_7()
    {
        var range = IPRange.Parse ("127.0.0.1-127.0.0.255");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);

        range = IPRange.Parse (" 127.0.0.1 - 127.0.0.255 ");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.1"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);

        range = IPRange.Parse ("127.0.0.0-127.0.0.255");
        Assert.AreEqual (IPAddress.Parse ("127.0.0.0"), range.Begin);
        Assert.AreEqual (IPAddress.Parse ("127.0.0.255"), range.End);
    }

    [TestMethod]
    [Description ("Разбор текстовой спецификации диапазона")]
    public void IPRange_Parse_8()
    {
        Assert.ThrowsException<ArgumentOutOfRangeException>
            (
                () => IPRange.Parse ("Hello")
            );

        Assert.ThrowsException<ArgumentNullException>
            (
                () => IPRange.Parse (null!)
            );

        Assert.ThrowsException<ArgumentNullException>
            (
                () => IPRange.Parse (string.Empty)
            );

        Assert.ThrowsException<ArgumentOutOfRangeException>
            (
                () => IPRange.Parse ("1-")
            );
    }

    [TestMethod]
    [Description ("Проверка на равенство")]
    public void IPRange_Equals_1()
    {
        var first = IPRange.Parse ("1.1.1.1-2.2.2.2");
        var second = IPRange.Parse ("1.1.1.1-2.2.2.2");
        Assert.IsTrue (first.Equals (second));

        second = IPRange.Parse ("1.1.1.1");
        Assert.IsFalse (first.Equals (second));
    }

    [TestMethod]
    [Description ("Текстовое представление диапазона адресов")]
    public void IPRange_ToString_1()
    {
        var range = IPRange.Parse ("1.1.1.1-2.2.2.2");
        Assert.AreEqual ("1.1.1.1-2.2.2.2", range.ToString());

        range = IPRange.Parse ("1.1.1.1");
        Assert.AreEqual ("1.1.1.1", range.ToString());
    }
}
