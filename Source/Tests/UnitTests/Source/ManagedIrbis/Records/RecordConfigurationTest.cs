// ReSharper disable IdentifierTypo
// ReSharper disable CheckNamespace
// ReSharper disable StringLiteralTypo
// ReSharper disable UseObjectOrCollectionInitializer

using System.IO;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using ManagedIrbis;
using ManagedIrbis.Records;

#nullable enable

namespace UnitTests.Source.ManagedIrbis.Records
{
    [TestClass]
    public sealed class RecordConfigurationTest
        : Common.CommonUnitTest
    {
        private Record _GetRecord() => new Record()
            .Add (10, "^a5-7110-0177-9^d300.00")
            .Add (11, "^a0378-5955")
            .Add (60, "14")
            .Add (101, "rus")
            .Add (101, "eng")
            .Add (102, "RU")
            .Add (102, "US")
            .Add (210, "^aМосква^cВся Москва^d1993")
            .Add (675, "37(470.311)(03)")
            .Add (902, "^aГПНТБ СО РАН")
            .Add (903, "37/К 88-602720")
            .Add (905, "^F2^21")
            .Add (907, "^A20020530^BДСМ^CПК")
            .Add (907, "^A20081218^BОЛН^CКР")
            .Add (908, "К 88")
            .Add (910, "^A0^B32^C20070104^DБИНТ^E7.50^H107206G^=2^U2004/7^S20070104^!ХР")
            .Add (910, "^A0^B33^C20070104^DБИНТ^E60.00^H107216G^U2004/7^S20070104^!ХР")
            .Add (910, "^A0^B557^C19990924^DЧЗ^H107236G^=2^U2004/7")
            .Add (910, "^A0^B558^C19990924^DЧЗ^H107246G^=2^U2004/7")
            .Add (910, "^A0^B559^C19990924^DЧЗ^H107256G^=2^U2004/7")
            .Add (910, "^AU^B556^C19990924^DХР^E2400^H107226G^112^U1996/28^Y60")
            .Add (910, "^AU^BЗИ-1^C20071226^DЖГ^S20140604^125^!КДИ^01^TЗИ")
            .Add (920, "PAZK")
            .Add (950, "картинка")
            .Add (951, "^AПример PDF-файла.PDF^TПример внешнего объекта в виде PDF-файла - с постраничным просмотром^N14")
            .Add (953, "ресурс")
            .Add (999, "0000002")
        ;

        [TestMethod]
        [Description ("Конструктор по умолчанию")]
        public void RecordConfiguration_Constructor_1()
        {
            var configuration = new RecordConfiguration();
            Assert.AreEqual ("@all", configuration.AllFormat);
            Assert.AreEqual ("@brief", configuration.BriefFormat);
            Assert.AreEqual (102, configuration.CountryTag);
            Assert.AreEqual (905, configuration.CustomizationTag);
            Assert.AreEqual (910, configuration.ExemplarTag);
            Assert.AreEqual (951, configuration.FullTextTag);
            Assert.AreEqual (902, configuration.HolderTag);
            Assert.AreEqual (950, configuration.ImageTag);
            Assert.AreEqual (903, configuration.IndexTag);
            Assert.AreEqual (10, configuration.IsbnTag);
            Assert.AreEqual (11, configuration.IssnTag);
            Assert.AreEqual (60, configuration.KnowledgeTag);
            Assert.AreEqual (101, configuration.LanguageTag);
            Assert.AreEqual (907, configuration.OperatorTag);
            Assert.AreEqual (999, configuration.RentalTag);
            Assert.AreEqual (953, configuration.ResourceTag);
            Assert.AreEqual (920, configuration.WorksheetTag);
        }

        [TestMethod]
        [Description ("Свойство AllFormat")]
        public void RecordConfiguration_AllFormat_1()
        {
            const string newValue = "@newAll";
            var configuration = new RecordConfiguration();
            configuration.AllFormat = newValue;
            Assert.AreEqual (newValue, configuration.AllFormat);
        }

        [TestMethod]
        [Description ("Свойство BriedFormat")]
        public void RecordConfiguration_BriefFormat_1()
        {
            const string newValue = "@newBrief";
            var configuration = new RecordConfiguration();
            configuration.BriefFormat = newValue;
            Assert.AreEqual (newValue, configuration.BriefFormat);
        }

        [TestMethod]
        [Description ("Свойство CountryTag")]
        public void RecordConfiguration_CountryTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.CountryTag = newValue;
            Assert.AreEqual (newValue, configuration.CountryTag);
        }

