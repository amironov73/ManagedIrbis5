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
    public sealed class AthraRecordTest
    {
        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void AthraRecord_Construction_1()
        {
            var athra = new AthraRecord();
            Assert.IsNull (athra.Examples);
            Assert.IsNull (athra.Notes);
            Assert.IsNull (athra.Record);
            Assert.IsNull (athra.See);
            Assert.IsNull (athra.Technology);
            Assert.IsNull (athra.Worksheet);
            Assert.IsNull (athra.CataloguerNotes);
            Assert.IsNull (athra.CataloguingRules);
            Assert.IsNull (athra.ExclusionInformation);
            Assert.IsNull (athra.IdentificationSources);
            Assert.IsNull (athra.InformationSources);
            Assert.IsNull (athra.LinkedTitles);
            Assert.IsNull (athra.MainTitle);
            Assert.IsNull (athra.SeeAlso);
            Assert.IsNull (athra.UsageInformation);
            Assert.IsNull (athra.UsageNotes);
            Assert.IsNull (athra.UserData);
            Assert.IsNull (athra.WorkPlaces);
            Assert.IsNull (athra.NonIdentificationSources);
            Assert.IsNull (athra.SeeAlsoNote);
        }

    }
}
