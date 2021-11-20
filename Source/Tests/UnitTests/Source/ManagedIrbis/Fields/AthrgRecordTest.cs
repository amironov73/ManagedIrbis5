// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class AthrgRecordTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthrgRecord_Construction_1()
        {
            var athrg = new AthrgRecord();
            Assert.IsNull (athrg.Example);
            Assert.IsNull (athrg.Notes);
            Assert.IsNull (athrg.See);
            Assert.IsNull (athrg.Settings);
            Assert.IsNull (athrg.Technology);
            Assert.IsNull (athrg.Worksheet);
            Assert.IsNull (athrg.BinaryResource);
            Assert.IsNull (athrg.CataguingRules);
            Assert.IsNull (athrg.CataloguerNotes);
            Assert.IsNull (athrg.ExclusionInformation);
            Assert.IsNull (athrg.ExternalObject);
            Assert.IsNull (athrg.IdentificationSources);
            Assert.IsNull (athrg.InformationSources);
            Assert.IsNull (athrg.MainTitle);
            Assert.IsNull (athrg.SeeAlso);
            Assert.IsNull (athrg.SeeNote);
            Assert.IsNull (athrg.UsageInformation);
            Assert.IsNull (athrg.UsageNotes);
            Assert.IsNull (athrg.SeeAlsoNote);
        }

    }
}
