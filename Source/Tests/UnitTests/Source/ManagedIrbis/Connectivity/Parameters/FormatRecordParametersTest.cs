// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis;
using ManagedIrbis.Infrastructure;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.Parameters;

[TestClass]
public sealed class FormatRecordParametersTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void FormatRecordParameters_Construction_1()
    {
        var parameters = new FormatRecordParameters();
        Assert.IsNull (parameters.Database);
        Assert.IsNull (parameters.Format);
        Assert.AreEqual (0, parameters.Mfn);
        Assert.IsNull (parameters.Mfns);
        Assert.IsNull (parameters.Record);
        Assert.IsNull (parameters.Records);
    }

    [TestMethod]
    [Description ("Присвоение")]
    public void FormatRecordParameters_Construction_2()
    {
        var parameters = new FormatRecordParameters
        {
            Result = new [] { "some", "results" },
            Database = "IBIS",
            Format = "@brief",
            Mfn = 123,
            Mfns = new [] { 1, 2, 3 },
            Record = new Record(),
            Records = new [] { new Record(), new Record() }
        };
        Assert.AreEqual (2, parameters.Result.Count);
        Assert.AreEqual ("IBIS", parameters.Database);
        Assert.AreEqual (123, parameters.Mfn);
        Assert.AreEqual (3, parameters.Mfns.Length);
        Assert.AreEqual (0, parameters.Record.Mfn);
        Assert.AreEqual (2, parameters.Records.Length);
    }
}