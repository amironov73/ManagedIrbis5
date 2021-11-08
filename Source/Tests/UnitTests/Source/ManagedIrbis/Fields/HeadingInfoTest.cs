// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using AM.Json;
using AM.Runtime;
using AM.Text;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Fields;

using Microsoft.VisualStudio.TestTools.UnitTesting;

#nullable enable

namespace UnitTests.ManagedIrbis.Fields
{
    [TestClass]
    public sealed class HeadingInfoTest
    {
        private HeadingInfo _GetHeading() => new ()
        {
            Title = "Русская литература",
            Subtitle1 = "Проза",
            GeographicalSubtitle1 = "Санкт-Петербург",
            ChronologicalSubtitle = "19 в.",
            Aspect = "Сборники"
        };

        private Field _GetField() => new Field (HeadingInfo.Tag)
            .Add ('a', "Русская литература")
            .Add ('a', "Проза")
            .Add ('a', "Санкт-Петербург")
            .Add ('a', "19 в.")
            .Add ('a', "Сборники");

        [TestMethod]
        public void HeadingInfo_Construction_1()
        {
            var heading = new HeadingInfo();
            Assert.IsNull (heading.Title);
            Assert.IsNull (heading.Subtitle1);
            Assert.IsNull (heading.Subtitle2);
            Assert.IsNull (heading.Subtitle3);
            Assert.IsNull (heading.GeographicalSubtitle1);
            Assert.IsNull (heading.GeographicalSubtitle2);
            Assert.IsNull (heading.GeographicalSubtitle3);
            Assert.IsNull (heading.ChronologicalSubtitle);
            Assert.IsNull (heading.UnknownSubFields);
            Assert.IsNull (heading.Aspect);
            Assert.IsNull (heading.Field);
            Assert.IsNull (heading.UserData);
        }

        private void _TestSerialization
            (
                HeadingInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<HeadingInfo>();
            Assert.IsNotNull (second);
            Assert.AreEqual (first.Title, second.Title);
            Assert.AreEqual (first.Subtitle1, second.Subtitle1);
            Assert.AreEqual (first.Subtitle2, second.Subtitle2);
            Assert.AreEqual (first.Subtitle3, second.Subtitle3);
            Assert.AreEqual (first.GeographicalSubtitle1, second.GeographicalSubtitle1);
            Assert.AreEqual (first.GeographicalSubtitle2, second.GeographicalSubtitle2);
            Assert.AreEqual (first.GeographicalSubtitle3, second.GeographicalSubtitle3);
            Assert.AreEqual (first.ChronologicalSubtitle, second.ChronologicalSubtitle);
            Assert.AreEqual (first.Aspect, second.Aspect);
            Assert.AreSame (first.UnknownSubFields, second.UnknownSubFields);
            Assert.IsNull (second.Field);
            Assert.IsNull (second.UserData);
        }

        [TestMethod]
        public void HeadingInfo_Serialization_1()
        {
            var heading = new HeadingInfo();
            _TestSerialization (heading);

            heading.UserData = "User data";
            _TestSerialization (heading);

            heading = _GetHeading();
            _TestSerialization (heading);
        }

        [TestMethod]
        public void HeadingInfo_ParseField_1()
        {
            var field = _GetField();
            var heading = HeadingInfo.ParseField (field);
            Assert.AreSame (field, heading.Field);
            Assert.AreEqual (field.GetFirstSubFieldValue ('a'), heading.Title);
            Assert.AreEqual (field.GetFirstSubFieldValue ('b'), heading.Subtitle1);
            Assert.AreEqual (field.GetFirstSubFieldValue ('c'), heading.Subtitle2);
            Assert.AreEqual (field.GetFirstSubFieldValue ('d'), heading.Subtitle3);
            Assert.AreEqual (field.GetFirstSubFieldValue ('g'), heading.GeographicalSubtitle1);
            Assert.AreEqual (field.GetFirstSubFieldValue ('e'), heading.GeographicalSubtitle2);
            Assert.AreEqual (field.GetFirstSubFieldValue ('o'), heading.GeographicalSubtitle3);
            Assert.AreEqual (field.GetFirstSubFieldValue ('h'), heading.ChronologicalSubtitle);
            Assert.AreEqual (field.GetFirstSubFieldValue ('9'), heading.Aspect);
            Assert.IsNotNull (heading.UnknownSubFields);
            Assert.AreEqual (0, heading.UnknownSubFields!.Length);
            Assert.IsNull (heading.UserData);
        }

        [TestMethod]
        public void HeadingInfo_ParseRecord_1()
        {
            var record = new Record();
            var field = _GetField();
            record.Fields.Add (field);
            var heading = HeadingInfo.ParseRecord (record);
            Assert.AreEqual (1, heading.Length);
            Assert.AreSame (field, heading[0].Field);
            Assert.AreEqual (field.GetFirstSubFieldValue ('a'), heading[0].Title);
            Assert.AreEqual (field.GetFirstSubFieldValue ('b'), heading[0].Subtitle1);
            Assert.AreEqual (field.GetFirstSubFieldValue ('c'), heading[0].Subtitle2);
            Assert.AreEqual (field.GetFirstSubFieldValue ('d'), heading[0].Subtitle3);
            Assert.AreEqual (field.GetFirstSubFieldValue ('g'), heading[0].GeographicalSubtitle1);
            Assert.AreEqual (field.GetFirstSubFieldValue ('e'), heading[0].GeographicalSubtitle2);
            Assert.AreEqual (field.GetFirstSubFieldValue ('o'), heading[0].GeographicalSubtitle3);
            Assert.AreEqual (field.GetFirstSubFieldValue ('h'), heading[0].ChronologicalSubtitle);
            Assert.AreEqual (field.GetFirstSubFieldValue ('9'), heading[0].Aspect);
            Assert.IsNotNull (heading[0].UnknownSubFields);
            Assert.AreEqual (0, heading[0].UnknownSubFields!.Length);
            Assert.IsNull (heading[0].UserData);
        }

