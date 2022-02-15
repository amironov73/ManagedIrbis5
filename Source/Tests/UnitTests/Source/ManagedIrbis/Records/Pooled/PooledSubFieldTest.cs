// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Records;

namespace UnitTests.ManagedIrbis.Records;

#nullable enable

[TestClass]
public sealed class PooledSubFieldTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void PooledSubField_Constructor_1()
    {
        var subfield = new PooledSubField();
        Assert.AreEqual (default, subfield.Code);
        Assert.IsNull (subfield.Value);
    }

    [TestMethod]
    [Description ("Инициализация")]
    public void PooledSubField_Init_1()
    {
        var subfield = new PooledSubField();
        subfield.Init ('a', "SubFieldA");
        Assert.AreEqual ('a', subfield.Code);
        Assert.AreEqual ("SubFieldA", subfield.Value);
    }

    [TestMethod]
    [Description ("Деинициализация")]
    public void PooledSubField_Dispose_1()
    {
        var subfield = new PooledSubField
        {
            Code = 'a',
            Value = "SubFieldA"
        };
        subfield.Dispose();
        Assert.AreEqual (default, subfield.Code);
        Assert.IsNull (subfield.Value);
    }
}
