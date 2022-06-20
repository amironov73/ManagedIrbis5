// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;

using AM.ComponentModel;

using ManagedIrbis;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Gbl;
using ManagedIrbis.Gbl.Infrastructure;
using ManagedIrbis.Gbl.Infrastructure.Ast;
using ManagedIrbis.Processing;
using ManagedIrbis.Providers;

#nullable enable

namespace UnitTests.ManagedIrbis.Gbl;

[TestClass]
public sealed class SyncGblEngineTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public void SyncGblEngine_Construction_1()
    {
        var irbisProvider = new NullProvider();
        var serviceProvider = new ServiceAggregator();
        using var engine = new SyncGblEngine (irbisProvider, serviceProvider);
        Assert.AreSame (irbisProvider, engine.IrbisProvider);
        Assert.AreSame (serviceProvider, engine.ServiceProvider);
    }

    [TestMethod]
    [Description ("Создание контекста")]
    public void SyncGblEngine_CreateContext_1()
    {
        var irbisProvider = new NullProvider();
        var serviceProvider = new ServiceAggregator();
        using var engine = new SyncGblEngine (irbisProvider, serviceProvider);

        var recordSource = new NullRecordSource();
        var recordSink = new NullRecordSink();
        var context = engine.CreateContext (recordSource, recordSink);
        Assert.AreSame (recordSource, context.SyncRecordSource);
        Assert.AreSame (recordSink, context.SyncRecordSink);
    }

    [TestMethod]
    [Description ("Обработка записей")]
    public void SyncGblEngine_CorrectRecords_1()
    {
        var irbisProvider = new NullProvider();
        var serviceProvider = new ServiceAggregator();
        using var engine = new SyncGblEngine (irbisProvider, serviceProvider);

        var recordSource = new ListRecordSource (new[]
        {
            new Record() { Mfn = 1 },
            new Record() { Mfn = 2 }
        });
        var recordSink = new NullRecordSink();
        var context = engine.CreateContext (recordSource, recordSink);

        var program = new []
        {
            new GblNop()
        };
        var result = engine.CorrectRecords (context, program);
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
        Assert.IsNull (result.Exception);
        Assert.AreEqual (2, result.RecordsSupposed);
        Assert.AreEqual (2, result.RecordsProcessed);
        Assert.AreEqual (2, result.RecordsSucceeded);
        Assert.AreEqual (0, result.RecordsFailed);
    }

}
