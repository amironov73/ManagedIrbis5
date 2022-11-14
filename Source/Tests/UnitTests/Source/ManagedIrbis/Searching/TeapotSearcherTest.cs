// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.AOT.Stemming;
using AM.ComponentModel;
using AM.PlatformAbstraction;

using ManagedIrbis.Direct;
using ManagedIrbis.Searching;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Search;

[TestClass]
public class TeapotSearcherTest
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
    [Description ("Свежесозданный объект")]
    public void TeapotSearcher_Construction_1()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider);

        Assert.AreSame (serviceProvider, searcher.ServiceProvider);
        Assert.IsNotNull (searcher.Prefixes);
        Assert.AreEqual (4, searcher.Prefixes.Count);
    }

    [TestMethod]
    [Description ("Построение запроса по одному слову")]
    public void TeapotSearcher_BuildSearchExpression_1()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider);

        var expression = searcher.BuildSearchExpression ("concrete");

        Assert.AreEqual
            (
                "K=concrete$ + A=concrete$ + M=concrete$ + T=concrete$",
                expression
            );
    }

    [TestMethod]
    [Description ("Построение запроса по словосочетанию со стоп-словом")]
    public void TeapotSearcher_BuildSearchExpression_2()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider);

        var expression = searcher.BuildSearchExpression ("fire and water");

        Assert.AreEqual
            (
                """K=fire$ + A=fire$ + M=fire$ + T=fire$ + "K=fire and water$" + "A=fire and water$" + "M=fire and water$" + "T=fire and water$" + K=water$ + A=water$ + M=water$ + T=water$""",
                expression
            );
    }

    [TestMethod]
    [Description ("Построение запроса по словосочетанию, содержащему только стоп-слова")]
    public void TeapotSearcher_BuildSearchExpression_3()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider);

        var expression = searcher.BuildSearchExpression ("and or not");

        Assert.AreEqual
            (
                "\"K=and or not$\" + \"A=and or not$\" + \"M=and or not$\" + \"T=and or not$\"",
                expression
            );
    }

    [TestMethod]
    [Description ("Построение запроса по сочетанию чисел")]
    public void TeapotSearcher_BuildSearchExpression_4()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider);

        var expression = searcher.BuildSearchExpression ("1941-1945");

        Assert.AreEqual
            (
                """K=1941$ + A=1941$ + M=1941$ + T=1941$ + K=1941-1945$ + A=1941-1945$ + M=1941-1945$ + T=1941-1945$ + K=1945$ + A=1945$ + M=1945$ + T=1945$""",
                expression
            );
    }

    [TestMethod]
    [Description ("Применение стеммера")]
    public void TeapotSearcher_BuildSearchExpression_5()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var stemmer = new RussianStemmer();
        var searcher = new TeapotSearcher (serviceProvider)
        {
            Stemmer = stemmer
        };

        var expression = searcher.BuildSearchExpression ("кошки");

        Assert.AreEqual
            (
                "K=кошк$ + A=кошк$ + M=кошк$ + T=кошк$",
                expression
            );
    }

    [TestMethod]
    [Description ("Применение стеммера")]
    public void TeapotSearcher_BuildSearchExpression_6()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var stemmer = new RussianStemmer();
        var searcher = new TeapotSearcher (serviceProvider)
        {
            Stemmer = stemmer
        };

        var expression = searcher.BuildSearchExpression ("кошки и собака");

        Assert.AreEqual
            (
                "K=кошк$ + A=кошк$ + M=кошк$ + T=кошк$ + \"K=кошки и собака$\" + \"A=кошки и собака$\" + \"M=кошки и собака$\" + \"T=кошки и собака$\" + K=собак$ + A=собак$ + M=собак$ + T=собак$",
                expression
            );
    }

    [TestMethod]
    [Description ("Построение запроса по пустой строке")]
    public void TeapotSearcher_BuildSearchExpression_7()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider);

        var expression = searcher.BuildSearchExpression (string.Empty);

        Assert.AreEqual
            (
                string.Empty,
                expression
            );
    }

    [TestMethod]
    [Description ("Поиск без усечения")]
    public void TeapotSearcher_Search_1()
    {
        using var provider = _GetProvider();
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider)
        {
            Suffix = string.Empty
        };

        var found = searcher.Search (provider, "Case");
        Assert.IsNotNull (found);
        Assert.AreEqual (2, found.Length);
    }

    [TestMethod]
    [Description ("Поиск несуществующего")]
    public void TeapotSearcher_Search_2()
    {
        using var provider = _GetProvider();
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider)
        {
            Suffix = string.Empty
        };

        var found = searcher.Search (provider, "ZZZZ QQQQ");
        Assert.IsNotNull (found);
        Assert.AreEqual (0, found.Length);
    }
}
