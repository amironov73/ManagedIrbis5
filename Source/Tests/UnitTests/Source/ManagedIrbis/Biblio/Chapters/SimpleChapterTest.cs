// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio.Chapters;

[TestClass]
public sealed class SimpleChapterTest
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void SimpleChapter_Construction_1()
    {
        var chapter = new SimpleChapter();
        Assert.IsTrue (chapter.Active);
        Assert.IsNotNull (chapter.Attributes);
        Assert.AreEqual (0, chapter.Attributes.Count);
        Assert.IsFalse (chapter.IsServiceChapter);
        Assert.IsNotNull (chapter.Settings);
        Assert.IsNull (chapter.Parent);
        Assert.IsNotNull (chapter.Children);
        Assert.AreEqual (0, chapter.Children.Count);
        Assert.IsNull (chapter.SearchExpression);
        Assert.IsNull (chapter.Format);
        Assert.IsNull (chapter.Order);
    }
}