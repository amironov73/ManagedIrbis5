// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* UniforPlus9.cs -- группа технических форматных выходов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using AM;
using AM.IO;
using AM.Text;

using ManagedIrbis.Fields;
using ManagedIrbis.Infrastructure;

#endregion

#nullable enable

namespace ManagedIrbis.Pft.Infrastructure.Unifors
{
    //
    // Группа технических форматных выходов
    //

    static class UniforPlus9
    {
        #region Private members

        private static bool _CheckUrlExist
        (
            string address
        )
        {
            using (var client = new System.Net.WebClient())
            {
                try
                {
                    client.DownloadString(address);

                    return true;
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            "UniforPlus9::_CheckUrlExist",
                            exception
                        );
                }
            }

            return false;
        }

        private static FileSpecification? _GetFileSpecification
            (
                string expression
            )
        {
            var navigator = new TextNavigator(expression);
            var pathText = navigator.ReadUntil(',').ToString();
            navigator.ReadChar();
            string dbName = null;
            if (pathText == "0"
                || pathText == "1"
                || pathText == "11")
            {
                if (navigator.PeekChar() == ',')
                {
                    navigator.ReadChar();
                }
            }
            else
            {
                dbName = navigator.ReadUntil(',').ToString();
                navigator.ReadChar();
            }

            var fileName = navigator.GetRemainingText().ToString();
            if (string.IsNullOrEmpty(pathText)
                || string.IsNullOrEmpty(fileName))
            {
                return null;
            }

            var path = (IrbisPath)pathText.ParseInt32();
            var result = new FileSpecification
                {
                    Path = path,
                    Database = dbName,
                    FileName = fileName
                };

            return result;
        }

        #endregion

        #region Public methods

        // ================================================================

        //
        // Вернуть ANSI-символ с заданным кодом – &uf('+9F')
        // Вид функции: +9F.
        // Назначение: Вернуть ANSI-символ с заданным кодом.
        // Присутствует в версиях ИРБИС с 2008.1.
        // Формат (передаваемая строка):
        // +9F<код>
        //
        // Примеры:
        //
        // Такой форматный выход может пригодиться, например,
        // когда надо вывести в литерале символ, совпадающий
        // с ограничителями литерала.
        //
        // Для формата
        //
        // '11111',&Uf('+9F39'),'22222'
        //
        // результат расформатирования будет
        //
        // 11111'22222
        //

        /// <summary>
        /// Get character with given code.
        /// </summary>
        public static void GetCharacter
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                if (Utility.TryParseInt32(expression, out var code))
                {
                    var output = ((char)code).ToString();
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        // ================================================================

        //
        // Вернуть путь из заданного полного пути/имени – &uf('+92')
        // Вид функции: +92.
        // Назначение: Вернуть путь из заданного полного пути/имени.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +92
        //

        /// <summary>
        /// Get directory name from full path.
        /// </summary>
        public static void GetDirectoryName
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = Path.GetDirectoryName(expression);
                if (!string.IsNullOrEmpty(output))
                {
                    if (!output.EndsWith
                        (
                            Path.DirectorySeparatorChar.ToString()
                        ))
                    {
                        output += Path.DirectorySeparatorChar;
                    }

                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        // ================================================================

        //
        // Вернуть имя диска из заданного полного пути/имени – &uf('+94')
        // Вид функции: +94.
        // Назначение: Вернуть имя диска из заданного полного пути/имени.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +94
        //

        /// <summary>
        /// Get drive name from full path.
        /// </summary>
        public static void GetDrive
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var match = Regex.Match(expression, "^[A-Za-z]:");
                var output = match.Value;
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Вернуть расширение из заданного полного пути/имени – &uf('+93')
        // Вид функции: +93.
        // Назначение: Вернуть расширение из заданного полного пути/имени.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +93
        //

        /// <summary>
        /// Get extension from full path.
        /// </summary>
        public static void GetExtension
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = Path.GetExtension(expression);
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Вставить данные из заданного текстового файла – &uf('+9C')
        // Вид функции: +9C.
        // Назначение: Вставить данные из заданного текстового файла.
        // Формат (передаваемая строка):
        // +9С<path>,<dbname>,<filename>
        // где:
        // <path> – определяет путь к файлу и принимает значения:
        // 0 – основная директория системы (для ИРБИС32 – та,
        // где находятся исполняемые модули; для ИРБИС64 – та,
        // где находятся исполняемые модули сервера);
        // 1 – общая директория баз данных(по умолчанию \DATAI);
        // 10 – директория конкретной БД;
        // <dbname> – имя БД (имеет смысл только при path= 10).
        // По умолчанию – предполагается текущая БД;
        // <filename> – имя файла;
        //

        /// <summary>
        /// UNIFOR('+9C'): Get file content.
        /// </summary>
        public static void GetFileContent
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var specification
                    = _GetFileSpecification(expression);
                if (!ReferenceEquals(specification, null))
                {
                    var content = context.Provider.ReadTextFile(specification);
                    context.WriteAndSetFlag(node, content);
                }
            }
        }

