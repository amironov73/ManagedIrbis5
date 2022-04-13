// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using ManagedIrbis.Biblio;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Biblio.Chapters;

[TestClass]
public sealed class ChaperWithDictionary
{
    [TestMethod]
    [Description ("Конструктор по умолчанию")]
    public void ChapterWithDictionary_Construction_1()
    {
        var chapter = new ChapterWithDictionary();
        Assert.IsTrue (chapter.Active);
        Assert.IsNotNull (chapter.Attributes);
        Assert.AreEqual (0, chapter.Attributes.Count);
        Assert.IsTrue (chapter.IsServiceChapter);
        Assert.IsNotNull (chapter.Settings);
        Assert.IsNull (chapter.Parent);
        Assert.IsNotNull (chapter.Children);
        Assert.AreEqual (0, chapter.Children.Count);
        Assert.IsNotNull (chapter.Dictionary);
        Assert.AreEqual (0, chapter.Dictionary.Count);
        Assert.IsNotNull (chapter.Terms);
        Assert.AreEqual (0, chapter.Terms.Count);
        Assert.IsNotNull (chapter.ExcludeList);
        Assert.AreEqual (0, chapter.ExcludeList.Count);
        Assert.IsNull (chapter.SelectClause);
        Assert.IsNull (chapter.OrderByClause);
        Assert.IsNull (chapter.ExtendedFormat);
    }

}