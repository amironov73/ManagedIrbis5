// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio.Chapters;

[TestClass]
public sealed class ChapterWithTextTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ChapterWithText_Construction_1()
    {
        var chapter = new ChapterWithText();
        Assert.IsTrue (chapter.IsActive);
        Assert.IsNotNull (chapter.Attributes);
        Assert.AreEqual (0, chapter.Attributes.Count);
        Assert.IsTrue (chapter.IsServiceChapter);
        Assert.IsNotNull (chapter.Settings);
        Assert.IsNull (chapter.Parent);
        Assert.IsNotNull (chapter.Children);
        Assert.AreEqual (0, chapter.Children.Count);
        Assert.IsNull (chapter.Text);
    }
}