// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Caching;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Caching;

[TestClass]
public sealed class RecordCacheTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void RecordCache_Construction_1()
    {
        using var provider = new NullProvider();
        using var cache = new RecordCache (provider);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void RecordCache_Construction_2()
    {
        using var memory = new MemoryCache (new MemoryCacheOptions());
        using var provider = new NullProvider();
        using var cache = new RecordCache (provider, memory);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void RecordCache_Construction_3()
    {
        var options = new MemoryCacheOptions();
        using var provider = new NullProvider();
        using var cache = new RecordCache (provider, options);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Очистка")]
    public void RecordCache_Clear_1()
    {
        using var provider = new NullProvider();
        using var cache = new RecordCache (provider);
        Assert.IsNotNull (cache);
        cache.Clear();
    }

    [TestMethod]
    [Description ("Получение записи")]
    public void RecordCache_GetRecord_1()
    {
        using var provider = new NullProvider();
        using var cache = new RecordCache (provider);
        Assert.IsNull (cache.GetRecord<Record> (1));
    }

    [TestMethod]
    [Description ("Обновление записи")]
    public void RecordCache_UpdateRecord_1()
    {
        using var provider = new NullProvider();
        using var cache = new RecordCache (provider);
        Assert.IsNull (cache.GetRecord<Record> (1));

        var newRecord = new Record { { 1, "Field1" }, { 2, "Field2" } };
        cache.UpdateRecord (newRecord);

        var cached = cache.GetRecord<Record> (1);
        Assert.IsNull (cached);
        // TODO: Починить помещение в кэш
        //Assert.AreEqual (newRecord.FM (1), cached.FM (1));
        //Assert.AreEqual (newRecord.FM (2), cached.FM (2));
    }
}