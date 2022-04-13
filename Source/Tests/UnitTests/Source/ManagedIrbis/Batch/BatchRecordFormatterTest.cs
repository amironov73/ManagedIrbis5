// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.Linq;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Batch;

[TestClass]
public sealed class BatchRecordFormatterTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void BatchRecordFormatter_Construction_1()
    {
        const int batchSize = 1000;
        var connection = new NullProvider();
        var formatter = new BatchRecordFormatter
            (
                connection,
                Constants.Ibis,
                IrbisFormat.Brief,
                batchSize,
                Array.Empty<int>()
            );
        Assert.AreSame (connection, formatter.Connection);
        Assert.AreEqual (batchSize, formatter.BatchSize);
        Assert.AreEqual (Constants.Ibis, formatter.Database);
        Assert.AreEqual (IrbisFormat.Brief, formatter.Format);
        Assert.AreEqual (0, formatter.RecordsFormatted);
        Assert.AreEqual (0, formatter.TotalRecords);
    }

    [TestMethod]
    [Description ("Конструктор")]
    [ExpectedException (typeof (ArgumentException))]
    public void BatchRecordFormatter_Construction_2()
    {
        const int batchSize = 1000;
        var formatter = new BatchRecordFormatter
            (
                "none",
                Constants.Ibis,
                IrbisFormat.Brief,
                batchSize,
                Array.Empty<int>()
            );
        Assert.AreEqual (batchSize, formatter.BatchSize);
        Assert.AreEqual (Constants.Ibis, formatter.Database);
        Assert.AreEqual (IrbisFormat.Brief, formatter.Format);
        Assert.AreEqual (0, formatter.RecordsFormatted);
        Assert.AreEqual (0, formatter.TotalRecords);
    }

    [TestMethod]
    [Description ("Форматирование интервала записей")]
    public void BatchRecordFormatter_Interval_1()
    {
        var connection = new NullProvider();
        var formatted = BatchRecordFormatter.Interval
                (
                    connection,
                    Constants.Ibis,
                    IrbisFormat.Brief,
                    1,
                    100,
                    50
                )
            .ToArray();
        Assert.IsNotNull (formatted);
        Assert.AreEqual (0, formatted.Length);
    }

    [TestMethod]
    [Description ("Форматирование всех записей")]
    public void BatchRecordFormatter_FormatAll_1()
    {
        var connection = new NullProvider();
        var formatter = new BatchRecordFormatter
            (
                connection,
                Constants.Ibis,
                IrbisFormat.Brief,
                100,
                Array.Empty<int>()
            );
        var formatted = formatter.FormatAll();
        Assert.IsNotNull (formatted);
        Assert.AreEqual (0, formatted.Count);
    }

    [TestMethod]
    [Description ("Форматирование найденных записей")]
    public void BatchRecordFormatter_Search_1()
    {
        var connection = new NullProvider();
        var formatted = BatchRecordFormatter.Search
                (
                    connection,
                    Constants.Ibis,
                    IrbisFormat.Brief,
                    "no such record",
                    100
                )
            .ToArray();
        Assert.IsNotNull (formatted);
        Assert.AreEqual (0, formatted.Length);
    }

    [TestMethod]
    [Description ("Форматирование всех записей")]
    [ExpectedException (typeof (ArgumentOutOfRangeException))]
    public void BatchRecordFormatter_WholeDatabase_1()
    {
        var connection = new NullProvider();
        var formatted = BatchRecordFormatter.WholeDatabase
                (
                    connection,
                    Constants.Ibis,
                    IrbisFormat.Brief,
                    100
                )
            .ToArray();
        Assert.IsNotNull (formatted);
        Assert.AreEqual (0, formatted.Length);
    }

}