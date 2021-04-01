// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Readers;

#nullable enable

namespace UnitTests.ManagedIrbis.Readers
{
    [TestClass]
    public class IriProfileTest
    {
        [TestMethod]
        public void IriProfile_Construction_1()
        {
            var profile = new IriProfile();
            Assert.IsFalse(profile.Active);
            Assert.IsNull(profile.ID);
            Assert.IsNull(profile.Title);
            Assert.IsNull(profile.Query);
            Assert.AreEqual(0, profile.Periodicity);
            Assert.IsNull(profile.LastServed);
            Assert.IsNull(profile.Database);
            Assert.IsNull(profile.Field);
            Assert.IsNull(profile.UnknownSubFields);
            Assert.IsNull(profile.Reader);
            Assert.IsNull(profile.UserData);
        }

        private static Field Parse(int tag, string value)
        {
            var result = new Field { Tag = tag };
            result.DecodeBody(value);

            return result;
        }

        private Field _GetField()
        {
            return Parse(IriProfile.Tag, "^A1^B1^CБиблиографоведение^DBBK=78.5$^E7^IIBIS^F20160701");
        }

        private IriProfile _GetProfile()
        {
            return new IriProfile
            {
                Active = true,
                ID = "1",
                Title = "Библиографоведение",
                Query = "BBK=78.5$",
                Periodicity = 7,
                LastServed = "20160701",
                Database = "IBIS"
            };
        }

        [TestMethod]
        public void IriProfile_ParseField_1()
        {
            var field = _GetField();
            var profile = IriProfile.ParseField(field);
            Assert.IsTrue(profile.Active);
            Assert.AreEqual("1", profile.ID);
            Assert.AreEqual("Библиографоведение", profile.Title);
            Assert.AreEqual("BBK=78.5$", profile.Query);
            Assert.AreEqual(7, profile.Periodicity);
            Assert.AreEqual("20160701", profile.LastServed);
            Assert.AreEqual("IBIS", profile.Database);
            Assert.AreEqual(0, profile.UnknownSubFields!.Length);
        }

        [TestMethod]
        public void IriProfile_ParseRecord_1()
        {
            var record = new Record();
            record.Fields.Add(_GetField());
            record.Fields.Add(Parse(IriProfile.Tag, "^A1^B2^CБиблиотечное краеведение^D\"K=Библиотечное краеведение$\"^E7^IIBIS^F20160701"));

            var profiles = IriProfile.ParseRecord(record);
            Assert.AreEqual(2, profiles.Length);
            Assert.IsTrue(profiles[1].Active);
            Assert.AreEqual("2", profiles[1].ID);
            Assert.AreEqual("Библиотечное краеведение", profiles[1].Title);
            Assert.AreEqual("\"K=Библиотечное краеведение$\"", profiles[1].Query);
            Assert.AreEqual(7, profiles[1].Periodicity);
            Assert.AreEqual("20160701", profiles[1].LastServed);
            Assert.AreEqual("IBIS", profiles[1].Database);
            Assert.AreEqual(0, profiles[1].UnknownSubFields!.Length);
        }

        private void _TestSerialization
            (
                IriProfile first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<IriProfile>();

            Assert.IsNotNull(second);
            Assert.AreEqual(first.Active, second!.Active);
            Assert.AreEqual(first.Database, second.Database);
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.Query, second.Query);
        }

        [TestMethod]
        public void IriProfile_Serialization_1()
        {
            var profile = new IriProfile();
            _TestSerialization(profile);

            profile = _GetProfile();
            _TestSerialization(profile);

            profile.Reader = new ReaderInfo();
            profile.UserData = "User data";
            _TestSerialization(profile);
        }

        [TestMethod]
        public void IriProfile_Save_Read_1()
        {
            IriProfile[] first = {_GetProfile()};
            var fileName = Path.GetTempFileName();
            IriProfile.SaveToFile(fileName, first);
            var array = IriProfile.LoadFromFile(fileName);
            Assert.AreEqual(1, array.Length);
            var second = array[0];
            Assert.AreEqual("1", second.ID);
            Assert.AreEqual("Библиографоведение", second.Title);
            Assert.AreEqual("BBK=78.5$", second.Query);
            Assert.AreEqual(7, second.Periodicity);
            Assert.AreEqual("20160701", second.LastServed);
            Assert.AreEqual("IBIS", second.Database);
            Assert.IsNull(second.UnknownSubFields);
        }

        [TestMethod]
        public void IriProfile_ToXml_1()
        {
            var profile = new IriProfile();
            Assert.AreEqual("<iri-profile active=\"false\" periodicity=\"0\" />", XmlUtility.SerializeShort(profile));

            profile = _GetProfile();
            Assert.AreEqual("<iri-profile active=\"true\" id=\"1\" title=\"Библиографоведение\" query=\"BBK=78.5$\" periodicity=\"7\" lastServed=\"20160701\" database=\"IBIS\" />", XmlUtility.SerializeShort(profile));
        }

        [Ignore]
        [TestMethod]
        public void IriProfile_ToJson_1()
        {
            var profile = new IriProfile();
            Assert.AreEqual("{\"active\":false,\"periodicity\":0}", JsonUtility.SerializeShort(profile));

            profile = _GetProfile();
            Assert.AreEqual("{'active':true,'id':'1','title':'Библиографоведение','query':'BBK=78.5$','periodicity':7,'lastServed':'20160701','database':'IBIS'}", JsonUtility.SerializeShort(profile));
        }
    }
}
