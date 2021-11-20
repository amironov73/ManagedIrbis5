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
    public sealed class AthrcRecordTest
        : Common.CommonUnitTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthrcRecord_Construction_1()
        {
            var athrc = new AthrcRecord();
            Assert.IsNull (athrc.Examples);
            Assert.IsNull (athrc.Notes);
            Assert.IsNull (athrc.See);
            Assert.IsNull (athrc.Settings);
            Assert.IsNull (athrc.Technology);
            Assert.IsNull (athrc.Worksheet);
            Assert.IsNull (athrc.BinaryResource);
            Assert.IsNull (athrc.CataloguerNotes);
            Assert.IsNull (athrc.CataloguingRules);
            Assert.IsNull (athrc.CollectiveKind);
            Assert.IsNull (athrc.ExclusionInformation);
            Assert.IsNull (athrc.ExternalObject);
            Assert.IsNull (athrc.IdentificationSources);
            Assert.IsNull (athrc.InformationSources);
            Assert.IsNull (athrc.LinkedTitles);
            Assert.IsNull (athrc.MainTitle);
            Assert.IsNull (athrc.SeeAlso);
            Assert.IsNull (athrc.SeeNote);
            Assert.IsNull (athrc.UsageInformation);
            Assert.IsNull (athrc.UsageNotes);
            Assert.IsNull (athrc.SeeAlsoNote);
        }

    }
}
