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
public sealed class SpecialSettingsTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void SpecialSettings_Construction_1()
    {
        var settings = new SpecialSettings();
        Assert.IsNotNull (settings.Dictionary);
        Assert.IsNull (settings.Name);
    }

}
