// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis;
using ManagedIrbis.Caching;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Caching;

[TestClass]
public sealed class ContextCacheTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void ContextCache_Construction_1()
    {
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void ContextCache_Construction_2()
    {
        using var memory = new MemoryCache (new MemoryCacheOptions());
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider, memory);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void ContextCache_Construction_3()
    {
        var options = new MemoryCacheOptions();
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider, options);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Очистка")]
    public void ContextCache_Clear_1()
    {
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider);
        Assert.IsNotNull (cache);
        cache.Clear();
    }

    [TestMethod]
    [Description ("Получение документа")]
    public void ContextCache_GetDocument_1()
    {
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "brief.pft"
        };
        Assert.IsNull (cache.GetDocument (specification));
    }

    [TestMethod]
    [Description ("Получение меню")]
    public void ContextCache_GetMenu_1()
    {
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "disc.mnu"
        };
        Assert.IsNull (cache.GetMenu (specification));
    }

    [TestMethod]
    [Description ("Получение записи")]
    public void ContextCache_GetRecord_1()
    {
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider);
        Assert.IsNull (cache.GetRecord<Record> (1));
    }

    [TestMethod]
    [Description ("Получение дерева")]
    public void ContextCache_GetTree_1()
    {
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "ii.tre"
        };
        Assert.IsNull (cache.GetTree (specification));
    }

    [TestMethod]
    [Description ("Получение рабочего листа")]
    public void ContextCache_GetWs_1()
    {
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "spec31.ws"
        };
        Assert.IsNull (cache.GetWs (specification));
    }

    [TestMethod]
    [Description ("Получение рабочего листа")]
    public void ContextCache_GetWss_1()
    {
        using var provider = new NullProvider();
        using var cache = new ContextCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "11.wss"
        };
        Assert.IsNull (cache.GetWss (specification));
    }

}
