﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlus3.cs --
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

using AM;
using AM.Text;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Providers;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Работа со строками.
    //
    static class UniforPlus3
    {
        #region Public methods

        // ================================================================

        //
        // Декодирование строки из UTF-8 – &uf('+3W')
        // Вид функции: +3W.
        // Назначение: Декодирование строки из UTF-8.
        // Формат(передаваемая строка):
        // +3W<данные>
        //

        /// <summary>
        /// Convert text from UTF8 to CP1251.
        /// </summary>
        public static void ConvertToAnsi
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = Utility.ChangeEncoding
                    (
                        IrbisEncoding.Utf8,
                        IrbisEncoding.Ansi,
                        expression
                    );
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Кодирование строки в UTF-8 – &uf('+3U')
        // Вид функции: +3U.
        // Назначение: Кодирование строки в UTF-8.
        // Формат(передаваемая строка):
        // +3U<данные>
        //

        /// <summary>
        /// Convert text from CP1251 to UTF8.
        /// </summary>
        public static void ConvertToUtf
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = Utility.ChangeEncoding
                    (
                        IrbisEncoding.Ansi,
                        IrbisEncoding.Utf8,
                        expression
                    );
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Перевод знака + в %2B – &uf('+3+')
        // Вид функции: +3+.
        // Назначение: Перевод знака + в %2B.
        // Формат (передаваемая строка):
        // +3+<данные>
        //

        /// <summary>
        /// Replace '+' sign with %2B
        /// </summary>
        public static void ReplacePlus
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = expression.Replace("+", "%2B");
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Декодирование данных из URL – &uf('+3D')
        // Вид функции: +3D.
        // Назначение: Декодирование данных из URL.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        // +3D<данные>
        //

        /// <summary>
        /// Decode text from the URL.
        /// </summary>
        public static void UrlDecode
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = IrbisUtility.UrlDecode
                    (
                        expression,
                        IrbisEncoding.Utf8
                    );
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Кодирование данных для представления в URL – &uf('+3E')
        // Вид функции: +3E.
        // Назначение: Кодирование данных для представления в URL.
        // Присутствует в версиях ИРБИС с 2005.2.
        // Формат (передаваемая строка):
        //  +3E<данные>
        //
        // Пример:
        //
        // &unifor('+3E', v1007)
        //

        /// <summary>
        /// Encode the text to URL format.
        /// </summary>
        public static void UrlEncode
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = IrbisUtility.UrlEncode
                    (
                        expression,
                        IrbisEncoding.Utf8
                    );
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // ibatrak
        //
        // Форматирование полей записи в клиентское представление без заголовка
        //
        // &uf ('+3A')
        //

        /// <summary>
        /// Encode the record to the plain text format.
        /// </summary>
        public static void FieldsToText
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var record = context.Record;
            if (!ReferenceEquals(record, null))
            {
                var output = record.ToPlainText();
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        private static readonly char[] _specialChars = { '&', '"', '<', '>' };

        //
        // ibatrak
        //
        // Замена специальных символов HTML.
        //
        // Неописанная функция
        // &unifor('+3H')
        // Кривая реализация htmlspecialchars
        // заменяет
        // & на &quot; (здесь ошибка -- надо на &amp;)
        // " на &quot;
        // < на &lt;
        // > на &gt;
        // одинарные кавычки не кодирует
        //

        public static void HtmlSpecialChars
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                if (expression.ContainsAnySymbol(_specialChars))
                {
                    var builder = new StringBuilder(expression);
                    builder.Replace("&", "&quot;");
                    builder.Replace("\"", "&quot;");
                    builder.Replace("<", "&lt;");
                    builder.Replace(">", "&gt;");
                    expression = builder.ToString();
                }

                context.WriteAndSetFlag(node, expression);
            }
        }

        // ================================================================

        //
        // ibatrak
        //
        // Неописанная функция
        // &unifor('+3J')
        // Поиск термина и вывод количества ссылок
        // Поведение в целом аналогично &unifor('J'),
        // однако в отличие от &unifor('J') поиск идет
        // не полному совпадению, а по вхождению искомого термина в найденный,
        // отличается способ передачи параметра базы данных
        //

        public static void GetTermCount
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            var parts = expression.Split
                (
                    CommonSeparators.Comma,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            var provider = context.Provider;
            var database = parts[0];
            var key = IrbisText.ToUpper(parts[1]).ThrowIfNull();
            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database;
            }

            var parameters = new TermParameters
            {
                StartTerm = key,
                Database = database,
                NumberOfTerms = 10
            };
            var terms = provider.ReadTerms(parameters);
            if (terms is { Length: not 0 })
            {
                var info = terms[0];
                if (info.Text?.StartsWith(key) ?? false)
                {
                    var output = info.Count.ToInvariantString();
                    context.Write(node, output);
                }
            }
        }

        // ================================================================

        //
        // Расформатирует найденные по запросу записи  - &uf('+3S')
        // Вид функции: +3S.
        //
        // Назначение: Расформатирует найденные по запросу записи.
        // Если [количество выводимых записей]=0,
        // то возвращает только количество найденных по запросу документов.
        //
        // Формат (передаваемая строка):
        //
        // +3S[имя базы],[количество выводимых записей],[ограничитель][формат][ограничитель],[формат или @имя файла с форматом]
        //

        public static void SearchFormat
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            var provider = context.Provider;
            var navigator = new TextNavigator(expression);
            var database = navigator.ReadUntil(',').ToString();
            if (navigator.ReadChar() != ',')
            {
                return;
            }

            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database;
            }

            var count = navigator.ReadUntil(',').ToString().SafeToInt32();
            if (navigator.ReadChar() != ',')
            {
                return;
            }

            var separator = navigator.ReadChar();
            if (separator == '\0')
            {
                return;
            }

            var searchExpression = navigator.ReadUntil(separator).ToString();
            if (navigator.ReadChar() != separator)
            {
                return;
            }

            if (count != 0 && navigator.ReadChar() != ',')
            {
                return;
            }

            var format = navigator.GetRemainingText().ToString();
            format = format.Trim();
            if (string.IsNullOrEmpty(format) && count != 0)
            {
                return;
            }

            if (format.StartsWith("@"))
            {
                var fileName = format.Substring(1);
                var extension = Path.GetExtension(fileName);
                if (string.IsNullOrEmpty(extension))
                {
                    fileName += ".pft";
                }
                var specification = new FileSpecification
                    {
                        Path = IrbisPath.MasterFile,
                        Database = database,
                        FileName = fileName
                    };
                format = provider.ReadTextFile(specification);
                if (string.IsNullOrEmpty(format))
                {
                    return;
                }
            }

            var saveDatabase = provider.Database;
            try
            {
                if (!string.IsNullOrEmpty(database))
                {
                    provider.Database = database;
                }

                var found = provider.Search(searchExpression);
                if (count == 0)
                {
                    context.WriteAndSetFlag(node, found.Length.ToInvariantString());

                    return;
                }

                if (found.Length == 0)
                {
                    return;
                }

                PftProgram program;
                try
                {
                    // TODO some caching

                    program = PftUtility.CompileProgram(format);
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            "UniforPlus3::SearchFormat",
                            exception
                        );

                    return;
                }

                if (count < 0)
                {
                    Array.Reverse(found);
                    count = -count;
                }

                for (var i = 0; i < count; i++)
                {
                    using var guard = new PftContextGuard(context);
                    var nestedContext = guard.ChildContext;
                    nestedContext.Reset();
                    nestedContext.Output = context.Output;

                    var mfn = found[i];
                    if (provider.ReadRecord(mfn) is { } record)
                    {
                        nestedContext.Record = record;
                        program.Execute(nestedContext);
                    }
                }
            }
            finally
            {
                provider.Database = saveDatabase;
            }
        }

        // ================================================================

        //
        // Вывод количества документов, найденных во внешней базе
        // по команде G.
        // (команда возвращает строку RESULT=[кол-во найденных
        // по запросу документов]) – &uf('+3G')
        //
        // Вид функции: +3G.
        //
        // Назначение: Вывод количества документов, найденных
        // во внешней базе по команде G.
        // (команда возвращает строку RESULT=[кол-во найденных
        // по запросу документов]).
        //
        // Формат (передаваемая строка):
        //
        // +3G[URL к внешнему сайту WEB ИРБИС, с запросом G]
        //

        /// <summary>
        /// ibatrak Поиск количества терминов во внешней базе
        /// </summary>
        public static void GetExternalDbTermRecordCount
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                context.WriteAndSetFlag(node, "0");

                return;
            }

            var content = string.Empty;
            try
            {
                var client = new System.Net.WebClient();
                content = client.DownloadString(expression);
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "UniforPlus3::GetExternalDbTermRecordCount",
                        exception
                    );
            }

            var match = Regex.Match(content, "RESULT=([0-9]+)");
            var count = 0;
            if (match.Success)
            {
                count = match.Groups[1].Value.SafeToInt32();
            }

            context.WriteAndSetFlag(node, count.ToInvariantString());
        }

        // ================================================================

        //
        // ibatrak
        //
        // Функция введена для оптимизации скорости проверки наличия
        // текста в полнотекстовой базе данных – &uf('+3C')
        // Вид функции: +3C.
        //
        // Назначение: Функция предназначена для обрезания перед
        // помещением в словарь базы данных значения, формируемого
        // путём соединения префикса TXT= и содержимого подполя
        // ^B ссылки на текстовый файл полнотекстовой базы.
        // Подробнее см. в подразделе Префикс TXT статьи Схема полнотекстовой базы данных.
        //
        // Присутствует в версиях ИРБИС с 2013.1.
        //
        // http://wiki.elnit.org/index.php/%D0%A1%D1%85%D0%B5%D0%BC%D0%B0_%D0%BF%D0%BE%D0%BB%D0%BD%D0%BE%D1%82%D0%B5%D0%BA%D1%81%D1%82%D0%BE%D0%B2%D0%BE%D0%B9_%D0%B1%D0%B0%D0%B7%D1%8B_%D0%B4%D0%B0%D0%BD%D0%BD%D1%8B%D1%85#.D0.9F.D1.80.D0.B5.D1.84.D0.B8.D0.BA.D1.81_TXT
        //
        // Префикс TXT=
        // Термин словаря, начинающийся с префикса TXT=, предназначен
        // для поиска записи(ей) базы данных, соответствующей(их)
        // известной ссылке на текст.
        //
        // За префиксом TXT= следует:
        //
        // Значение подполя ^B — в том случае, если термин
        // (вместе с префиксом) не превышает определённое количество
        // символов (250 символов).
        // Определённым образом укороченное значение подполя ^B
        // - в том случае, если термин превышает указанное количество символов.
        // Вторая часть правила позволяет уменьшить необходимость
        // перебора терминов словаря при поиске записей по ссылке на
        // конкретную страницу текста в случае длинных путей и имён
        // файлов (таких, что длина термина превышает 250 символов).
        // Укорочение происходит таким образом, чтобы в поисковом
        // термине фигурировал номер страницы.
        //

        /// <summary>
        /// ibatrak Усечение имени файла для префикса TXT= полнотекстового поиска
        /// </summary>
        public static void TruncateFullTextFileName
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                return;
            }

            var path = expression;
            if (path.Length > 249)
            {
                var pageNumRegex = new Regex("__.*$");
                var m = pageNumRegex.Match(path);
                if (m.Success)
                {
                    path = path.Substring(0, 249 - m.Length) + m.Value;
                }
                else if (path.Length > 253)
                {
                    path = expression.Substring(0, 253);
                }
            }

            context.WriteAndSetFlag(node, path);
        }

        // ================================================================

        //
        // ibatrak
        //
        // Неописанная функция
        // &unifor ('+3T')
        //
        // Делит строку на 2 фрагмента по символу запятой,
        // разбирает как double, делит, возвращает целую часть результата
        //

        /// <summary>
        /// ibatrak Усечение имени файла для префикса TXT= полнотекстового поиска
        /// </summary>
        public static void Divide
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                context.WriteAndSetFlag(node, "0");

                return;
            }

            var parts = expression.Split
                (
                    CommonSeparators.Comma,
                    2
                );
            if (parts.Length == 1)
            {
                context.WriteAndSetFlag(node, "0");

                return;
            }

            var dividend = parts[0].SafeToDouble();
            if (!Utility.TryParseDouble(parts[1], out var divisor))
            {
                return;
            }

            try
            {
                var result = Math.Truncate(dividend / divisor);
                if (!double.IsInfinity(result) && !double.IsNaN(result))
                {
                    context.WriteAndSetFlag(node, result.ToInvariantString());
                }
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        "UniforPlus3::Divide",
                        exception
                    );
            }
        }

    }

    #endregion
}
