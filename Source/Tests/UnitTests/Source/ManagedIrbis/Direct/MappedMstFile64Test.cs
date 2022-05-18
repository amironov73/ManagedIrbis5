// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System.IO;

using ManagedIrbis.Direct;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct;

[TestClass]
public sealed class MappedMstFile64Test
    : Common.CommonUnitTest
{
    private string _GetFileName()
    {
        return Path.Combine
            (
                Irbis64RootPath,
                "Datai/IBIS/ibis.mst"
            );
    }

    [TestMethod]
    [Description ("Чтение записи")]
    public void MappedMstFile64_ReadRecord_1()
    {
        var fileName = _GetFileName();

        using var file = new MappedMstFile64 (fileName);
        Assert.AreSame (fileName, file.FileName);
        Assert.AreEqual (333, file.ControlRecord.NextMfn);

        var record = file.ReadRecord (22951100L);
        Assert.AreEqual (100, record.Dictionary.Count);

        var expected = "Tag: 200, Position: 2652, Length: 173, Text: ^AКуда пойти учиться?^EИнформ. - реклам. справ^FЗ. М. Акулова, А. М. Бабич ; ред. А. С. Павловский [и др.]";
        Assert.AreEqual
            (
                expected,
                record.Dictionary[87].ToString()
            );
    }
}
