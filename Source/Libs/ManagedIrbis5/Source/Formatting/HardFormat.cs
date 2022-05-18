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
using System.Text;

using AM;
using AM.Collections;
using AM.Text;

using ManagedIrbis.Records;

namespace ManagedIrbis.Formatting;

/// <summary>
/// Захардкоженный формат.
/// </summary>
public sealed class HardFormat
{
    #region Construction

    /// <summary>
    /// Конструктор по умолчанию.
    /// </summary>
    public HardFormat()
        : this (RecordConfiguration.GetDefault())
    {
    }

    /// <summary>
    /// Конструктор.
    /// </summary>
    public HardFormat
        (
            RecordConfiguration configuration
        )
    {
        Sure.NotNull (configuration);

        _configuration = configuration;
    }

    #endregion

    #region Private members

    private readonly RecordConfiguration _configuration;

    /// <summary>
    /// Ограничители, после которых нельзя ставить точку
    /// </summary>
    private static readonly char[] _delimiters = { '!', '?', '.', ',' };

    private static char _GetLastChar
        (
            StringBuilder builder
        )
    {
        while (true)
        {
            var position = builder.Length - 1;
            if (position < 0)
            {
                break;
            }

            var result = builder[position];
            if (!char.IsWhiteSpace (result))
            {
                return result;
            }
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
    /// Авторы книги.
    /// </summary>
    public void Authors
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
        }
    }

    /// <summary>
    /// Область заглавия.
    /// </summary>
    public void TitleArea
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var title = record.GetField (200);
        if (title is null)
        {
            return;
        }

        // обозначение и номер тома
        builder.AppendWithSuffix (title.GetSubFieldValue ('v'), " : ");

        // заглавие тома
        builder.Append (title.GetSubFieldValue ('a'));

        // параллельные заглавия
        var parallel = record.Fields.GetField (510);
        if (!parallel.IsNullOrEmpty())
        {
            foreach (var one in parallel)
            {
                builder.AppendWithPrefix (one.GetSubFieldValue ('d'), " = ");
            }
        }

        // сведения, относящиеся к заглавию
        builder.AppendWithPrefix (title.GetSubFieldValue ('e'), ":");

        // вторая и третья единицы деления
        var issue = record.GetField (923);
        if (issue is not null)
        {
            if (_AppendWithPrefix (builder, issue.GetSubFieldValue ('h'), " ") // обозначение второй единицы деления
                || _AppendWithPrefix (builder, issue.GetSubFieldValue ('i'), " ")) // заглавие второй единицы деления
            {
                _AddDot (builder);
            }

            if (_AppendWithPrefix (builder, issue.GetSubFieldValue ('k'), " ") // обозначение третьей единицы деления
                || _AppendWithPrefix (builder, issue.GetSubFieldValue ('l'), " ")) // заглавие третьей единицы деления
            {
                _AddDot (builder);
            }
        }

        // первые сведения об ответственности
        builder.AppendWithPrefix (title.GetSubFieldValue ('f'), " / ");

        // последующие сведения об ответственности
        builder.AppendWithPrefix (title.GetSubFieldValue ('g'), " ; ");
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
    /// Выходные данные.
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
            if (_Append (builder, imprint.GetSubFieldValue ('a')) // место издания
                || _AppendWithPrefix (builder, imprint.GetSubFieldValue ('c'), " : ")) // издательство
            {
                prefix = ", ";
            }

            // год издания
            builder.AppendWithPrefix (imprint.GetSubFieldValue ('d'), prefix);
        }
    }

    /// <summary>
    /// Физические характеристики.
    /// </summary>
    public void PhysicalCharacteristics
        (
            StringBuilder builder,
            Record record
        )
    {
        Sure.NotNull (builder);
        Sure.NotNull (record);

        var physical = record.GetField (215);
        if (physical is not null)
        {
            _AddSeparator (builder);

            // объем (цифры)
            builder.Append (physical.GetSubFieldValue ('a'));

            // единица измерения
            var unit = Utility.NonEmpty (physical.GetSubFieldValue ('1'), "с.");
            builder.AppendWithPrefix (unit, " ");

            // иллюстрации
            builder.AppendWithPrefix (physical.GetSubFieldValue ('c'), " : ");
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

        foreach (var serie in record.EnumerateField (225))
        {
            _AddSeparator (builder);
            builder.Append ('(');

            // наименование (заглавие) серии
            builder.Append (serie.GetSubFieldValue ('a'));

            // параллельное наименование серии
            builder.AppendWithPrefix (serie.GetSubFieldValue ('d'), " = ");

            // сведения, относящиеся к наименованию серии
            builder.AppendWithPrefix (serie.GetSubFieldValue ('e'), " : ");

            // сведения об ответственности
            builder.AppendWithPrefix (serie.GetSubFieldValue ('f'), " / ");

            // ISSN
            builder.AppendWithPrefix (serie.GetSubFieldValue ('x'), ". - ISSN ");

            // номер выпуска
            builder.AppendWithPrefix (serie.GetSubFieldValue ('v'), " ; ");

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
            var price = one.GetSubFieldValue ('d'); // цена (цифры)
            if (!string.IsNullOrEmpty (price))
            {
                _AddSeparator (builder);
                var currency = Utility.NonEmpty (one.GetSubFieldValue ('c'), "руб."); // валюта
                builder.AppendWithSuffix (price, " " + currency);
            }

            var isbn = one.GetSubFields ('a', 'e', 'n');
            foreach (var two in isbn)
            {
                var three = two.Value;
                if (!string.IsNullOrEmpty (three))
                {
                    _AddSeparator (builder);
                    builder.AppendWithPrefix (three, "ISBN ");
                }
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

        Authors (builder, record);
        CommonInfo (builder, record);
        FirstAuthor (builder, record);
        TitleArea (builder, record);
        Edition (builder, record);
        Imprint (builder, record);
        PhysicalCharacteristics (builder, record);
        Series (builder, record);
        IsbnAndPrice (builder, record);
        Print203 (builder, record);
    }

    #endregion
}
