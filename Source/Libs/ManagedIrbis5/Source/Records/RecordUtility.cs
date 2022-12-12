// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global

/* RecordUtility.cs -- вспомогательные методы для работы с записями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Records;

/// <summary>
/// Вспомогательные методы для работы с запиями.
/// </summary>
public static class RecordUtility
{
    #region Public methods

    /// <summary>
    /// Выполнение действий для каждого повторения указанного поля.
    /// </summary>
    public static Record ForEach
        (
            this Record record,
            int tag,
            Action<RepeatingGroup.Repeat> action
        )
    {
        var group = new RepeatingGroup (record, tag);
        group.ForEach (action);

        return record;
    }

    /// <summary>
    /// Замена полей с указанной меткой.
    /// </summary>
    public static Record ReplaceField
        (
            this Record record,
            int tag,
            IEnumerable<Field> newFields
        )
    {
        Sure.NotNull (record);
        Sure.NotNull (newFields);

        record.RemoveField (tag);
        record.Fields.AddRange (newFields);

        return record;
    }

    /// <summary>
    /// Замена значения подполя.
    /// </summary>
    public static Record ReplaceValue
        (
            this Record record,
            int tag,
            char code,
            string? oldValue,
            string? newValue
        )
    {
        foreach (var field in record.EnumerateField (tag))
        {
            field.ReplaceSubField (code, oldValue, newValue);
        }

        return record;
    }

    /// <summary>
    /// Установка значения поля (до первого разделителя).
    /// </summary>
    public static Record SetValue
        (
            this Record record,
            int tag,
            string? newValue
        )
    {
        var found = false;
        foreach (var field in record.EnumerateField (tag))
        {
            field.Value = newValue;
            found = true;
        }

        if (!found)
        {
            record.Add (tag, newValue);
        }

        return record;
    }

    /// <summary>
    /// Установка значения подполя.
    /// </summary>
    public static Record SetValue
        (
            this Record record,
            int tag,
            char code,
            string? newValue
        )
    {
        foreach (var field in record.EnumerateField (tag))
        {
            field.SetSubFieldValue (code, newValue);
        }

        return record;
    }

    /// <summary>
    /// Простое расформатирование записи на уровне <c>"v123^a"</c>.
    /// </summary>
    public static bool SimpleFormat
        (
            this Record? record,
            TextWriter output,
            string? format
        )
    {
        // TODO обеспечить поддержку IRecord

        if (record is null || string.IsNullOrEmpty (format))
        {
            return false;
        }

        var result = false; // флаг: был ли вывод?
        var navigator = new ValueTextNavigator (format); // нафигатор по тексту формата
        while (!navigator.IsEOF)
        {
            ReadOnlySpan<char> prefix = default, // префикс-литерал
                before = default, // ведущий условный литерал
                after = default, // замыкающий условный литерал
                suffix = default; // суффикс-литерал
            var skipFirst = false; // флаг: подавление вывода первого повторения
            var skipLast = false; // флаг: подавление вывода последнего повторения
            var tag = 0; // метка поля
            var code = '\0'; // код подполя (опциональный)

            if (!navigator.SkipWhitespace())
            {
                // достигнут конец текста
                break;
            }

            var chr = navigator.ReadChar();
            if (chr == '\'')
            {
                // простой литерал
                var text = navigator.ReadUntil ('\'');
                output.Write (text);
                result = true;
                navigator.ReadChar();

                // продолжаем с начала
                continue;
            }

            if (chr == ',')
            {
                continue;
            }

            if (chr == '"')
            {
                // префикс-литерал

                prefix = navigator.ReadUntil ('"');
                navigator.ReadChar();
                navigator.SkipWhitespace();
                chr = navigator.ReadChar();
            }

            if (chr == '|')
            {
                // ведущий условный литерал

                before = navigator.ReadUntil ('|');
                navigator.ReadChar();
                navigator.SkipWhitespace();
                chr = navigator.ReadChar();
                if (chr == '+')
                {
                    // подавление вывода ведущего условного литерала при первом повторении поля

                    skipFirst = true;
                    navigator.SkipWhitespace();
                    chr = navigator.ReadChar();
                }
            }

            if (chr != 'v' && chr != 'V' && chr != 'n' && chr != 'N'
                && chr != 'd' && chr != 'D')
            {
                // неизвестная команда
                throw new FormatException (nameof (format));
            }

            var command = char.ToUpperInvariant (chr); // команда вывода

            chr = navigator.ReadChar();
            if (!chr.IsArabicDigit())
            {
                // метка поля - не число
                throw new FormatException (nameof (format));
            }

            // разбор метки кода
            while (chr.IsArabicDigit())
            {
                tag = tag * 10 + chr - '0';
                chr = navigator.ReadChar();
            }

            if (chr != ValueTextNavigator.EOF && char.IsWhiteSpace (chr))
            {
                // между меткой и подполем могут быть пробелы

                navigator.SkipWhitespace();
                chr = navigator.ReadChar();
            }

            if (chr == '^')
            {
                // код подполя

                code = navigator.ReadChar();
                navigator.SkipWhitespace();
                chr = navigator.ReadChar();
            }

            if (chr == '+')
            {
                // подавление вывода замыкающего условного литерала при последнем повторении поля

                skipLast = true;
                navigator.SkipWhitespace();
                chr = navigator.ReadChar();
            }

            if (chr == ',')
            {
                goto DoFormat;
            }

            if (chr == '|')
            {
                // замыкающий условный литерал

                after = navigator.ReadUntil ('|');
                navigator.ReadChar();
                navigator.SkipWhitespace();
                chr = navigator.ReadChar();
            }

            if (chr == ',')
            {
                goto DoFormat;
            }

            if (chr == '"')
            {
                // суффикс-литерал

                suffix = navigator.ReadUntil ('"');
                navigator.ReadChar();
                navigator.SkipWhitespace();
            }

            DoFormat: // переходим к собственно форматированию

            // простые проверки перед началом форматирования
            if (skipFirst && before.IsEmpty || skipLast && after.IsEmpty)
            {
                // имеем плюсик при отсутствии соответствующего повторяющегося литерала
                throw new FormatException (nameof (format));
            }

            var count = code == '\0' ? record.Count (tag) : record.Count (tag, code);
            if (command == 'N')
            {
                // префикс или суффикс выводятся только при отсутсвии

                if (count == 0)
                {
                    if (!prefix.IsEmpty)
                    {
                        output.Write (prefix);
                        result = true;
                    }

                    if (!suffix.IsEmpty)
                    {
                        output.Write (suffix);
                        result = true;
                    }
                }
            }
            else if (command == 'D')
            {
                // префикс или суффикс выводятся только при наличии

                if (count != 0)
                {
                    if (!prefix.IsEmpty)
                    {
                        output.Write (prefix);
                        result = true;
                    }

                    if (!suffix.IsEmpty)
                    {
                        output.Write (suffix);
                        result = true;
                    }
                }
            }
            else
            {
                // команда 'V' -- крутим цикл

                for (var index = 0; index < count; index++)
                {
                    if (index == 0)
                    {
                        // первое повторение поля

                        if (!prefix.IsEmpty)
                        {
                            output.Write (prefix);
                            result = true;
                        }

                        if (!skipFirst && !before.IsEmpty)
                        {
                            output.Write (before);
                            result = true;
                        }
                    }
                    else
                    {
                        if (!before.IsEmpty)
                        {
                            output.Write (before);
                            result = true;
                        }
                    }

                    var value = code == '\0'
                        ? record.FM (tag, index)
                        : record.FM (tag, index, code);
                    if (!string.IsNullOrEmpty (value))
                    {
                        output.Write (value);
                        result = true;
                    }

                    if (index == count - 1)
                    {
                        // последнее повторение поля

                        if (!skipLast && !after.IsEmpty)
                        {
                            output.Write (after);
                            result = true;
                        }

                        if (!suffix.IsEmpty)
                        {
                            output.Write (suffix);
                            result = true;
                        }
                    }
                    else
                    {
                        if (!after.IsEmpty)
                        {
                            output.Write (after);
                            result = true;
                        }
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// Простое расформатирование записи на уровне <c>"v123^a"</c>.
    /// </summary>
    public static string? SimpleFormat
        (
            this Record? record,
            string? format
        )
    {
        if (record is null || string.IsNullOrEmpty (format))
        {
            return null;
        }

        var output = new StringWriter();

        return SimpleFormat (record, output, format) ? output.ToString() : null;
    }

    #endregion
}
