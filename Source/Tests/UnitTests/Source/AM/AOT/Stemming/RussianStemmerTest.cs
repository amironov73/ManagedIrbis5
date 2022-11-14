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
public sealed class RussianStemmerTest
{
    [TestMethod]
    [Description ("Единственное число, нижний регистр")]
    public void RussianStemmer_Stem_1()
    {
        var stemmer = new RussianStemmer();

        // именительный: кто, что
        Assert.AreEqual ("кон", stemmer.Stem ("конь"));
        Assert.AreEqual ("окн", stemmer.Stem ("окно"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошка"));

        // родительный: кого, чего
        Assert.AreEqual ("кон", stemmer.Stem ("коня"));
        Assert.AreEqual ("окн", stemmer.Stem ("окна"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошки"));

        // дательный: кому, чему
        Assert.AreEqual ("кон", stemmer.Stem ("коню"));
        Assert.AreEqual ("окн", stemmer.Stem ("окну"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошке"));

        // винительный: кого, что
        Assert.AreEqual ("кон", stemmer.Stem ("коня"));
        Assert.AreEqual ("окн", stemmer.Stem ("окно"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошку"));

        // творительный: кем, чем
        Assert.AreEqual ("кон", stemmer.Stem ("конем"));
        Assert.AreEqual ("окн", stemmer.Stem ("окном"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошкой"));

        // предложный: о ком, о чем
        Assert.AreEqual ("кон", stemmer.Stem ("коне"));
        Assert.AreEqual ("окн", stemmer.Stem ("окне"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошке"));

        // местный: где
        Assert.AreEqual ("кон", stemmer.Stem ("коне"));
        Assert.AreEqual ("окн", stemmer.Stem ("окне"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошке"));
    }

    [TestMethod]
    [Description ("Единственное число, верхний регистр")]
    public void RussianStemmer_Stem_2()
    {
        var stemmer = new RussianStemmer();

        // именительный: кто, что
        Assert.AreEqual ("кон", stemmer.Stem ("КОНЬ"));
        Assert.AreEqual ("окн", stemmer.Stem ("ОКНО"));
        Assert.AreEqual ("кошк", stemmer.Stem ("КОШКА"));

        // родительный: кого, чего
        Assert.AreEqual ("кон", stemmer.Stem ("КОНЯ"));
        Assert.AreEqual ("окн", stemmer.Stem ("ОКНА"));
        Assert.AreEqual ("кошк", stemmer.Stem ("КОШКИ"));

        // дательный: кому, чему
        Assert.AreEqual ("кон", stemmer.Stem ("КОНЮ"));
        Assert.AreEqual ("окн", stemmer.Stem ("ОКНУ"));
        Assert.AreEqual ("кошк", stemmer.Stem ("КОШКЕ"));

        // винительный: кого, что
        Assert.AreEqual ("кон", stemmer.Stem ("КОНЯ"));
        Assert.AreEqual ("окн", stemmer.Stem ("ОКНО"));
        Assert.AreEqual ("кошк", stemmer.Stem ("КОШКУ"));

        // творительный: кем, чем
        Assert.AreEqual ("кон", stemmer.Stem ("КОНЕМ"));
        Assert.AreEqual ("окн", stemmer.Stem ("ОКНОМ"));
        Assert.AreEqual ("кошк", stemmer.Stem ("КОШКОЙ"));

        // предложный: о ком, о чем
        Assert.AreEqual ("кон", stemmer.Stem ("КОНЕ"));
        Assert.AreEqual ("окн", stemmer.Stem ("ОКНЕ"));
        Assert.AreEqual ("кошк", stemmer.Stem ("КОШКЕ"));

        // местный: где
        Assert.AreEqual ("кон", stemmer.Stem ("КОНЕ"));
        Assert.AreEqual ("окн", stemmer.Stem ("ОКНЕ"));
        Assert.AreEqual ("кошк", stemmer.Stem ("КОШКЕ"));
    }

    [TestMethod]
    [Description ("Множественное число, нижний регистр")]
    public void RussianStemmer_Stem_3()
    {
        var stemmer = new RussianStemmer();

        // именительный: кто, что
        Assert.AreEqual ("кон", stemmer.Stem ("кони"));
        Assert.AreEqual ("окн", stemmer.Stem ("окна"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошки"));

        // родительный: кого, чего
        Assert.AreEqual ("кон", stemmer.Stem ("коней"));
        Assert.AreEqual ("окон", stemmer.Stem ("окон"));
        Assert.AreEqual ("кошек", stemmer.Stem ("кошек"));

        // дательный: кому, чему
        Assert.AreEqual ("кон", stemmer.Stem ("коням"));
        Assert.AreEqual ("окн", stemmer.Stem ("окнам"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошкам"));

        // винительный: кого, что
        Assert.AreEqual ("кон", stemmer.Stem ("коней"));
        Assert.AreEqual ("окон", stemmer.Stem ("окон"));
        Assert.AreEqual ("кошек", stemmer.Stem ("кошек"));

        // творительный: кем, чем
        Assert.AreEqual ("кон", stemmer.Stem ("конями"));
        Assert.AreEqual ("окн", stemmer.Stem ("окнами"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошками"));

        // предложный: о ком, о чем
        Assert.AreEqual ("кон", stemmer.Stem ("конях"));
        Assert.AreEqual ("окн", stemmer.Stem ("окнах"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошках"));

        // местный: где
        Assert.AreEqual ("окн", stemmer.Stem ("окнах"));
        Assert.AreEqual ("окн", stemmer.Stem ("окнах"));
        Assert.AreEqual ("кошк", stemmer.Stem ("кошках"));
    }
}
