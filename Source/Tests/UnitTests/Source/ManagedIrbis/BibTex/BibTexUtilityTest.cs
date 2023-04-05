// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

#region Using directives

using System;
using System.IO;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis.BibTex;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#endregion

#nullable enable

namespace UnitTests.ManagedIrbis.BibTex;

[TestClass]
public class BibTexUtilityTest
    : Common.CommonUnitTest
{
    private string BibTexPath
        (
            string fileName
        )
    {
        return Path.Combine (TestDataPath, "BibTex", fileName);
    }

    [TestMethod]
    [ExpectedException (typeof (NotImplementedException))]
    public void BibTexUtility_ReadFile_1()
    {
        var fileName = BibTexPath ("database.bib");
        BibTexUtility.ReadFile (fileName);
    }
}
