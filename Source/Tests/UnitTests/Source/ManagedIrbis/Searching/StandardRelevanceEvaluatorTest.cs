// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using AM.ComponentModel;

using ManagedIrbis;
using ManagedIrbis.Searching;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Search;

[TestClass]
public class StandardRelevanceEvaluatorTest
{
    [TestMethod]
    public void StandardRelevanceEvaluator_Construction_1()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE";

        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        Assert.AreSame (serviceProvider, evaluator.ServiceProvider);
        Assert.IsNotNull (evaluator.Coefficients);
        Assert.AreEqual (searchExpression, evaluator.SearchExpression);
        Assert.IsNotNull (evaluator.Terms);
        Assert.AreEqual (1, evaluator.Terms.Count);
        Assert.AreEqual ("CONCRETE", evaluator.Terms[0]);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_Construction_2()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE$";

        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        Assert.AreSame (serviceProvider, evaluator.ServiceProvider);
        Assert.IsNotNull (evaluator.Coefficients);
        Assert.AreEqual (searchExpression, evaluator.SearchExpression);
        Assert.IsNotNull (evaluator.Terms);
        Assert.AreEqual (1, evaluator.Terms.Count);
        Assert.AreEqual ("CONCRETE", evaluator.Terms[0]);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_Construction_3()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        Assert.AreSame (serviceProvider, evaluator.ServiceProvider);
        Assert.IsNotNull (evaluator.Coefficients);
        Assert.AreEqual (searchExpression, evaluator.SearchExpression);
        Assert.IsNotNull (evaluator.Terms);
        Assert.AreEqual (2, evaluator.Terms.Count);
        Assert.AreEqual ("CONCRETE", evaluator.Terms[0]);
        Assert.AreEqual ("BYRON", evaluator.Terms[1]);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_1()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record();
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (0.0, result);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_2()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record
        {
            { 100, 'a', "Concrete building" }
        };
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (1.33, result, 0.01);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_3()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record
        {
            { 100, 'a', "Concrete building" },
            { 110, 'b', "Byron poems" }
        };
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (2.66, result, 0.01);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_4()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record
        {
            { 100, 'a', "Concrete" },
            { 110, 'b', "Byron" }
        };
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (4.0, result);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_5()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record
        {
            { 200, 'a', "Concrete building" },
            { 700, 'b', "Byron" }
        };
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (36.66, result, 0.01);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_6()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record
        {
            { 100, 'a', "Building with concrete" }
        };
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (1.0, result);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_7()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record
        {
            { 100, 'a', "Building with concrete" },
            { 110, 'b', "Byron poems" }
        };
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (2.33, result, 0.01);
    }

    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_8()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record
        {
            { 200, 'a', "Building with concrete" },
            { 700, 'b', "Byron" }
        };
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (33.0, result);
    }


    [TestMethod]
    public void StandardRelevanceEvaluator_EvaluateRelevance_9()
    {
        var serviceProvider = ServiceProviderUtility.CreateNullProvider();
        const string searchExpression = "K=CONCRETE * A=BYRON";

        var record = new Record
        {
            { 200, 'a', "Concrete" },
            { 201, 'c' }, // пустое подполе для проверки, обрабатываются ли они
            { 700, 'b', "Byron" }
        };
        var evaluator = new StandardRelevanceEvaluator
            (
                serviceProvider,
                searchExpression
            );

        var result = evaluator.EvaluateRelevance (record);
        Assert.AreEqual (44.0, result);
    }
}
