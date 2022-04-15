// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;

using ManagedIrbis.BibTex;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.BibTex;

[TestClass]
public sealed class BibTexParserTest
{
    [TestMethod]
    [ExpectedException (typeof (NotImplementedException))]
    public void BibTexParser_Parse_1()
    {
        var parser = new BibTexParser();
        parser.Parse (Array.Empty<Token>());
    }
}