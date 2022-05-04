// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#nullable enable

using System.IO;

using ManagedIrbis;
using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTests.ManagedIrbis.Direct;

[TestClass]
public class MappedXrfFile64Test
    : Common.CommonUnitTest
{
    private string _GetFileName()
    {
        return Path.Combine
            (
                Irbis64RootPath,
                "Datai/IBIS/ibis.xrf"
            );
    }

    [TestMethod]
    [Description ("Удачное чтение записи")]
    public void MappedXrfFile64_ReadRecord_1()
    {
        var fileName = _GetFileName();
        using var file = new MappedXrfFile64 (fileName);
        Assert.AreSame (fileName, file.FileName);
        var record = file.ReadRecord (1);
        Assert.AreEqual (22951100L, record.Offset);
        Assert.AreEqual (0, (int)record.Status);
    }

    [TestMethod]
    [Description ("Неудачное чтение записи")]
    [ExpectedException (typeof (IrbisException))]
    public void MappedXrfFile64_ReadRecord_2()
    {
        const int mfn = 1000001;
        var fileName = _GetFileName();
        using var file = new MappedXrfFile64 (fileName);
        Assert.AreSame (fileName, file.FileName);
        var record = file.ReadRecord (mfn);
        Assert.AreEqual (0L, record.Offset);
        Assert.AreEqual (0, (int) record.Status);
    }
}
