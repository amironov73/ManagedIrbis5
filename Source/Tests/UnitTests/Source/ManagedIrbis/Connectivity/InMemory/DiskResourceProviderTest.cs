// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using ManagedIrbis.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory;

[TestClass]
public sealed class DiskResourceProviderTest
    : Common.CommonUnitTest
{
    private string _GetDataiPath()
    {
        return Path.Combine (Irbis64RootPath, "Datai");
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void DiskResourceProvider_Construction_1()
    {
        var path = _GetDataiPath();
        var provider = new DiskResourceProvider (path);
        Assert.AreEqual (path, provider.RootPath);
        Assert.IsTrue (provider.ReadOnly);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void DiskResourceProvider_Construction_2()
    {
        var path = _GetDataiPath();
        var provider = new DiskResourceProvider (path, false);
        Assert.AreEqual (path, provider.RootPath);
        Assert.IsFalse (provider.ReadOnly);
    }

    [TestMethod]
    [Description ("Конструктор")]
    [ExpectedException (typeof (ArgumentException))]
    public void DiskResourceProvider_Construction_3()
    {
        const string path = "nosuchdirectory";
        var provider = new DiskResourceProvider (path);
        Assert.AreEqual (path, provider.RootPath);
        Assert.IsTrue (provider.ReadOnly);
    }

    [TestMethod]
    [Description ("Дамп")]
    [ExpectedException (typeof (NotImplementedException))]
    public void DiskResourceProvider_Dump_1()
    {
        var provider = new DiskResourceProvider (_GetDataiPath());
        using var output = new StringWriter();
        provider.Dump (output);
        var dump = output.ToString();
        Assert.IsNotNull (dump);
    }

    [TestMethod]
    [Description ("Получение списка ресурсов")]
    [ExpectedException (typeof (NotImplementedException))]
    public void DiskResourceProvider_ListResources_1()
    {
        var provider = new DiskResourceProvider (_GetDataiPath());
        var list = provider.ListResources (".");
        Assert.IsNotNull (list);
    }

    [TestMethod]
    [Description ("Чтение ресурса")]
    [ExpectedException (typeof (NotImplementedException))]
    public void DiskResourceProvider_ReadResource_1()
    {
        var provider = new DiskResourceProvider (_GetDataiPath());
        Assert.IsNull (provider.ReadResource ("nosuchfile"));
    }

    [TestMethod]
    [Description ("Проверка существования ресурса")]
    [ExpectedException (typeof (NotImplementedException))]
    public void DiskResourceProvider_ResourceExist_1()
    {
        var provider = new DiskResourceProvider (_GetDataiPath());
        Assert.IsFalse (provider.ResourceExists ("nosuchfile"));
    }

    [TestMethod]
    [Description ("Запись ресурса")]
    [ExpectedException (typeof (NotImplementedException))]
    public void DiskResourceProvider_WriteResource_1()
    {
        var provider = new DiskResourceProvider (_GetDataiPath());
        Assert.IsTrue (provider.WriteResource ("nosuchfile", "content"));
    }
}
