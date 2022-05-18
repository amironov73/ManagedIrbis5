// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

#nullable enable

using ManagedIrbis;
using ManagedIrbis.Formatting;

using Microsoft.VisualStudio.TestTools.UnitTesting;
namespace UnitTests.ManagedIrbis.Formatting;

[TestClass]
public sealed class HardFormatTest
{
    [TestMethod]
    [Description ("Рабочий лист")]
    public void HardFormat_Worksheet_1()
    {
        var record = new Record();
        var format = new HardFormat();

        Assert.IsNull (format.Worksheet (record));
    }
}