        [TestMethod]
        [Description ("Свойство CustomizationTag")]
        public void RecordConfiguration_CustomizationTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.CustomizationTag = newValue;
            Assert.AreEqual (newValue, configuration.CustomizationTag);
        }

        [TestMethod]
        [Description ("Свойство ExemplarTag")]
        public void RecordConfiguration_ExemplarTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.ExemplarTag = newValue;
            Assert.AreEqual (newValue, configuration.ExemplarTag);
        }

        [TestMethod]
        [Description ("Свойство FullTextTag")]
        public void RecordConfiguration_FullTextTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.FullTextTag = newValue;
            Assert.AreEqual (newValue, configuration.FullTextTag);
        }

        [TestMethod]
        [Description ("Свойство HolderTag")]
        public void RecordConfiguration_HolderTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.HolderTag = newValue;
            Assert.AreEqual (newValue, configuration.HolderTag);
        }

        [TestMethod]
        [Description ("Свойство ImageTag")]
        public void RecordConfiguration_ImageTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.ImageTag = newValue;
            Assert.AreEqual (newValue, configuration.ImageTag);
        }

        [TestMethod]
        [Description ("Свойство IndexTag")]
        public void RecordConfiguration_IndexTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.IndexTag = newValue;
            Assert.AreEqual (newValue, configuration.IndexTag);
        }

        [TestMethod]
        [Description ("Свойство IsbnTag")]
        public void RecordConfiguration_IsbnTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.IsbnTag = newValue;
            Assert.AreEqual (newValue, configuration.IsbnTag);
        }

        [TestMethod]
        [Description ("Свойство IssnTag")]
        public void RecordConfiguration_IssnTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.IssnTag = newValue;
            Assert.AreEqual (newValue, configuration.IssnTag);
        }

        [TestMethod]
        [Description ("Свойство KnowledgeTag")]
        public void RecordConfiguration_KnowledgeTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.KnowledgeTag = newValue;
            Assert.AreEqual (newValue, configuration.KnowledgeTag);
        }

        [TestMethod]
        [Description ("Свойство LanguageTag")]
        public void RecordConfiguration_LanguageTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.LanguageTag = newValue;
            Assert.AreEqual (newValue, configuration.LanguageTag);
        }

        [TestMethod]
        [Description ("Свойство OperatorTag")]
        public void RecordConfiguration_OperatorTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.OperatorTag = newValue;
            Assert.AreEqual (newValue, configuration.OperatorTag);
        }

        [TestMethod]
        [Description ("Свойство RentalTag")]
        public void RecordConfiguration_RentalTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.RentalTag = newValue;
            Assert.AreEqual (newValue, configuration.RentalTag);
        }

        [TestMethod]
        [Description ("Свойство ResourceTag")]
        public void RecordConfiguration_ResourceTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.ResourceTag = newValue;
            Assert.AreEqual (newValue, configuration.ResourceTag);
        }

        [TestMethod]
        [Description ("Свойство WorksheetTag")]
        public void RecordConfiguration_WorksheetTag_1()
        {
            const int newValue = 111;
            var configuration = new RecordConfiguration();
            configuration.WorksheetTag = newValue;
            Assert.AreEqual (newValue, configuration.WorksheetTag);
        }

        [TestMethod]
        [Description ("Код страны")]
        public void RecordConfiguration_GetCountryCode_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            Assert.AreEqual ("RU", configuration.GetCountryCode (record));
        }

        [TestMethod]
        [Description ("Код страны")]
        public void RecordConfiguration_GetCountryCode_2()
        {
            var configuration = new RecordConfiguration();
            var record = new Record();
            Assert.AreEqual ("EN", configuration.GetCountryCode (record, "EN"));
        }

        [TestMethod]
        [Description ("Коды стран")]
        public void RecordConfiguration_GetCountryCodes_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var codes = configuration.GetCountryCodes (record);
            Assert.AreEqual (2, codes.Length);
            Assert.AreEqual ("RU", codes[0]);
            Assert.AreEqual ("US", codes[1]);
        }

        [TestMethod]
        [Description ("Коды стран")]
        public void RecordConfiguration_GetCountryCodes_2()
        {
            var configuration = new RecordConfiguration();
            var record = new Record();
            var codes = configuration.GetCountryCodes (record, "RU");
            Assert.AreEqual (1, codes.Length);
            Assert.AreEqual ("RU", codes[0]);
        }

        [TestMethod]
        [Description ("Коды стран")]
        public void RecordConfiguration_GetCountryCodes_3()
        {
            var configuration = new RecordConfiguration();
            var record = new Record();
            var codes = configuration.GetCountryCodes (record);
            Assert.AreEqual (0, codes.Length);
        }

        [TestMethod]
        [Description ("Поле для настройки библиографической записи")]
        public void RecordConfiguration_GetCustomizationField_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var field = configuration.GetCustomizationField (record);
            Assert.IsNotNull (field);
            Assert.AreEqual (905, field.Tag);
        }

        [TestMethod]
        [Description ("Получение конфигурации по умолчанию")]
        public void RecordConfiguration_GetDefault_1()
        {
            var configuration = RecordConfiguration.GetDefault();
            Assert.IsNotNull (configuration);
            Assert.AreEqual ("@all", configuration.AllFormat);
            Assert.AreEqual ("@brief", configuration.BriefFormat);
            Assert.AreEqual (102, configuration.CountryTag);
            Assert.AreEqual (905, configuration.CustomizationTag);
            Assert.AreEqual (910, configuration.ExemplarTag);
            Assert.AreEqual (951, configuration.FullTextTag);
            Assert.AreEqual (902, configuration.HolderTag);
            Assert.AreEqual (950, configuration.ImageTag);
            Assert.AreEqual (903, configuration.IndexTag);
            Assert.AreEqual (10, configuration.IsbnTag);
            Assert.AreEqual (11, configuration.IssnTag);
            Assert.AreEqual (60, configuration.KnowledgeTag);
            Assert.AreEqual (101, configuration.LanguageTag);
            Assert.AreEqual (907, configuration.OperatorTag);
            Assert.AreEqual (999, configuration.RentalTag);
            Assert.AreEqual (953, configuration.ResourceTag);
            Assert.AreEqual (920, configuration.WorksheetTag);
        }

        [TestMethod]
        [Description ("Экземпляры")]
        public void RecordConfiguration_GetExemplarFields_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var fields = configuration.GetExemplarFields (record);
            Assert.AreEqual (7, fields.Length);
        }

        [TestMethod]
        [Description ("Экземпляры")]
        public void RecordConfiguration_GetExemplars_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var exemplars = configuration.GetExemplars (record);
            Assert.AreEqual (7, exemplars.Length);
        }

        [TestMethod]
        [Description ("Держатель документа")]
        public void RecordConfiguration_GetHolderField_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var field = configuration.GetHolderField (record);
            Assert.IsNotNull (field);
            Assert.AreEqual (configuration.HolderTag, field.Tag);
        }

        [TestMethod]
        [Description ("Держатель документа")]
        public void RecordConfiguration_GetHolderFields_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var fields = configuration.GetHolderFields (record);
            Assert.AreEqual (1, fields.Length);
            Assert.AreEqual (configuration.HolderTag, fields[0].Tag);
        }

        [TestMethod]
        [Description ("Графические данные")]
        public void RecordConfiguration_GetImageField_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var field = configuration.GetImageField (record);
            Assert.IsNotNull (field);
            Assert.AreEqual (configuration.ImageTag, field.Tag);
        }

        [TestMethod]
        [Description ("Графические данные")]
        public void RecordConfiguration_GetImageFields_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var fields = configuration.GetImageFields (record);
            Assert.AreEqual (1, fields.Length);
            Assert.AreEqual (configuration.ImageTag, fields[0].Tag);
        }

        [TestMethod]
        [Description ("Шифр документа в базе")]
        public void RecordConfiguration_GetIndex_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var index = configuration.GetIndex (record);
            Assert.AreEqual ("37/К 88-602720", index);
        }

        [TestMethod]
        [Description ("ISBN")]
        public void RecordConfiguration_GetIsbnFields_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var isbn = configuration.GetIsbnFields (record);
            Assert.IsNotNull (isbn);
            Assert.AreEqual (1, isbn.Length);
            Assert.AreEqual (configuration.IsbnTag, isbn[0].Tag);
            Assert.AreEqual ("5-7110-0177-9", isbn[0].GetFirstSubFieldValue ('a'));
        }

        [TestMethod]
        [Description ("ISBN")]
        public void RecordConfiguration_GetIsbn_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var isbn = configuration.GetIsbn (record);
            Assert.IsNotNull (isbn);
            Assert.AreEqual (1, isbn.Length);
            Assert.AreEqual ("5-7110-0177-9", isbn[0].Isbn);
        }

        [TestMethod]
        [Description ("ISSN")]
        public void RecordConfiguration_GetIssnFields_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var issn = configuration.GetIssnFields (record);
            Assert.IsNotNull (issn);
            Assert.AreEqual (1, issn.Length);
            Assert.AreEqual (configuration.IssnTag, issn[0].Tag);
            Assert.AreEqual ("0378-5955", issn[0].GetFirstSubFieldValue ('a'));
        }

        [TestMethod]
        [Description ("ISSN")]
        public void RecordConfiguration_GetIssn_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var issn = configuration.GetIssn (record);
            Assert.IsNotNull (issn);
            Assert.AreEqual (1, issn.Length);
            Assert.AreEqual ("0378-5955", issn[0].Issn);
        }

        [TestMethod]
        [Description ("Раздел знаний")]
        public void RecordConfiguration_GetKnowledgeSection_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var section = configuration.GetKnowledgeSection (record);
            Assert.AreEqual ("14", section);
        }

        [TestMethod]
        [Description ("Код языка текста")]
        public void RecordConfiguration_GetLanguageCode_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var language = configuration.GetLanguageCode (record);
            Assert.AreEqual ("rus", language);
        }

        [TestMethod]
        [Description ("Код языка текста")]
        public void RecordConfiguration_GetLanguageCode_2()
        {
            var configuration = new RecordConfiguration();
            var record = new Record();
            var language = configuration.GetLanguageCode (record, "eng");
            Assert.AreEqual ("eng", language);
        }

        [TestMethod]
        [Description ("Коды языков текста")]
        public void RecordConfiguration_GetLanguageCodes_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var languages = configuration.GetLanguageCodes (record);
            Assert.AreEqual (2, languages.Length);
            Assert.AreEqual ("rus", languages[0]);
            Assert.AreEqual ("eng", languages[1]);
        }

        [TestMethod]
        [Description ("Коды языков текста")]
        public void RecordConfiguration_GetLanguageCodes_2()
        {
            var configuration = new RecordConfiguration();
            var record = new Record();
            var languages = configuration.GetLanguageCodes (record, "eng");
            Assert.AreEqual (1, languages.Length);
            Assert.AreEqual ("eng", languages[0]);
        }

        [TestMethod]
        [Description ("Технология")]
        public void RecordConfiguration_GetOperatorFields_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var operators = configuration.GetOperatorFields (record);
            Assert.AreEqual (2, operators.Length);
        }

        [TestMethod]
        [Description ("Количество выдач документа")]
        public void RecordConfiguration_GetRentalCount_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            Assert.AreEqual (2, configuration.GetRentalCount (record));
        }

        [TestMethod]
        [Description ("Внутренние двоичные ресурсы записи")]
        public void RecordConfiguration_GetResourceField_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var resource = configuration.GetResourceField (record);
            Assert.IsNotNull (resource);
            Assert.AreEqual (configuration.ResourceTag, resource.Tag);
        }

        [TestMethod]
        [Description ("Внутренние двоичные ресурсы записи")]
        public void RecordConfiguration_GetResourceFields_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var resources = configuration.GetResourceFields (record);
            Assert.AreEqual (1, resources.Length);
            Assert.AreEqual (configuration.ResourceTag, resources[0].Tag);
        }

        [TestMethod]
        [Description ("Технология")]
        public void RecordConfiguration_GetTechnology_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            var technology = configuration.GetTechnology (record);
            Assert.AreEqual (2, technology.Length);
        }

        [TestMethod]
        [Description ("Получение кода рабочего листа")]
        public void RecordConfiguration_GetWorksheet_1()
        {
            var configuration = new RecordConfiguration();
            var record = _GetRecord();
            Assert.AreEqual ("PAZK", configuration.GetWorksheet (record));
        }

        [TestMethod]
        [Description ("Чтение конфигурации из указанного файла")]
        public void RecordConfiguration_LoadConfiguration_1()
        {
            var fileName = Path.Combine (TestDataPath, "record-configuration.json");
            var configuration = RecordConfiguration.LoadConfiguration (fileName);
            Assert.IsNotNull (configuration);
            Assert.AreEqual ("@all", configuration.AllFormat);
            Assert.AreEqual ("@brief", configuration.BriefFormat);
            Assert.AreEqual (102, configuration.CountryTag);
            Assert.AreEqual (905, configuration.CustomizationTag);
            Assert.AreEqual (910, configuration.ExemplarTag);
            Assert.AreEqual (951, configuration.FullTextTag);
            Assert.AreEqual (902, configuration.HolderTag);
            Assert.AreEqual (950, configuration.ImageTag);
            Assert.AreEqual (903, configuration.IndexTag);
            Assert.AreEqual (10, configuration.IsbnTag);
            Assert.AreEqual (11, configuration.IssnTag);
            Assert.AreEqual (60, configuration.KnowledgeTag);
            Assert.AreEqual (101, configuration.LanguageTag);
            Assert.AreEqual (907, configuration.OperatorTag);
            Assert.AreEqual (999, configuration.RentalTag);
            Assert.AreEqual (953, configuration.ResourceTag);
            Assert.AreEqual (920, configuration.WorksheetTag);
        }

        [TestMethod]
        [Description ("Сохранение конфигурации в указанный файл")]
        public void RecordConfiguration_SaveConfiguration_1()
        {
            var configuration = RecordConfiguration.GetDefault();
            var fileName = Path.GetTempFileName();
            configuration.SaveConfiguration (fileName);
            var text = File.ReadAllText (fileName);
            Assert.AreEqual ("{\"all\":\"@all\",\"brief\":\"@brief\",\"country\":102,\"customization\":905,\"exemplar\":910,\"fulltext\":951,\"holder\":902,\"image\":950,\"index\":903,\"isbn\":10,\"issn\":11,\"knowledge\":60,\"language\":101,\"operator\":907,\"rental\":999,\"resource\":953,\"worksheet\":920}", text);
        }
    }
}
