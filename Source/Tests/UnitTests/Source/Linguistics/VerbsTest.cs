// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo

using System.Linq;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Linguistics;

#nullable enable

namespace UnitTests.Linguistics;

[TestClass]
public sealed class VerbsTest
{
    [TestMethod]
    public void Verbs_FindAll_1()
    {
        var found = Verbs.FindAll ("рисовать").ToArray();
        Assert.AreEqual (1, found.Length);
        Assert.AreEqual (VerbAspect.Imperfect, found[0].Aspect);
    }
}
