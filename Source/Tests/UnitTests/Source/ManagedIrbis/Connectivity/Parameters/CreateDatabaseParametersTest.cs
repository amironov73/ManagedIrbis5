// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.Parameters;

[TestClass]
public sealed class CreateDatabaseParametersTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void CreateDatabaseParameters_Construction_1()
    {
        var parameters = new CreateDatabaseParameters();
        Assert.IsNull (parameters.Database);
        Assert.IsNull (parameters.Description);
        Assert.IsFalse (parameters.ReaderAccess);
        Assert.IsNull (parameters.Template);
    }

    [TestMethod]
    [Description ("Присвоение")]
    public void CreateDatabaseParameters_Construction_2()
    {
        var parameters = new CreateDatabaseParameters()
        {
            Database = "IBIS",
            Description = "Тестовая база",
            ReaderAccess = true,
            Template = "NO"
        };
        Assert.AreEqual ("IBIS", parameters.Database);
        Assert.AreEqual ("Тестовая база", parameters.Description);
        Assert.IsTrue (parameters.ReaderAccess);
        Assert.AreEqual ("NO", parameters.Template);
    }

}