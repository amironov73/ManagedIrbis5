// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio.Chapters;

[TestClass]
public sealed class MenuSubChapterTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void MenuSubChapter_Construction_1()
    {
        var chapter = new MenuSubChapter();
        Assert.IsTrue (chapter.IsActive);
        Assert.IsNotNull (chapter.Attributes);
        Assert.AreEqual (0, chapter.Attributes.Count);
        Assert.IsFalse (chapter.IsServiceChapter);
        Assert.IsNotNull (chapter.Settings);
        Assert.IsNull (chapter.Parent);
        Assert.IsNotNull (chapter.Children);
        Assert.AreEqual (0, chapter.Children.Count);
        Assert.IsNotNull (chapter.Records);
        Assert.AreEqual (0, chapter.Records.Count);
        Assert.IsNull (chapter.Key);
        Assert.IsNull (chapter.MainChapter);
        Assert.IsNull (chapter.Value);
    }
}
