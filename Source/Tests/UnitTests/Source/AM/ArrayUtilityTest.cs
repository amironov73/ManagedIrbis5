// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;
using AM.Collections;

namespace UnitTests.AM;

#pragma warning disable CA1861

[TestClass]
public sealed class ArrayUtilityTest
{
    private sealed class MyClass
        : ICloneable
    {
        public int Value { get; set; }

        public object Clone()
        {
            return MemberwiseClone();
        }
    }

    [TestMethod]
    public void ArrayUtility_ChangeType_1()
    {
        string[] source = { "1", "2", "3" };
        var target = ArrayUtility.ChangeType<string, object> (source);
        Assert.AreEqual (source.Length, target.Length);
        for (var i = 0; i < source.Length; i++)
        {
            Assert.IsTrue
                (
                    ReferenceEquals (source[i], target[i])
                );
        }
    }

    [TestMethod]
    public void ArrayUtility_ChangeType_2()
    {
        string[] source = { "1", "2", "3" };
        var target = ArrayUtility.ChangeType<object> (source);
        Assert.AreEqual (source.Length, target.Length);
        for (var i = 0; i < source.Length; i++)
        {
            Assert.IsTrue
                (
                    ReferenceEquals (source[i], target[i])
                );
        }
    }

    [TestMethod]
    public void ArrayUtility_Compare_1()
    {
        int[] first = { 1, 2, 3 };
        int[] second = { 1, 3, 4 };
        Assert.IsTrue (ArrayUtility.Compare (first, second) < 0);

        first = Array.Empty<int>();
        second = Array.Empty<int>();
        Assert.IsTrue (ArrayUtility.Compare (first, second) == 0);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentException))]
    public void ArrayUtility_Compare_2()
    {
        int[] first = { };
        int[] second = { 1, 3, 4 };
        Assert.IsTrue (ArrayUtility.Compare (first, second) < 0);
    }

    [TestMethod]
    public void ArrayUtility_Convert_1()
    {
        int[] source = { 1, 2, 3 };
        var target = ArrayUtility.Convert<int, short> (source);
        Assert.AreEqual (source.Length, target.Length);
        for (var i = 0; i < source.Length; i++)
        {
            Assert.IsTrue
                (
                    (short)source[i] == target[i]
                );
        }
    }

    [TestMethod]
    public void ArrayUtility_Create_1()
    {
        var array = ArrayUtility.Create (10, 235);
        Assert.AreEqual (10, array.Length);
        for (var i = 0; i < array.Length; i++)
        {
            Assert.AreEqual (235, array[i]);
        }
    }

    [TestMethod]
    public void ArrayUtility_GetOccurrence_1()
    {
        int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        Assert.AreEqual (1, array.GetOccurrence (0));
        Assert.AreEqual (1, array.GetOccurrence (0, 0));
        Assert.AreEqual (0, array.GetOccurrence (10));
        Assert.AreEqual (1, array.GetOccurrence (10, 1));
    }

    [TestMethod]
    public void ArrayUtility_GetSlice_1()
    {
        int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var actual = array.GetSlice (3, 4);
        int[] expected = { 4, 5, 6, 7 };
        CollectionAssert.AreEqual (expected, actual);

        actual = array.GetSlice (9, 5);
        expected = new[] { 10 };
        CollectionAssert.AreEqual (expected, actual);

        actual = array.GetSlice (3, 0);
        expected = Array.Empty<int>();
        CollectionAssert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void ArrayUtility_GetSlice_2()
    {
        int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var actual = array.GetSlice (3);
        int[] expected = { 4, 5, 6, 7, 8, 9, 10 };
        CollectionAssert.AreEqual (expected, actual);

        actual = array.GetSlice (9);
        expected = new[] { 10 };
        CollectionAssert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void ArrayUtility_GetSlice_3()
    {
        int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var actual = array.GetSlice (300);
        var expected = Array.Empty<int>();
        CollectionAssert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void ArrayUtility_GetSlice_4()
    {
        int[] array = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        var actual = array.GetSlice (300, 10);
        var expected = Array.Empty<int>();
        CollectionAssert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void ArrayUtility_IsNullOrEmpty_1()
    {
        Assert.IsTrue (((int[]?)null).IsNullOrEmpty());
        Assert.IsTrue (Array.Empty<int>().IsNullOrEmpty());
        Assert.IsFalse (new int[1].IsNullOrEmpty());
    }

    [TestMethod]
    public void ArrayUtility_IsNullOrEmpty_2()
    {
        Assert.IsTrue (ArrayUtility.IsNullOrEmpty (null));
        Assert.IsTrue (ArrayUtility.IsNullOrEmpty (Array.Empty<int>()));
        Assert.IsFalse (ArrayUtility.IsNullOrEmpty (new int[1]));
    }

    [TestMethod]
    public void ArrayUtility_Merge_1()
    {
        int[] array1 = { 1, 2, 3 };
        int[] array2 = { 5, 6, 7 };
        var result = ArrayUtility.Merge (array1, array2);
        Assert.AreEqual (6, result.Length);
        Assert.AreEqual (1, result[0]);
        Assert.AreEqual (2, result[1]);
        Assert.AreEqual (3, result[2]);
        Assert.AreEqual (5, result[3]);
        Assert.AreEqual (6, result[4]);
        Assert.AreEqual (7, result[5]);
    }

    [TestMethod]
    public void ArrayUtility_Merge_2()
    {
        int[] array1 = { 1, 2, 3 };
        var result = ArrayUtility.Merge (array1);
        Assert.AreEqual (array1.Length, result.Length);
        Assert.AreEqual (array1[0], result[0]);
        Assert.AreEqual (array1[1], result[1]);
        Assert.AreEqual (array1[2], result[2]);
    }

    [TestMethod]
    public void ArrayUtility_Merge_3()
    {
        var result = ArrayUtility.Merge<int>();
        Assert.AreEqual (0, result.Length);
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentNullException))]
    public void ArrayUtility_Merge_4()
    {
        ArrayUtility.Merge<int> (null, null);
    }

    [TestMethod]
    public void ArrayUtility_SafeLength_1()
    {
        var array = Array.Empty<int>();
        Assert.AreEqual (0, ArrayUtility.SafeLength (array));
        array = new[] { 1, 2, 3 };
        Assert.AreEqual (3, ArrayUtility.SafeLength (array));
        array = null;
        Assert.AreEqual (0, ArrayUtility.SafeLength (array));
    }

    [TestMethod]
    public void ArrayUtility_ToString_1()
    {
        int[] array = { 1, 2, 3 };
        var lines = ArrayUtility.ToString (array);
        Assert.AreEqual (array.Length, lines.Length);
        for (var i = 0; i < array.Length; i++)
        {
            Assert.AreEqual
                (
                    array[i].ToString(),
                    lines[i]
                );
        }
    }

    [TestMethod]
    public void ArrayUtility_ToString_2()
    {
        string?[] array = { "one", null, "three" };
        var lines = ArrayUtility.ToString (array);
        Assert.AreEqual (3, lines.Length);
        Assert.AreEqual ("one", lines[0]);
        Assert.AreEqual ("(null)", lines[1]);
        Assert.AreEqual ("three", lines[2]);
    }

    [TestMethod]
    public void ArrayUtility_Clone_1()
    {
        var source = new MyClass[3];
        for (var i = 0; i < source.Length; i++)
        {
            source[i] = new MyClass { Value = i };
        }

        var target = ArrayUtility.Clone (source);
        Assert.AreEqual (source.Length, target.Length);
        for (var i = 0; i < source.Length; i++)
        {
            Assert.AreEqual (source[i].Value, target[i].Value);
        }
    }

    [TestMethod]
    public void ArrayUtility_Coincide_1()
    {
        int[] first = { 1, 2, 3, 4, 5, 6, 7 };
        int[] second = { 3, 4, 5, 6, 7, 8, 9 };
        Assert.IsTrue (ArrayUtility.Coincide (first, 3, second, 1, 2));
        Assert.IsFalse (ArrayUtility.Coincide (first, 3, second, 2, 2));
    }

    [TestMethod]
    public void ArrayUtility_SplitArray_1()
    {
        int[] mainArray = { 1, 2, 3, 4, 5, 6, 7 };
        var split = ArrayUtility.SplitArray (mainArray, 1);
        Assert.AreEqual (1, split.Length);
        Assert.AreEqual (mainArray.Length, split[0].Length);

        split = ArrayUtility.SplitArray (mainArray, 2);
        Assert.AreEqual (2, split.Length);
        Assert.AreEqual (4, split[0].Length);
        Assert.AreEqual (3, split[1].Length);

        split = ArrayUtility.SplitArray (mainArray, 3);
        Assert.AreEqual (3, split.Length);
        Assert.AreEqual (3, split[0].Length);
        Assert.AreEqual (3, split[1].Length);
        Assert.AreEqual (1, split[2].Length);

        split = ArrayUtility.SplitArray (mainArray, 4);
        Assert.AreEqual (4, split.Length);
        Assert.AreEqual (2, split[0].Length);
        Assert.AreEqual (2, split[1].Length);
        Assert.AreEqual (2, split[2].Length);
        Assert.AreEqual (1, split[3].Length);

        split = ArrayUtility.SplitArray (mainArray, 7);
        Assert.AreEqual (7, split.Length);
        Assert.AreEqual (1, split[0].Length);
        Assert.AreEqual (1, split[1].Length);
        Assert.AreEqual (1, split[2].Length);
        Assert.AreEqual (1, split[3].Length);
        Assert.AreEqual (1, split[4].Length);
        Assert.AreEqual (1, split[5].Length);
        Assert.AreEqual (1, split[6].Length);

        split = ArrayUtility.SplitArray (mainArray, 8);
        Assert.AreEqual (8, split.Length);
        Assert.AreEqual (1, split[0].Length);
        Assert.AreEqual (1, split[1].Length);
        Assert.AreEqual (1, split[2].Length);
        Assert.AreEqual (1, split[3].Length);
        Assert.AreEqual (1, split[4].Length);
        Assert.AreEqual (1, split[5].Length);
        Assert.AreEqual (1, split[6].Length);
        Assert.AreEqual (0, split[7].Length);
    }
}
