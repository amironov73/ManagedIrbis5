// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CollectionNeverQueried.Local
// ReSharper disable CollectionNeverUpdated.Local
// ReSharper disable UseObjectOrCollectionInitializer

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Collections;

#endregion

#nullable enable

namespace UnitTests.AM.Collections;

[TestClass]
public sealed class CloneableCollectionTest
{
    [TestMethod]
    [Description ("Клонирование")]
    public void CloneableCollection_Clone()
    {
        var source = new CloneableCollection<int>
        {
            212,
            85,
            06
        };
        var clone = (CloneableCollection<int>) source.Clone();

        Assert.AreEqual (source.Count, clone.Count);
        for (var i = 0; i < source.Count; i++)
        {
            Assert.AreEqual (source[i], clone[i]);
        }
    }
}
