// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ExpressionIsAlwaysNull
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable RedundantCast
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.Text;

using AM.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public sealed class UnsafeByteNavigatorTest
    : Common.CommonUnitTest
{
    private byte[] _GetData()
    {
        // 9 bytes
        return new byte[] { 3, 14, 15, 9, 26, 5, 35, 89, 79 };
    }

    private unsafe UnsafeByteNavigator _GetNavigator()
    {
        var data = _GetData();
        fixed (byte* ptr = data)
        {
            return new UnsafeByteNavigator (ptr, data.Length);
        }
    }

    [TestMethod]
    public void UnsafeByteNavigator_Construction_1()
    {
        var navigator = _GetNavigator();
        Assert.AreEqual (0, navigator.Position);
        Assert.AreEqual (9, navigator.Length);
        Assert.IsFalse (navigator.IsEOF);
        Assert.AreEqual (Encoding.Default, navigator.Encoding);
    }

    [TestMethod]
    public void UnsafeByteNavigator_Clone_1()
    {
        var first = _GetNavigator();
        var second = first.Clone();
        Assert.AreEqual (first.Position, second.Position);
        Assert.AreEqual (first.Length, second.Length);
        Assert.AreSame (first.Encoding, second.Encoding);
    }

    [TestMethod]
    public void UnsafeByteNavigator_GetRemainingData_1()
    {
        var navigator = _GetNavigator();
        navigator.MoveAbsolute (2);
        var data = navigator.GetRemainingData();
        Assert.AreEqual (7, data.Length);
        Assert.AreEqual ((byte)15, data[0]);
    }

    [TestMethod]
    public void UnsafeByteNavigator_GetRemainingData_2()
    {
        var navigator = _GetNavigator();
        navigator.MoveAbsolute (9);
        var data = navigator.GetRemainingData();
        Assert.IsTrue (data.IsEmpty);
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsControl_1()
    {
        var navigator = _GetNavigator();
        Assert.IsTrue (navigator.IsControl());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsDigit_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsDigit());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsEof_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsEOF);
        navigator.MoveAbsolute (navigator.Length);
        Assert.IsTrue (navigator.IsEOF);
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsLetter_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsLetter());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsLetterOrDigit_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsLetterOrDigit());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsNumber_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsNumber());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsPunctuation_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsPunctuation());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsSeparator_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsSeparator());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsSurrogate_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsSurrogate());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsSymbol_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsSymbol());
    }

    [TestMethod]
    public void UnsafeByteNavigator_IsWhiteSpace_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsWhiteSpace());
    }

    [TestMethod]
    public void UnsafeByteNavigator_MoveAbsolute_1()
    {
        var navigator = _GetNavigator();
        navigator.MoveAbsolute (1);
        Assert.AreEqual (1, navigator.Position);

        navigator.MoveAbsolute (9);
        Assert.AreEqual (9, navigator.Position);

        navigator.MoveAbsolute (-9);
        Assert.AreEqual (0, navigator.Position);

        navigator.MoveAbsolute (99);
        Assert.AreEqual (9, navigator.Position);
    }

    [TestMethod]
    public void UnsafeByteNavigator_MoveRelative_1()
    {
        var navigator = _GetNavigator();
        navigator.MoveRelative (1);
        Assert.AreEqual (1, navigator.Position);

        navigator.MoveRelative (8);
        Assert.AreEqual (9, navigator.Position);

        navigator.MoveRelative (1);
        Assert.AreEqual (9, navigator.Position);

        navigator.MoveRelative (-9);
        Assert.AreEqual (0, navigator.Position);

        navigator.MoveRelative (-1);
        Assert.AreEqual (0, navigator.Position);

        navigator.MoveRelative (99);
        Assert.AreEqual (9, navigator.Position);
    }

    [TestMethod]
    public void UnsafeByteNavigator_PeekByte_1()
    {
        var navigator = _GetNavigator();
        Assert.AreEqual ((byte)3, navigator.PeekByte());
        Assert.AreEqual ((byte)3, navigator.PeekByte());

        navigator.MoveRelative (1);
        Assert.AreEqual ((byte)14, navigator.PeekByte());
        Assert.AreEqual ((byte)14, navigator.PeekByte());

        navigator.MoveAbsolute (99);
        Assert.AreEqual (UnsafeByteNavigator.EOF, navigator.PeekByte());
        Assert.AreEqual (UnsafeByteNavigator.EOF, navigator.PeekByte());
    }

    [TestMethod]
    public unsafe void UnsafeByteNavigator_PeekChar_1()
    {
        byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
        fixed (byte* ptr = data)
        {
            var navigator = new UnsafeByteNavigator (ptr, data.Length);
            Assert.AreEqual ('H', navigator.PeekChar());
            Assert.AreEqual ('H', navigator.PeekChar());
            navigator.MoveAbsolute (99);
            Assert.AreEqual ('\0', navigator.PeekChar());
            Assert.AreEqual ('\0', navigator.PeekChar());
        }
    }

    [TestMethod]
    public void UnsafeByteNavigator_ReadByte_1()
    {
        var navigator = _GetNavigator();
        Assert.AreEqual (3, navigator.ReadByte());
        Assert.AreEqual (14, navigator.ReadByte());
        navigator.MoveAbsolute (99);
        Assert.AreEqual (-1, navigator.ReadByte());
        Assert.AreEqual (-1, navigator.ReadByte());
    }

    [TestMethod]
    public unsafe void UnsafeByteNavigator_ReadChar_1()
    {
        byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
        fixed (byte* ptr = data)
        {
            var navigator = new UnsafeByteNavigator (ptr, data.Length);
            Assert.AreEqual ('H', navigator.ReadChar());
            Assert.AreEqual ('e', navigator.ReadChar());
            navigator.MoveAbsolute (99);
            Assert.AreEqual ('\0', navigator.ReadChar());
            Assert.AreEqual ('\0', navigator.ReadChar());
        }
    }

    /*
    [TestMethod]
    public void UnsafeByteNavigator_ReadLine_1()
    {
        // CR + LF
        byte[] data = {72, 101, 108, 108, 111, 13, 10, 87, 111, 114, 108, 100};
        UnsafeByteNavigator navigator = new UnsafeByteNavigator(data);
        Assert.AreEqual("Hello", navigator.ReadLine().ToString());
        Assert.AreEqual("World", navigator.ReadLine().ToString());
        Assert.IsTrue(navigator.IsEOF);
        Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
        Assert.IsTrue(navigator.IsEOF);
        Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
    }

    [TestMethod]
    public void UnsafeByteNavigator_ReadLine_2()
    {
        // LF only
        byte[] data = {72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100};
        UnsafeByteNavigator navigator = new UnsafeByteNavigator(data);
        Assert.AreEqual("Hello", navigator.ReadLine().ToString());
        Assert.AreEqual("World", navigator.ReadLine().ToString());
        Assert.IsTrue(navigator.IsEOF);
        Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
        Assert.IsTrue(navigator.IsEOF);
        Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
    }

    [TestMethod]
    public void UnsafeByteNavigator_SkipLine_1()
    {
        // CR + LF
        byte[] data = { 72, 101, 108, 108, 111, 13, 10, 87, 111, 114, 108, 100 };
        UnsafeByteNavigator navigator = new UnsafeByteNavigator(data);
        navigator.ReadChar();
        navigator.SkipLine();
        Assert.AreEqual("World", navigator.ReadLine().ToString());
        Assert.IsTrue(navigator.IsEOF);
        Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
        Assert.IsTrue(navigator.IsEOF);
        Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
    }

    [TestMethod]
    public void UnsafeByteNavigator_SkipLine_2()
    {
        // LF only
        byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
        UnsafeByteNavigator navigator = new UnsafeByteNavigator(data);
        navigator.ReadChar();
        navigator.SkipLine();
        Assert.AreEqual("World", navigator.ReadLine().ToString());
        Assert.IsTrue(navigator.IsEOF);
        Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
        Assert.IsTrue(navigator.IsEOF);
        Assert.IsTrue(string.IsNullOrEmpty(navigator.ReadLine().ToString()));
    }
    */

    [TestMethod]
    public void UnsafeByteNavigator_SkipLine_3()
    {
        var navigator = _GetNavigator();
        navigator.MoveAbsolute (99);
        navigator.SkipLine();
        Assert.IsTrue (navigator.IsEOF);
    }
}
