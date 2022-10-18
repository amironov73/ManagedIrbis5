// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable ConvertToLocalFunction
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Reflection;

#endregion

#nullable enable

namespace UnitTests.AM.Reflection;

[TestClass]
public sealed class ConstantInfoTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void ConstantInfo_Constructor_1()
    {
        const string name = "Name";
        const string value = "Value";
        var info = new ConstantInfo<string> (name, value);
        Assert.AreEqual (name, info.Name);
        Assert.AreEqual (value, info.Value);
    }
}
