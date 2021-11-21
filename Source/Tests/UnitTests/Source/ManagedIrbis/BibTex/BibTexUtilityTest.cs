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

namespace UnitTests.ManagedIrbis.BibTex
{
    [TestClass]
    public class BibTexUtilityTest
        : Common.CommonUnitTest
    {
        private string BibTexPath
            (
                string fileName
            )
        {
            return Path.Combine (this.TestDataPath, "BibTex", fileName);
        }

        [TestMethod]
        [ExpectedException (typeof (NotImplementedException))]
        public void BibTexUtility_ReadFile_1()
        {
            var fileName = BibTexPath ("database.bib");
            BibTexUtility.ReadFile (fileName);
        }
    }
}
