// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* AsyncHardFormat.cs -- асинхронный захардкоженный формат
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AM;
using AM.AppServices;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;
using ManagedIrbis.Menus;
using ManagedIrbis.Pft.Infrastructure.Unifors;
using ManagedIrbis.Providers;
using ManagedIrbis.Records;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;

#endregion

#pragma warning disable CA1822 // member can be marked as static

#nullable enable

namespace ManagedIrbis.Formatting;

/// <summary>
/// Асинхронный захардкоженный формат.
/// </summary>
public sealed class AsyncHardFormat
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
    public AsyncHardFormat()
        : this
        (
            Magna.Host,
            new NullProvider()
        )
    {
        // пустое тело конструктора
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public AsyncHardFormat
        (
            IHost host,
            IAsyncProvider provider,
            RecordConfiguration? configuration = null,
            IStringLocalizer? localizer = null,
            IFormatDriver? driver = null,
            IMemoryCache? cache = null
        )
    {
        Sure.NotNull (provider);

        AreaSeparator = string.Empty;
        _logger = LoggingUtility.GetLogger (host.Services, typeof (HardFormat));

        var services = host.Services;
        _configuration = configuration
            ?? services.GetService<RecordConfiguration> ()
            ?? RecordConfiguration.GetDefault();

        _localizer = localizer
            ?? (IStringLocalizer?) services.GetService<IStringLocalizer<HardFormat>>()
            ?? new NullLocalizer();

        _driver = driver
            ?? services.GetService<IFormatDriver>()
            ?? new PlainFormatDriver();

        cache ??= services.GetService<IMemoryCache>();

        _provider = provider;

        if (cache is not null)
        {
            _cache = cache;
        }
        else
        {
            var cacheOptions = new MemoryCacheOptions
            {
                ExpirationScanFrequency = TimeSpan.FromMinutes (5)
            };
            _cache = new MemoryCache (cacheOptions);
        }
    }

    #endregion

    #region Private members

    private readonly ILogger _logger;

    private readonly RecordConfiguration _configuration;

    private readonly IAsyncProvider _provider;

    private readonly IMemoryCache _cache;

    private readonly IStringLocalizer _localizer;

    private readonly IFormatDriver _driver;

    /// <summary>
    /// Ограничители, после которых нельзя ставить точку.
    /// </summary>
    private static readonly char[] _delimiters = { '!', '?', '.', ',' };

    private static void _AddSeparator
        (
            StringBuilder builder
        )
    {
        var lastChar = builder.LastNonSpaceChar();
        if (lastChar != '-') // проверяем, есть ли предыдущий разделитель
        {
            var needDot = Array.IndexOf (_delimiters, lastChar) < 0;
            builder.Append (needDot ? ". - " : " - ");
        }
    }

    private static void _AddWithSeparator
        (
            StringBuilder builder,
            string? text
        )
    {
        if (!string.IsNullOrEmpty (text))
        {
            _AddSeparator (builder);
            builder.Append (text);
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
    /// Перекодирование значения через меню.
    /// </summary>
    public async Task<string?> TranslateMenuAsync
        (
            string menuName,
            string? key
        )
    {
        Sure.NotNullNorEmpty (menuName);

        if (!_cache.TryGetValue (menuName, out MenuFile? menu))
        {
            var specification = new FileSpecification()
            {
                Database = _provider.EnsureDatabase(),
                Path = IrbisPath.MasterFile,
                FileName = menuName
            };
            menu = await _provider.ReadMenuAsync (specification);
        }

        if (menu is null)
        {
            _logger.LogDebug ("Can't find menu {MenuName}", menuName);
            return key;
        }

        if (string.IsNullOrEmpty (key))
        {
            _logger.LogDebug ("Empty key for menu {MenuName}", menuName);
            return key;
        }

        var result = menu.GetString (key);

        return result;
    }

    /// <summary>
    /// Получение записи по ее шифру в базе.
    /// </summary>
    public async Task<Record?> RecordByIndexAsync
        (
            string index
        )
    {
        Sure.NotNullNorEmpty (index);

        if (_cache.TryGetValue (index, out Record? result))
        {
            _logger.LogDebug ("Can't find record by index \"{Index}\"", index);
            return result;
        }

        result = await _provider.ByIndexAsync (index);
        _cache.Set (index, result);

        return result;
    }

    /// <summary>
    /// Получение рабочего листа записи.
    /// </summary>
    public string? Worksheet
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var result = _configuration.GetWorksheet (record);
        if (string.IsNullOrEmpty (result))
        {
            _logger.LogDebug ("No worksheet in MFN {Mfn}", record.Mfn);
        }

        return result;
    }

    /// <summary>
    /// Переход на новую строку.
    /// </summary>
    public void NewLine
        (
            StringBuilder builder
        )
    {
        Sure.NotNull (builder);

        builder.Trim();
        if (builder.Length == 0)
        {
            // не переходим на новую строку при пустом тексте
            return;
        }

        builder.Append ('\n');
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

        var builderLength = builder.Length;
        if (builderLength == 0)
        {
            // текст библиографического описания не должен
            // начинаться с перехода на новую область
            return;
        }

        // не должно быть подряд двух переходов на новую область
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

    /// <summary>
    /// Переход к новой области описания.
    /// </summary>
    public void NewArea
        (
            StringBuilder builder,
            string? areaText
        )
    {
        Sure.NotNull (builder);

        if (!string.IsNullOrEmpty (areaText))
        {
            NewArea (builder);
            builder.Append (areaText);
        }
    }

    /// <summary>
    /// Расстановочные шифры.
    /// </summary>
    public void ShelfIndex
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        // систематический шифр
        var systematic = record.FM (906)
            ?? record.FM (675, '*') // УДК
            ?? record.FM (621, '*') // ББК
            ?? record.FM (964) // ГРНТИ
            ?? record.FM (686); // индексы другой классификации

        // авторский знак
        var authorSign = record.FM (908);

        if (!string.IsNullOrEmpty (systematic)
            && !string.IsNullOrEmpty (authorSign))
        {
            builder.Append (systematic);
            NewLine (builder);
            builder.Append (authorSign);
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
                builder.Append (author ['a']);

                // римские цифры
                builder.AppendWithSpacePrefix (author ['d'], " ");

                // инициалы и их расширение
                builder.AppendWithPrefix (author ['g', 'b'], ", ");

                if (author.HaveSubField ('1', 'c', 'f'))
                {
                    // 1 - неотъемлемая часть имени
                    // c - дополнения к именам, кроме дат
                    // f - годы жизни

                    builder.Append (" (");
                    builder.AppendWithSeparator
                        (
                            "; ",
                            author ['1'],
                            author ['c'],
                            author ['f']
                        );
                    builder.Append (')');
                }

                builder.AppendSpace();
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
        additionals.NotUsed ();

        foreach (var common in commons)
        {
            if (!first && common.HaveSubField ('c'))
            {
                builder.AppendDot();
            }

            // заглавие
            builder.Append (common ['c']);
            first = false;
        }

        foreach (var common in commons)
        {
            // сведения, относящиеся к заглавию
            builder.AppendWithSpacePrefix (common ['e'], " : ");

            // сведения об ответственности
            builder.AppendWithSpacePrefix (common ['f'], " / ");
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
                builder.AppendWithSpacePrefix (common ['d'], prefix);
            }

            // издательство
            builder.AppendWithSpacePrefix (common ['g'], " : ");

            first = false;
        }

        // годы общей части
        foreach (var common in commons)
        {
            var start = common ['h']; // начало издания
            var stop = common ['z']; // окончание издания

            builder.AppendWithPrefix (start, ", ");
            if (!string.IsNullOrEmpty (stop) && stop != start)
            {
                builder.AppendWithSpacePrefix (stop, " - ");
            }
        }

        if (commons.Length != 0)
        {
            builder.AppendDot();
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
            builder.Append (author.FM ('a'));
            builder.AppendWithPrefix (author.FM ('g', 'b'), ", ");
            builder.AppendDot();
            builder.AppendSpace();
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

            // заглавие второго произведения
            builder.AppendWithSpacePrefix (volume.GetSubFieldValue ('b'), " ; ");

            // заглавие третьего произведения
            builder.AppendWithSpacePrefix (volume.GetSubFieldValue ('c'), " ; ");
        }

        // статьи сборника без общего заглавия
        foreach (var article in record.EnumerateField (922))
        {
            builder.AppendDot();
            builder.Append (article.GetSubFieldValue ('c'));
        }

        // сведения, относящиеся к заглавию
        builder.AppendWithSpacePrefix (field.GetSubFieldValue ('e'), " : ");

        // вторая и третья единицы деления
        var issue = record.GetField (923);
        if (issue is not null)
        {
            volumeNumber = issue ['h']; // обозначение второй единицы деления
            title = issue ['i']; // заглавие второй единицы деления
            if (!string.IsNullOrEmpty (volumeNumber) || !string.IsNullOrEmpty (title))
            {
                builder.AppendDot();
                builder.AppendWithSeparator (" : ", volumeNumber, title);
                builder.AppendDot();
            }

            volumeNumber = issue ['k']; // обозначение третьей единицы деления
            title = issue ['l']; // заглавие третьей единицы деления
            if (!string.IsNullOrEmpty (volumeNumber) || !string.IsNullOrEmpty (title))
            {
                builder.AppendDot();
                builder.AppendWithSeparator (" : ", volumeNumber, title);
                builder.AppendDot();
            }
        }

        // первые сведения об ответственности
        builder.AppendWithSpacePrefix (field ['f'], " / ");

        // последующие сведения об ответственности
        builder.AppendWithSpacePrefix (field ['g'], " ; ");
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
            builder.Append (edition ['a']);
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
            var city = string.IsNullOrEmpty (imprint ['?']);

            // издательство
            var publisher = imprint.GetSubFieldValue ('i', 'c');

            if (city)
            {
                flag = _Append (builder, imprint ['a']); // место издания - город 1

                var city2 = imprint ['x']; // город 2
                var city3 = imprint ['y']; // город 3

                if (!string.IsNullOrEmpty (city2) || !string.IsNullOrEmpty (city3))
                {
                    // "и др." для городов
                    var etal = imprint ['2'];

                    if (!string.IsNullOrEmpty (etal))
                    {
                        builder.AppendWithSpacePrefix (etal, " ");
                    }
                    else
                    {
                        builder.AppendWithSpacePrefix (city2, " ; ");
                        builder.AppendWithSpacePrefix (city3, " ; ");
                    }
                }

            }

            // издательство
            if (_AppendWithPrefix (builder, publisher, " : "))
            {
                flag = true;
            }

            // функция издающей организации
            if (_AppendWithPrefixAndSuffix (builder, imprint ['6'], " [", "]"))
            {
                flag = true;
            }

            if (flag)
            {
                prefix = ", ";
            }

            // пояснения к году, стоящие перед ним
            var explanation = imprint ['5'];
            if (explanation is not null)
            {
                builder.AppendWithPrefix (explanation, prefix);
                prefix = " ";
            }

            // год издания
            builder.AppendWithSpacePrefix (imprint.GetSubFieldValue ('d'), prefix);

            // место печати
            var place = imprint ['1'];

            // типография
            var house = imprint.GetSubFieldValue ('g', 't');

            // дата печати
            var date = imprint ['h'];

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

                builder.AppendWithSpacePrefix (date, prefix);
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
            builder.Append (physical ['a']);

            // единица измерения
            var unit = Utility.NonEmpty (physical ['1'], "с.");
            builder.AppendWithSpacePrefix (unit, " ");

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
            builder.AppendWithSpacePrefix (physical ['e'], " + ");

            // единица измерения сопроводительного материала
            builder.AppendWithSpacePrefix (physical ['2'], " ");

            // размер
            builder.AppendWithPrefixAndSuffix (physical ['d'], " ; ", " см.");

            // тираж
            builder.AppendWithDotPrefix (physical ['x'], ". - Тираж ");
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
            builder.Append (field ['a']);

            // параллельное наименование серии
            builder.AppendWithSpacePrefix (field ['d'], " = ");

            // сведения, относящиеся к наименованию серии
            builder.AppendWithSpacePrefix (field ['e'], " : ");

            // сведения об ответственности
            builder.AppendWithSpacePrefix (field ['f'], " / ");

            // ISSN
            builder.AppendWithDotPrefix (field ['x'], ". - ISSN ");

            // номер выпуска
            builder.AppendWithSpacePrefix (field ['v'], " ; ");

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

                builder.AppendDot();
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
            builder.AppendDot();
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
            var mainDocument = one ['x'];

            if (mainDocument == "0")
            {
                _AddSeparator (builder);

                builder.Append (one ['a']); // тип номера
                builder.AppendWithSpacePrefix (one ['b'], " "); // собственно номер
                builder.AppendDot();
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
            builder.Append (one ['a']);
            builder.AppendWithBrackets (one ['p']);
            builder.AppendWithSpacePrefix (one ['c'], " : ");
            builder.AppendDot();
        }
    }

    /// <summary>
    /// Выпуск журнала/газеты.
    /// </summary>
    public async void MagazineIssue
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

        var magazine = await RecordByIndexAsync (summaryIndex);
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
        builder.Append (title ['a']);

        // подзаголовочные сведения
        builder.AppendWithSpacePrefix (title ['e'], " : ");

        // первые сведения об ответственности
        builder.AppendWithSpacePrefix (title['f'], " / ");

        // последующие сведения об ответственности
        builder.AppendWithSpacePrefix (title ['g'], " ; ");

        builder.AppendWithDotPrefix (record.FM (934), ". - "); // год
        builder.AppendWithDotPrefix (record.FM (935), ". - Т. "); // том
        builder.AppendWithDotPrefix (record.FM (936), ". - № "); // номер
        builder.AppendWithBrackets (record.FM (931, 'c')); // дата выхода
        builder.AppendWithDotPrefix (magazine.FM (110, 'x'), ". - Периодичность: "); // периодичность
        builder.AppendWithDotPrefix (magazine.FM (11, 'a'), ". - ISSN "); // ISSN

        builder.AppendDot();
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
        var title = field.FM ('c');
        if (string.IsNullOrEmpty (title))
        {
            return;
        }

        // признак начала области источника плюс заглавие
        builder.AppendDot (" // ");
        builder.Append (title);

        // сведения, относящиеся к заглавию
        builder.AppendWithSpacePrefix (field.FM ('e'), " : ");

        // первичные сведения об ответственности
        builder.AppendWithSpacePrefix (field.FM ('f'), " / ");

        _AddSeparator (builder);

        // год издания
        builder.Append (field.FM ('j'));

        // обозначение и номер первой единицы деления
        if (field.HaveSubField ('a', 'v'))
        {
            _AddSeparator (builder);

            builder.Append (field.FM ('v'));
            builder.Append (field.FM ('a'));
        }

        // обозначение и номер второй единицы деления
        if (field.HaveSubField ('h', 'i'))
        {
            _AddSeparator (builder);

            builder.Append (field.FM ('h'));
            builder.Append (field.FM ('i'));
        }

        if (field.HaveSubField ('s'))
        {
            _AddSeparator (builder);
            var unit = Utility.NonEmpty (field.FM ('1'), "С.");
            builder.AppendWithPrefix (field.FM ('s'), unit + " ");
        }

        builder.AppendDot();
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
                builder.Append (_localizer [items.Length == 1 ? "Страна" : "Страны"]);
                builder.Append (": ");
                var and = _localizer[" и "];
                builder.AppendList (items, KnownCountries.TranslateCode, union: and);
                builder.AppendDot();
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
                builder.Append (_localizer [items.Length == 1 ? "Язык" : "Языки"]);
                builder.Append (": ");
                var and = _localizer[" и "];
                builder.AppendList (items, KnownLanguages.TranslateCode, union: and);
                builder.AppendDot();
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

        var prefix = _localizer["Аннотация: "].ToString();
        foreach (var annotation in record.FMA (331))
        {
            NewArea (builder);
            builder.Append (prefix);
            prefix = null;

            // для "Рассмотрены возможности языка <Object Pascal>."
            var decoded = UniforPlusS.DecodeTitle (annotation, true);
            builder.Append (decoded);
            builder.AppendDot();
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
                    NewLine (builder);
                }

                builder.Append ($"{index.ToInvariantString()}. ");
                builder.Append (field ['a']);
                builder.AppendWithSpacePrefix (field ['b'], " - ");
                builder.AppendWithSpacePrefix (field ['c'], " - ");
                builder.AppendWithSpacePrefix (field ['d'], " - ");

                builder.AppendDot();
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

        Subjects (builder, record, 606, _localizer ["Рубрики"]);
        Subjects (builder, record, 607, _localizer ["Географические рубрики"]);
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
            var keywordsTitle = _localizer["Ключевые слова: "];
            builder.Append (keywordsTitle);

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

            builder.AppendDot();
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
            var exemplarsTitle = _localizer["Экземпляры: "];
            builder.Append (exemplarsTitle);
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
    /// Ссылка - внешний объект, поле 951.
    /// </summary>
    public void ExternalObject
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var fields = record.Fields.GetField (951);
        if (!fields.IsNullOrEmpty())
        {
            NewArea (builder);

            var first = true;
            foreach (var field in fields)
            {
                var link = field ['i'];
                if (string.IsNullOrEmpty (link))
                {
                    continue;
                }

                if (!first)
                {
                    NewLine (builder);
                }

                var title = field ['t'] ?? _localizer ["URL"];
                _driver.Link (builder, link, title);
                first = false;
            }
        }
    }

    /// <summary>
    /// Поле 337 - примечания об электронном документе.
    /// </summary>
    public void NotesForElectronicDocument
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        // |. - |v337^e,|. - |v337^f,|. - |v337^g,|. - |v337^h,|. - |v337^a
        foreach (var field in record.EnumerateField (337))
        {
            _AddWithSeparator (builder, field['e']); // подполе E: источник основного заглавия
            _AddWithSeparator (builder, field['f']); // подполе F: примечание о библиографической истории
            _AddWithSeparator (builder, field['g']); // подполе G: примечание о датах
            _AddWithSeparator (builder, field['h']); // подполе H: другие примечания
            _AddWithSeparator (builder, field['a']); // подполе A: примечания в "собранном" виде
        }
    }

    /// <summary>
    /// Поле 982: патент, отчет о НИР, НТД и ЮД - спец. сведения.
    /// </summary>
    public async void Patent
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        // TODO: отображать правильно

        var fields = record.Fields.GetField (982);
        foreach (var field in fields)
        {
            NewArea (builder);
            var key = field['0']; // подполе 0: вид патентного или НТД документа
            var kind = await TranslateMenuAsync ("vpatu.mnu", key);
            builder.AppendWithSeparator
                (
                    " ",
                    kind,
                    field['9'] // подполе 9: номер документа
                );
        }
    }

    /// <summary>
    /// Поле 902 - держатель документа.
    /// </summary>
    public void DocumentHolder
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var holders = record.Fields.GetField (902);
        if (!holders.IsNullOrEmpty())
        {
            NewArea (builder);
            var title = holders.Length switch
            {
                1 => "Держатель документа",
                _ => "Держатели документа"
            };

            builder.Append (_localizer[title]);
            builder.Append (": ");
            foreach (var holder in holders)
            {
                // |v902^a,| : |v902^b
                NewLine (builder);
                builder.AppendWithSeparator
                    (
                        " : ",
                        holder['a'], // подполе A: наименование организации
                        holder['b']  // подполе B: почтовый адрес
                    );
            }
        }
    }

    /// <summary>
    /// Различные примечания.
    /// </summary>
    public void Comments
        (
            StringBuilder builder,
            Record record
        )
    {
        void AddField (int tag)
        {
            foreach (var field in record.EnumerateField (tag))
            {
                NewArea (builder, field.Value);
            }
        }

        Sure.NotNull (builder);
        Sure.NotNull (record);

        AddField (314); // поле 314: примечания об интеллектуальной ответственности
        AddField (320); // поле 320: примечания о наличии библиографии
        AddField (912); // поле 912: примечания о языке
        AddField (300); // поле 300: общие примечания

        // поле 541: перевод заглавия
        foreach (var field in record.EnumerateField (541))
        {
            NewArea (builder);
            builder.Append (_localizer["Перевод заглавия"]);
            builder.Append (": ");
            builder.Append (field.Value);
        }
    }

    /// <summary>
    /// Краткое описание.
    /// </summary>
    public string Brief
        (
            Record record
        )
    {
        Sure.NotNull (record);

        var builder = StringBuilderPool.Shared.Get();
        Brief (builder, record);

        return builder.ReturnShared();
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

        _logger.LogTrace ("Begin BriefDescription for MFN {Mfn}", record.Mfn);

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
        Patent (builder, record);
        Countries (builder, record);
        Languages (builder, record);
        Print203 (builder, record);
        Comments (builder, record);

        _logger.LogTrace ("End BriefDescription for MFN {Mfn}", record.Mfn);
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

        _logger.LogTrace ("Begin FullDescription for MFN {Mfn}", record.Mfn);

        Brief (builder, record);
        Subjects (builder, record);
        Keywords (builder, record);
        Annotation (builder, record);
        ExternalObject (builder, record);
        NotesForElectronicDocument (builder, record);
        DocumentHolder (builder, record);
        Exemplars (builder, record);

        _logger.LogTrace ("End FullDescription for MFN {Mfn}", record.Mfn);
    }

    #endregion
}
