// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ObjectCreationAsStatement
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using System;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.ConsoleIO;

#endregion

#nullable enable

namespace UnitTests.AM.ConsoleIO;

[TestClass]
public class NullConsoleTest
{
    [TestMethod]
    public void NullConsole_BackgroundColor_1()
    {
        var console = new NullConsole();
        console.BackgroundColor = ConsoleColor.Black;
        Assert.AreEqual (ConsoleColor.Black, console.BackgroundColor);
    }

    [TestMethod]
    public void NullConsole_ForegroundColor_1()
    {
        var console = new NullConsole();
        console.ForegroundColor = ConsoleColor.Black;
        Assert.AreEqual (ConsoleColor.Black, console.ForegroundColor);
    }

    [TestMethod]
    public void NullConsole_KeyAvailable_1()
    {
        var console = new NullConsole();
        Assert.IsFalse (console.KeyAvailable);
    }

    [TestMethod]
    public void NullConsole_Title_1()
    {
        var console = new NullConsole();
        console.Title = "Title";
        Assert.AreEqual ("Title", console.Title);
    }

    [TestMethod]
    public void NullConsole_Clear_1()
    {
        var console = new NullConsole();
        console.Clear();
        Assert.IsTrue (true);
    }

    [TestMethod]
    public void NullConsole_ReadKey_1()
    {
        var console = new NullConsole();
        var info = console.ReadKey (false);
        Assert.AreEqual ((ConsoleKey)0, info.Key);
    }

    [TestMethod]
    public void NullConsole_Read_1()
    {
        var console = new NullConsole();
        Assert.AreEqual (-1, console.Read());
    }

    [TestMethod]
    public void NullConsole_ReadLine_1()
    {
        var console = new NullConsole();
        Assert.IsNull (console.ReadLine());
    }

    [TestMethod]
    public void NullConsole_Write_1()
    {
        var console = new NullConsole();
        console.Write ("Some text");
        Assert.IsTrue (true);
    }

    [TestMethod]
    public void NullConsole_WriteLine_1()
    {
        var console = new NullConsole();
        console.WriteLine();
        Assert.IsTrue (true);
    }
}
