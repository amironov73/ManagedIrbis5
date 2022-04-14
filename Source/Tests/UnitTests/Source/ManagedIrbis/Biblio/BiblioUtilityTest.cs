// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio;

[TestClass]
public sealed class BiblioUtilityTest
{
    [TestMethod]
    [Description ("Добавление точки в конце текста")]
    public void BiblioUtility_AddTrailingDot_1()
    {
        Assert.AreEqual (string.Empty, BiblioUtility.AddTrailingDot (string.Empty));
        Assert.AreEqual ("У попа была собака.", BiblioUtility.AddTrailingDot ("У попа была собака"));
        Assert.AreEqual ("У попа была собака.", BiblioUtility.AddTrailingDot ("У попа была собака."));
        Assert.AreEqual ("У попа была собака!", BiblioUtility.AddTrailingDot ("У попа была собака!"));
        Assert.AreEqual ("У попа была собака?", BiblioUtility.AddTrailingDot ("У попа была собака?"));
    }

}