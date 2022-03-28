// ReSharper disable CheckNamespace

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.IO;

#nullable enable

namespace UnitTests.AM.IO;

[TestClass]
public class FileUtilityTest
{
    [TestMethod]
    public void FileUtility_ConvertToFileName_1()
    {
        Assert.AreEqual ("Hello.cs", FileUtility.ConvertToFileName ("Hello.cs"));
        Assert.AreEqual ("Hello.cs", FileUtility.ConvertToFileName (" Hello.cs "));
        Assert.AreEqual ("Hello_World.cs", FileUtility.ConvertToFileName ("Hello:World.cs"));
        Assert.AreEqual ("Hello_World.cs", FileUtility.ConvertToFileName (" Hello::World.cs "));
    }
}
