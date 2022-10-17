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

using System;
using System.IO;
using System.Text;

using AM;
using AM.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public sealed class ValueByteNavigatorTest
    : Common.CommonUnitTest
{
    private byte[] _GetData()
    {
        // 9 bytes
        return new byte[] { 3, 14, 15, 9, 26, 5, 35, 89, 79 };
    }

    private ValueByteNavigator _GetNavigator()
    {
        return new ValueByteNavigator (_GetData());
    }

    [TestMethod]
    public void ValueByteNavigator_Construction_1()
    {
        var navigator = _GetNavigator();
        Assert.AreEqual (0, navigator.Position);
        Assert.AreEqual (9, navigator.Length);
        Assert.IsFalse (navigator.IsEOF);
    }

    [TestMethod]
    public void ValueByteNavigator_FromFile_1()
    {
        var fileName = Path.Combine (TestDataPath, "empty.mst");
        var navigator = ValueByteNavigator.FromFile (fileName);
        Assert.IsFalse (navigator.IsEOF);
        Assert.AreEqual (0, navigator.Position);
        Assert.AreEqual (36, navigator.Length);
    }

    [TestMethod]
    public void ValueByteNavigator_GetRemainingData_1()
    {
        var navigator = _GetNavigator();
        navigator.MoveAbsolute (2);
        var data = navigator.GetRemainingData();
        Assert.AreEqual (7, data.Length);
        Assert.AreEqual ((byte)15, data[0]);
    }

    [TestMethod]
    public void ValueByteNavigator_GetRemainingData_2()
    {
        var navigator = _GetNavigator();
        navigator.MoveAbsolute (9);
        var data = navigator.GetRemainingData();
        Assert.IsTrue (data.IsEmpty);
    }

    [TestMethod]
    public void ValueByteNavigator_IsControl_1()
    {
        var navigator = _GetNavigator();
        Assert.IsTrue (navigator.IsControl());
    }

    [TestMethod]
    public void ValueByteNavigator_IsDigit_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsDigit());
    }

    [TestMethod]
    public void ValueByteNavigator_IsEof_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsEOF);
        navigator.MoveAbsolute (navigator.Length);
        Assert.IsTrue (navigator.IsEOF);
    }

    [TestMethod]
    public void ValueByteNavigator_IsLetter_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsLetter());
    }

    [TestMethod]
    public void ValueByteNavigator_IsLetterOrDigit_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsLetterOrDigit());
    }

    [TestMethod]
    public void ValueByteNavigator_IsNumber_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsNumber());
    }

    [TestMethod]
    public void ValueByteNavigator_IsPunctuation_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsPunctuation());
    }

    [TestMethod]
    public void ValueByteNavigator_IsSeparator_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsSeparator());
    }

    [TestMethod]
    public void ValueByteNavigator_IsSurrogate_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsSurrogate());
    }

    [TestMethod]
    public void ValueByteNavigator_IsSymbol_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsSymbol());
    }

    [TestMethod]
    public void ValueByteNavigator_IsWhiteSpace_1()
    {
        var navigator = _GetNavigator();
        Assert.IsFalse (navigator.IsWhiteSpace());
    }

    [TestMethod]
    public void ValueByteNavigator_MoveAbsolute_1()
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
    public void ValueByteNavigator_MoveRelative_1()
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
    public void ValueByteNavigator_PeekByte_1()
    {
        var navigator = _GetNavigator();
        Assert.AreEqual ((byte)3, navigator.PeekByte());
        Assert.AreEqual ((byte)3, navigator.PeekByte());

        navigator.MoveRelative (1);
        Assert.AreEqual ((byte)14, navigator.PeekByte());
        Assert.AreEqual ((byte)14, navigator.PeekByte());

        navigator.MoveAbsolute (99);
        Assert.AreEqual (ValueByteNavigator.EOF, navigator.PeekByte());
        Assert.AreEqual (ValueByteNavigator.EOF, navigator.PeekByte());
    }

    [TestMethod]
    public void ValueByteNavigator_PeekChar_1()
    {
        byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
        var navigator = new ValueByteNavigator (data, Encoding.UTF8);
        Assert.AreEqual ('H', navigator.PeekChar());
        Assert.AreEqual ('H', navigator.PeekChar());
        navigator.ReadByte();
        Assert.AreEqual ('e', navigator.PeekChar());
        Assert.AreEqual ('e', navigator.PeekChar());
    }

    [TestMethod]
    public void ValueByteNavigator_PeekChar_2()
    {
        byte[] data =
        {
            208, 163, 32, 208, 191, 208, 190, 208, 191,
            208, 176, 32, 208, 177, 209, 139, 208, 187, 208, 176,
            32, 209, 129, 208, 190, 208, 177, 208, 176, 208, 186,
            208, 176
        };
        var navigator = new ValueByteNavigator (data, Encoding.UTF8);
        Assert.AreEqual ('У', navigator.PeekChar());
        Assert.AreEqual ('У', navigator.PeekChar());
        navigator.ReadByte();
        navigator.ReadByte();
        Assert.AreEqual (' ', navigator.PeekChar());
        Assert.AreEqual (' ', navigator.PeekChar());
        navigator.ReadByte();
        Assert.AreEqual ('п', navigator.PeekChar());
        Assert.AreEqual ('п', navigator.PeekChar());
    }

    [TestMethod]
    public void ValueByteNavigator_ReadByte_1()
    {
        var navigator = _GetNavigator();
        Assert.AreEqual (3, navigator.ReadByte());
        Assert.AreEqual (14, navigator.ReadByte());
        navigator.MoveAbsolute (99);
        Assert.AreEqual (-1, navigator.ReadByte());
        Assert.AreEqual (-1, navigator.ReadByte());
    }

    [TestMethod]
    public void ValueByteNavigator_ReadChar_1()
    {
        byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100, 33 };
        var navigator = new ValueByteNavigator (data, Encoding.UTF8);
        Assert.AreEqual ('H', navigator.ReadChar());
        Assert.AreEqual ('e', navigator.ReadChar());
        Assert.AreEqual ('l', navigator.ReadChar());
        Assert.AreEqual ('l', navigator.ReadChar());
        Assert.AreEqual ('o', navigator.ReadChar());
        Assert.AreEqual ('\n', navigator.ReadChar());
        Assert.AreEqual ('W', navigator.ReadChar());
        Assert.AreEqual ('o', navigator.ReadChar());
        Assert.AreEqual ('r', navigator.ReadChar());
        Assert.AreEqual ('l', navigator.ReadChar());
        Assert.AreEqual ('d', navigator.ReadChar());
        Assert.AreEqual ('!', navigator.ReadChar());
        Assert.AreEqual ('\0', navigator.ReadChar());
        Assert.AreEqual ('\0', navigator.ReadChar());
    }

    [TestMethod]
    public void ValueByteNavigator_ReadChar_2()
    {
        byte[] data =
        {
            208, 163, 32, 208, 191, 208, 190, 208, 191,
            208, 176, 32, 208, 177, 209, 139, 208, 187, 208, 176,
            32, 209, 129, 208, 190, 208, 177, 208, 176, 208, 186,
            208, 176
        };
        var navigator = new ValueByteNavigator (data, Encoding.UTF8);
        Assert.AreEqual ('У', navigator.ReadChar());
        Assert.AreEqual (' ', navigator.ReadChar());
        Assert.AreEqual ('п', navigator.ReadChar());
        Assert.AreEqual ('о', navigator.ReadChar());
        Assert.AreEqual ('п', navigator.ReadChar());
        Assert.AreEqual ('а', navigator.ReadChar());
        Assert.AreEqual (' ', navigator.ReadChar());
        Assert.AreEqual ('б', navigator.ReadChar());
        Assert.AreEqual ('ы', navigator.ReadChar());
        Assert.AreEqual ('л', navigator.ReadChar());
        Assert.AreEqual ('а', navigator.ReadChar());
        Assert.AreEqual (' ', navigator.ReadChar());
        Assert.AreEqual ('с', navigator.ReadChar());
        Assert.AreEqual ('о', navigator.ReadChar());
        Assert.AreEqual ('б', navigator.ReadChar());
        Assert.AreEqual ('а', navigator.ReadChar());
        Assert.AreEqual ('к', navigator.ReadChar());
        Assert.AreEqual ('а', navigator.ReadChar());
        Assert.AreEqual ('\0', navigator.ReadChar());
        Assert.AreEqual ('\0', navigator.ReadChar());
    }

    [TestMethod]
    public void ValueByteNavigator_ReadChar_3()
    {
        byte[] data =
        {
            211, 32, 239, 238, 239, 224, 32, 225,
            251, 235, 224, 32, 241, 238, 225, 224, 234, 224
        };
        var navigator = new ValueByteNavigator (data, Encoding.GetEncoding (1251));
        Assert.AreEqual ('У', navigator.ReadChar());
        Assert.AreEqual (' ', navigator.ReadChar());
        Assert.AreEqual ('п', navigator.ReadChar());
        Assert.AreEqual ('о', navigator.ReadChar());
        Assert.AreEqual ('п', navigator.ReadChar());
        Assert.AreEqual ('а', navigator.ReadChar());
        Assert.AreEqual (' ', navigator.ReadChar());
        Assert.AreEqual ('б', navigator.ReadChar());
        Assert.AreEqual ('ы', navigator.ReadChar());
        Assert.AreEqual ('л', navigator.ReadChar());
        Assert.AreEqual ('а', navigator.ReadChar());
        Assert.AreEqual (' ', navigator.ReadChar());
        Assert.AreEqual ('с', navigator.ReadChar());
        Assert.AreEqual ('о', navigator.ReadChar());
        Assert.AreEqual ('б', navigator.ReadChar());
        Assert.AreEqual ('а', navigator.ReadChar());
        Assert.AreEqual ('к', navigator.ReadChar());
        Assert.AreEqual ('а', navigator.ReadChar());
        Assert.AreEqual ('\0', navigator.ReadChar());
        Assert.AreEqual ('\0', navigator.ReadChar());
    }

    private static ReadOnlySpan<byte> AsSpan
        (
            string text
        )
    {
        return Encoding.UTF8.GetBytes (text);
    }

    [TestMethod]
    public void ValueByteNavigator_ReadLine_1()
    {
        // CR + LF
        byte[] data = { 72, 101, 108, 108, 111, 13, 10, 87, 111, 114, 108, 100 };
        var navigator = new ValueByteNavigator (data);
        Assert.AreEqual (0, Utility.CompareSpans (AsSpan ("Hello"), navigator.ReadLine()));
        Assert.AreEqual (0, Utility.CompareSpans (AsSpan ("World"), navigator.ReadLine()));
        Assert.IsTrue (navigator.IsEOF);
        Assert.AreEqual (0, navigator.ReadLine().Length);
        Assert.IsTrue (navigator.IsEOF);
        Assert.AreEqual (0, navigator.ReadLine().Length);
    }

    [TestMethod]
    public void ValueByteNavigator_ReadLine_2()
    {
        // LF only
        byte[] data =
        {
            208, 163, 32, 208, 191, 208, 190, 208, 191,
            208, 176, 10, 208, 177, 209, 139, 208, 187, 208, 176,
            32, 209, 129, 208, 190, 208, 177, 208, 176, 208, 186,
            208, 176
        };
        var navigator = new ValueByteNavigator (data);
        Assert.AreEqual (0, Utility.CompareSpans (AsSpan ("У попа"), navigator.ReadLine()));
        Assert.AreEqual (0, Utility.CompareSpans (AsSpan ("была собака"), navigator.ReadLine()));
        Assert.IsTrue (navigator.IsEOF);
        Assert.AreEqual (0, navigator.ReadLine().Length);
        Assert.IsTrue (navigator.IsEOF);
        Assert.AreEqual (0, navigator.ReadLine().Length);
    }

    [TestMethod]
    public void ValueByteNavigator_SkipLine_1()
    {
        // CR + LF
        byte[] data = { 72, 101, 108, 108, 111, 13, 10, 87, 111, 114, 108, 100 };
        var navigator = new ValueByteNavigator (data);
        navigator.ReadChar();
        navigator.SkipLine();
        Assert.AreEqual (0, Utility.CompareSpans (AsSpan ("World"), navigator.ReadLine()));
        Assert.IsTrue (navigator.IsEOF);
        Assert.AreEqual (0, navigator.ReadLine().Length);
        Assert.IsTrue (navigator.IsEOF);
        Assert.AreEqual (0, navigator.ReadLine().Length);
    }

    [TestMethod]
    public void ValueByteNavigator_SkipLine_2()
    {
        // LF only
        byte[] data = { 72, 101, 108, 108, 111, 10, 87, 111, 114, 108, 100 };
        var navigator = new ValueByteNavigator (data);
        navigator.ReadChar();
        navigator.SkipLine();
        Assert.AreEqual (0, Utility.CompareSpans (AsSpan ("World"), navigator.ReadLine()));
        Assert.IsTrue (navigator.IsEOF);
        Assert.AreEqual (0, navigator.ReadLine().Length);
        Assert.IsTrue (navigator.IsEOF);
        Assert.AreEqual (0, navigator.ReadLine().Length);
    }

    [TestMethod]
    public void ValueByteNavigator_SkipLine_3()
    {
        var navigator = _GetNavigator();
        navigator.MoveAbsolute (99);
        navigator.SkipLine();
        Assert.IsTrue (navigator.IsEOF);
    }
}
