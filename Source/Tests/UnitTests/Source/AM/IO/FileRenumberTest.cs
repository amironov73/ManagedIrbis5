// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

#endregion

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public sealed class FileRenumberTest
{
    [TestMethod]
    [Description ("Простой случай")]
    public void FileRenumber_GenerateNames_1()
    {
        var renamer = new FileRenumber();
        var sourceFiles = new [] { "hello1", "hello2", "hello100" };
        var generated = renamer.GenerateNames (sourceFiles);
        var expected = new List<FileRenumber.Bunch>()
        {
            new ("hello1", "hello001"),
            new ("hello2", "hello002"),
        };
        CollectionAssert.AreEqual (expected, generated);
    }

    [TestMethod]
    [Description ("Все числа однозначные")]
    public void FileRenumber_GenerateNames_2()
    {
        var renamer = new FileRenumber();
        var sourceFiles = new [] { "hello1", "hello2", "hello3" };
        var generated = renamer.GenerateNames (sourceFiles);
        var expected = new List<FileRenumber.Bunch>();
        CollectionAssert.AreEqual (expected, generated);
    }

    [TestMethod]
    [Description ("Некторые без цифр")]
    public void FileRenumber_GenerateNames_3()
    {
        var renamer = new FileRenumber();
        var sourceFiles = new [] { "hello1", "hello", "hello300" };
        var generated = renamer.GenerateNames (sourceFiles);
        var expected = new List<FileRenumber.Bunch>()
        {
            new ("hello1", "hello001"),
        };
        CollectionAssert.AreEqual (expected, generated);
    }

    [TestMethod]
    [Description ("Вторая группа")]
    public void FileRenumber_GenerateNames_4()
    {
        var renamer = new FileRenumber { GroupNumber = 1 };
        var sourceFiles = new [] { "hello1world2", "hello2world3", "hello300world100" };
        var generated = renamer.GenerateNames (sourceFiles);
        var expected = new List<FileRenumber.Bunch>()
        {
            new ("hello1world2", "hello1world002"),
            new ("hello2world3", "hello2world003"),
        };
        CollectionAssert.AreEqual (expected, generated);
    }

    [TestMethod]
    [Description ("Вторая группа, в некоторых отсутствует")]
    public void FileRenumber_GenerateNames_5()
    {
        var renamer = new FileRenumber { GroupNumber = 1 };
        var sourceFiles = new [] { "hello1world2", "hello2world", "hello300world100" };
        var generated = renamer.GenerateNames (sourceFiles);
        var expected = new List<FileRenumber.Bunch>()
        {
            new ("hello1world2", "hello1world002"),
        };
        CollectionAssert.AreEqual (expected, generated);
    }

    [TestMethod]
    [Description ("Явно заданное число знаков")]
    public void FileRenumber_GenerateNames_6()
    {
        var renamer = new FileRenumber { GroupWidth = 5 };
        var sourceFiles = new [] { "hello1", "hello2", "hello300" };
        var generated = renamer.GenerateNames (sourceFiles);
        var expected = new List<FileRenumber.Bunch>()
        {
            new ("hello1", "hello00001"),
            new ("hello2", "hello00002"),
            new ("hello300", "hello00300"),
        };
        CollectionAssert.AreEqual (expected, generated);
    }

    [TestMethod]
    [Description ("Имена, состоящеие из одних цифр")]
    public void FileRenumber_GenerateNames_7()
    {
        var renamer = new FileRenumber();
        var sourceFiles = new [] { "1", "2", "100" };
        var generated = renamer.GenerateNames (sourceFiles);
        var expected = new List<FileRenumber.Bunch>()
        {
            new ("1", "001"),
            new ("2", "002"),
        };
        CollectionAssert.AreEqual (expected, generated);
    }

    [TestMethod]
    [Description ("Использование префикса")]
    public void FileRenumber_GenerateNames_8()
    {
        var renamer = new FileRenumber() { Prefix = "prefix_", Start = 1 };
        var sourceFiles = new [] { "hello1", "hello2", "hello100" };
        var generated = renamer.GenerateNames (sourceFiles);
        var expected = new List<FileRenumber.Bunch>()
        {
            new ("hello1", "prefix_001"),
            new ("hello2", "prefix_002"),
            new ("hello100", "prefix_003"),
        };
        CollectionAssert.AreEqual (expected, generated);
    }
}
