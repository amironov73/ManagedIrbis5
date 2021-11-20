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
    public sealed class AthrsRecordTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthrsRecord_Construction_1()
        {
            var athrs = new AthrsRecord();
            Assert.IsNull (athrs.Bbk);
            Assert.IsNull (athrs.Example);
            Assert.IsNull (athrs.Notes);
            Assert.IsNull (athrs.See);
            Assert.IsNull (athrs.Settings);
            Assert.IsNull (athrs.Technology);
            Assert.IsNull (athrs.Udk);
            Assert.IsNull (athrs.Worksheet);
            Assert.IsNull (athrs.CataguingRules);
            Assert.IsNull (athrs.CataloguerNotes);
            Assert.IsNull (athrs.ExclusionInformation);
            Assert.IsNull (athrs.ExternalObject);
            Assert.IsNull (athrs.IdentificationSources);
            Assert.IsNull (athrs.InformationSources);
            Assert.IsNull (athrs.MainTitle);
            Assert.IsNull (athrs.SeeAlso);
            Assert.IsNull (athrs.SeeNote);
            Assert.IsNull (athrs.UsageNotes);
            Assert.IsNull (athrs.SeeAlsoHeadings);
            Assert.IsNull (athrs.SeeAlsoNames);
            Assert.IsNull (athrs.SeeAlsoNote);
            Assert.IsNull (athrs.SeeAlsoOrganizations);
            Assert.IsNull (athrs.SeeAlsoGeoName);
        }

    }
}