        // ================================================================

        //
        // Проверить наличие файла/корректность URL – &uf('+9L')
        // Вид функции: +9L.
        // Назначение: Проверить наличие файла/корректность URL.
        // Присутствует в версиях ИРБИС с 2013.1.
        // Формат (передаваемая строка):
        // +9L<path>,<dbname>,<filename>
        // где:
        // <path> – определяет путь к файлу и принимает значения:
        // 0 – папка, в которой установлена серверная часть ИРБИС
        // (<IRBIS_SERVER_ROOT>); 1 – общая директория баз данных
        // (по умолчанию <IRBIS_SERVER_ROOT>\DATAI);
        // 2, 3, 10 – папка БД<dbname>; 11 – абсолютный путь/URL.
        // <dbname> – имя БД(имеет смысл только при path= 2, 3, 10).
        // <filename> – путь и имя файла или URL.
        // Функция возвращает: 0 – если файл отсутствует/некорректный URL;
        // 1 – если файл присутствует/корректный URL.
        // Примеры:
        // &uf('+9L1,,\deposit\rksu.fst')
        // (&uf('+9L10,',&uf('+D'),',',v951^A))
        //

        /// <summary>
        /// UNIFOR('+9L'): check whether the file exist
        /// </summary>
        public static void FileExist
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var specification
                    = _GetFileSpecification(expression);
                if (!ReferenceEquals(specification, null))
                {
                    bool result;

                    var fileName = specification.FileName
                        .ThrowIfNull("specification.FileName");
                    if (fileName.StartsWith("http:")
                        || fileName.StartsWith("https:")
                        || fileName.StartsWith("ftp:"))
                    {
                        result = _CheckUrlExist(fileName);
                    }
                    else
                    {
                        result = context.Provider
                            .FileExist(specification);
                    }

                    var output = result ? "1" : "0";
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        // ================================================================

        //
        // Вернуть имя файла из заданного полного пути/имени – &uf('+91')
        // Вид функции: +91.
        // Назначение: Вернуть имя файла из заданного полного пути/имени.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +91
        //

        /// <summary>
        /// Get file name from full path.
        /// </summary>
        public static void GetFileName
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var output = Path.GetFileName(expression);
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Вернуть размер файла в байтах. – &uf('+9A')
        // Вид функции: +9A.
        // Назначение: Вернуть размер файла в байтах.
        // Формат (передаваемая строка):
        // +9A
        //

        /// <summary>
        /// UNIFOR('+9A'):  Get file size.
        /// </summary>
        public static void GetFileSize
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                long fileSize = 0;

                try
                {
                    var fileInfo = new FileInfo(expression);
                    fileSize = fileInfo.Length;
                }
                catch (Exception exception)
                {
                    Magna.TraceException
                        (
                            "UniforPlus9::GetFileSize",
                            exception
                        );
                }

                var output = fileSize.ToInvariantString();
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Вернуть номер текущего повторения в повторяющейся группе – &uf('+90')
        // Вид функции: +90.
        // Назначение: Вернуть номер текущего повторения в повторяющейся группе.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +90
        //

        /// <summary>
        /// Get field repeat.
        /// </summary>
        public static void GetIndex
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var index = context.Index;
            if (!ReferenceEquals(context.CurrentGroup, null))
            {
                index++;
            }

            var text = index.ToInvariantString();
            context.WriteAndSetFlag(node, text);
        }

