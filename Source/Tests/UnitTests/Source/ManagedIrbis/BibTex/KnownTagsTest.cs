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
public sealed class KnownTagsTest
{
    [TestMethod]
    [Description ("Получение массива значений констант")]
    public void KnownTags_ListValues_1()
    {
        var values = KnownTags.ListValues();
        Assert.AreEqual (27, values.Length);
    }
}
