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
public sealed class EnglishStemmerTest
{
    [TestMethod]
    [Description ("Единственное число, нижний регистр")]
    public void EnglishStemmer_Stem_1()
    {
        var stemmer = new EnglishStemmer();

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

        Assert.AreEqual ("consol", stemmer.Stem ("consolation"));
        Assert.AreEqual ("consol", stemmer.Stem ("consolations"));
        Assert.AreEqual ("consolatori", stemmer.Stem ("consolatory"));
        Assert.AreEqual ("consol", stemmer.Stem ("console"));
        Assert.AreEqual ("consol", stemmer.Stem ("consoled"));
        Assert.AreEqual ("consol", stemmer.Stem ("consoles"));
        Assert.AreEqual ("consolid", stemmer.Stem ("consolidate"));
        Assert.AreEqual ("consolid", stemmer.Stem ("consolidated"));
        Assert.AreEqual ("consolid", stemmer.Stem ("consolidating"));
        Assert.AreEqual ("consol", stemmer.Stem ("consoling"));
        Assert.AreEqual ("consol", stemmer.Stem ("consolingly"));
        Assert.AreEqual ("consol", stemmer.Stem ("consols"));

        Assert.AreEqual ("conson", stemmer.Stem ("consonant"));

        Assert.AreEqual ("consort", stemmer.Stem ("consort"));
        Assert.AreEqual ("consort", stemmer.Stem ("consorted"));
        Assert.AreEqual ("consort", stemmer.Stem ("consorting"));

        Assert.AreEqual ("conspicu", stemmer.Stem ("conspicuous"));
        Assert.AreEqual ("conspicu", stemmer.Stem ("conspicuously"));

        Assert.AreEqual ("conspiraci", stemmer.Stem ("conspiracy"));
        Assert.AreEqual ("conspir", stemmer.Stem ("conspirator"));
        Assert.AreEqual ("conspir", stemmer.Stem ("conspirators"));
        Assert.AreEqual ("conspir", stemmer.Stem ("conspire"));
        Assert.AreEqual ("conspir", stemmer.Stem ("conspired"));
        Assert.AreEqual ("conspir", stemmer.Stem ("conspiring"));

        Assert.AreEqual ("constabl", stemmer.Stem ("constable"));
        Assert.AreEqual ("constabl", stemmer.Stem ("constables"));
        Assert.AreEqual ("constanc", stemmer.Stem ("constance"));
        Assert.AreEqual ("constanc", stemmer.Stem ("constancy"));

        Assert.AreEqual ("constant", stemmer.Stem ("constant"));

        Assert.AreEqual ("knack", stemmer.Stem ("knack"));
        Assert.AreEqual ("knackeri", stemmer.Stem ("knackeries"));
        Assert.AreEqual ("knack", stemmer.Stem ("knacks"));

        Assert.AreEqual ("knag", stemmer.Stem ("knag"));

        Assert.AreEqual ("knave", stemmer.Stem ("knave"));
        Assert.AreEqual ("knave", stemmer.Stem ("knaves"));
        Assert.AreEqual ("knavish", stemmer.Stem ("knavish"));

        Assert.AreEqual ("knead", stemmer.Stem ("kneaded"));
        Assert.AreEqual ("knead", stemmer.Stem ("kneading"));

        Assert.AreEqual ("knee", stemmer.Stem ("knee"));
        Assert.AreEqual ("kneel", stemmer.Stem ("kneel"));
        Assert.AreEqual ("kneel", stemmer.Stem ("kneeled"));
        Assert.AreEqual ("kneel", stemmer.Stem ("kneeling"));
        Assert.AreEqual ("kneel", stemmer.Stem ("kneels"));
        Assert.AreEqual ("knee", stemmer.Stem ("knees"));

        Assert.AreEqual ("knell", stemmer.Stem ("knell"));

        Assert.AreEqual ("knelt", stemmer.Stem ("knelt"));

        Assert.AreEqual ("knew", stemmer.Stem ("knew"));

        Assert.AreEqual ("knick", stemmer.Stem ("knick"));

        Assert.AreEqual ("knif", stemmer.Stem ("knif"));

        Assert.AreEqual ("knife", stemmer.Stem ("knife"));

        Assert.AreEqual ("knight", stemmer.Stem ("knight"));
        Assert.AreEqual ("knight", stemmer.Stem ("knightly"));
        Assert.AreEqual ("knight", stemmer.Stem ("knights"));

        Assert.AreEqual ("knit", stemmer.Stem ("knit"));
        Assert.AreEqual ("knit", stemmer.Stem ("knits"));
        Assert.AreEqual ("knit", stemmer.Stem ("knitted"));
        Assert.AreEqual ("knit", stemmer.Stem ("knitting"));

        Assert.AreEqual ("knive", stemmer.Stem ("knives"));

        Assert.AreEqual ("knob", stemmer.Stem ("knob"));
        Assert.AreEqual ("knob", stemmer.Stem ("knobs"));

        Assert.AreEqual ("knock", stemmer.Stem ("knock"));
        Assert.AreEqual ("knock", stemmer.Stem ("knocked"));
        Assert.AreEqual ("knocker", stemmer.Stem ("knocker"));
        Assert.AreEqual ("knocker", stemmer.Stem ("knockers"));
        Assert.AreEqual ("knock", stemmer.Stem ("knocking"));
        Assert.AreEqual ("knock", stemmer.Stem ("knocks"));

        Assert.AreEqual ("knopp", stemmer.Stem ("knopp"));

        Assert.AreEqual ("knot", stemmer.Stem ("knot"));
        Assert.AreEqual ("knot", stemmer.Stem ("knots"));
    }
}
