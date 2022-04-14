// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using AM.Text.Output;

using ManagedIrbis.Biblio;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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