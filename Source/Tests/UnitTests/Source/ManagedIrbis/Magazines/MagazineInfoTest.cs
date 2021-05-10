// ReSharper disable CheckNamespace
// ReSharper disable ForCanBeConvertedToForeach
// ReSharper disable IdentifierTypo
// ReSharper disable InvokeAsExtensionMethod
// ReSharper disable StringLiteralTypo

using Microsoft.VisualStudio.TestTools.UnitTesting;

using AM.Json;
using AM.Runtime;
using AM.Xml;

using ManagedIrbis;
using ManagedIrbis.Magazines;

#nullable enable

namespace UnitTests.ManagedIrbis.Magazines
{
    [TestClass]
    public class MagazineInfoTest
    {
        private static Field Parse(int tag, string text)
        {
            var result = new Field { Tag = tag };
            result.DecodeBody(text);

            return result;
        }

        private Record _GetMagazine()
        {
            var result = new Record();
            result.Fields.Add(Parse(102, "SU"));
            result.Fields.Add(Parse(101, "rus"));
            result.Fields.Add(Parse(919, "^Arus^N0102^KPSBO"));
            result.Fields.Add(Parse(920, "J"));
            result.Fields.Add(Parse(200, "^AЗвезда Востока^Eлитературно-художественный и общественно-политический журнал"));
            result.Fields.Add(Parse(210, "^CИздательство литературы и искусства имени Гафура Гуляма^AТашкент^D1939"));
            result.Fields.Add(Parse(110, "^Ta^Ba^Df^X12^Z16+"));
            result.Fields.Add(Parse(621, "С(Узб)"));
            result.Fields.Add(Parse(906, "С(Узб)"));
            result.Fields.Add(Parse(60, "10"));
            result.Fields.Add(Parse(907, "^CКР^A20171115^BКобаковаЛА"));
            result.Fields.Add(Parse(908, "З-43"));
            result.Fields.Add(Parse(903, "З596747388"));
            result.Fields.Add(Parse(934, "1981"));
            result.Fields.Add(Parse(909, "^Q1981^DФП^H1^kЖ54482"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H9^kЖ54047"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H6^kЖ53859"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H5^kЖ53858"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H4^kЖ53857"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H3^kЖ53856"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H2^kЖ53855"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H12^kЖ54481"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H11^kЖ54480"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H10^kЖ57709"));
            result.Fields.Add(Parse(909, "^Q1980^DФП^H1^kЖ53854"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H9^kЖ51539"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H8^kЖ51538"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H7^kЖ51537"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H6^kЖ51536"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H5^kЖ51419"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H4^kЖ51418"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H3^kЖ51233"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H2^kЖ51232"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H12^kЖ53449"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H11^kЖ51671"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H10^kЖ51670"));
            result.Fields.Add(Parse(909, "^Q1979^DФП^H1^kЖ50887"));
            result.Fields.Add(Parse(909, "^Q1978^DФП^H9^kЖ50377"));
            result.Fields.Add(Parse(909, "^Q1978^DФП^H7^kЖ50215"));
            result.Fields.Add(Parse(909, "^Q1978^DФП^H6^kЖ50214"));
            result.Fields.Add(Parse(909, "^Q1978^DФП^H3^kЖ49588"));
            result.Fields.Add(Parse(909, "^Q1978^DФП^H2^kЖ48791"));
            result.Fields.Add(Parse(909, "^Q1978^DФП^H10^kЖ50546"));
            result.Fields.Add(Parse(909, "^Q1977^H5^kЖ48224"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H9^kЖ48492"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H8^kЖ48491"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H7^kЖ48490"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H6^kЖ48225"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H4^kЖ48223"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H3^kЖ48015"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H2^kЖ48014"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H12^kЖ48789"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H11^kЖ48612"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H10^kЖ48611"));
            result.Fields.Add(Parse(909, "^Q1977^DФП^H1^k1"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H9-11^k1"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H8^kЖ45888"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H7^kЖ45887"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H6^kЖ45886"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H5^kЖ45460"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H4^kЖ45459"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H3^kЖ45458"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H2^kЖ44877"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H12^kЖ46188"));
            result.Fields.Add(Parse(909, "^Q1976^DФП^H1^kЖ44876"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H9^k1"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H8^kЖ44441"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H7^kЖ44440"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H6^kЖ44275"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H5^kЖ44274"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H4^kЖ44207"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H3^kЖ43571"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H2^kЖ43131"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H12^kЖ44755"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H11^kЖ44532"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H10^kЖ44531"));
            result.Fields.Add(Parse(909, "^Q1975^DФП^H1^kЖ43130"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H9^kЖ41274"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H7^kЖ49587"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H6^kЖ40996"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H4^kЖ40994"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H3^kЖ40993"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H2^kЖ40992"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H12^kЖ42033"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H11^kЖ42815"));
            result.Fields.Add(Parse(909, "^Q1974^DФП^H10^kЖ41873"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H9^kЖ39254"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H8^kЖ39253"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H7^kЖ39252"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H6^kЖ39251"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H5^kЖ38934"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H4^kЖ38933"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H3^kЖ38932"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H2^kЖ37750"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H12^kЖ39429"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H11^kЖ39428"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H10^kЖ39466"));
            result.Fields.Add(Parse(909, "^Q1973^DФП^H1^kЖ37749"));
            result.Fields.Add(Parse(909, "^Q1972^H5^kЖ37003"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H9^kЖ37466"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H6^kЖ37004"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H4^kЖ36854"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H3^kЖ36853"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H2^kЖ36852"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H12^kЖ37748"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H11^kЖ37469"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H10^kЖ37467"));
            result.Fields.Add(Parse(909, "^Q1972^DФП^H1^kЖ36851"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H9^kЖ36200"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H8^kЖ36087"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H7^kЖ36086"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H5^kЖ35250"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H4^kЖ35163"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H3^kЖ35018"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H2^kЖ35017"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H12^kЖ36203"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H11^kЖ36202"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H10^kЖ36201"));
            result.Fields.Add(Parse(909, "^Q1971^DФП^H1^kЖ35016"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H9^kЖ34561"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H8^kЖ34560"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H7^kЖ34559"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H6^kЖ34113"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H5^kЖ33905"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H4^kЖ33904"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H3^kЖ33903"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H2^kЖ33902"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H12^kЖ35015"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H11^kЖ35014"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H10^kЖ35013"));
            result.Fields.Add(Parse(909, "^Q1970^DФП^H1^kЖ33901"));
            result.Fields.Add(Parse(905, "^D3^F2^S1^21"));

            return result;
        }

        [TestMethod]
        public void MagazineInfo_Construction_1()
        {
            var magazine = new MagazineInfo();
            Assert.IsNull(magazine.Index);
            Assert.IsNull(magazine.Description);
            Assert.IsNull(magazine.Title);
            Assert.IsNull(magazine.SubTitle);
            Assert.IsNull(magazine.SeriesNumber);
            Assert.IsNull(magazine.SeriesTitle);
            Assert.IsNull(magazine.MagazineType);
            Assert.IsNull(magazine.MagazineKind);
            Assert.IsNull(magazine.Periodicity);
            Assert.IsNull(magazine.Cumulation);
            Assert.AreEqual(0, magazine.Mfn);
            Assert.IsNull(magazine.UserData);
        }

        [TestMethod]
        public void MagazineInfo_Parse_1()
        {
            var magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.AreEqual("Звезда Востока", magazine!.Title);
            Assert.IsNotNull(magazine.Cumulation);
            Assert.AreEqual(117, magazine.Cumulation!.Length);
        }

        private void _TestSerialization
            (
                MagazineInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<MagazineInfo>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Index, second!.Index);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.Title, second.Title);
            Assert.AreEqual(first.SubTitle, second.SubTitle);
            Assert.AreEqual(first.SeriesNumber, second.SeriesNumber);
            Assert.AreEqual(first.SeriesTitle, second.SeriesTitle);
            Assert.AreEqual(first.MagazineType, second.MagazineType);
            Assert.AreEqual(first.MagazineKind, second.MagazineKind);
            Assert.AreEqual(first.Periodicity, second.Periodicity);
            Assert.AreEqual(first.Mfn, second.Mfn);
        }

        [TestMethod]
        public void MagazineInfo_Serialization_1()
        {
            var magazine = new MagazineInfo();
            _TestSerialization(magazine);

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            _TestSerialization(magazine!);
        }

        [TestMethod]
        public void MagazineInfo_ToXml_1()
        {
            var magazine = new MagazineInfo();
            Assert.AreEqual("<magazine />", XmlUtility.SerializeShort(magazine));

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.AreEqual("<magazine index=\"З596747388\" title=\"Звезда Востока\" sub-title=\"литературно-художественный и общественно-политический журнал\" series-number=\"\" series-title=\"\" magazine-type=\"a\" magazine-kind=\"a\" periodicity=\"12\"><cumulation year=\"1981\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж54482\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж54047\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж53859\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"5\" set=\"Ж53858\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж53857\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж53856\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж53855\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж54481\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж54480\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж57709\" /><cumulation year=\"1980\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж53854\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж51539\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"8\" set=\"Ж51538\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж51537\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж51536\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"5\" set=\"Ж51419\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж51418\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж51233\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж51232\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж53449\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж51671\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж51670\" /><cumulation year=\"1979\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж50887\" /><cumulation year=\"1978\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж50377\" /><cumulation year=\"1978\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж50215\" /><cumulation year=\"1978\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж50214\" /><cumulation year=\"1978\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж49588\" /><cumulation year=\"1978\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж48791\" /><cumulation year=\"1978\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж50546\" /><cumulation year=\"1977\" volume=\"\" place=\"\" numbers=\"5\" set=\"Ж48224\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж48492\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"8\" set=\"Ж48491\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж48490\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж48225\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж48223\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж48015\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж48014\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж48789\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж48612\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж48611\" /><cumulation year=\"1977\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"1\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"9-11\" set=\"1\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"8\" set=\"Ж45888\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж45887\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж45886\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"5\" set=\"Ж45460\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж45459\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж45458\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж44877\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж46188\" /><cumulation year=\"1976\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж44876\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"1\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"8\" set=\"Ж44441\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж44440\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж44275\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"5\" set=\"Ж44274\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж44207\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж43571\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж43131\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж44755\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж44532\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж44531\" /><cumulation year=\"1975\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж43130\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж41274\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж49587\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж40996\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж40994\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж40993\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж40992\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж42033\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж42815\" /><cumulation year=\"1974\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж41873\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж39254\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"8\" set=\"Ж39253\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж39252\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж39251\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"5\" set=\"Ж38934\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж38933\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж38932\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж37750\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж39429\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж39428\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж39466\" /><cumulation year=\"1973\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж37749\" /><cumulation year=\"1972\" volume=\"\" place=\"\" numbers=\"5\" set=\"Ж37003\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж37466\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж37004\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж36854\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж36853\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж36852\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж37748\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж37469\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж37467\" /><cumulation year=\"1972\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж36851\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж36200\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"8\" set=\"Ж36087\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж36086\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"5\" set=\"Ж35250\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж35163\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж35018\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж35017\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж36203\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж36202\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж36201\" /><cumulation year=\"1971\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж35016\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"9\" set=\"Ж34561\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"8\" set=\"Ж34560\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"7\" set=\"Ж34559\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"6\" set=\"Ж34113\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"5\" set=\"Ж33905\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"4\" set=\"Ж33904\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"3\" set=\"Ж33903\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"2\" set=\"Ж33902\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"12\" set=\"Ж35015\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"11\" set=\"Ж35014\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"10\" set=\"Ж35013\" /><cumulation year=\"1970\" volume=\"\" place=\"ФП\" numbers=\"1\" set=\"Ж33901\" /></magazine>",
                XmlUtility.SerializeShort(magazine!));
        }

        [TestMethod]
        public void MagazineInfo_ToJson_1()
        {
            var magazine = new MagazineInfo();
            Assert.AreEqual("{\"mfn\":0}", JsonUtility.SerializeShort(magazine));

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.AreEqual("{\"index\":\"\\u0417596747388\",\"title\":\"\\u0417\\u0432\\u0435\\u0437\\u0434\\u0430 \\u0412\\u043E\\u0441\\u0442\\u043E\\u043A\\u0430\",\"sub-title\":\"\\u043B\\u0438\\u0442\\u0435\\u0440\\u0430\\u0442\\u0443\\u0440\\u043D\\u043E-\\u0445\\u0443\\u0434\\u043E\\u0436\\u0435\\u0441\\u0442\\u0432\\u0435\\u043D\\u043D\\u044B\\u0439 \\u0438 \\u043E\\u0431\\u0449\\u0435\\u0441\\u0442\\u0432\\u0435\\u043D\\u043D\\u043E-\\u043F\\u043E\\u043B\\u0438\\u0442\\u0438\\u0447\\u0435\\u0441\\u043A\\u0438\\u0439 \\u0436\\u0443\\u0440\\u043D\\u0430\\u043B\",\"series-number\":\"\",\"series-title\":\"\",\"magazine-type\":\"a\",\"magazine-kind\":\"a\",\"periodicity\":\"12\",\"cumulation\":[{\"year\":\"1981\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041654482\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041654047\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041653859\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"5\",\"set\":\"\\u041653858\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041653857\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041653856\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041653855\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041654481\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041654480\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041657709\",\"unknown\":[]},{\"year\":\"1980\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041653854\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041651539\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"8\",\"set\":\"\\u041651538\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041651537\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041651536\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"5\",\"set\":\"\\u041651419\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041651418\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041651233\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041651232\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041653449\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041651671\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041651670\",\"unknown\":[]},{\"year\":\"1979\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041650887\",\"unknown\":[]},{\"year\":\"1978\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041650377\",\"unknown\":[]},{\"year\":\"1978\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041650215\",\"unknown\":[]},{\"year\":\"1978\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041650214\",\"unknown\":[]},{\"year\":\"1978\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041649588\",\"unknown\":[]},{\"year\":\"1978\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041648791\",\"unknown\":[]},{\"year\":\"1978\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041650546\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\",\"numbers\":\"5\",\"set\":\"\\u041648224\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041648492\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"8\",\"set\":\"\\u041648491\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041648490\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041648225\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041648223\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041648015\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041648014\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041648789\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041648612\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041648611\",\"unknown\":[]},{\"year\":\"1977\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"1\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9-11\",\"set\":\"1\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"8\",\"set\":\"\\u041645888\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041645887\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041645886\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"5\",\"set\":\"\\u041645460\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041645459\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041645458\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041644877\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041646188\",\"unknown\":[]},{\"year\":\"1976\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041644876\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"1\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"8\",\"set\":\"\\u041644441\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041644440\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041644275\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"5\",\"set\":\"\\u041644274\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041644207\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041643571\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041643131\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041644755\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041644532\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041644531\",\"unknown\":[]},{\"year\":\"1975\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041643130\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041641274\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041649587\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041640996\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041640994\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041640993\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041640992\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041642033\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041642815\",\"unknown\":[]},{\"year\":\"1974\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041641873\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041639254\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"8\",\"set\":\"\\u041639253\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041639252\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041639251\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"5\",\"set\":\"\\u041638934\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041638933\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041638932\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041637750\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041639429\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041639428\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041639466\",\"unknown\":[]},{\"year\":\"1973\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041637749\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\",\"numbers\":\"5\",\"set\":\"\\u041637003\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041637466\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041637004\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041636854\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041636853\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041636852\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041637748\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041637469\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041637467\",\"unknown\":[]},{\"year\":\"1972\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041636851\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041636200\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"8\",\"set\":\"\\u041636087\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041636086\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"5\",\"set\":\"\\u041635250\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041635163\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041635018\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041635017\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041636203\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041636202\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041636201\",\"unknown\":[]},{\"year\":\"1971\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041635016\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"9\",\"set\":\"\\u041634561\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"8\",\"set\":\"\\u041634560\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"7\",\"set\":\"\\u041634559\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"6\",\"set\":\"\\u041634113\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"5\",\"set\":\"\\u041633905\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"4\",\"set\":\"\\u041633904\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"3\",\"set\":\"\\u041633903\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"2\",\"set\":\"\\u041633902\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"12\",\"set\":\"\\u041635015\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"11\",\"set\":\"\\u041635014\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"10\",\"set\":\"\\u041635013\",\"unknown\":[]},{\"year\":\"1970\",\"volume\":\"\",\"place\":\"\\u0424\\u041F\",\"numbers\":\"1\",\"set\":\"\\u041633901\",\"unknown\":[]}],\"orders\":[],\"mfn\":0}", JsonUtility.SerializeShort(magazine));
        }

        [TestMethod]
        public void MagazineInfo_ToString_1()
        {
            var magazine = new MagazineInfo();
            Assert.AreEqual("", magazine.ToString());

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.AreEqual("Звезда Востока: литературно-художественный и общественно-политический журнал",
                magazine!.ToString());
        }

        [TestMethod]
        public void MagazineInfo_Verify_1()
        {
            var magazine = new MagazineInfo();
            Assert.IsFalse(magazine.Verify(false));

            magazine = MagazineInfo.Parse(_GetMagazine());
            Assert.IsNotNull(magazine);
            Assert.IsTrue(magazine!.Verify(false));
        }
    }
}
