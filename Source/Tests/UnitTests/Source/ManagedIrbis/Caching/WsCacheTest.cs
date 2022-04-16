// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis;
using ManagedIrbis.Caching;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;
using ManagedIrbis.Workspace;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Caching;

[TestClass]
public sealed class WsCacheTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void WsCache_Construction_1()
    {
        using var provider = new NullProvider();
        using var cache = new WsCache (provider);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void WsCache_Construction_2()
    {
        using var memory = new MemoryCache (new MemoryCacheOptions());
        using var provider = new NullProvider();
        using var cache = new WsCache (provider, memory);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void WsCache_Construction_3()
    {
        var options = new MemoryCacheOptions();
        using var provider = new NullProvider();
        using var cache = new WsCache (provider, options);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Очистка")]
    public void WsCache_Clear_1()
    {
        using var provider = new NullProvider();
        using var cache = new WsCache (provider);
        Assert.IsNotNull (cache);
        cache.Clear();
    }

    [TestMethod]
    [Description ("Получение рабочего листа")]
    public void WsCache_GetWs_1()
    {
        using var provider = new NullProvider();
        using var cache = new WsCache (provider);
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
    public void WsCache_GetWss_1()
    {
        using var provider = new NullProvider();
        using var cache = new WsCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "111.wss"
        };
        Assert.IsNull (cache.GetWss (specification));
    }

    [TestMethod]
    [Description ("Обновление рабочего листа")]
    [ExpectedException (typeof (FormatException))]
    public void WsCache_UpdateWs_1()
    {
        using var provider = new NullProvider();
        using var cache = new WsCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "spec31.ws"
        };
        Assert.IsNull (cache.GetWs (specification));

        var newWs = new WsFile();
        newWs.Pages.Add (new WorksheetPage()
        {
            Name = "NewPage",
            Items =
            {
                new WorksheetItem()
                {
                    Tag = "1111",
                    EditMode = "1"
                }
            }
        });
        cache.UpdateWs (specification, newWs);

        var cached = cache.GetWs (specification);
        Assert.IsNotNull (cached);
        // TODO исправить восстановление
    }

    [TestMethod]
    [Description ("Обновление рабочего листа")]
    [ExpectedException (typeof (FormatException))]
    public void WsCache_UpdateWsы_1()
    {
        using var provider = new NullProvider();
        using var cache = new WsCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "11.wsы"
        };
        Assert.IsNull (cache.GetWss (specification));

        var newWss = new WssFile();
        newWss.Items.Add (new WorksheetItem()
        {
            Tag = "1111",
            EditMode = "1"
        });
        cache.UpdateWss (specification, newWss);

        var cached = cache.GetWss (specification);
        Assert.IsNotNull (cached);
        // TODO исправить восстановление
    }
}