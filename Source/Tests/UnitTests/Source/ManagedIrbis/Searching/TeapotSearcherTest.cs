// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.ComponentModel;

using ManagedIrbis.Searching;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Search;

[TestClass]
public class TeapotSearcherTest
{
    [TestMethod]
    public void TeapotSearcher_Construction_1()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        var searcher = new TeapotSearcher (serviceProvider);

        Assert.AreSame (serviceProvider, searcher.ServiceProvider);
        Assert.IsNotNull (searcher.Prefixes);
        Assert.AreEqual (4, searcher.Prefixes.Count);
    }

    [TestMethod]
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
    public void TeapotSearcher_BuildSearchExpression_3()
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
}