        // ================================================================

        //
        // Вернуть номер поколения ИРБИС – &uf('+9V')
        // Вид функции: +9V.
        // Назначение: Возвращает поколение системы, в которой
        // осуществляется расформатирование. Может быть полезен при
        // разработке единых форматов, которые по-разному выполняются
        // в ИРБИС 32 и ИРБИС 64.
        // Присутствует в версиях ИРБИС с 2011.1.
        // Формат (передаваемая строка):
        // +9V
        // который возвращает:
        // 32 – если форматирование выполняется в ИРБИС 32;
        // 64 – если в ИРБИС 64 (и ИРБИС 128).
        //

        /// <summary>
        /// Get IRBIS generation (family): 32 or 64
        /// </summary>
        public static void GetGeneration
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            context.WriteAndSetFlag(node, context.Provider.GetGeneration());
        }

        // ================================================================

        //
        // Вернуть длину исходной строки – &uf('+95')
        // Вид функции: +95.
        // Назначение: Вернуть длину исходной строки.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +95<строка>
        //

        /// <summary>
        /// Get string length.
        /// </summary>
        public static void StringLength
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var output = "0";
            if (!string.IsNullOrEmpty(expression))
            {
                output = expression.Length.ToInvariantString();
            }

            context.WriteAndSetFlag(node, output);
        }

        // ================================================================

        //
        // Заменить в заданной строке один символ на другой – &uf('+98')
        // Вид функции: +98.
        // Назначение: Заменить в заданной строке один символ
        // на другой (регистр учитывается).
        // Присутствует в версиях ИРБИС с 2007.2.
        // Формат (передаваемая строка):
        // +98ab<строка>
        // где:
        // a – заменяемый символ;
        // b – заменяющий символ.
        // Примеры:
        // В результате выполнения формата
        // &uf('+98 0', f(1,5,0))
        // получится значение
        // 00001
        //

        /// <summary>
        /// Replace character in text.
        /// </summary>
        public static void ReplaceCharacter
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var navigator = new TextNavigator(expression);
                var first = navigator.ReadChar();
                var second = navigator.ReadChar();
                var text = navigator.GetRemainingText().ToString();
                if (!string.IsNullOrEmpty(text))
                {
                    var output = text.Replace(first, second);
                    context.WriteAndSetFlag(node, output);
                }
            }
        }

        // ================================================================

        //
        // Заменить в исходных данных некоторую заданную последовательность
        // символов другой заданной последовательностью символов – &uf('+9I')
        // Вид функции: +9I.
        // Назначение: Заменить в исходных данных некоторую заданную
        // последовательность символов другой заданной
        // последовательностью символов.
        // Присутствует в версиях ИРБИС с 2009.1.
        // Формат (передаваемая строка):
        // +9I!AAAA!/BBBB/<данные>
        // где АААА – последовательность символов, подлежащая замене;
        // ВВВВ – заменяющая последовательность символов;
        // символ ! – уникальный разделитель, отсутствующий в строке АААА;
        // символ / – уникальный разделитель, отсутствующий в строке ВВВВ.
        // ВВВВ может быть пустым значением, в этом случае последовательность
        // АААА будет удаляться.Обрабатываются ВСЕ (а не только первое)
        // вхождения АААА в исходные данные. В качестве разделителей можно
        // использовать ТОЛЬКО символы стандартного набора(с кодом менее 128).
        //

        /// <summary>
        /// Replace substring.
        /// </summary>
        public static void ReplaceString
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var navigator = new TextNavigator(expression);
                var firstDelimiter = navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    return;
                }

                var first = navigator.ReadUntil(firstDelimiter).ToString();
                navigator.ReadChar();
                if (navigator.IsEOF
                    || string.IsNullOrEmpty(first))
                {
                    return;
                }

                var secondDelimiter = navigator.ReadChar();
                if (navigator.IsEOF)
                {
                    return;
                }

                var second = navigator.ReadUntil(secondDelimiter).ToString();
                if (navigator.ReadChar() != secondDelimiter)
                {
                    return;
                }

                var text = navigator.GetRemainingText().ToString();
                if (string.IsNullOrEmpty(text))
                {
                    return;
                }

                var output = text.Replace(first, second);
                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Преобразовать заданную строку в список слов – &uf('+9G')
        // Вид функции: +9G.
        // Назначение: Преобразовать заданную строку в список слов.
        // Присутствует в версиях ИРБИС с 2008.1.
        // Формат (передаваемая строка):
        // +9G<строка>
        // Границы слов определяются на основе таблицы алфавитных символов.
        //

        /// <summary>
        /// Split text to word array.
        /// </summary>
        public static void SplitWords
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var words = PftUtility.ExtractWords(expression);
                for (var i = 0; i < words.Length; i++)
                {
                    var word = words[i].ToUpperInvariant();
                    context.WriteAndSetFlag(node, word);
                    context.WriteLine(node);
                }
            }
        }

        // ================================================================

        //
        // Вернуть часть строки – &uf('+96')
        // Вид функции: +96.
        // Назначение: Вернуть часть строки.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +96A*SSS.NNN#<строка>
        // где:
        // A – направление: 0 – с начала строки; 1 – с конца;
        // SSS – смещение;
        // NNN – кол-во символов.
        //
        // Примеры:
        //
        // &uf('+960*0.4#'v100)
        // &uf('+960*5.4#'v100)
        // &uf('+961*0.4#'v100)
        //

        /// <summary>
        /// Extract substring.
        /// </summary>
        public static void Substring
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                string output;
                var parts = expression.Split
                    (
                        CommonSeparators.NumberSign,
                        2
                    );

                if (parts.Length == 2)
                {
                    var prefix = parts[0];
                    output = parts[1];

                    var navigator = new TextNavigator(prefix);
                    var direction = navigator.ReadChar();
                    var offset = 0;
                    var length = output.Length;
                    string temp;
                    var haveLength = false;
                    if (navigator.PeekChar() == '*')
                    {
                        navigator.ReadChar();
                        temp = navigator.ReadInteger().ToString();
                        Utility.TryParseInt32(temp, out offset);
                    }

                    if (navigator.PeekChar() == '.')
                    {
                        navigator.ReadChar();
                        temp = navigator.ReadInteger().ToString();
                        Utility.TryParseInt32(temp, out length);
                        haveLength = true;
                    }

                    if (direction != '0')
                    {
                        if (!haveLength)
                        {
                            length -= offset;
                            offset = 0;
                        }
                        else
                        {
                            offset = output.Length - offset - length;
                        }
                    }

                    output = PftUtility.SafeSubString(output, offset, length);
                }
                else
                {
                    output = expression.Substring(1);
                }

                context.WriteAndSetFlag(node, output);
            }
        }

        // ================================================================

        //
        // Вернуть заданную строку в верхнем регистре – &uf('+97')
        // Вид функции: +97.
        // Назначение: Вернуть заданную строку в верхнем регистре.
        // Присутствует в версиях ИРБИС с 2006.1.
        // Формат (передаваемая строка):
        // +97<строка>
        //

        /// <summary>
        /// Convert text to upper case.
        /// </summary>
        public static void ToUpper
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var output = IrbisText.ToUpper(expression);
            context.WriteAndSetFlag(node, output);
        }

        // ================================================================

        //
        // Найти подстроку – &uf('+9S')
        // Вид функции: +9S.
        // Назначение: Возвращает позицию первого символа найденного
        // вхождения подстроки в исходную строку. Считается, что
        // символы в строке нумеруются с 1. Если подстрока не найдена,
        // то возвращает 0. Комментарий: следует отметить, что
        // в функции Вернуть часть строки – &uf('+96 указывают смещение,
        // а не позицию символа.
        // Присутствует в версиях ИРБИС с 2013.1.
        // Формат (передаваемая строка):
        // +9S!подстрока!<исходная_строка>
        // где подстрока – подстрока, которую нужно найти;
        // <исходная_строка> – исходная строка для поиска;
        // символ ! – уникальный разделитель, отсутствующий
        // в искомой подстроке.
        //

        /// <summary>
        /// Возвращает позицию первого символа найденного вхождения подстроки
        /// в исходную строку. Считается, что символы в строке нумеруются с 1.
        /// Если подстрока не найдена, то возвращает 0.
        /// </summary>
        public static void FindSubstring
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression))
            {
                goto NOTFOUND;
            }

            var navigator = new TextNavigator(expression);
            var delimiter = navigator.ReadChar();
            var substring = navigator.ReadUntil(delimiter).ToString();
            if (string.IsNullOrEmpty(substring)
                || navigator.ReadChar() != delimiter)
            {
                goto NOTFOUND;
            }

            var text = navigator.GetRemainingText().ToString();
            if (string.IsNullOrEmpty(text))
            {
                goto NOTFOUND;
            }

            var position = text.IndexOf
            (
                substring,
                StringComparison.CurrentCultureIgnoreCase
            );
            if (position < 0)
            {
                goto NOTFOUND;
            }

            var output = (position + 1).ToInvariantString();
            context.WriteAndSetFlag(node, output);

            return;

            NOTFOUND:
            context.WriteAndSetFlag(node, "0");
        }

        // ================================================================

        //
        // ibatrak
        //
        // &uf('+9T')
        // Неописанная функция
        // Выводит последовательные числа.
        // Вид: &uf('+9TA/B')
        // где A - начальное число, B - конечное (включая)
        // Выводимые числа выравниваются нулями по правому краю
        // по ширине числа A
        // В числе A лишние символы (пробелы и не-цифры) игнорируются.
        // Если A не удаётся интерпретировать как число,
        // оно считается равным 0.
        // Если слэш и число B отсутствуют, то выводится лишь A.
        // Если слэш присутствует и B не удаётся интерпретировать
        // как число, оно считается равным 0.
        //

        public static void PrintNumbers
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

            if (!expression.Contains("/"))
            {
                context.WriteAndSetFlag(node, expression);

                return;
            }

            var parts = expression.Split
                (
                    CommonSeparators.Slash,
                    2
                );
            var left = parts[0];
            var right = parts[1];
            var width = left.Length;
            var digits = Regex.Match(left, "-?\\d+").Value;
            var start = digits.SafeToInt64();
            var stop = right.SafeToInt64();
            var first = true;
            while (start <= stop)
            {
                if (!first)
                {
                    context.WriteLine(node);
                }

                var text = start.ToInvariantString();
                var length = text.Length;
                if (width < length)
                {
                    text = text.Substring(length - width);
                }
                else if (width > length)
                {
                    text = text.PadLeft(width, '0');
                }

                context.WriteAndSetFlag(node, text);

                start++;
                first = false;
            }
        }

        // ================================================================

        struct DecimalSuffix
        {
            public string Suffix { get; set; }
            public double Value { get; set; }
        }

        static readonly DecimalSuffix[] _suffixes =
        {
            new DecimalSuffix{Suffix =  "b", Value = 1E0},
            new DecimalSuffix{Suffix = "Kb", Value = 1E3},
            new DecimalSuffix{Suffix = "Mb", Value = 1E6},
            new DecimalSuffix{Suffix = "Gb", Value = 1E9},
            new DecimalSuffix{Suffix = "Tb", Value = 1E12},
            new DecimalSuffix{Suffix = "Pb", Value = 1E15},
            new DecimalSuffix{Suffix = "Eb", Value = 1E18},
            new DecimalSuffix{Suffix = "Zb", Value = 1E21},
            new DecimalSuffix{Suffix = "Yb", Value = 1E24},
        };

        //
        // ibatrak
        //
        // &uf('+9E')
        // Неописанная функция
        // Форматирует переданное число как размер файла.
        // Вид: &uf('+9EN')
        // где N - размер файла.
        // Если N < 1000, выводится суффикс b,
        // иначе, если N < 1000000, выводится суффкис Kb,
        // и так далее.
        //

        public static void FormatFileSize
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            var result = "0";

            // irbis64 использует int32, все что больше становится 0

            if (string.IsNullOrEmpty(expression))
            {
                goto DONE;
            }

            var size = expression.SafeToDouble(0.0);
            if (size <= 0)
            {
                goto DONE;
            }

            for (var i = _suffixes.Length - 1; i >= 0; i--)
            {
                var entry = _suffixes[i];
                if (size >= entry.Value)
                {
                    // ReSharper disable once CompareOfFloatsByEqualityOperator
                    if (entry.Value == 1.0) //-V3024
                    {
                        result = string.Format
                            (
                                CultureInfo.InvariantCulture,
                                "{0} {1}",
                                size,
                                entry.Suffix
                            );
                    }
                    else
                    {
                        result = string.Format
                            (
                                CultureInfo.InvariantCulture,
                                "{0:F3} {1}",
                                size / entry.Value,
                                entry.Suffix
                            );
                    }

                    break;
                }
            }

            DONE:
            context.WriteAndSetFlag(node, result);
        }

        // ================================================================

        //
        // Удалить заданный файл – &uf('+9K')
        // Вид функции: +9K.
        //
        // Назначение: Удалить заданный файл. Если имя файла задано
        // в виде маски, то удаляться будут все соответствующие
        // маске файлы.
        //
        // Присутствует в версиях ИРБИС с 2010.1.
        //
        // Формат (передаваемая строка):
        //
        // +9K<полный путь и имя файла>
        //

        public static void DeleteFiles
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

            var directoryName = Path.GetDirectoryName(expression);
            if (string.IsNullOrEmpty(directoryName))
            {
                return;
            }

            var files = Directory.GetFiles
                (
                    directoryName,
                    Path.GetFileName(expression)
                );
            foreach (var oneFile in files)
            {
                File.Delete(oneFile);
            }
        }

        // ================================================================

        //
        // Сохранить заданный внутренний двоичный объект в заданном файле - &uf('+9D')
        // Вид функции: +9D.
        //
        // Назначение: Сохранить заданный внутренний двоичный объект в заданном файле.
        //
        // Присутствует в версиях ИРБИС с 2006.2.
        //
        // Формат (передаваемая строка):
        //
        // +9DNN#<полный путь>
        // где NN – номер внутреннего двоичного объекта.
        //

        public static void SaveBinaryResource
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
                    CommonSeparators.NumberSign,
                    2
                );
            if (parts.Length != 2)
            {
                return;
            }

            var occurrence = parts[0].SafeToInt32();
            var path = parts[1];
            var record = context.Record;
            if (!ReferenceEquals(record, null) && occurrence > 0)
            {
                // TODO implement
                // var tag = Irbis64Config.IniParam("MAIN", "TAGINTERNALRESOURCE", "953").SafeParseInt32();
                var tag = 953;

                var field = record.Fields.GetField(tag, occurrence - 1);
                if (!ReferenceEquals(field, null))
                {
                    var resource = BinaryResource.Parse(field);
                    var format = resource.Kind;
                    var content = resource.Resource;
                    if (!ReferenceEquals(content, null))
                    {
                        if (!string.IsNullOrEmpty(format)
                            && string.IsNullOrEmpty(Path.GetExtension(path)))
                        {
                            path += "." + format;
                        }

                        var bytes = resource.Decode();
                        File.WriteAllBytes(path, bytes);
                    }
                }
            }
        }

        // ================================================================

        //
        // Представить заданный двоичный файл в кодированном виде – &uf('+9J')
        // Вид функции: +9J.
        //
        // Назначение: Представить заданный двоичный файл в виде:
        // ^A<тип_файла>^B<данные файла перекодированные на основе URLEncode>.
        //
        // Присутствует в версиях ИРБИС с 2010.1.
        //
        // Формат (передаваемая строка):
        //
        // +9J<полный путь и имя файла>
        //
        // Имя файла может задаваться в виде маски, в этом случае использоваться
        // будет первый найденный соответствующей маске файл.
        //

        /// <summary>
        /// Чтение файла как двоичного ресурса.
        /// </summary>
        public static void ReadFileAsBinaryResource
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

            var path = Path.GetDirectoryName(expression);
            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            var name = Path.GetFileName(expression);
            var files = Directory.GetFiles(path, name);
            if (files.Length == 0)
            {
                return;
            }
            var file = files[0];
            var format = Path.GetExtension(file).TrimStart('.');
            byte[] content;
            try
            {
                content = File.ReadAllBytes(file);
            }
            catch (Exception exception)
            {
                Magna.TraceException
                    (
                        nameof(UniforPlus9) + "::" + nameof(ReadFileAsBinaryResource),
                        exception
                    );

                return;
            }

            var resource = new BinaryResource
            {
                Kind = format
            };
            resource.Resource = resource.Encode(content);
            var field = resource.ToField();
            var output = field.ToText();
            context.WriteAndSetFlag(node, output);
        }

        // ================================================================

        //
        // ibatrak
        //
        // Неописанная функция unifor('+9H')
        // Параметры
        // +9H!строка А!строка Б
        // символ ! - разделитель
        // Если строка Б не пустая возвращает конкатенацию строк А и Б
        // иначе пустую строку
        //

        /// <summary>
        /// Условная конкатенация строк.
        /// </summary>
        public static void ConcatenateStrings
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (string.IsNullOrEmpty(expression)
                || expression.Length < 3)
            {
                return;
            }

            var separator = expression[0];
            var position = expression.IndexOf(separator, 1);
            if (position < 0)
            {
                return;
            }

            var first = expression.Substring(1, position - 1); //-V3057
            var second = expression.Substring(position + 1);
            if (string.IsNullOrEmpty(second))
            {
                return;
            }

            var output = first + second;
            context.WriteAndSetFlag(node, output);
        }

        // ================================================================

        //
        // Групповая установка глобальных переменных – &uf('+99')
        // Вид функции: +99.
        //
        // Назначение: Групповая установка глобальных переменных.
        // Применяется для ИРБИС-Навигатора.
        // Исходная строка закодирована с помощью URL-кодировки.
        // После раскодировки рассматривается как список строк
        // (аналогично TStringList.Text в Delphi).
        // Каждая строка этого списка имеет структуру:
        // NNN#<значение_глобальной_переменной_NNN>.
        //
        // Присутствует в версиях ИРБИС с 2006.1.
        //
        // Формат (передаваемая строка):
        //
        // +99
        //

        public static void AssignGlobals
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            if (!string.IsNullOrEmpty(expression))
            {
                var globals = context.Globals;
                globals.Clear(); // ???
                var decoded = IrbisUtility
                    .UrlDecode(expression, IrbisEncoding.Utf8)
                    .ThrowIfNull();
                var lines = decoded.SplitLines();
                foreach (var line in lines)
                {
                    if (string.IsNullOrEmpty(line))
                    {
                        continue;
                    }

                    var field = new Field();
                    field.Decode(line);
                    globals.Append(field.Tag, field.ToText());
                }
            }
        }

        // ================================================================

        // ibatrak
        private static void _ShowTerm
            (
                PftContext context,
                PftNode? node,
                string? expression,
                bool reverseOrder
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
            var startTerm = IrbisText.ToUpper(parts[1]);
            if (string.IsNullOrEmpty(database))
            {
                database = provider.Database;
            }

            var parameters = new TermParameters
            {
                Database = database,
                NumberOfTerms = 2,
                StartTerm = startTerm,
                ReverseOrder = reverseOrder
            };
            var terms = provider.ReadTerms(parameters);
            if (terms.Length != 0)
            {
                string? output;

                // если найдено точное совпадение термина, вернем следующий термин
                // при поиске в обратном направлении всегда возвращается предыдущий термин
                if (terms.Length == 2 && (terms[0].Text == startTerm || reverseOrder))
                {
                    output = terms[1].Text;
                }
                else
                {
                    // не найдено, вернем первый найденный термин
                    output = terms[0].Text;
                }

                context.WriteAndSetFlag(node, output);
            }
        }

        /// <summary>
        /// Search for next term.
        /// </summary>
        public static void NextTerm
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            _ShowTerm(context, node, expression, false);
        }

        /// <summary>
        /// Search for next term.
        /// </summary>
        public static void PreviousTerm
            (
                PftContext context,
                PftNode? node,
                string? expression
            )
        {
            _ShowTerm(context, node, expression, true);
        }

        // ================================================================

        //
        // Преобразование римского числа в арабское – &uf('+9R')
        // Вид функции: +9R.
        // Назначение: Преобразование римского числа в арабское.
        // Присутствует в версиях ИРБИС с 2011.1.
        // Формат (передаваемая строка):
        // +9R<римское_число>
        //

        static readonly string[] _replaceRom = { "CM", "CD", "XC", "XL", "IX", "IV" };
        static readonly string[] _replaceNum = { "DCCCC", "CCCC", "LXXXX", "XXXX", "VIIII", "IIII" };
        static readonly string[] _roman = { "M", "CM", "D", "CD", "C", "XC", "L", "XL", "X", "IX", "V", "IV", "I" };
        static readonly int[] arabic = { 1000, 900, 500, 400, 100, 90, 50, 40, 10, 9, 5, 4, 1 };

        //
        // Borrowed from
        // https://stackoverflow.com/questions/14900228/roman-numerals-to-integers
        //
        /// <summary>
        /// Converts a Roman number string into a Arabic number
        /// </summary>
        /// <param name="romanNumber">the Roman number string</param>
        /// <returns>the Arabic number (0 if the given string is not convertible to a Roman number)</returns>
        internal static int ToArabicNumber
            (
                string romanNumber
            )
        {
            return Enumerable.Range(0, _replaceRom.Length)
                .Aggregate
                    (
                        romanNumber,
                        (agg, cur) => agg.Replace(_replaceRom[cur], _replaceNum[cur]),
                        agg => agg.ToArray()
                    )
                .Aggregate
                    (
                        0,
                        (agg, cur) =>
                            {
                                var idx = Array.IndexOf(_roman, cur.ToString());
                                return idx < 0 ? 0 : agg + arabic[idx];
                            },
                        agg => agg
                    );
        }

        /// <summary>
        /// ibatrak преобразование арабского числа в римское
        /// </summary>
        public static void RomanToArabic
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

            var num = ToArabicNumber(expression.ToUpperInvariant());
            var output = num.ToInvariantString();

            context.WriteAndSetFlag(node, output);
        }

        // ================================================================

        //
        // Преобразование арабского числа в римское – &uf('+9X')
        // Вид функции: +9X.
        // Назначение: Преобразование арабского числа в римское.
        // Присутствует в версиях ИРБИС с 2011.1.
        // Формат (передаваемая строка):
        // +9X<арабское_число>
        //

        /// <summary>
        /// ibatrak преобразование арабского числа в римское
        /// </summary>
        public static void ArabicToRoman
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

            var num = expression.SafeToInt32();
            var output = UniforS.ToRoman(num);

            context.WriteAndSetFlag(node, output);
        }

        #endregion

    } // class UniforPlus9

} // namespace ManagedIrbis.Pft.Infrastructure.Unifors
