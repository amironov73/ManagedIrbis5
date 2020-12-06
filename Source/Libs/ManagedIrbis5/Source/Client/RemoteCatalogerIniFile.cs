// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertToAutoProperty
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* RemoteCatalogerIniFile.cs -- серверный INI-файл для АРМ Каталогизатор.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM;
using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Серверный INI-файл для АРМ Каталогизатор.
    /// </summary>
    public class RemoteCatalogerIniFile
    {
        #region Constants

        /// <summary>
        /// Display section name.
        /// </summary>
        public const string Display = "Display";

        /// <summary>
        /// Entry section name.
        /// </summary>
        public const string Entry = "Entry";

        /// <summary>
        /// Main section name.
        /// </summary>
        public const string Main = "Main";

        /// <summary>
        /// Private section name.
        /// </summary>
        public const string Private = "Private";

        #endregion

        #region Properties

        /// <summary>
        /// Имя файла пакетного задания для АВТОВВОДА.
        /// </summary>
        public string AutoinFile => RequireValue
            (
                Main,
                "AUTOINFILE",
                "autoin.gbl"
            );

        /// <summary>
        /// Разрешает (значение 1) или запрещает (значение 0)
        /// автоматическое слияние двух версий записи при корректировке
        /// (при получении сообщения о несовпадении версий – в ситуации,
        /// когда одну запись пытаются одновременно откорректировать
        /// два и более пользователей) Автоматическое слияние проводится
        /// по формальному алгоритму: неповторяющиеся поля заменяются,
        /// а оригинальные значения повторяющихся полей суммируются
        /// </summary>
        public bool AutoMerge => GetBoolean
            (
                Main,
                "AUTOMERGE",
                "0"
            );

        /// <summary>
        /// Имя краткого (строкa) формата показа.
        /// </summary>
        public string BriefPft => RequireValue(Main, "BRIEFPFT", "brief.pft");

        /// <summary>
        /// Интервал в мин., по истечении которого клиент посылает
        /// на сервер уведомление о том, что он «жив».
        /// </summary>
        public int ClientTimeLive => GetValue(Main, "CLIENT_TIME_LIVE", 15);

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования
        /// для копирования.
        /// </summary>
        public string CopyMnu => RequireValue(Main, "COPYMNU", "fst.mnu");

        /// <summary>
        /// Метка поля «количество выдач» в БД ЭК.
        /// </summary>
        public string CountTag => RequireValue(Main, "DBNTAGSPROS", "999");

        /// <summary>
        /// Имя файла списка БД для АРМа Каталогизатора/Комплектатора.
        /// </summary>
        public string DatabaseList => RequireValue(Main, "DBNNAMECAT", "dbnam2.mnu");

        /// <summary>
        /// Имя формата для ФЛК документа в целом.
        /// </summary>
        public string DbnFlc => RequireValue(Entry, "DBNFLC", "dbnflc.pft");

        /// <summary>
        /// Имя базы данных по умолчанию.
        /// </summary>
        public string DefaultDb => RequireValue(Main, "DEFAULTDB", "IBIS");

        /// <summary>
        /// Имя шаблона для создания новой БД.
        /// </summary>
        public string EmptyDbn => RequireValue(Main, "EMPTYDBN", "BLANK");

        /// <summary>
        /// Метка поля «экземпляры» в БД ЭК.
        /// </summary>
        public string ExemplarTag => RequireValue(Main, "DBNTAGEKZ", "910");

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования
        /// для экспорта.
        /// </summary>
        public string ExportMenu => RequireValue(Main, "EXPORTMNU", "export.mnu");

        /// <summary>
        /// Имя файла-справочника со списком доступных РЛ.
        /// </summary>
        public string FormatMenu => RequireValue(Main, "FMTMNU", "fmt.mnu");

        /// <summary>
        /// Имя БД, содержащей тематический рубрикатор ГРНТИ.
        /// </summary>
        public string HelpDbn => RequireValue(Main, "HELPDBN", "HELP");

        /// <summary>
        /// Имя файла-справочника со списком ТВП переформатирования
        /// для импорта.
        /// </summary>
        public string ImportMenu => RequireValue(Main, "IMPORTMNU", "import.mnu");

        /// <summary>
        /// Префикс инверсии для шифра документа в БД ЭК.
        /// </summary>
        public string IndexPrefix => RequireValue(Main, "DBNPREFSHIFR", "I=");

        /// <summary>
        /// Метка поля «шифр документа» в БД ЭК.
        /// </summary>
        public string IndexTag => RequireValue(Main, "DBNTAGSHIFR", "903");

        /// <summary>
        /// INI-file.
        /// </summary>
        public IniFile Ini { get; private set; }

        /// <summary>
        /// Имя файла-справочника со списком постоянных запросов.
        /// </summary>
        public string IriMenu => RequireValue(Main, "IRIMNU", "iri.mnu");

        /// <summary>
        /// Размер порции для показа кратких описаний.
        /// </summary>
        public int MaxBriefPortion => GetValue(Main, "MAXBRIEFPORTION", 10);

        /// <summary>
        /// Максимальное количество отмеченных документов.
        /// </summary>
        public int MaxMarked => GetValue(Main, "MAXMARKED", 10);

        /// <summary>
        /// Имя файла-справочника со списком доступных форматов
        /// показа документов.
        /// </summary>
        public string PftMenu => RequireValue(Main, "PFTMNU", "pft.mnu");

        /// <summary>
        /// Имя оптимизационного файла, который определяет принцип
        /// формата ОПТИМИЗИРОВАННЫЙ (в АРМах Читатель и Каталогизатор).
        /// Для БД электронного каталога (IBIS) значение PFTW.OPT
        /// определяет в качестве оптимизированных  RTF-форматы,
        /// а значение PFTW_H.OPT – HTML-форматы
        /// </summary>
        public string PftOpt => RequireValue(Main, "PFTOPT", "pft.opt");

        /// <summary>
        /// Определяет режим работы АРМа для «читателя»,
        /// иначе преподавателя.
        /// При значении параметра 1 в АРМе будут скрыты все режимы,
        /// связанные с корректировкой, удалением, переносом данных,
        /// т. е. только просмотр и вывод на печать. При старте будет
        /// устанавливаться БД каталога, заданная в настройке
        /// (параметр «dbn» секции «private»), или БД IBIS.
        /// При значении параметра 2 в АРМе будут скрыты все режимы,
        /// связанные с корректировкой, удалением данных, но останется
        /// доступным режим переноса данных.
        /// При значении параметра 3 – вариант 2 с добавлением режима
        /// удаления данных.
        /// </summary>
        public int ReaderMode => GetValue(Main, "ReaderMode", 0);

        /// <summary>
        /// Имя дополнительного INI-файла со сценариями поиска для БД.
        /// </summary>
        public string SearchIni => RequireValue(Main, "SEARCHINI", string.Empty);

        /// <summary>
        /// Имя эталонной БД Электронного каталога.
        /// </summary>
        public string StandardDbn => RequireValue(Main, "ETALONDBN", "IBIS");

        /// <summary>
        /// Директория для сохранения временных (выходных) данных.
        /// </summary>
        public string WorkDirectory => RequireValue(Main, "WORKDIR", "/irbiswrk");

        /// <summary>
        /// Имя файла оптимизации РЛ ввода.
        /// </summary>
        public string WsOpt => RequireValue(Main, "WSOPT", "ws.opt");

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public RemoteCatalogerIniFile
            (
                IniFile iniFile
            )
        {
            Ini = iniFile;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Get boolean value
        /// </summary>
        public bool GetBoolean
            (
                string sectionName,
                string keyName,
                string defaultValue
            )
        {
            Sure.NotNullNorEmpty(sectionName, nameof(sectionName));
            Sure.NotNullNorEmpty(keyName, nameof(keyName));
            Sure.NotNullNorEmpty(defaultValue, nameof(defaultValue));

            string text = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                )
                .ThrowIfNull("Ini.GetValue");

            return Utility.ToBoolean(text);
        }

        /// <summary>
        /// Get value.
        /// </summary>
        public string? GetValue
            (
                string sectionName,
                string keyName,
                string? defaultValue
            )
        {
            Sure.NotNullNorEmpty(sectionName, nameof(sectionName));
            Sure.NotNullNorEmpty(keyName, nameof(keyName));

            var result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                );

            return result;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        public string RequireValue
            (
                string sectionName,
                string keyName,
                string defaultValue
            )
        {
            Sure.NotNullNorEmpty(sectionName, nameof(sectionName));
            Sure.NotNullNorEmpty(keyName, nameof(keyName));

            string result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                )
                .ThrowIfNull(keyName);

            return result;
        }

        /// <summary>
        /// Get value.
        /// </summary>
        public T? GetValue<T>
            (
                string sectionName,
                string keyName,
                T? defaultValue
            )
        {
            Sure.NotNullNorEmpty(sectionName, nameof(sectionName));
            Sure.NotNullNorEmpty(keyName, nameof(keyName));

            var result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                );

            return result;
        }

        /// <summary>
        /// Set value.
        /// </summary>
        public RemoteCatalogerIniFile SetValue
            (
                string sectionName,
                string keyName,
                string? value
            )
        {
            Sure.NotNullNorEmpty(sectionName, nameof(sectionName));
            Sure.NotNullNorEmpty(keyName, nameof(keyName));

            Ini.SetValue
                (
                    sectionName,
                    keyName,
                    value
                );

            return this;
        }

        /// <summary>
        /// Set value.
        /// </summary>
        public RemoteCatalogerIniFile SetValue<T>
            (
                string sectionName,
                string keyName,
                T? value
            )
        {
            Sure.NotNullNorEmpty(sectionName, nameof(sectionName));
            Sure.NotNullNorEmpty(keyName, nameof(keyName));

            Ini.SetValue
                (
                    sectionName,
                    keyName,
                    value
                );

            return this;
        }

        #endregion
    }
}
