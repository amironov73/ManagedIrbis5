// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

/* RecordUtility.cs -- вспомогательные методы для работы с записями
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using AM;
using AM.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Records
{
    /// <summary>
    /// Вспомогательные методы для работы с запиями.
    /// </summary>
    public static class RecordUtility
    {
        #region Public methods

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

            var navigator = new ValueTextNavigator (format); // нафигатор по тексту формата
            ReadOnlySpan<char> prefix = default, // префикс-литерал
                before = default, // ведущий условный литерал
                after = default, // замыкающий условный литерал
                suffix = default; // суффикс-литерал
            var skipFirst = false; // флаг: подавление вывода первого повторения
            var skipLast = false;  // флаг: подавление вывода последнего повторения
            var tag = 0; // метка поля
            var code = '\0'; // код подполя (опциональный)

            navigator.SkipWhitespace();
            var chr = navigator.ReadChar();
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

            var command = char.ToUpperInvariant (chr);

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

            if (chr == '|')
            {
                // замыкающий условный литерал

                after = navigator.ReadUntil ('|');
                navigator.ReadChar();
                navigator.SkipWhitespace();
                chr = navigator.ReadChar();
            }

            if (chr == '"')
            {
                // суффикс-литерал

                suffix = navigator.ReadUntil ('"');
                navigator.ReadChar();
                navigator.SkipWhitespace();
                chr = navigator.ReadChar();
            }

            if (chr != ValueTextNavigator.EOF && !char.IsWhiteSpace (chr))
            {
                // какие-то команды после суффикс-литерала
                throw new FormatException (nameof (format));
            }

            var result = false; // флаг: был ли вывод?
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

                } // for
            }

            return result;

        } // method SimpleFormat

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

        } // method SimpleFormat

        #endregion

    } // class RecordUtiltity

} // namespace ManagedIrbis.Records
