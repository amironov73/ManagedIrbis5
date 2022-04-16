// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Caching;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Caching;

[TestClass]
public sealed class DocumentCacheTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void DocumentCache_Construction_1()
    {
        using var provider = new NullProvider();
        using var cache = new DocumentCache (provider);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void DocumentCache_Construction_2()
    {
        using var memory = new MemoryCache (new MemoryCacheOptions());
        using var provider = new NullProvider();
        using var cache = new DocumentCache (provider, memory);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void DocumentCache_Construction_3()
    {
        var options = new MemoryCacheOptions();
        using var provider = new NullProvider();
        using var cache = new DocumentCache (provider, options);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Очистка")]
    public void DocumentCache_Clear_1()
    {
        using var provider = new NullProvider();
        using var cache = new DocumentCache (provider);
        Assert.IsNotNull (cache);
        cache.Clear();
    }

    [TestMethod]
    [Description ("Получение документа")]
    public void DocumentCache_GetDocument_1()
    {
        using var provider = new NullProvider();
        using var cache = new DocumentCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "brief.pft"
        };
        Assert.IsNull (cache.GetDocument (specification));
    }

    [TestMethod]
    [Description ("Обновление документа")]
    public void DocumentCache_UpdateDocument_1()
    {
        using var provider = new NullProvider();
        using var cache = new DocumentCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "brief.pft"
        };
        var newContent = "Hello";
        cache.UpdateDocument (specification, newContent);
        Assert.AreEqual (newContent, cache.GetDocument (specification));
    }

}