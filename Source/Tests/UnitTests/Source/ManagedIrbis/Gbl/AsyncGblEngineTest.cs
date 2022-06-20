// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System;
using System.Threading.Tasks;

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
public sealed class AsyncGblEngineTest
{
    [TestMethod]
    [Description ("Конструктор")]
    public async Task AsyncGblEngine_Construction_1()
    {
        var irbisProvider = new NullProvider();
        var serviceProvider = new ServiceAggregator();
        await using var engine = new AsyncGblEngine (irbisProvider, serviceProvider);
        Assert.AreSame (irbisProvider, engine.IrbisProvider);
        Assert.AreSame (serviceProvider, engine.ServiceProvider);
    }

    [TestMethod]
    [Description ("Создание контекста")]
    public async Task AsyncGblEngine_CreateContext_1()
    {
        var irbisProvider = new NullProvider();
        var serviceProvider = new ServiceAggregator();
        await using var engine = new AsyncGblEngine (irbisProvider, serviceProvider);

        var recordSource = new NullRecordSource();
        var recordSink = new NullRecordSink();
        var context = engine.CreateContext (recordSource, recordSink);
        Assert.AreSame (recordSource, context.AsyncRecordSource);
        Assert.AreSame (recordSink, context.AsyncRecordSink);
    }

    [TestMethod]
    [Description ("Обработка записей")]
    public async Task AsyncGblEngine_CorrectRecordsAsync_1()
    {
        var irbisProvider = new NullProvider();
        var serviceProvider = new ServiceAggregator();
        await using var engine = new AsyncGblEngine (irbisProvider, serviceProvider);

        var recordSource = new ListRecordSource (new []
        {
            new Record() { Mfn = 1 },
            new Record() { Mfn = 2 }
        });
        var recordSink = new NullRecordSink();
        var context = engine.CreateContext (recordSource, recordSink);

        var program = new[]
        {
            new GblNop(),
            new GblNop()
        };
        var result = await engine.CorrectRecordsAsync (context, program);
        Assert.IsNotNull (result);
        Assert.IsFalse (result.Canceled);
        Assert.IsNull (result.Exception);
        Assert.AreEqual (2, result.RecordsSupposed);
        Assert.AreEqual (2, result.RecordsProcessed);
        Assert.AreEqual (2, result.RecordsSucceeded);
        Assert.AreEqual (0, result.RecordsFailed);
    }
}
