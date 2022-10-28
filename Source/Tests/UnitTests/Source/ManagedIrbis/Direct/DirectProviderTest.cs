// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.IO;
using System.IO.MemoryMappedFiles;

using AM;
using AM.IO;
using AM.PlatformAbstraction;

using ManagedIrbis;
using ManagedIrbis.Direct;
using ManagedIrbis.Providers;

#endregion


using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct;

[TestClass]
public sealed class DirectProviderTest
    : Common.CommonUnitTest
{
    private DirectProvider _GetProvider()
    {
        var rootPath = Irbis64RootPath;
        var result = new DirectProvider (rootPath)
        {
            Database = "IBIS",
            PlatformAbstraction = new TestingPlatformAbstraction()
        };

        result.Connect();

        return result;
    }

    [TestMethod]
    [Description ("Получение максимального MFN")]
    public void DirectProvider_GetMaxMfn_1()
    {
        using var provider = _GetProvider();
        var maxMfn = provider.GetMaxMfn();
        Assert.AreEqual (332, maxMfn);
    }

    [TestMethod]
    [Description ("Простое чтение записи")]
    public void DirectProvider_ReadRecord_1()
    {
        const int mfn = 1;
        using var provider = _GetProvider();
        var record = provider.ReadRecord (mfn);
        Assert.IsNotNull (record);
        Assert.AreEqual (mfn, record.Mfn);
    }

    [TestMethod]
    [Description ("Простейший поиск")]
    public void DirectProvider_Search_1()
    {
        using var provider = _GetProvider();
        var found = provider.Search ("K=CASE");
        Assert.IsNotNull (found);
        Assert.AreEqual (2, found.Length);
    }
}
