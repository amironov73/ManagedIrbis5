using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM;

// ReSharper disable CheckNamespace

namespace UnitTests.AM;

[TestClass]
public sealed class CaseInsensitiveExpandoObjectTest
{
    [TestMethod]
    public void CaseInsensitiveExpandoObject_TryGetValue_1()
    {
        var expando = (dynamic) new CaseInsensitiveExpandoObject();

        expando.key = "value";
        Assert.AreEqual ("value", expando.key);
        Assert.AreEqual ("value", expando.Key);
    }

    [TestMethod]
    public void CaseInsensitiveExpandoObject_TryGetValue_2()
    {
        var expando = (dynamic) new CaseInsensitiveExpandoObject();

        expando["key"] = "value";
        Assert.AreEqual ("value", expando.key);
        Assert.AreEqual ("value", expando.Key);
    }

    [TestMethod]
    public void CaseInsensitiveExpandoObject_TryGetIndex_1()
    {
        var expando = (dynamic) new CaseInsensitiveExpandoObject();

        expando["key"] = "value";
        Assert.AreEqual ("value", expando["key"]);
        Assert.AreEqual ("value", expando["Key"]);
    }

    [TestMethod]
    public void CaseInsensitiveExpandoObject_TryGetIndex_2()
    {
        var expando = (dynamic) new CaseInsensitiveExpandoObject();

        expando.key = "value";
        Assert.AreEqual ("value", expando["key"]);
        Assert.AreEqual ("value", expando["Key"]);
    }
}
