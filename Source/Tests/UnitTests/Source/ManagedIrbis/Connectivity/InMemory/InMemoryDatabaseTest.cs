// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis;
using ManagedIrbis.InMemory;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Connectivity.InMemory;

[TestClass]
public sealed class InMemoryDatabaseTest
{
    private InMemoryDatabase _GetDatabase()
    {
        return new InMemoryDatabase ("IBIS");
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void InMemoryDatabase_Construction_1()
    {
        const string name = "IBIS";
        var database = new InMemoryDatabase (name);
        Assert.AreEqual (name, database.Name);
        Assert.IsNotNull (database.Master);
        Assert.IsNotNull (database.Inverted);
        Assert.IsFalse (database.ReadOnly);
    }

    [TestMethod]
    [Description ("Дамп базы данных")]
    public void InMemoryDatabase_Dump_1()
    {
        var database = _GetDatabase();
        var output = new StringWriter();
        database.Dump (output);
        var dump = output.ToString();
        Assert.IsNotNull (dump);
    }

    [TestMethod]
    [Description ("Чтение записи")]
    public void InMemoryDatabase_ReadRecord_1()
    {
        var database = _GetDatabase();
        var record = database.ReadRecord (1);
        Assert.IsNull (record);
    }

    [TestMethod]
    [Description ("Сохранение записи")]
    public void InMemoryDatabase_WriteRecord_1()
    {
        var database = _GetDatabase();
        var record = new Record();
        var result = database.WriteRecord (record);
        Assert.IsTrue (result);
    }

    [TestMethod]
    [Description ("Блокировка")]
    public void InMemoryDatabase_Locked_1()
    {
        var database = _GetDatabase();
        Assert.IsFalse (database.Locked);
        database.Locked = true;
        Assert.IsTrue (database.Locked);
    }
}