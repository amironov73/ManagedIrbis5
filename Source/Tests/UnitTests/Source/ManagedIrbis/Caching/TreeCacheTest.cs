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
using ManagedIrbis.Trees;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Caching;

[TestClass]
public sealed class TreeCacheTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void TreeCache_Construction_1()
    {
        using var provider = new NullProvider();
        using var cache = new TreeCache (provider);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void TreeCache_Construction_2()
    {
        using var memory = new MemoryCache (new MemoryCacheOptions());
        using var provider = new NullProvider();
        using var cache = new TreeCache (provider, memory);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void TreeCache_Construction_3()
    {
        var options = new MemoryCacheOptions();
        using var provider = new NullProvider();
        using var cache = new TreeCache (provider, options);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Очистка")]
    public void TreeCache_Clear_1()
    {
        using var provider = new NullProvider();
        using var cache = new TreeCache (provider);
        Assert.IsNotNull (cache);
        cache.Clear();
    }

    [TestMethod]
    [Description ("Получение дерева")]
    public void TreeCache_GetTree_1()
    {
        using var provider = new NullProvider();
        using var cache = new TreeCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "ii.tre"
        };
        Assert.IsNull (cache.GetTree (specification));
    }

    [TestMethod]
    [Description ("Обновление дерева")]
    public void TreeCache_UpdateTree_1()
    {
        using var provider = new NullProvider();
        using var cache = new TreeCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "ii.tre"
        };
        Assert.IsNull (cache.GetTree (specification));

        var newTree = new TreeFile();
        newTree.AddRoot ("Code").AddChild ("Comment");
        cache.UpdateTree (specification, newTree);

        var cached = cache.GetTree (specification);
        Assert.IsNotNull (cached);
        // TODO исправить
        //Assert.AreEqual ("Code", cached.Roots[0].Value);
        //Assert.AreEqual ("Comment", cached.Roots[0].Children[0].Value);
    }
}
