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
    public class MagazineIssueInfoTest
        : CommonMagazineTest
    {
        private Record _GetIssueWithoutArticles()
        {
            var result = new Record();
            result.Fields.Add(Parse(920, "NJP"));
            result.Fields.Add(Parse(907, "^CКТ^A20171027^BМануйловаТС"));
            result.Fields.Add(Parse(933, "Ш620"));
            result.Fields.Add(Parse(903, "Ш620/1992/12"));
            result.Fields.Add(Parse(934, "1992"));
            result.Fields.Add(Parse(936, "12"));
            result.Fields.Add(Parse(999, "0000000"));
            result.Fields.Add(Parse(463, "^wШ620/1992/Подшивка № 10028 июль-дек. (7-12)"));
            result.Fields.Add(Parse(907, "^CРЖ^A20171027^Bmiron"));
            result.Fields.Add(Parse(910, "^Ap^b1^c?^dФП^pШ620/1992/Подшивка № 10028 июль-дек. (7-12)^iП10028"));
            result.Fields.Add(Parse(905, "^D1^F2^S1^21"));

            return result;
        }

        private Record _GetIssueWithArticles()
        {
            var result = new Record();
            result.Fields.Add(Parse(920, "NJ"));
            result.Fields.Add(Parse(907, "^CРЖ^A20170913^BБудаговскаянв"));
            result.Fields.Add(Parse(933, "О174142"));
            result.Fields.Add(Parse(903, "О174142/2017/102"));
            result.Fields.Add(Parse(934, "2017"));
            result.Fields.Add(Parse(936, "102"));
            result.Fields.Add(Parse(931, "^C13-19 сентября^!20170919"));
            result.Fields.Add(Parse(907, "^CРЖ^A20170914^Bпанюшкинатн"));
            result.Fields.Add(Parse(907, "^CРЖ^A20170915^BНаумочкинаММ"));
            result.Fields.Add(Parse(951, "^Aregional_2017_102.pdf^TПолный текст^N28^H01^120170915^216636304"));
            result.Fields.Add(Parse(6629, "EL"));
            result.Fields.Add(Parse(907, "^CFT^A20170915^Bkudelya"));
            result.Fields.Add(Parse(907, "^CПК^A20171004^BКурчинскаяЛН"));
            result.Fields.Add(Parse(907, "^CПК^A20171006^BКурчинскаяЛН"));
            result.Fields.Add(Parse(922, "^FОрлова Е.^?Елена^CМиссионерский стан на ангинской земле^41, 16^0a-фот. цв.^GЕ. Орлова"));
            result.Fields.Add(Parse(922, "^FЮдин Ю.^?Юрий^CСергей Левченко: \"Форсайт Байкальского региона\" должен стать ежегодным^42^0a-фот.^GЮ. Юдин"));
            result.Fields.Add(Parse(922, "^FМихайлов Ю.^?Юрий^CЗдоровье всего дороже^42^0a-фот.^GЮ. Михайлов"));
            result.Fields.Add(Parse(922, "^FИлошваи А.^?Артем^CВеликий сын нашей страны^43^0a-фот.^GА. Илошваи"));
            result.Fields.Add(Parse(922, "^FБагаев Ю.^?Юрий^CВыбор сделан^44^0a-фот.^GЮ. Багаев"));
            result.Fields.Add(Parse(922, "^FЮдин Ю.^?Юрий^CИнновации для развития Приангарья^44^GЮ. Юдин"));
            result.Fields.Add(Parse(922, "^FВиговская А.^?Анна^CГубернаторская оценка^45^0a-фот.^GА. Виговская"));
            result.Fields.Add(Parse(922, "^FАндреева О.^?Ольга^CЕвродрова - альтернативное топливо^46^0a-фот.^GО. Андреева"));
            result.Fields.Add(Parse(922, "^FШагунова Л.^?Людмила^CСделано у нас^47^0a-фот.^GЛ. Шагунова"));
            result.Fields.Add(Parse(922, "^FМамонтова Ю.^?Юлия^CСледствие вели прокуроры^48^0a-фот.^GЮ. Мамонтова"));
            result.Fields.Add(Parse(922, "^FШагунова Л.^?Людмила^CСуглан для северян^410^0a-фот.^GЛ. Шагунова"));
            result.Fields.Add(Parse(922, "^FОрлова Е.^?Елена^CЗвезды вновь на Байкале^412^0a-фот.^GЕ. Орлова"));
            result.Fields.Add(Parse(922, "^FВиговская А.^?Анна^C\"Омулевая бочка\" в подарок к юбилею^412^0a-фот.^GА. Виговская"));
            result.Fields.Add(Parse(922, "^FЛаврентьев Ю. В.^?Юрий Васильевич^2Белых Е.^,Екатерина^S570 беседовала^CНаши цели благодарны, наши помыслы чисты!^413^0a-фот.^GЮ. В. Лаврентьев ; беседовала Е. Белых"));
            result.Fields.Add(Parse(922, "^FШагунова Л.^?Людмила^CКитай становится ближе^413^0a-фот.^GЛ. Шагунова"));
            result.Fields.Add(Parse(922, "^FИванишина Н.^?Наталья^CМастер-универсал^414^0a-фот.^GН. Иванишина"));
            result.Fields.Add(Parse(922, "^FЮдин Ю.^?Юрий^CАфинское золото Федора Балтуева^415^0a-фот.^GЮ. Юдин"));
            result.Fields.Add(Parse(922, "^FЮдин Ю.^?Юрий^CПамятник водолазам ВОВ открылся в Слюдянке^416^0a-фот. цв.^GЮ. Юдин"));
            result.Fields.Add(Parse(910, "^A0^B1^FМОЭ^C20170913^DФ403^E"));
            result.Fields.Add(Parse(910, "^A0^B2^FМОЭ^C20170913^DФ304^E"));
            result.Fields.Add(Parse(910, "^A0^B3^FМОЭ^C20170913^DФ304^E"));
            result.Fields.Add(Parse(910, "^A0^B4^FМОЭ^DФ104^U^C20170914"));
            result.Fields.Add(Parse(910, "^A0^B5^DЭБ^G2017^H^E^C20170915"));
            result.Fields.Add(Parse(905, "^D1^J1^S1^21"));
            result.Fields.Add(Parse(999, "1"));

            return result;
        }

        [TestMethod]
        public void MagazineIssueInfo_Construction_1()
        {
            var issue = new MagazineIssueInfo();
            Assert.IsNull(issue.Articles);
            Assert.IsNull(issue.Description);
            Assert.IsNull(issue.DocumentCode);
            Assert.IsNull(issue.Exemplars);
            Assert.AreEqual(0, issue.LoanCount);
            Assert.AreEqual(0, issue.Mfn);
            Assert.IsNull(issue.MagazineCode);
            Assert.IsNull(issue.Number);
            Assert.IsNull(issue.NumberForSorting);
            Assert.IsNull(issue.Supplement);
            Assert.IsNull(issue.Volume);
            Assert.IsNull(issue.UserData);
        }

        [TestMethod]
        public void MagazienIssueInfo_Parse_1()
        {
            var record = _GetIssueWithoutArticles();
            var issue = MagazineIssueInfo.Parse(record);
            Assert.IsNotNull(issue!.Articles);
            Assert.AreEqual(0, issue.Articles!.Length);
            Assert.IsNotNull(issue.Exemplars);
            Assert.AreEqual(1, issue.Exemplars!.Length);
        }

        [TestMethod]
        public void MagazienIssueInfo_Parse_2()
        {
            var record = _GetIssueWithArticles();
            var issue = MagazineIssueInfo.Parse(record);
            Assert.IsNotNull(issue!.Articles);
            Assert.AreEqual(18, issue.Articles!.Length);
            Assert.IsNotNull(issue.Exemplars);
            Assert.AreEqual(5, issue.Exemplars!.Length);
        }

        private void _TestSerialization_1
            (
                MagazineIssueInfo first
            )
        {
            var bytes = first.SaveToMemory();
            var second = bytes.RestoreObjectFromMemory<MagazineIssueInfo>();
            Assert.IsNotNull(second);
            Assert.AreEqual(first.Mfn, second!.Mfn);
            Assert.AreEqual(first.Description, second.Description);
            Assert.AreEqual(first.DocumentCode, second.DocumentCode);
            Assert.AreEqual(first.MagazineCode, second.MagazineCode);
            Assert.AreEqual(first.Number, second.Number);
            Assert.AreEqual(first.Volume, second.Volume);
            Assert.AreEqual(first.Year, second.Year);
            if (first.Articles is not null)
            {
                Assert.IsNotNull(second!.Articles);
                Assert.AreEqual(first.Articles.Length, second.Articles!.Length);
            }

            if (first.Exemplars is not null)
            {
                Assert.IsNotNull(second.Exemplars);
                Assert.AreEqual(first.Exemplars.Length, second.Exemplars!.Length);
            }
        }

        [TestMethod]
        public void MagazineIssueInfo_Serialization_1()
        {
            var issue = new MagazineIssueInfo();
            _TestSerialization_1(issue);

            issue = MagazineIssueInfo.Parse(_GetIssueWithoutArticles());
            _TestSerialization_1(issue);

            issue = MagazineIssueInfo.Parse(_GetIssueWithArticles());
            _TestSerialization_1(issue);
        }

        [TestMethod]
        public void MagazineIssueInfo_ToXml_1()
        {
            var issue = new MagazineIssueInfo();
            Assert.AreEqual("<issue />", XmlUtility.SerializeShort(issue));

            issue = MagazineIssueInfo.Parse(_GetIssueWithoutArticles());
            Assert.AreEqual("<issue index=\"Ш620/1992/12\" document-code=\"Ш620/1992/12\" magazine-code=\"Ш620\" year=\"1992\" number=\"12\" worksheet=\"NJP\"><exemplar status=\"p\" number=\"1\" date=\"?\" place=\"ФП\" binding-index=\"Ш620/1992/Подшивка № 10028 июль-дек. (7-12)\" binding-number=\"П10028\" mfn=\"0\" marked=\"false\" /></issue>", XmlUtility.SerializeShort(issue));

            issue = MagazineIssueInfo.Parse(_GetIssueWithArticles());
            Assert.AreEqual("<issue index=\"О174142/2017/102\" document-code=\"О174142/2017/102\" magazine-code=\"О174142\" year=\"2017\" number=\"102\" supplement=\"13-19 сентября\" worksheet=\"NJ\"><article><author familyName=\"Орлова\" initials=\"Е.\" fullName=\"Елена\" cantBeInverted=\"false\" /><title title=\"Миссионерский стан на ангинской земле\" first=\"Е. Орлова\" /></article><article><author familyName=\"Юдин\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Сергей Левченко: &quot;Форсайт Байкальского региона&quot; должен стать ежегодным\" first=\"Ю. Юдин\" /></article><article><author familyName=\"Михайлов\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Здоровье всего дороже\" first=\"Ю. Михайлов\" /></article><article><author familyName=\"Илошваи\" initials=\"А.\" fullName=\"Артем\" cantBeInverted=\"false\" /><title title=\"Великий сын нашей страны\" first=\"А. Илошваи\" /></article><article><author familyName=\"Багаев\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Выбор сделан\" first=\"Ю. Багаев\" /></article><article><author familyName=\"Юдин\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Инновации для развития Приангарья\" first=\"Ю. Юдин\" /></article><article><author familyName=\"Виговская\" initials=\"А.\" fullName=\"Анна\" cantBeInverted=\"false\" /><title title=\"Губернаторская оценка\" first=\"А. Виговская\" /></article><article><author familyName=\"Андреева\" initials=\"О.\" fullName=\"Ольга\" cantBeInverted=\"false\" /><title title=\"Евродрова - альтернативное топливо\" first=\"О. Андреева\" /></article><article><author familyName=\"Шагунова\" initials=\"Л.\" fullName=\"Людмила\" cantBeInverted=\"false\" /><title title=\"Сделано у нас\" first=\"Л. Шагунова\" /></article><article><author familyName=\"Мамонтова\" initials=\"Ю.\" fullName=\"Юлия\" cantBeInverted=\"false\" /><title title=\"Следствие вели прокуроры\" first=\"Ю. Мамонтова\" /></article><article><author familyName=\"Шагунова\" initials=\"Л.\" fullName=\"Людмила\" cantBeInverted=\"false\" /><title title=\"Суглан для северян\" first=\"Л. Шагунова\" /></article><article><author familyName=\"Орлова\" initials=\"Е.\" fullName=\"Елена\" cantBeInverted=\"false\" /><title title=\"Звезды вновь на Байкале\" first=\"Е. Орлова\" /></article><article><author familyName=\"Виговская\" initials=\"А.\" fullName=\"Анна\" cantBeInverted=\"false\" /><title title=\"&quot;Омулевая бочка&quot; в подарок к юбилею\" first=\"А. Виговская\" /></article><article><author familyName=\"Лаврентьев\" initials=\"Ю. В.\" fullName=\"Юрий Васильевич\" cantBeInverted=\"false\" /><author familyName=\"Белых\" initials=\"Е.\" fullName=\"Екатерина\" cantBeInverted=\"false\" /><title title=\"Наши цели благодарны, наши помыслы чисты!\" first=\"Ю. В. Лаврентьев ; беседовала Е. Белых\" /></article><article><author familyName=\"Шагунова\" initials=\"Л.\" fullName=\"Людмила\" cantBeInverted=\"false\" /><title title=\"Китай становится ближе\" first=\"Л. Шагунова\" /></article><article><author familyName=\"Иванишина\" initials=\"Н.\" fullName=\"Наталья\" cantBeInverted=\"false\" /><title title=\"Мастер-универсал\" first=\"Н. Иванишина\" /></article><article><author familyName=\"Юдин\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Афинское золото Федора Балтуева\" first=\"Ю. Юдин\" /></article><article><author familyName=\"Юдин\" initials=\"Ю.\" fullName=\"Юрий\" cantBeInverted=\"false\" /><title title=\"Памятник водолазам ВОВ открылся в Слюдянке\" first=\"Ю. Юдин\" /></article><exemplar status=\"0\" number=\"1\" date=\"20170913\" place=\"Ф403\" channel=\"МОЭ\" mfn=\"0\" marked=\"false\" /><exemplar status=\"0\" number=\"2\" date=\"20170913\" place=\"Ф304\" channel=\"МОЭ\" mfn=\"0\" marked=\"false\" /><exemplar status=\"0\" number=\"3\" date=\"20170913\" place=\"Ф304\" channel=\"МОЭ\" mfn=\"0\" marked=\"false\" /><exemplar status=\"0\" number=\"4\" date=\"20170914\" place=\"Ф104\" channel=\"МОЭ\" mfn=\"0\" marked=\"false\" /><exemplar status=\"0\" number=\"5\" date=\"20170915\" place=\"ЭБ\" mfn=\"0\" marked=\"false\" /><loanCount>1</loanCount></issue>", XmlUtility.SerializeShort(issue));
        }

        [TestMethod]
        public void MagazineIssueInfo_ToJson_1()
        {
            var issue = new MagazineIssueInfo();
            Assert.AreEqual("{\"mfn\":0,\"loanCount\":0}", JsonUtility.SerializeShort(issue));

            issue = MagazineIssueInfo.Parse(_GetIssueWithoutArticles());
            Assert.AreEqual("{\"mfn\":0,\"index\":\"\\u0428620/1992/12\",\"document-code\":\"\\u0428620/1992/12\",\"magazine-code\":\"\\u0428620\",\"year\":\"1992\",\"number\":\"12\",\"worksheet\":\"NJP\",\"articles\":[],\"exemplars\":[{\"status\":\"p\",\"number\":\"1\",\"date\":\"?\",\"place\":\"\\u0424\\u041F\",\"binding-index\":\"\\u0428620/1992/\\u041F\\u043E\\u0434\\u0448\\u0438\\u0432\\u043A\\u0430 \\u2116 10028 \\u0438\\u044E\\u043B\\u044C-\\u0434\\u0435\\u043A. (7-12)\",\"binding-number\":\"\\u041F10028\",\"mfn\":0,\"marked\":false}],\"loanCount\":0}", JsonUtility.SerializeShort(issue));

            issue = MagazineIssueInfo.Parse(_GetIssueWithArticles());
            Assert.AreEqual("{\"mfn\":0,\"index\":\"\\u041E174142/2017/102\",\"document-code\":\"\\u041E174142/2017/102\",\"magazine-code\":\"\\u041E174142\",\"year\":\"2017\",\"number\":\"102\",\"supplement\":\"13-19 \\u0441\\u0435\\u043D\\u0442\\u044F\\u0431\\u0440\\u044F\",\"worksheet\":\"NJ\",\"articles\":[{\"authors\":[{\"familyName\":\"\\u041E\\u0440\\u043B\\u043E\\u0432\\u0430\",\"initials\":\"\\u0415.\",\"fullName\":\"\\u0415\\u043B\\u0435\\u043D\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u041C\\u0438\\u0441\\u0441\\u0438\\u043E\\u043D\\u0435\\u0440\\u0441\\u043A\\u0438\\u0439 \\u0441\\u0442\\u0430\\u043D \\u043D\\u0430 \\u0430\\u043D\\u0433\\u0438\\u043D\\u0441\\u043A\\u043E\\u0439 \\u0437\\u0435\\u043C\\u043B\\u0435\",\"first\":\"\\u0415. \\u041E\\u0440\\u043B\\u043E\\u0432\\u0430\"}},{\"authors\":[{\"familyName\":\"\\u042E\\u0434\\u0438\\u043D\",\"initials\":\"\\u042E.\",\"fullName\":\"\\u042E\\u0440\\u0438\\u0439\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0421\\u0435\\u0440\\u0433\\u0435\\u0439 \\u041B\\u0435\\u0432\\u0447\\u0435\\u043D\\u043A\\u043E: \\u0022\\u0424\\u043E\\u0440\\u0441\\u0430\\u0439\\u0442 \\u0411\\u0430\\u0439\\u043A\\u0430\\u043B\\u044C\\u0441\\u043A\\u043E\\u0433\\u043E \\u0440\\u0435\\u0433\\u0438\\u043E\\u043D\\u0430\\u0022 \\u0434\\u043E\\u043B\\u0436\\u0435\\u043D \\u0441\\u0442\\u0430\\u0442\\u044C \\u0435\\u0436\\u0435\\u0433\\u043E\\u0434\\u043D\\u044B\\u043C\",\"first\":\"\\u042E. \\u042E\\u0434\\u0438\\u043D\"}},{\"authors\":[{\"familyName\":\"\\u041C\\u0438\\u0445\\u0430\\u0439\\u043B\\u043E\\u0432\",\"initials\":\"\\u042E.\",\"fullName\":\"\\u042E\\u0440\\u0438\\u0439\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0417\\u0434\\u043E\\u0440\\u043E\\u0432\\u044C\\u0435 \\u0432\\u0441\\u0435\\u0433\\u043E \\u0434\\u043E\\u0440\\u043E\\u0436\\u0435\",\"first\":\"\\u042E. \\u041C\\u0438\\u0445\\u0430\\u0439\\u043B\\u043E\\u0432\"}},{\"authors\":[{\"familyName\":\"\\u0418\\u043B\\u043E\\u0448\\u0432\\u0430\\u0438\",\"initials\":\"\\u0410.\",\"fullName\":\"\\u0410\\u0440\\u0442\\u0435\\u043C\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0412\\u0435\\u043B\\u0438\\u043A\\u0438\\u0439 \\u0441\\u044B\\u043D \\u043D\\u0430\\u0448\\u0435\\u0439 \\u0441\\u0442\\u0440\\u0430\\u043D\\u044B\",\"first\":\"\\u0410. \\u0418\\u043B\\u043E\\u0448\\u0432\\u0430\\u0438\"}},{\"authors\":[{\"familyName\":\"\\u0411\\u0430\\u0433\\u0430\\u0435\\u0432\",\"initials\":\"\\u042E.\",\"fullName\":\"\\u042E\\u0440\\u0438\\u0439\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0412\\u044B\\u0431\\u043E\\u0440 \\u0441\\u0434\\u0435\\u043B\\u0430\\u043D\",\"first\":\"\\u042E. \\u0411\\u0430\\u0433\\u0430\\u0435\\u0432\"}},{\"authors\":[{\"familyName\":\"\\u042E\\u0434\\u0438\\u043D\",\"initials\":\"\\u042E.\",\"fullName\":\"\\u042E\\u0440\\u0438\\u0439\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0418\\u043D\\u043D\\u043E\\u0432\\u0430\\u0446\\u0438\\u0438 \\u0434\\u043B\\u044F \\u0440\\u0430\\u0437\\u0432\\u0438\\u0442\\u0438\\u044F \\u041F\\u0440\\u0438\\u0430\\u043D\\u0433\\u0430\\u0440\\u044C\\u044F\",\"first\":\"\\u042E. \\u042E\\u0434\\u0438\\u043D\"}},{\"authors\":[{\"familyName\":\"\\u0412\\u0438\\u0433\\u043E\\u0432\\u0441\\u043A\\u0430\\u044F\",\"initials\":\"\\u0410.\",\"fullName\":\"\\u0410\\u043D\\u043D\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0413\\u0443\\u0431\\u0435\\u0440\\u043D\\u0430\\u0442\\u043E\\u0440\\u0441\\u043A\\u0430\\u044F \\u043E\\u0446\\u0435\\u043D\\u043A\\u0430\",\"first\":\"\\u0410. \\u0412\\u0438\\u0433\\u043E\\u0432\\u0441\\u043A\\u0430\\u044F\"}},{\"authors\":[{\"familyName\":\"\\u0410\\u043D\\u0434\\u0440\\u0435\\u0435\\u0432\\u0430\",\"initials\":\"\\u041E.\",\"fullName\":\"\\u041E\\u043B\\u044C\\u0433\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0415\\u0432\\u0440\\u043E\\u0434\\u0440\\u043E\\u0432\\u0430 - \\u0430\\u043B\\u044C\\u0442\\u0435\\u0440\\u043D\\u0430\\u0442\\u0438\\u0432\\u043D\\u043E\\u0435 \\u0442\\u043E\\u043F\\u043B\\u0438\\u0432\\u043E\",\"first\":\"\\u041E. \\u0410\\u043D\\u0434\\u0440\\u0435\\u0435\\u0432\\u0430\"}},{\"authors\":[{\"familyName\":\"\\u0428\\u0430\\u0433\\u0443\\u043D\\u043E\\u0432\\u0430\",\"initials\":\"\\u041B.\",\"fullName\":\"\\u041B\\u044E\\u0434\\u043C\\u0438\\u043B\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0421\\u0434\\u0435\\u043B\\u0430\\u043D\\u043E \\u0443 \\u043D\\u0430\\u0441\",\"first\":\"\\u041B. \\u0428\\u0430\\u0433\\u0443\\u043D\\u043E\\u0432\\u0430\"}},{\"authors\":[{\"familyName\":\"\\u041C\\u0430\\u043C\\u043E\\u043D\\u0442\\u043E\\u0432\\u0430\",\"initials\":\"\\u042E.\",\"fullName\":\"\\u042E\\u043B\\u0438\\u044F\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0421\\u043B\\u0435\\u0434\\u0441\\u0442\\u0432\\u0438\\u0435 \\u0432\\u0435\\u043B\\u0438 \\u043F\\u0440\\u043E\\u043A\\u0443\\u0440\\u043E\\u0440\\u044B\",\"first\":\"\\u042E. \\u041C\\u0430\\u043C\\u043E\\u043D\\u0442\\u043E\\u0432\\u0430\"}},{\"authors\":[{\"familyName\":\"\\u0428\\u0430\\u0433\\u0443\\u043D\\u043E\\u0432\\u0430\",\"initials\":\"\\u041B.\",\"fullName\":\"\\u041B\\u044E\\u0434\\u043C\\u0438\\u043B\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0421\\u0443\\u0433\\u043B\\u0430\\u043D \\u0434\\u043B\\u044F \\u0441\\u0435\\u0432\\u0435\\u0440\\u044F\\u043D\",\"first\":\"\\u041B. \\u0428\\u0430\\u0433\\u0443\\u043D\\u043E\\u0432\\u0430\"}},{\"authors\":[{\"familyName\":\"\\u041E\\u0440\\u043B\\u043E\\u0432\\u0430\",\"initials\":\"\\u0415.\",\"fullName\":\"\\u0415\\u043B\\u0435\\u043D\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0417\\u0432\\u0435\\u0437\\u0434\\u044B \\u0432\\u043D\\u043E\\u0432\\u044C \\u043D\\u0430 \\u0411\\u0430\\u0439\\u043A\\u0430\\u043B\\u0435\",\"first\":\"\\u0415. \\u041E\\u0440\\u043B\\u043E\\u0432\\u0430\"}},{\"authors\":[{\"familyName\":\"\\u0412\\u0438\\u0433\\u043E\\u0432\\u0441\\u043A\\u0430\\u044F\",\"initials\":\"\\u0410.\",\"fullName\":\"\\u0410\\u043D\\u043D\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0022\\u041E\\u043C\\u0443\\u043B\\u0435\\u0432\\u0430\\u044F \\u0431\\u043E\\u0447\\u043A\\u0430\\u0022 \\u0432 \\u043F\\u043E\\u0434\\u0430\\u0440\\u043E\\u043A \\u043A \\u044E\\u0431\\u0438\\u043B\\u0435\\u044E\",\"first\":\"\\u0410. \\u0412\\u0438\\u0433\\u043E\\u0432\\u0441\\u043A\\u0430\\u044F\"}},{\"authors\":[{\"familyName\":\"\\u041B\\u0430\\u0432\\u0440\\u0435\\u043D\\u0442\\u044C\\u0435\\u0432\",\"initials\":\"\\u042E. \\u0412.\",\"fullName\":\"\\u042E\\u0440\\u0438\\u0439 \\u0412\\u0430\\u0441\\u0438\\u043B\\u044C\\u0435\\u0432\\u0438\\u0447\",\"cantBeInverted\":false},{\"familyName\":\"\\u0411\\u0435\\u043B\\u044B\\u0445\",\"initials\":\"\\u0415.\",\"fullName\":\"\\u0415\\u043A\\u0430\\u0442\\u0435\\u0440\\u0438\\u043D\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u041D\\u0430\\u0448\\u0438 \\u0446\\u0435\\u043B\\u0438 \\u0431\\u043B\\u0430\\u0433\\u043E\\u0434\\u0430\\u0440\\u043D\\u044B, \\u043D\\u0430\\u0448\\u0438 \\u043F\\u043E\\u043C\\u044B\\u0441\\u043B\\u044B \\u0447\\u0438\\u0441\\u0442\\u044B!\",\"first\":\"\\u042E. \\u0412. \\u041B\\u0430\\u0432\\u0440\\u0435\\u043D\\u0442\\u044C\\u0435\\u0432 ; \\u0431\\u0435\\u0441\\u0435\\u0434\\u043E\\u0432\\u0430\\u043B\\u0430 \\u0415. \\u0411\\u0435\\u043B\\u044B\\u0445\"}},{\"authors\":[{\"familyName\":\"\\u0428\\u0430\\u0433\\u0443\\u043D\\u043E\\u0432\\u0430\",\"initials\":\"\\u041B.\",\"fullName\":\"\\u041B\\u044E\\u0434\\u043C\\u0438\\u043B\\u0430\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u041A\\u0438\\u0442\\u0430\\u0439 \\u0441\\u0442\\u0430\\u043D\\u043E\\u0432\\u0438\\u0442\\u0441\\u044F \\u0431\\u043B\\u0438\\u0436\\u0435\",\"first\":\"\\u041B. \\u0428\\u0430\\u0433\\u0443\\u043D\\u043E\\u0432\\u0430\"}},{\"authors\":[{\"familyName\":\"\\u0418\\u0432\\u0430\\u043D\\u0438\\u0448\\u0438\\u043D\\u0430\",\"initials\":\"\\u041D.\",\"fullName\":\"\\u041D\\u0430\\u0442\\u0430\\u043B\\u044C\\u044F\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u041C\\u0430\\u0441\\u0442\\u0435\\u0440-\\u0443\\u043D\\u0438\\u0432\\u0435\\u0440\\u0441\\u0430\\u043B\",\"first\":\"\\u041D. \\u0418\\u0432\\u0430\\u043D\\u0438\\u0448\\u0438\\u043D\\u0430\"}},{\"authors\":[{\"familyName\":\"\\u042E\\u0434\\u0438\\u043D\",\"initials\":\"\\u042E.\",\"fullName\":\"\\u042E\\u0440\\u0438\\u0439\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u0410\\u0444\\u0438\\u043D\\u0441\\u043A\\u043E\\u0435 \\u0437\\u043E\\u043B\\u043E\\u0442\\u043E \\u0424\\u0435\\u0434\\u043E\\u0440\\u0430 \\u0411\\u0430\\u043B\\u0442\\u0443\\u0435\\u0432\\u0430\",\"first\":\"\\u042E. \\u042E\\u0434\\u0438\\u043D\"}},{\"authors\":[{\"familyName\":\"\\u042E\\u0434\\u0438\\u043D\",\"initials\":\"\\u042E.\",\"fullName\":\"\\u042E\\u0440\\u0438\\u0439\",\"cantBeInverted\":false}],\"title\":{\"title\":\"\\u041F\\u0430\\u043C\\u044F\\u0442\\u043D\\u0438\\u043A \\u0432\\u043E\\u0434\\u043E\\u043B\\u0430\\u0437\\u0430\\u043C \\u0412\\u041E\\u0412 \\u043E\\u0442\\u043A\\u0440\\u044B\\u043B\\u0441\\u044F \\u0432 \\u0421\\u043B\\u044E\\u0434\\u044F\\u043D\\u043A\\u0435\",\"first\":\"\\u042E. \\u042E\\u0434\\u0438\\u043D\"}}],\"exemplars\":[{\"status\":\"0\",\"number\":\"1\",\"date\":\"20170913\",\"place\":\"\\u0424403\",\"channel\":\"\\u041C\\u041E\\u042D\",\"mfn\":0,\"marked\":false},{\"status\":\"0\",\"number\":\"2\",\"date\":\"20170913\",\"place\":\"\\u0424304\",\"channel\":\"\\u041C\\u041E\\u042D\",\"mfn\":0,\"marked\":false},{\"status\":\"0\",\"number\":\"3\",\"date\":\"20170913\",\"place\":\"\\u0424304\",\"channel\":\"\\u041C\\u041E\\u042D\",\"mfn\":0,\"marked\":false},{\"status\":\"0\",\"number\":\"4\",\"date\":\"20170914\",\"place\":\"\\u0424104\",\"channel\":\"\\u041C\\u041E\\u042D\",\"mfn\":0,\"marked\":false},{\"status\":\"0\",\"number\":\"5\",\"date\":\"20170915\",\"place\":\"\\u042D\\u0411\",\"mfn\":0,\"marked\":false}],\"loanCount\":1}", JsonUtility.SerializeShort(issue));
        }

        [TestMethod]
        public void MagazineIssueInfo_Verify_1()
        {
            var issue = new MagazineIssueInfo();
            Assert.IsFalse(issue.Verify(false));

            issue = MagazineIssueInfo.Parse(_GetIssueWithoutArticles());
            Assert.IsTrue(issue.Verify(false));

            issue = MagazineIssueInfo.Parse(_GetIssueWithArticles());
            Assert.IsTrue(issue.Verify(false));
        }
    }
}
