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
public sealed class CloneableDictionaryTest
{
    [TestMethod]
    [Description ("Клонирование")]
    public void CloneableDictionary_Clone()
    {
        var source = new CloneableDictionary<int, string>
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" }
        };

        var clone = (CloneableDictionary<int, string>) source.Clone();

        Assert.AreEqual (source.Count, clone.Count);
        var keys = source.Keys;
        foreach (var key in keys)
        {
            Assert.AreEqual (source[key], clone[key]);
        }
    }
}
