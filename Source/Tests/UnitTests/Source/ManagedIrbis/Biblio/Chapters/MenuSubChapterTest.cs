// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

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