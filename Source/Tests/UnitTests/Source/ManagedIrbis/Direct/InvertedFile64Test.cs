// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;
using System.IO;

using AM;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis.Direct;

#nullable enable

namespace UnitTests.ManagedIrbis.Direct;

[TestClass]
public class InvertedFile64Test
    : Common.CommonUnitTest
{
    private string _GetFileName()
    {
        return Path.Combine
            (
                Irbis64RootPath,
                "Datai/IBIS/ibis.ifp"
            );
    }

    private string _CreateDatabase()
    {
        var random = new Random();
        var directory = Path.Combine
            (
                Path.GetTempPath(),
                random.Next().ToInvariantString()
            );
        Directory.CreateDirectory (directory);
        var path = Path.Combine (directory, "database");
        DirectUtility.CreateDatabase64 (path);
        var result = path + ".mst";

        return result;
    }

    [TestMethod]
    [Description ("Конструктор")]
    public void InvertedFile64_Construction_1()
    {
        var fileName = _GetFileName();
        using var inverted = new InvertedFile64 (fileName, DirectAccessMode.ReadOnly);
        Assert.IsFalse (inverted.Fragmented);
        Assert.AreSame (fileName, inverted.FileName);
        Assert.AreEqual (DirectAccessMode.ReadOnly, inverted.Mode);
        Assert.IsNotNull (inverted.IfpControlRecord);
        Assert.IsNotNull (inverted.Ifp);
        Assert.IsNotNull (inverted.L01);
        Assert.IsNotNull (inverted.N01);
        Assert.IsNull (inverted.AdditionalControlRecord);
        Assert.IsNull (inverted.AdditionalIfp);
        Assert.IsNull (inverted.AdditionalL01);
        Assert.IsNull (inverted.AdditionalN01);
    }
}