        [TestMethod]
        public void HeadingInfo_ToField_1()
        {
            var heading = _GetHeading();
            var field = heading.ToField();
            Assert.AreEqual (HeadingInfo.Tag, field.Tag);
            Assert.AreEqual (5, field.Subfields.Count);
            Assert.AreEqual (heading.Title, field.GetFirstSubFieldValue ('a'));
            Assert.AreEqual (heading.Subtitle1, field.GetFirstSubFieldValue ('b'));
            Assert.AreEqual (heading.Subtitle2, field.GetFirstSubFieldValue ('c'));
            Assert.AreEqual (heading.Subtitle3, field.GetFirstSubFieldValue ('d'));
            Assert.AreEqual (heading.GeographicalSubtitle1, field.GetFirstSubFieldValue ('g'));
            Assert.AreEqual (heading.GeographicalSubtitle2, field.GetFirstSubFieldValue ('e'));
            Assert.AreEqual (heading.GeographicalSubtitle3, field.GetFirstSubFieldValue ('o'));
            Assert.AreEqual (heading.ChronologicalSubtitle, field.GetFirstSubFieldValue ('h'));
            Assert.AreEqual (heading.Aspect, field.GetFirstSubFieldValue ('9'));
        }

        [TestMethod]
        public void HeadingInfo_ApplyToField_1()
        {
            var field = new Field (HeadingInfo.Tag)
                .Add ('a', "???")
                .Add ('b', "???");
            var heading = _GetHeading();
            heading.ApplyToField (field);
            Assert.AreEqual (5, field.Subfields.Count);
            Assert.AreEqual (heading.Title, field.GetFirstSubFieldValue ('a'));
            Assert.AreEqual (heading.Subtitle1, field.GetFirstSubFieldValue ('b'));
            Assert.AreEqual (heading.Subtitle2, field.GetFirstSubFieldValue ('c'));
            Assert.AreEqual (heading.Subtitle3, field.GetFirstSubFieldValue ('d'));
            Assert.AreEqual (heading.GeographicalSubtitle1, field.GetFirstSubFieldValue ('g'));
            Assert.AreEqual (heading.GeographicalSubtitle2, field.GetFirstSubFieldValue ('e'));
            Assert.AreEqual (heading.GeographicalSubtitle3, field.GetFirstSubFieldValue ('o'));
            Assert.AreEqual (heading.ChronologicalSubtitle, field.GetFirstSubFieldValue ('h'));
            Assert.AreEqual (heading.Aspect, field.GetFirstSubFieldValue ('9'));
        }

        [TestMethod]
        public void HeadingInfo_Verify_1()
        {
            var heading = new HeadingInfo();
            Assert.IsFalse (heading.Verify (false));

            heading = _GetHeading();
            Assert.IsTrue (heading.Verify (false));
        }

        [TestMethod]
        public void HeadingInfo_ToXml_1()
        {
            var heading = new HeadingInfo();
            Assert.AreEqual ("<heading />", XmlUtility.SerializeShort (heading));

            heading = _GetHeading();
            Assert.AreEqual (
                "<heading><title>Русская литература</title><subtitle1>Проза</subtitle1><geoSubtitle1>Санкт-Петербург</geoSubtitle1><chronoSubtitle>19 в.</chronoSubtitle><aspect>Сборники</aspect></heading>",
                XmlUtility.SerializeShort (heading));
        }

        [TestMethod]
        public void HeadingInfo_ToJson_1()
        {
            var heading = new HeadingInfo();
            Assert.AreEqual ("{}", JsonUtility.SerializeShort (heading));

            heading = _GetHeading();
            Assert.AreEqual (
                "{\"title\":\"\\u0420\\u0443\\u0441\\u0441\\u043A\\u0430\\u044F \\u043B\\u0438\\u0442\\u0435\\u0440\\u0430\\u0442\\u0443\\u0440\\u0430\",\"subtitle1\":\"\\u041F\\u0440\\u043E\\u0437\\u0430\",\"geoSubtitle1\":\"\\u0421\\u0430\\u043D\\u043A\\u0442-\\u041F\\u0435\\u0442\\u0435\\u0440\\u0431\\u0443\\u0440\\u0433\",\"chronoSubtitle\":\"19 \\u0432.\",\"aspect\":\"\\u0421\\u0431\\u043E\\u0440\\u043D\\u0438\\u043A\\u0438\"}",
                JsonUtility.SerializeShort (heading));
        }

        [TestMethod]
        public void HeadingInfo_ToString_1()
        {
            var heading = new HeadingInfo();
            Assert.AreEqual
                (
                    "(null)",
                    heading.ToString().DosToUnix()
                );

            heading = _GetHeading();
            Assert.AreEqual
                (
                    "Русская литература -- Проза -- Санкт-Петербург -- 19 в. -- Сборники",
                    heading.ToString().DosToUnix()
                );
        }
    }
}
