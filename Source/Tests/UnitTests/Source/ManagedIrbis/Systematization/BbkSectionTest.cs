// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

using System;

using ManagedIrbis.Systematization;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Systematization;

[TestClass]
public sealed class BbkSectionTest
{
    [TestMethod]
    public void BbkSection_Construction_1()
    {
        const string publicLibrary = "1";
        const string scienceLibrary = "А";
        const string wording = "Общенаучное и междисциплинарное знание";
        var section = new BbkSection (publicLibrary, scienceLibrary, wording);
        Assert.AreEqual (publicLibrary, section.PublicLibrary);
        Assert.AreEqual (scienceLibrary, section.ScienceLibrary);
        Assert.AreEqual (wording, section.Wording);
    }

    [TestMethod]
    public void BbkSection_FindPublicSection_1()
    {
        var found = BbkSection.FindPublicSection ("71.1");
        Assert.IsNotNull (found);
        Assert.AreEqual ("71", found.PublicLibrary);
    }

    [TestMethod]
    public void BbkSection_FindPublicSection_2()
    {
        var found = BbkSection.FindPublicSection ("73.1");
        Assert.IsNull (found);
    }

    [TestMethod]
    public void BbkSection_FindSectionSection_1()
    {
        var found = BbkSection.FindScienceSection ("Ч11");
        Assert.IsNotNull (found);
        Assert.AreEqual ("Ч1", found.ScienceLibrary);
    }

    [TestMethod]
    public void BbkSection_FindSectionSection_2()
    {
        var found = BbkSection.FindScienceSection ("Ч3");
        Assert.IsNull (found);
    }

    [TestMethod]
    public void BbkSection_IsPublicLibrary_1()
    {
        Assert.IsTrue (BbkSection.IsPublicLibrary ("71.1"));
    }

    [TestMethod]
    public void BbkSection_IsPublicLibrary_2()
    {
        Assert.IsFalse (BbkSection.IsPublicLibrary ("73.1"));
    }

    [TestMethod]
    public void BbkSection_IsScienceLibrary_1()
    {
        Assert.IsTrue (BbkSection.IsScienceLibrary ("Ч1"));
    }

    [TestMethod]
    public void BbkSection_IsScienceLibrary_2()
    {
        Assert.IsFalse (BbkSection.IsScienceLibrary ("Ч3"));
    }

    [TestMethod]
    public void BbkSection_SelfCheck_1()
    {
        Assert.IsTrue (BbkSection.SelfCheck (false));
    }

    [TestMethod]
    public void BbkSection_ToPublic_1()
    {
        var converted = BbkSection.ToPublic ("Ч11");
        Assert.AreEqual ("711", converted); // TODO должна быть точка!
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentOutOfRangeException))]
    public void BbkSection_ToPublic_2()
    {
        BbkSection.ToPublic ("Ч31");
    }

    [TestMethod]
    public void BbkSection_ToScience_1()
    {
        var converted = BbkSection.ToScience ("71.1");
        Assert.AreEqual ("Ч1.1", converted); // TODO не должно быть точки!
    }

    [TestMethod]
    [ExpectedException (typeof (ArgumentOutOfRangeException))]
    public void BbkSection_ToScience_2()
    {
        BbkSection.ToScience ("73.1");
    }

    [TestMethod]
    public void BbkSection_ToString_1()
    {
        const string publicLibrary = "1";
        const string scienceLibrary = "А";
        const string wording = "Общенаучное и междисциплинарное знание";
        var section = new BbkSection (publicLibrary, scienceLibrary, wording);
        Assert.AreEqual ("1 - А: Общенаучное и междисциплинарное знание", section.ToString());
    }
}
