// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local

/* RemoteCatalogerIniFile.cs -- серверный INI-файл для АРМ Каталогизатор.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;

using AM;
using AM.IO;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Серверный INI-файл для АРМ Каталогизатор.
/// </summary>
public class RemoteCatalogerIniFile
{
    #region Constants

    /// <summary>
    /// Имя секции <c>[Display]</c>.
    /// </summary>
    public const string Display = "Display";

    /// <summary>
    /// Имя секции <c>[Entry]</c>.
    /// </summary>
    public const string Entry = "Entry";

    /// <summary>
    /// Имя секции <c>[Main]</c>.
    /// </summary>
    public const string Main = "Main";

    /// <summary>
    /// Имя секции <c>[Private]</c>.
    /// </summary>
    public const string Private = "Private";

    #endregion

    #region Properties

    /// <summary>
    /// Имя файла пакетного задания для АВТОВВОДА.
    /// </summary>
    [Description ("Автоввод")]
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
    [Description ("Автоматическое слияние")]
    public bool AutoMerge => GetBoolean
        (
            Main,
            "AUTOMERGE",
            "0"
        );

    /// <summary>
    /// Имя краткого (строкa) формата показа.
    /// </summary>
    [Description ("Краткий формат")]
    public string BriefPft => RequireValue (Main, "BRIEFPFT", "brief.pft");

    /// <summary>
    /// Интервал в минутах, по истечении которого клиент посылает
    /// на сервер уведомление о том, что он «жив».
    /// </summary>
    [Description ("Интервал подтверждения")]
    public int ClientTimeLive => GetValue (Main, "CLIENT_TIME_LIVE", 15);

    /// <summary>
    /// Имя файла-справочника со списком ТВП переформатирования
    /// для копирования.
    /// </summary>
    [Description ("Справочник для копирования")]
    public string CopyMnu => RequireValue (Main, "COPYMNU", "fst.mnu");

    /// <summary>
    /// Метка поля «количество выдач» в БД ЭК.
    /// </summary>
    [Description ("Поле с количеством выдач")]
    public string CountTag => RequireValue (Main, "DBNTAGSPROS", "999");

    /// <summary>
    /// Имя файла списка БД для АРМа Каталогизатора/Комплектатора.
    /// </summary>
    [Description ("Список баз данных для Каталогизатора")]
    public string DatabaseList => RequireValue (Main, "DBNNAMECAT", "dbnam2.mnu");

    /// <summary>
    /// Имя формата для ФЛК документа в целом.
    /// </summary>
    [Description ("ФЛК документа в целом")]
    public string DbnFlc => RequireValue (Entry, "DBNFLC", "dbnflc.pft");

    /// <summary>
    /// Имя базы данных по умолчанию.
    /// </summary>
    [Description ("База по умолчанию")]
    public string DefaultDb => RequireValue (Main, "DEFAULTDB", "IBIS");

    /// <summary>
    /// Имя шаблона для создания новой БД.
    /// </summary>
    [Description ("Шаблон базы данных")]
    public string EmptyDbn => RequireValue (Main, "EMPTYDBN", "BLANK");

    /// <summary>
    /// Метка поля «экземпляры» в БД ЭК.
    /// </summary>
    [Description ("Поле \"экземпляры\"")]
    public string ExemplarTag => RequireValue (Main, "DBNTAGEKZ", "910");

    /// <summary>
    /// Имя файла-справочника со списком ТВП переформатирования
    /// для экспорта.
    /// </summary>
    [Description ("Справочник для экспорта")]
    public string ExportMenu => RequireValue (Main, "EXPORTMNU", "export.mnu");

    /// <summary>
    /// Имя файла-справочника со списком доступных РЛ.
    /// </summary>
    [Description ("Справочник рабочих листов")]
    public string FormatMenu => RequireValue (Main, "FMTMNU", "fmt.mnu");

    /// <summary>
    /// Имя БД, содержащей тематический рубрикатор ГРНТИ.
    /// </summary>
    [Description ("База ГРНТИ")]
    public string HelpDbn => RequireValue (Main, "HELPDBN", "HELP");

    /// <summary>
    /// Имя файла-справочника со списком ТВП переформатирования
    /// для импорта.
    /// </summary>
    [Description ("Справочник для импорта")]
    public string ImportMenu => RequireValue (Main, "IMPORTMNU", "import.mnu");

    /// <summary>
    /// Префикс инверсии для шифра документа в БД ЭК.
    /// </summary>
    [Description ("Префикс шифра документа")]
    public string IndexPrefix => RequireValue (Main, "DBNPREFSHIFR", "I=");

    /// <summary>
    /// Метка поля «шифр документа» в БД ЭК.
    /// </summary>
    [Description ("Метка поля \"шифр докумнета\"")]
    public string IndexTag => RequireValue (Main, "DBNTAGSHIFR", "903");

    /// <summary>
    /// INI-файл.
    /// </summary>
    [Browsable (false)]
    public IniFile Ini { get; }

    /// <summary>
    /// Имя файла-справочника со списком постоянных запросов.
    /// </summary>
    [Description ("Справочник постоянных запросов")]
    public string IriMenu => RequireValue (Main, "IRIMNU", "iri.mnu");

    /// <summary>
    /// Размер порции для показа кратких описаний.
    /// </summary>
    [Description ("Размер порции для показа")]
    public int MaxBriefPortion => GetValue (Main, "MAXBRIEFPORTION", 10);

    /// <summary>
    /// Максимальное количество отмеченных документов.
    /// </summary>
    [Description ("Максимальное количество отмеченных документов")]
    public int MaxMarked => GetValue (Main, "MAXMARKED", 10);

    /// <summary>
    /// Имя файла-справочника со списком доступных форматов
    /// показа документов.
    /// </summary>
    [Description ("Справочник форматов показа")]
    public string PftMenu => RequireValue (Main, "PFTMNU", "pft.mnu");

    /// <summary>
    /// Имя оптимизационного файла, который определяет принцип
    /// формата ОПТИМИЗИРОВАННЫЙ (в АРМах Читатель и Каталогизатор).
    /// Для БД электронного каталога (IBIS) значение PFTW.OPT
    /// определяет в качестве оптимизированных  RTF-форматы,
    /// а значение PFTW_H.OPT – HTML-форматы
    /// </summary>
    [Description ("Файл оптимизации формата")]
    public string PftOpt => RequireValue (Main, "PFTOPT", "pft.opt");

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
    [Description ("Редим работы \"для читателя\"")]
    public int ReaderMode => GetValue (Main, "ReaderMode", 0);

    /// <summary>
    /// Имя дополнительного INI-файла со сценариями поиска для БД.
    /// </summary>
    [Description ("Сценарии поиска")]
    public string SearchIni => RequireValue (Main, "SEARCHINI", string.Empty);

    /// <summary>
    /// Имя эталонной БД Электронного каталога.
    /// </summary>
    [Description ("Эталонная база")]
    public string StandardDbn => RequireValue (Main, "ETALONDBN", "IBIS");

    /// <summary>
    /// Директория для сохранения временных (выходных) данных.
    /// </summary>
    [Description ("Директория для временных данных")]
    public string WorkDirectory => RequireValue (Main, "WORKDIR", "/irbiswrk");

    /// <summary>
    /// Имя файла оптимизации РЛ ввода.
    /// </summary>
    [Description ("Файл оптимизации рабочего листа")]
    public string WsOpt => RequireValue (Main, "WSOPT", "ws.opt");

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор.
    /// </summary>
    public RemoteCatalogerIniFile
        (
            IniFile iniFile
        )
    {
        Sure.NotNull (iniFile);

        Ini = iniFile;
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение логического значения для указанного ключа.
    /// </summary>
    public bool GetBoolean
        (
            string sectionName,
            string keyName,
            string defaultValue
        )
    {
        Sure.NotNullNorEmpty (sectionName);
        Sure.NotNullNorEmpty (keyName);
        Sure.NotNullNorEmpty (defaultValue);

        var text = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                )
            .ThrowIfNull();

        return Utility.ToBoolean (text);
    }

    /// <summary>
    /// Получение строкового значения, которое может отсутствовать.
    /// </summary>
    public string? GetValue
        (
            string sectionName,
            string keyName,
            string? defaultValue
        )
    {
        Sure.NotNullNorEmpty (sectionName);
        Sure.NotNullNorEmpty (keyName);

        var result = Ini.GetValue
            (
                sectionName,
                keyName,
                defaultValue
            );

        return result;
    }

    /// <summary>
    /// Получение строкового значения, которое обязано присутствовать.
    /// </summary>
    public string RequireValue
        (
            string sectionName,
            string keyName,
            string defaultValue
        )
    {
        Sure.NotNullNorEmpty (sectionName);
        Sure.NotNullNorEmpty (keyName);

        var result = Ini.GetValue
                (
                    sectionName,
                    keyName,
                    defaultValue
                )
            .ThrowIfNull (keyName);

        return result;
    }

    /// <summary>
    /// Получение типизированного значения для указанного ключа.
    /// </summary>
    public T? GetValue<T>
        (
            string sectionName,
            string keyName,
            T? defaultValue
        )
    {
        Sure.NotNullNorEmpty (sectionName);
        Sure.NotNullNorEmpty (keyName);

        var result = Ini.GetValue
            (
                sectionName,
                keyName,
                defaultValue
            );

        return result;
    }

    /// <summary>
    /// Задание строкового значения для указанного ключа.
    /// </summary>
    public RemoteCatalogerIniFile SetValue
        (
            string sectionName,
            string keyName,
            string? value
        )
    {
        Sure.NotNullNorEmpty (sectionName);
        Sure.NotNullNorEmpty (keyName);

        Ini.SetValue
            (
                sectionName,
                keyName,
                value
            );

        return this;
    }

    /// <summary>
    /// Задание типизированного значения для указанного ключа.
    /// </summary>
    public RemoteCatalogerIniFile SetValue<T>
        (
            string sectionName,
            string keyName,
            T? value
        )
    {
        Sure.NotNullNorEmpty (sectionName);
        Sure.NotNullNorEmpty (keyName);

        Ini.SetValue
            (
                sectionName,
                keyName,
                value
            );

        return this;
    }

    /// <summary>
    /// Задание типизированного значения для указанного ключа.
    /// </summary>
    public RemoteCatalogerIniFile SetValue<T>
        (
            string sectionName,
            string keyName,
            T? value,
            T? defaultValue
        )
        where T: IComparable<T>
    {
        Sure.NotNullNorEmpty (sectionName);
        Sure.NotNullNorEmpty (keyName);

        if (value?.CompareTo (defaultValue) == 0)
        {
            value = default;
        }

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
