// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio.Dictionaries;

[TestClass]
public sealed class TermCollectionTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void TermCollection_Construction_1()
    {
        var collection = new TermCollection();
        Assert.AreEqual (0, collection.Count);
    }
}
