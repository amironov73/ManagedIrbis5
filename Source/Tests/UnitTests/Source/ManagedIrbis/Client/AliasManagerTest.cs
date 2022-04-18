// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;

#nullable enable

namespace UnitTests.ManagedIrbis.Client;

[TestClass]
public sealed class AliasManagerTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void AliasManager_GetAliasValue_1()
    {
        var manager = new AliasManager();
        Assert.IsNull (manager.GetAliasValue ("hello"));
    }
}
