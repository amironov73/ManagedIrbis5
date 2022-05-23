// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo

/* HardFormat.cs -- захардкоженный формат
 * Ars Magna project, http://arsmagna.ru
 */

using System;
using System.Linq;
using System.Text;

using AM;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Fields;
using ManagedIrbis.Pft.Infrastructure.Unifors;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

namespace ManagedIrbis.Formatting;

/// <summary>
/// Захардкоженный формат.
/// </summary>
public sealed class HardFormat
{
    #region Properties

    /// <summary>
    /// Разделитель областей.
    /// </summary>
    public string? AreaSeparator { get; set; }

    #endregion

    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public HardFormat()
        : this (RecordConfiguration.GetDefault(), new NullProvider())
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HardFormat
        (
            RecordConfiguration configuration,
            ISyncProvider provider
        )
    {
        Sure.NotNull (configuration);
        Sure.NotNull (provider);

        AreaSeparator = String.Empty;
        _configuration = configuration;
        _provider = provider;
    }

    #endregion

    #region Private members

    private readonly RecordConfiguration _configuration;

    private readonly ISyncProvider _provider;

    /// <summary>
    /// Ограничители, после которых нельзя ставить точку
    /// </summary>
    private static readonly char[] _delimiters = { '!', '?', '.', ',' };

    private static char _GetLastChar
        (
            StringBuilder builder
        )
    {
        var position = builder.Length - 1;
        while (true)
        {
            if (position < 0)
            {
                break;
            }

            var result = builder[position];
            if (!char.IsWhiteSpace (result))
            {
                return result;
            }

            --position;
        }

        return '\0';
    }

    private static void _AddDot
        (
            StringBuilder builder
        )
    {
        var lastChar = _GetLastChar (builder);
        if (Array.IndexOf (_delimiters, lastChar) < 0)
        {
            builder.TrimEnd();
            builder.Append (". ");
        }
    }

    private static void _AddSeparator
        (
            StringBuilder builder
        )
    {
        var lastChar = _GetLastChar (builder);
        if (lastChar != '-')
        {
            var needDot = Array.IndexOf (_delimiters, lastChar) < 0;
            builder.Append (needDot ? ". - " : " - ");
        }
    }

    private static bool _Append
        (
            StringBuilder builder,
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.Append (text);
            return true;
        }

