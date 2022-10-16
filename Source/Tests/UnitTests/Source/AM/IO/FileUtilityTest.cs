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
public sealed class FileUtilityTest
{
    [TestMethod]
    public void FileUtility_ConvertToFileName_1()
    {
        Assert.AreEqual
            (
                "Hello.cs",
                FileUtility.ConvertToFileName ("Hello.cs")
            );

        Assert.AreEqual
            (
                "Hello.cs",
                FileUtility.ConvertToFileName (" Hello.cs ")
            );

        Assert.AreEqual
            (
                "Hello_World.cs",
                FileUtility.ConvertToFileName ("Hello:World.cs")
            );

        Assert.AreEqual
            (
                "Hello_World.cs",
                FileUtility.ConvertToFileName (" Hello::World.cs ")
            );
    }
}
