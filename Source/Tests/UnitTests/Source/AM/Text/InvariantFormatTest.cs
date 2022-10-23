// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Text;

#endregion

#nullable enable

namespace UnitTests.AM.Text;

[TestClass]
public sealed class InvariantFormatTest
{
    [TestMethod]
    [Description ("Расформатирование числа")]
    public void InvariantFormat_Format_1()
    {
        Assert.AreEqual
            (
                "10000",
                InvariantFormat.Format (10_000)
            );

        Assert.AreEqual
            (
                "10000.5",
                InvariantFormat.Format (10_000.5)
            );

        Assert.AreEqual
            (
                "10000.5",
                InvariantFormat.Format (10_000.5m)
            );
    }

    [TestMethod]
    [Description ("Форматная строка и единственный аргумент")]
    public void InvariantFormat_Format_2()
    {
        Assert.AreEqual
            (
                "Length=1000",
                InvariantFormat.Format ("Length={0}", 1_000)
            );

        Assert.AreEqual
            (
                "Length=1000.5",
                InvariantFormat.Format ("Length={0}", 1_000.5)
            );

        Assert.AreEqual
            (
                "Length=1000.5",
                InvariantFormat.Format
                    (
                        "Length={0}",
                        (object)1_000.5
                    )
            );
    }

    [TestMethod]
    [Description ("Форматная строка и два аргумента")]
    public void InvariantFormat_Format_3()
    {
        Assert.AreEqual
            (
                "Length=1000, Height=2000",
                InvariantFormat.Format
                    (
                        "Length={0}, Height={1}",
                        1_000,
                        2_000
                    )
            );

        Assert.AreEqual
            (
                "Length=1000.5, Height=2000.5",
                InvariantFormat.Format
                    (
                        "Length={0}, Height={1}",
                        1_000.5,
                        2_000.5
                    )
            );
    }

    [TestMethod]
    [Description ("Форматная строка и три аргумента")]
    public void InvariantFormat_Format_4()
    {
        Assert.AreEqual
            (
                "Length=1000, Height=2000, Width=3000",
                InvariantFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}",
                        1_000,
                        2_000,
                        3_000
                    )
            );

        Assert.AreEqual
            (
                "Length=1000.5, Height=2000.5, Width=3000.5",
                InvariantFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}",
                        1_000.5,
                        2_000.5,
                        3_000.5
                    )
            );
    }

    [TestMethod]
    [Description ("Форматная строка и четыре аргумента")]
    public void InvariantFormat_Format_5()
    {
        Assert.AreEqual
            (
                "Length=1000, Height=2000, Width=3000, Weight=4000",
                InvariantFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}, Weight={3}",
                        1_000,
                        2_000,
                        3_000,
                        4_000
                    )
            );

        Assert.AreEqual
            (
                "Length=1000.5, Height=2000.5, Width=3000.5, Weight=4000.5",
                InvariantFormat.Format
                    (
                        "Length={0}, Height={1}, Width={2}, Weight={3}",
                        1_000.5,
                        2_000.5,
                        3_000.5,
                        4_000.5
                    )
            );
    }

    [TestMethod]
    [Description ("Форматная строка и единственный аргумент денежного типа")]
    public void InvariantFormat_Format_6()
    {
        Assert.AreEqual
            (
                "Length=1000.5",
                InvariantFormat.Format ("Length={0}", 1_000.5m)
            );
    }
}
