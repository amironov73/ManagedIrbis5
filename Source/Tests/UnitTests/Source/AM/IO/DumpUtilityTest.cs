// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

#endregion

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public class DumpUtilityTest
{
    [TestMethod]
    public void DumpUtility_DumpToTextByte_1()
    {
        byte[] data = { 1, 2, 5, 10, 25, 50, 100 };

        var actual = DumpUtility.DumpToText (data)
            .TrimEnd();
        const string expected = "000000>   01 02 05 0A  19 32 64";

        Assert.AreEqual (expected, actual);
    }

    [TestMethod]
    public void DumpUtility_DumpToTextInt32_1()
    {
        int[] data = { 1, 2, 5, 10, 25, 50, 100 };

        var actual = DumpUtility.DumpToText (data)
            .TrimEnd();
        const string expected = "000000>   00000001 00000002 00000005 0000000A  00000019 00000032 00000064";

        Assert.AreEqual (expected, actual);
    }
}
