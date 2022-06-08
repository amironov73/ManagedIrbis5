// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct;

[TestClass]
public class XrfRecord64Test
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void XrfRecord64_Construction_1()
    {
        var record = new XrfRecord64();
        Assert.AreEqual (0L, record.Offset);
        Assert.AreEqual (0, (int)record.Status);
        Assert.IsFalse (record.IsDeleted);
        Assert.IsFalse (record.IsLocked);
    }

    [TestMethod]
    [Description ("Присваивание свойств")]
    public void XrfRecord64_Properties_1()
    {
        var record = new XrfRecord64();
        record.Offset = 23456;
        Assert.AreEqual (23456L, record.Offset);
        record.Status = RecordStatus.LogicallyDeleted;
        Assert.AreEqual (RecordStatus.LogicallyDeleted, record.Status);
        Assert.IsTrue (record.IsDeleted);
        Assert.IsFalse (record.IsLocked);
        record.Status = RecordStatus.Locked;
        Assert.IsTrue (record.IsLocked);
        Assert.IsFalse (record.IsDeleted);
    }

    [TestMethod]
    [Description ("Признак блокировки записи")]
    public void XrfRecord64_Locked_1()
    {
        var record = new XrfRecord64();
        record.Status = RecordStatus.Last;
        Assert.IsFalse (record.IsLocked);
        record.IsLocked = true;
        Assert.IsTrue (record.IsLocked);
        Assert.AreEqual (RecordStatus.Last | RecordStatus.Locked, record.Status);
        record.IsLocked = false;
        Assert.IsFalse (record.IsLocked);
        Assert.AreEqual (RecordStatus.Last, record.Status);
    }

    [TestMethod]
    [Description ("Текстовое представление")]
    public void XrfRecord64_ToString_1()
    {
        var record = new XrfRecord64();
        Assert.AreEqual ("Offset: 0, Status: None", record.ToString());

        record = new XrfRecord64
        {
            Offset = 2345,
            Status = RecordStatus.Last
        };
        Assert.AreEqual ("Offset: 2345, Status: Last", record.ToString());
    }
}
