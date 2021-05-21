// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Local
// ReSharper disable UnusedMember.Global

/* FormatError.cs -- коды ошибок PFT-форматтера
 * Ars Magna project, http://arsmagna.ru
 */

using AM;

#nullable enable

namespace ManagedIrbis.Pft
{
    /// <summary>
    /// Коды ошибок PFT-форматтера.
    /// В данный момент никак не используются.
    /// </summary>
    public static class FormatError
    {
        #region Nested classes

        class Pair
        {
            public int Code { get; set; }

            public string? Message { get; set; }
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Получение текста сообщения по коду ошибки.
        /// </summary>
        /// <param name="code">Код ошибки.</param>
        /// <returns>Текст сообщения об ошибке (или об ее отсутствии).</returns>
        public static string GetMessageForCode
            (
                int code
            )
        {
            foreach (var pair in _knownCodes)
            {
                if (pair.Code == code)
                {
                    return pair.Message.ThrowIfNull("pair.Message");
                }
            }

            return "Неизвестная ошибка";
        }

        #endregion

        #region Private members

        private static readonly Pair[] _knownCodes =
        {
            new() { Code = 1, Message = "Обнаружен конец формата " +
                                 "в процессе обработки повторяющейся группы. Возможно, " +
                                 "пропущена закрывающая скобка повторяющейся группы." },

            new() { Code = 2, Message = "Вложенность повторяющейся " +
                                 "группы (т.е. одна повторяющаяся группа расположена " +
                                 "внутри другой повторяющейся группы)."},

            new() { Code = 8, Message = "Команда IF без THEN." },

            new() { Code = 19, Message = "Непарная открывающаяся скобка (." },

            new() { Code = 20, Message = "Непарная закрывающаяся скобка ). Также может быть вызвано наличием " +
                                  "неправильного операнда в выражении."},

            new() { Code = 26, Message = "Два операнда различных " +
                                        "типов в одном операторе (например, попытка сложить " +
                                        "строковый операнд с числом)."},
            new() { Code = 28, Message = "Первый аргумент функции " +
                                        "REF - нечисловое выражение."},

            new() { Code = 51, Message = "Слишком много литералов " +
                                  "и/или условных команд связано с командой вывода поля."},

            new() { Code = 53, Message = "IF команда не завершена ключевым словом FI."},

            new() { Code = 54, Message = "Знак + не соответствует контексту: CDS/ISIS предполагает наличие " +
                                  "повторяющегося литерала за знаком +."},
            new() { Code = 55, Message = "Непарное ключевое слово FI."},

            new() { Code = 56, Message = "Переполнение рабочей " +
                                  "области: формат создает слишком большой выходной " +
                                  "текст, который система не может обработать."},

            new() { Code = 57, Message = "Зацикливание повторяющейся" +
                                        " группы"},

            new() { Code =58,Message = "Один или более аргументов функции F - нечисловые выражения."},

            new() { Code =60, Message = "Нестроковая функция используется как команда (только строковые функции " +
                                 "могут быть использованы в качестве команды)."},

            new() { Code =61, Message = "Аргумент функции A или Р - не команда вывода поля."},

            new() { Code =99, Message = "Неизвестная команда " +
                                 "(например, ошибка в правильности написания имени " +
                                 "функции или команды), возможен также пропуск " +
                                 "закрывающего ограничителя литерала."},

            new() { Code =101, Message = "Переполнение стека (возможно из-за наличия слишком сложного выражения)."},

            new() { Code =102, Message = "Некорректная работа со стеком (может быть из-за непарной открывающей " +
                                  "скобки)." }
        };

        #endregion
    }
}
