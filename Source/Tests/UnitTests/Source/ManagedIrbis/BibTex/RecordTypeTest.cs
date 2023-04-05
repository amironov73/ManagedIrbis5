// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis.BibTex;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.BibTex;

[TestClass]
public sealed class RecordTypeTest
{
    [TestMethod]
    [Description ("Получение массива значений констант")]
    public void RecordType_ListValues_1()
    {
        var values = RecordType.ListValues();
        Assert.AreEqual (14, values.Length);
    }
}
