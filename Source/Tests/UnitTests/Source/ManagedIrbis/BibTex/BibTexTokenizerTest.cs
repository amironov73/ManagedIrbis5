// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

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
public sealed class BibTexTokenizerTest
    : Common.CommonUnitTest
{
    [TestMethod]
    [ExpectedException (typeof (NotImplementedException))]
    public void BibTexTokenizer_Tokenize_1()
    {
        var tokenizer = new BibTexTokenizer();
        tokenizer.Tokenize ("this is a test");
    }
}
