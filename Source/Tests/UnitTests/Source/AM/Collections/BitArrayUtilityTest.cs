// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System.Collections;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#endregion

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class BitArrayUtilityTest
{
    [TestMethod]
    [Description ("Простые случаи сравнения битовых массивов")]
    public void BitArrayUtility_AreEqual_1()
    {
        var left = new BitArray (10) { [1] = true };
        var right = new BitArray (10) { [1] = true };

        Assert.IsTrue (BitArrayUtility.AreEqual (left, right));

        right[2] = true;
        Assert.IsFalse (BitArrayUtility.AreEqual (left, right));

        right = new BitArray (11) { [1] = true };
        Assert.IsFalse (BitArrayUtility.AreEqual (left, right));
    }
}
