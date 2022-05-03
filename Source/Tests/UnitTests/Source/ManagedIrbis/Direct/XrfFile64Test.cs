// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using AM;

using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct;

[TestClass]
public class XrfFile64Test
    : Common.CommonUnitTest
{
    private string _GetFileName()
    {
        return Unix.FindFileOrThrow
            (
                DirectUtility.CombinePath
                    (
                        Irbis64RootPath,
                        "Datai/IBIS/ibis.xrf"
                    )
            );
    }

    [TestMethod]
    [Description ("Удачное чтение записи")]
    [Ignore]
    public void XrfFile64_ReadRecord_1()
    {
        var fileName = _GetFileName();
        var mode = DirectAccessMode.ReadOnly;
        using var file = new XrfFile64 (fileName, mode);
        Assert.AreEqual (mode, file.Mode);
        Assert.AreEqual (fileName, file.FileName);

        var record = file.ReadRecord (1);
        Assert.AreEqual (22951100L, record.Offset);
        Assert.AreEqual (0, (int)record.Status);
    }

    [TestMethod]
    [Description ("Неудачное чтение записи")]
    [Ignore]
    [ExpectedException (typeof (ArgumentOutOfRangeException))]
    public void XrfFile64_ReadRecord_2()
    {
        var fileName = _GetFileName();
        var mode = DirectAccessMode.ReadOnly;
        using var file = new XrfFile64 (fileName, mode);
        file.ReadRecord (1111111);
    }

    [TestMethod]
    [Description ("Блокировка записи")]
    [Ignore]
    public void XrfFile64_LockRecord_1()
    {
        var fileName = _GetFileName();
        var mode = DirectAccessMode.Exclusive;
        using var file = new XrfFile64 (fileName, mode);
        Assert.AreEqual (mode, file.Mode);
        Assert.AreEqual (fileName, file.FileName);

        file.LockRecord (1, true);
        var record = file.ReadRecord (1);
        Assert.AreEqual (64, (int)record.Status);

        file.LockRecord (1, false);
        record = file.ReadRecord (1);
        Assert.AreEqual (0, (int)record.Status);
    }

    [TestMethod]
    [Description ("Переоткрытие файла")]
    [Ignore]
    public void XrfFile64_ReopenFile_1()
    {
        var fileName = _GetFileName();
        var mode = DirectAccessMode.Exclusive;
        using var file = new XrfFile64 (fileName, mode);

        mode = DirectAccessMode.ReadOnly;
        file.ReopenFile (mode);
        Assert.AreEqual (mode, file.Mode);
    }
}