        return false;
    }

    private static bool _AppendWithPrefix
        (
            StringBuilder builder,
            string? text,
            string? prefix
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.AppendWithPrefix (text, prefix);

            return true;
        }

        return false;
    }

    private static bool _AppendWithPrefixAndSuffix
        (
            StringBuilder builder,
            string? text,
            string? prefix,
            string? suffix
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.AppendWithPrefixAndSuffix (text, prefix, suffix);

            return true;
        }

        return false;
    }

    private static readonly char[] _codeDelimiters = { '=', '-' };

    /// <summary>
    /// Добавление кодированной информации вроде "a-ил.".
    /// </summary>
    private static void _AppendWithCode
        (
            StringBuilder builder,
            string? text,
            string? prefix = null
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            builder.Append (prefix);

            var position = text.IndexOfAny (_codeDelimiters);
            builder.Append
                (
                    position >= 0
                        ? text[(position + 1)..]
                        : text
                );
        }
    }

    #endregion

    #region Public methods

    /// <summary>
    /// Получение рабочего листа записи.
    /// </summary>
    public string? Worksheet
        (
            Record record
        )
    {
        Sure.NotNull (record);

        return _configuration.GetWorksheet (record);
    }

    /// <summary>
    /// Переход к новой области описания.
    /// </summary>
    public void NewArea
        (
            StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        if (string.IsNullOrEmpty (AreaSeparator))
        {
            return;
        }


        if (string.IsNullOrWhiteSpace (AreaSeparator))
        {
            // не даём накапливаться лишним переводам строки
            builder.TrimEnd();

            if (builder.Length != 0)
            {
                // текст библиографического описания не должен
                // начинаться с перехода на новую область
                builder.Append (AreaSeparator);
            }
        }
        else
        {
            var builderLength = builder.Length;
            if (builderLength != 0)
            {
                var areaLength = AreaSeparator.Length;
                var enable = areaLength < builderLength;
                if (enable)
                {
                    var text = builder.ToString (builderLength - areaLength, areaLength);
                    enable = text != AreaSeparator;
                }

                if (enable)
                {
                    builder.Append (AreaSeparator);
                }
            }
        }
    }

    /// <summary>
    /// Автор книги из общей части.
    /// </summary>
    public void CommonAuthor
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        foreach (var author in record.EnumerateField (961))
        {
            // автор - заголовок описания
            if (author.HaveSubField ('z'))
            {
                // фамилия
                builder.Append (author.GetSubFieldValue ('a'));

                // римские цифры
                builder.AppendWithPrefix (author.GetSubFieldValue ('d'), " ");

                // инициалы и их расширение
                builder.AppendWithPrefix (author.GetSubFieldValue ('g', 'b'), ", ");

                if (author.HaveSubField ('1', 'c', 'f'))
                {
                    // 1 - неотъемлемая часть имени
                    // c - дополнения к именам, кроме дат
                    // f - годы жизни

                    builder.Append (" (");
                    builder.AppendWithSeparator
                        (
                            "; ",
                            author.GetSubFieldValue ('1'),
                            author.GetSubFieldValue ('c'),
                            author.GetSubFieldValue ('f')
                        );
                    builder.Append (')');
                }
            }
        }
    }

    /// <summary>
    /// Из общей части: основные сведения.
    /// </summary>
    public void CommonInfo
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var first = true;
        var commons = record.Fields.GetField (461);
        var additionals = record.Fields.GetField (46); // дополнение

        foreach (var common in commons)
        {
            if (!first && common.HaveSubField ('c'))
            {
                _AddDot (builder);
            }

            // заглавие
            builder.Append (common.GetSubFieldValue ('c'));
            first = false;
        }

        foreach (var common in commons)
        {
            // сведения, относящиеся к заглавию
            builder.AppendWithPrefix (common.GetSubFieldValue ('e'), " : ");

            // сведения об ответственности
            builder.AppendWithPrefix (common.GetSubFieldValue ('f'), " / ");
        }

        // города и издательства из общей части
        first = true;
        foreach (var common in commons)
        {
            var prefix = string.Empty;
            if (first)
            {
                _AddSeparator (builder);
            }
            else
            {
                prefix = " ; ";
            }

            if (common.HaveNotSubField ('?')) // города не выводить?
            {
                // город
                builder.AppendWithPrefix (common.GetSubFieldValue ('d'), prefix);
            }

            // издательство
            builder.AppendWithPrefix (common.GetSubFieldValue ('g'), " : ");

            first = false;
        }

        // годы общей части
        foreach (var common in commons)
        {
            var start = common.GetSubFieldValue ('h'); // начало издания
            var stop = common.GetSubFieldValue ('z'); // окончание издания

            builder.AppendWithPrefix (start, ", ");
            if (!string.IsNullOrEmpty (stop) && stop != start)
            {
                builder.AppendWithPrefix (stop, " - ");
            }
        }

        if (commons.Length != 0)
        {
            _AddDot (builder);
        }
    }

    /// <summary>
    /// Первый автор (поле 700).
    /// </summary>
    public void FirstAuthor
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var author = record.GetField (700);
        if (author is not null)
        {
            builder.Append (author.GetSubFieldValue ('a'));
            builder.AppendWithPrefix (author.GetSubFieldValue ('g', 'b'), ", ");
            _AddDot (builder);
            builder.Append (' ');
        }
    }

    /// <summary>
    /// Область заглавия, поле 200.
    /// </summary>
    public void TitleArea
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var field = record.GetField (200);
        if (field is null)
        {
            return;
        }

        // обозначение и номер тома
        var volumeNumber = field.GetSubFieldValue ('v');

        // заглавие тома (основное заглавие)
        var title = field.GetSubFieldValue ('a');
        title = UniforPlusS.DecodeTitle (title, true); // для "<1000=тысяча> мелочей

        builder.AppendWithSeparator (" : ", volumeNumber, title);

        // параллельные заглавия
        var parallel = record.Fields.GetField (510);
        if (!parallel.IsNullOrEmpty())
        {
            foreach (var one in parallel)
            {
                builder.AppendWithPrefix (one.GetSubFieldValue ('d'), " = ");
            }
        }

        // несколько томов в одной книге
        foreach (var volume in record.EnumerateField (925))
        {
            builder.Append (", ");

            volumeNumber = volume.GetSubFieldValue ('v'); // обозначение и номер тома
            title = volume.GetSubFieldValue ('a'); // заглавие тома

            builder.AppendWithSeparator (" : ", volumeNumber, title);

            builder.AppendWithPrefix (volume.GetSubFieldValue ('b'), " ; "); // заглавие второго произведения
            builder.AppendWithPrefix (volume.GetSubFieldValue ('c'), " ; "); // заглавие третьего произведения
        }

        // статьи сборника без общего заглавия
        foreach (var article in record.EnumerateField (922))
        {
            builder.Append (". ");
            builder.Append (article.GetSubFieldValue ('c'));
        }

        // сведения, относящиеся к заглавию
        builder.AppendWithPrefix (field.GetSubFieldValue ('e'), " : ");

        // вторая и третья единицы деления
        var issue = record.GetField (923);
        if (issue is not null)
        {
            volumeNumber = issue.GetSubFieldValue ('h'); // обозначение второй единицы деления
            title = issue.GetSubFieldValue ('i'); // заглавие второй единицы деления
            if (!string.IsNullOrEmpty (volumeNumber) || !string.IsNullOrEmpty (title))
            {
                _AddDot (builder);
                builder.AppendWithSeparator (" : ", volumeNumber, title);
                _AddDot (builder);
            }

            volumeNumber = issue.GetSubFieldValue ('k'); // обозначение третьей единицы деления
            title = issue.GetSubFieldValue ('l'); // заглавие третьей единицы деления
            if (!string.IsNullOrEmpty (volumeNumber) || !string.IsNullOrEmpty (title))
            {
                _AddDot (builder);
                builder.AppendWithSeparator (" : ", volumeNumber, title);
                _AddDot (builder);
            }
        }

        // первые сведения об ответственности
        builder.AppendWithPrefix (field.GetSubFieldValue ('f'), " / ");

        // последующие сведения об ответственности
        builder.AppendWithPrefix (field.GetSubFieldValue ('g'), " ; ");
    }

    /// <summary>
    /// Сведения об издании.
    /// </summary>
    public void Edition
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var edition = record.GetField (205);
        if (edition is not null)
        {
            _AddSeparator (builder);
            builder.Append (edition.GetSubFieldValue ('a'));
        }
    }

    /// <summary>
    /// Выходные данные, поле 210.
    /// </summary>
    public void Imprint
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var imprint = record.GetField (210);
        if (imprint is not null)
        {
            _AddSeparator (builder);

            var prefix = string.Empty;

            // флаг: был вывод
            var flag = false;

            // выводить города?
            var city = string.IsNullOrEmpty (imprint.GetSubFieldValue ('?'));

            // издательство
            var publisher = imprint.GetSubFieldValue ('i', 'c');

            if (city)
            {
                flag = _Append (builder, imprint.GetSubFieldValue ('a')); // место издания - город 1

                var city2 = imprint.GetSubFieldValue('x'); // город 2
                var city3 = imprint.GetSubFieldValue('y'); // город 3

                if (!string.IsNullOrEmpty (city2) || !string.IsNullOrEmpty (city3))
                {
                    // "и др." для городов
                    var etal = imprint.GetSubFieldValue ('2');

                    if (!string.IsNullOrEmpty (etal))
                    {
                        builder.AppendWithPrefix (etal, " ");
                    }
                    else
                    {
                        builder.AppendWithPrefix (city2, " ; ");
                        builder.AppendWithPrefix (city3, " ; ");
                    }
                }

            }

            // издательство
            if (_AppendWithPrefix (builder, publisher, " : "))
            {
                flag = true;
            }

            // функция издающей организации
            if (_AppendWithPrefixAndSuffix (builder, imprint.GetSubFieldValue ('6'), " [", "]"))
            {
                flag = true;
            }

            if (flag)
            {
                prefix = ", ";
            }

            // пояснения к году, стоящие перед ним
            var explanation = imprint.GetSubFieldValue ('5');
            if (explanation is not null)
            {
                builder.AppendWithPrefix (explanation, prefix);
                prefix = " ";
            }

            // год издания
            builder.AppendWithPrefix (imprint.GetSubFieldValue ('d'), prefix);

            // место печати
            var place = imprint.GetSubFieldValue ('1');

            // типография
            var house = imprint.GetSubFieldValue ('g', 't');

            // дата печати
            var date = imprint.GetSubFieldValue ('h');

            prefix = string.Empty;
            if (!string.IsNullOrEmpty (place)
                || !string.IsNullOrEmpty (house)
                || !string.IsNullOrEmpty (date))
            {
                builder.Append (" (");
                if (_Append (builder, place))
                {
                    prefix = " : ";
                }

                if (_AppendWithPrefix (builder, house, prefix))
                {
                    prefix = ", ";
                }

                builder.AppendWithPrefix (date, prefix);
            }

        }
    }

    /// <summary>
    /// Физические характеристики, поле 215.
    /// </summary>
    public void PhysicalCharacteristics
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        foreach (var physical in record.EnumerateField (215))
        {
            _AddSeparator (builder);

            // объем (цифры)
            builder.Append (physical.GetSubFieldValue ('a'));

            // единица измерения
            var unit = Utility.NonEmpty (physical.GetSubFieldValue ('1'), "с.");
            builder.AppendWithPrefix (unit, " ");

            // иллюстрации
            var illustrations = physical.GetSubFields ('c', '0', '7', '8');
            if (!illustrations.IsNullOrEmpty())
            {
                _AppendWithCode (builder, illustrations[0].Value, " : ");

                for (var index = 1; index < illustrations.Length; index++)
                {
                    _AppendWithCode (builder, illustrations[index].Value, ", ");
                }
            }

            // сопроводительный материал
            builder.AppendWithPrefix (physical.GetSubFieldValue ('e'), " + ");

            // единица измерения сопроводительного материала
            builder.AppendWithPrefix (physical.GetSubFieldValue ('2'), " ");

            // размер
            builder.AppendWithPrefixAndSuffix (physical.GetSubFieldValue ('d'), " ; ", " см.");

            // тираж
            builder.AppendWithPrefix (physical.GetSubFieldValue ('x'), ". - Тираж ");
        }
    }

    /// <summary>
    /// Область серии.
    /// </summary>
    public void Series
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        foreach (var field in record.EnumerateField (225))
        {
            if (!field.HaveSubField ('a'))
            {
                // серии без заглавия пропускаем
                continue;
            }

            _AddSeparator (builder);
            builder.Append ('(');

            // наименование (заглавие) серии
            builder.Append (field.GetSubFieldValue ('a'));

            // параллельное наименование серии
            builder.AppendWithPrefix (field.GetSubFieldValue ('d'), " = ");

            // сведения, относящиеся к наименованию серии
            builder.AppendWithPrefix (field.GetSubFieldValue ('e'), " : ");

            // сведения об ответственности
            builder.AppendWithPrefix (field.GetSubFieldValue ('f'), " / ");

            // ISSN
            builder.AppendWithPrefix (field.GetSubFieldValue ('x'), ". - ISSN ");

            // номер выпуска
            builder.AppendWithPrefix (field.GetSubFieldValue ('v'), " ; ");

            builder.Append (").");
        }
    }

    /// <summary>
    /// ISBN и цена.
    /// </summary>
    public void IsbnAndPrice
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        foreach (var one in record.EnumerateField (10))
        {
            var isbn = one.GetSubFields ('a', 'e', 'n');
            var erroneous = one.GetSubFieldValue ('z'); // ошибочный ISBN
            var price = one.GetSubFieldValue ('d'); // цена (цифры)
            if (!string.IsNullOrEmpty (price)
                || !string.IsNullOrEmpty (erroneous)
                || !isbn.IsNullOrEmpty())
            {
                var prefix = string.Empty;

                var first = true; // признак первого вывода ISBN
                _AddSeparator (builder);

                if (!isbn.IsNullOrEmpty())
                {
                    foreach (var two in isbn)
                    {
                        var three = two.Value;
                        if (!string.IsNullOrEmpty (three))
                        {
                            if (!first)
                            {
                                _AddSeparator (builder);
                            }

                            builder.AppendWithPrefix (three, "ISBN ");
                            first = true;
                        }
                    }

                    prefix = " : ";
                }

                if (!string.IsNullOrEmpty (erroneous))
                {
                    if (!first)
                    {
                        _AddSeparator (builder);
                    }

                    builder.AppendWithPrefixAndSuffix (erroneous, "ISBN ", " (ошибочный)");
                    prefix = " : ";
                }

                if (!string.IsNullOrEmpty (price))
                {
                    builder.Append (prefix);

                    // валюта
                    var currency = Utility.NonEmpty
                        (
                            one.GetSubFieldValue ('c'),
                            "руб."
                        );
                    builder.AppendWithSuffix (price, " " + currency);
                }

                _AddDot (builder);
            }
        }
    }

    /// <summary>
    /// ISSN, поле 11.
    /// </summary>
    public void Issn
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var issn = record.FM (11, 'a');
        if (!string.IsNullOrEmpty (issn)
            && !issn.SameString ("XXXX-XXXX")
            && !issn.SameString ("ХХХХ-ХХХХ"))
        {
            _AddSeparator (builder);
            builder.AppendWithPrefix (issn, "ISSN ");
            _AddDot (builder);
        }
    }

    /// <summary>
    /// Идентификационный номер нетекстового материала, подполе 19.
    /// </summary>
    public void Identifier
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        foreach (var one in record.EnumerateField (19))
        {
            // основной документ или приложение
            var mainDocument = one.GetSubFieldValue ('x');

            if (mainDocument == "0")
            {
                _AddSeparator (builder);

                builder.Append (one.GetSubFieldValue ('a')); // тип номера
                builder.AppendWithPrefix (one.GetSubFieldValue ('b'), " "); // собственно номер

                _AddDot (builder);
            }
        }
    }

    /// <summary>
    /// Вид содержания, средства доступа и характеристика содержания.
    /// </summary>
    public void Print203
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        foreach (var one in record.EnumerateField (_configuration.ContentTypeTag))
        {
            _AddSeparator (builder);
            builder.Append (one.GetSubFieldValue ('a'));
            builder.AppendWithBrackets (one.GetSubFieldValue ('p'));
            builder.AppendWithPrefix (one.GetSubFieldValue ('c'), " : ");
            _AddDot (builder);
        }
    }

    /// <summary>
    /// Выпуск журнала/газеты.
    /// </summary>
    public void MagazineIssue
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var worksheet = Worksheet (record);
        if (string.IsNullOrEmpty (worksheet))
        {
            return;
        }

        var isIssue = worksheet.Contains ("NJ", StringComparison.InvariantCultureIgnoreCase);
        if (!isIssue)
        {
            return;
        }

        var summaryIndex = record.FM (933); // шифр сводной записи
        if (string.IsNullOrEmpty (summaryIndex))
        {
            return;
        }

        var magazine = _provider.ByIndex (summaryIndex);
        if (magazine is null)
        {
            return;
        }

        // область заглавия
        var title = magazine.GetField (200);
        if (title is null)
        {
            return;
        }

        // заглавие (название) журнала/газеты
        builder.Append (title.GetSubFieldValue ('a'));

        // подзаголовочные сведения
        builder.AppendWithPrefix (title.GetSubFieldValue ('e'), " : ");

        // первые сведения об ответственности
        builder.AppendWithPrefix (title.GetSubFieldValue ('f'), " / ");

        // последующие сведения об ответственности
        builder.AppendWithPrefix (title.GetSubFieldValue ('g'), " ; ");

        builder.AppendWithPrefix (record.FM (934), ". - "); // год
        builder.AppendWithPrefix (record.FM (935), ". - Т. "); // том
        builder.AppendWithPrefix (record.FM (936), ". - № "); // номер
        builder.AppendWithBrackets (record.FM (931, 'c')); // дата выхода
        builder.AppendWithPrefix (magazine.FM (110, 'x'), ". - Периодичность: "); // периодичность
        builder.AppendWithPrefix (magazine.FM (11, 'a'), ". - ISSN "); // ISSN

        _AddDot (builder);
    }

    /// <summary>
    /// Составная часть документа, например, статья в журнале или сборнике.
    /// Поле 463.
    /// </summary>
    public void Article
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var field = record.GetField (463);
        if (field is null)
        {
            return;
        }

        // заглавие журнала/газеты
        var title = field.GetSubFieldValue ('c');
        if (string.IsNullOrEmpty (title))
        {
            return;
        }

        // признак начала области источника плюс заглавие
        builder.Append (" // ");
        builder.Append (title);

        // сведения, относящиеся к заглавию
        builder.AppendWithPrefix (field.GetSubFieldValue ('e'), " : ");

        // первичные сведения об ответственности
        builder.AppendWithPrefix (field.GetSubFieldValue ('f'), " / ");

        _AddSeparator (builder);

        // год издания
        builder.Append (field.GetSubFieldValue ('j'));

        // обозначение и номер первой единицы деления
        if (field.HaveSubField ('a', 'v'))
        {
            _AddSeparator (builder);

            builder.Append (field.GetSubFieldValue ('v'));
            builder.Append (field.GetSubFieldValue ('a'));
        }

        // обозначение и номер второй единицы деления
        if (field.HaveSubField ('h', 'i'))
        {
            _AddSeparator (builder);

            builder.Append (field.GetSubFieldValue ('h'));
            builder.Append (field.GetSubFieldValue ('i'));
        }

        if (field.HaveSubField ('s'))
        {
            _AddSeparator (builder);
            var unit = Utility.NonEmpty (field.GetSubFieldValue ('1'), "С.");
            builder.AppendWithPrefix (field.GetSubFieldValue ('s'), unit + " ");
        }

        _AddDot (builder);
    }

    /// <summary>
    /// Страны издания, поле 102.
    /// </summary>
    public void Countries
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var items = record.FMA (102);
        if (!items.IsNullOrEmpty())
        {
            var need = items.Any
                (
                    item => !(string.Compare (item, "RU", StringComparison.OrdinalIgnoreCase) == 0
                        || string.Compare (item, "SU", StringComparison.OrdinalIgnoreCase) == 0)
                );

            if (need)
            {
                NewArea (builder);
                builder.Append (items.Length == 1 ? "Страна" : "Страны");
                builder.Append (": ");
                builder.AppendWithSeparator (", ", items);
                _AddDot (builder);
            }
        }
    }

    /// <summary>
    /// Языки документа, поле 101.
    /// </summary>
    public void Languages
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var items = record.FMA (101);
        if (!items.IsNullOrEmpty())
        {
            var need = items.Any
                (
                    item => string.Compare (item, "rus", StringComparison.OrdinalIgnoreCase) != 0
                );

            if (need)
            {
                NewArea (builder);
                builder.Append (items.Length == 1 ? "Язык" : "Языки");
                builder.Append (": ");
                builder.AppendWithSeparator (", ", items);
                _AddDot (builder);
            }
        }
    }

    /// <summary>
    /// Аннотация, поле 331.
    /// </summary>
    public void Annotation
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var prefix = "Аннотация: ";
        foreach (var annotation in record.FMA (331))
        {
            NewArea (builder);
            builder.Append (prefix);
            prefix = null;

            // для "Рассмотрены возможности языка <Object Pascal>."
            var decoded = UniforPlusS.DecodeTitle (annotation, true);
            builder.Append (decoded);
            _AddDot (builder);
        }
    }

    /// <summary>
    /// Вывод рубрик с указанной меткой.
    /// </summary>
    public void Subjects
        (
            StringBuilder builder,
            Record record,
            int tag,
            string title
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);
        Sure.Positive (tag);
        Sure.NotNull (title);

        var fields = record.Fields.GetField (tag);
        if (!fields.IsNullOrEmpty())
        {
            NewArea (builder);
            builder.Append (title);
            builder.Append (": ");
            var index = 1;
            foreach (var field in fields)
            {
                if (index != 1)
                {
                    NewArea (builder);
                }

                builder.Append ($"{index.ToInvariantString()}. ");
                builder.Append (field.GetSubFieldValue ('a'));
                builder.AppendWithPrefix (field.GetSubFieldValue ('b'), " - ");
                builder.AppendWithPrefix (field.GetSubFieldValue ('c'), " - ");
                builder.AppendWithPrefix (field.GetSubFieldValue ('d'), " - ");
                _AddDot (builder);

                index++;
            }
        }
    }

    /// <summary>
    /// Рубрики, поля 606, 607.
    /// </summary>
    public void Subjects
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        Subjects (builder, record, 606, "Рубрики");
        Subjects (builder, record, 607, "Географические рубрики");
    }

    /// <summary>
    /// Ключевые слова, поле 610.
    /// </summary>
    public void Keywords
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var keywords = record.FMA (610);
        if (!keywords.IsNullOrEmpty())
        {
            NewArea (builder);
            builder.Append ("Ключевые слова: ");

            var first = true;
            foreach (var keyword in keywords)
            {
                if (!first)
                {
                    builder.Append (", ");
                }

                var decoded = UniforPlusS.DecodeTitle (keyword, true);
                builder.Append (decoded);
                first = false;
            }
            _AddDot (builder);
        }
    }

    /// <summary>
    /// Экземпляры, поле 910.
    /// </summary>
    public void Exemplars
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var exemplars = ExemplarInfo.ParseRecord (record);
        if (!exemplars.IsNullOrEmpty())
        {
            NewArea (builder);
            builder.Append ("Экземпляры: ");
            var first = true;
            foreach (var exemplar in exemplars)
            {
                var number = exemplar.Amount ?? exemplar.Number;
                var place = exemplar.Place;
                if (!string.IsNullOrEmpty (number) && !string.IsNullOrEmpty (place))
                {
                    if (!first)
                    {
                        builder.Append (", ");
                    }

                    builder.AppendWithSeparator (" - ", number, place);
                    first = false;
                }
            }
        }
    }

    /// <summary>
    /// Краткое описание.
    /// </summary>
    public void Brief
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        CommonAuthor (builder, record);
        CommonInfo (builder, record);
        NewArea (builder);
        FirstAuthor (builder, record);
        NewArea (builder);
        TitleArea (builder, record);
        MagazineIssue (builder, record);
        Article (builder, record);
        Edition (builder, record);
        Imprint (builder, record);
        PhysicalCharacteristics (builder, record);
        Series (builder, record);
        IsbnAndPrice (builder, record);
        Issn (builder, record);
        Identifier (builder, record);
        Countries (builder, record);
        Languages (builder, record);
        Print203 (builder, record);
    }

    /// <summary>
    /// Полное описание.
    /// </summary>
    public void FullDescription
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        Brief (builder, record);
        Subjects (builder, record);
        Keywords (builder, record);
        Annotation (builder, record);
        Exemplars (builder, record);
    }

    #endregion
}
