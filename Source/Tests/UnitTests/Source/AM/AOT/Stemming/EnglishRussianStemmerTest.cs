// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.AOT.Stemming;

#endregion

#nullable enable

namespace UnitTests.AM.AOT.Stemming;

[TestClass]
public sealed class EnglishRussianStemmerTest
{
    [TestMethod]
    public void EnglishRussianStemmer_Stem_1()
    {
        var stemmer = new EnglishRussianStemmer();

        Assert.AreEqual ("consign", stemmer.Stem ("consign"));
        Assert.AreEqual ("consign", stemmer.Stem ("consigned"));
        Assert.AreEqual ("consign", stemmer.Stem ("consigning"));
        Assert.AreEqual ("consign", stemmer.Stem ("consignment"));

        Assert.AreEqual ("consist", stemmer.Stem ("consist"));
        Assert.AreEqual ("consist", stemmer.Stem ("consisted"));
        Assert.AreEqual ("consist", stemmer.Stem ("consistency"));
        Assert.AreEqual ("consist", stemmer.Stem ("consistent"));
        Assert.AreEqual ("consist", stemmer.Stem ("consistently"));
        Assert.AreEqual ("consist", stemmer.Stem ("consisting"));
        Assert.AreEqual ("consist", stemmer.Stem ("consists"));

        Assert.AreEqual ("кон", stemmer.Stem ("конь"));
        Assert.AreEqual ("окн", stemmer.Stem ("окно"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошка"));

        Assert.AreEqual ("кон", stemmer.Stem ("коня"));
        Assert.AreEqual ("окн", stemmer.Stem ("окна"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошки"));

        Assert.AreEqual ("кон", stemmer.Stem ("коню"));
        Assert.AreEqual ("окн", stemmer.Stem ("окну"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошке"));
    }
}
