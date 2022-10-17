// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;
using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Linq;

#endregion

#nullable enable

namespace UnitTests.AM.Linq;

[TestClass]
public sealed class SequenceTest
{
    [TestMethod]
    [Description ("Выдать первый элемент либо значение по умолчанию: есть элемент")]
    public void Sequence_FirstOr_1()
    {
        var sequence = new[] { 1, 2, 3 };
        var actual = Sequence.FirstOr (sequence, 0);
        const int expected = 1;
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Выдать первый элемент либо значение по умолчанию: нет элемента")]
    public void Sequence_FirstOr_2()
    {
        var sequence = Array.Empty<int>();
        var actual = Sequence.FirstOr (sequence, 0);
        const int expected = 0;
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Выдать первый элемент либо значение по умолчанию: предикат")]
    public void Sequence_FirstOr_3()
    {
        var sequence = Array.Empty<int>();
        var actual = Sequence.FirstOr (sequence, () => 123);
        const int expected = 123;
        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    [Description ("Формирование последовательности из одного элемента")]
    public void Sequence_FromItem_1()
    {
        var sequence = Sequence.FromItem (1);
        var array = sequence.ToArray();
        Assert.AreEqual (1, array.Length);
        Assert.AreEqual (1, array[0]);
    }

    [TestMethod]
    [Description ("Формирование последовательности из двух элементов")]
    public void Sequence_FromItems_1()
    {
        var sequence = Sequence.FromItems (1, 2);
        var array = sequence.ToArray();
        Assert.AreEqual (2, array.Length);
        Assert.AreEqual (1, array[0]);
        Assert.AreEqual (2, array[1]);
    }

    [TestMethod]
    [Description ("Формирование последовательности из трех элементов")]
    public void Sequence_FromItems_2()
    {
        var sequence = Sequence.FromItems (1, 2, 3);
        var array = sequence.ToArray();
        Assert.AreEqual (3, array.Length);
        Assert.AreEqual (1, array[0]);
        Assert.AreEqual (2, array[1]);
        Assert.AreEqual (3, array[2]);
    }
}
