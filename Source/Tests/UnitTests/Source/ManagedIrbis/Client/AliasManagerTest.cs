// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Client;

#endregion

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
