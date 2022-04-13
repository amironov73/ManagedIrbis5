// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis;
using ManagedIrbis.Batch;
using ManagedIrbis.Providers;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Batch;

[TestClass]
public sealed class BatchRecordWriterTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void BatchRecordWriter_Construction_1()
    {
        const int capacity = 100;
        var connection = new NullProvider();
        using var writer = new BatchRecordWriter
            (
                connection,
                Constants.Ibis,
                capacity
            );
        Assert.AreSame (connection, writer.Connection);
        Assert.AreEqual (Constants.Ibis, writer.Database);
        Assert.AreEqual (capacity, writer.Capacity);
    }

    [TestMethod]
    [Description ("Отправка записей на сервер")]
    public void BatchRecordWriter_Flush_1()
    {
        var connection = new NullProvider();
        using var writer = new BatchRecordWriter
            (
                connection,
                Constants.Ibis,
                100
            );
        writer.Append (new Record());
        writer.AddRange (new[] { new Record(), new Record() });
        writer.Flush();
        Assert.IsNotNull (writer);
    }

}