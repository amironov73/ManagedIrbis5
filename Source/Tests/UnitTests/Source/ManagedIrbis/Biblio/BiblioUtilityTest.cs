// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

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
