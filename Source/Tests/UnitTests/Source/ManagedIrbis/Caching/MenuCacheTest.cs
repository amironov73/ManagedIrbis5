// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis;
using ManagedIrbis.Caching;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Providers;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Caching;

[TestClass]
public sealed class MenuCacheTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void MenuCache_Construction_1()
    {
        using var provider = new NullProvider();
        using var cache = new MenuCache (provider);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void MenuCache_Construction_2()
    {
        using var memory = new MemoryCache (new MemoryCacheOptions());
        using var provider = new NullProvider();
        using var cache = new MenuCache (provider, memory);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void MenuCache_Construction_3()
    {
        var options = new MemoryCacheOptions();
        using var provider = new NullProvider();
        using var cache = new MenuCache (provider, options);
        Assert.AreSame (provider, cache.Provider);
    }

    [TestMethod]
    [Description ("Очистка")]
    public void MenuCache_Clear_1()
    {
        using var provider = new NullProvider();
        using var cache = new MenuCache (provider);
        Assert.IsNotNull (cache);
        cache.Clear();
    }

    [TestMethod]
    [Description ("Получение меню")]
    public void MenuCache_GetMenu_1()
    {
        using var provider = new NullProvider();
        using var cache = new MenuCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "disc.mnu"
        };
        Assert.IsNull (cache.GetMenu (specification));
    }

    [TestMethod]
    [Description ("Обновление меню")]
    public void MenuCache_UpdateMenu_1()
    {
        using var provider = new NullProvider();
        using var cache = new MenuCache (provider);
        var specification = new FileSpecification()
        {
            Path = IrbisPath.MasterFile,
            Database = "IBIS",
            FileName = "disc.mnu"
        };
        Assert.IsNull (cache.GetMenu (specification));

        var newMenu = new MenuFile();
        newMenu.Add ("Code", "Comment");
        cache.UpdateMenu (specification, newMenu);

        var cached = cache.GetMenu (specification);
        Assert.IsNotNull (cached);
        Assert.AreEqual ("Comment", cached.GetString ("Code"));
    }
}
