// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* PhoneUtility.cs -- утилиты для работы с номером телефона.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using AM.Text;

#endregion

#nullable enable

/*
 * https://ru.wikipedia.org/wiki/%D0%A2%D0%B5%D0%BB%D0%B5%D1%84%D0%BE%D0%BD%D0%BD%D1%8B%D0%B9_%D0%BD%D0%BE%D0%BC%D0%B5%D1%80
 *
 * Телефонный номер (или абонентский номер) — последовательность цифр
 * (реже также букв[1]), присвоенная пользователю или абоненту телефонной
 * сети, зная которую, можно ему позвонить. С технической точки зрения
 * телефонный номер — необходимое условие автоматической коммутации вызова,
 * которое определяет маршрут его прохождения и поиска телефонного
 * оборудования вызываемого пользователя для соединения (в рамках
 * сигнализации). Телефонный номер назначается обслуживающим персоналом
 * АТС или коммутатора, так чтобы каждый пользователь сети имел уникальную
 * идентификацию. При подключении к телефонной сети общего пользования
 * абонентский номер (идентификатор, ID) выделяется компанией-оператором
 * связи при заключении договора об оказании услуг телефонной связи.
 * В свою очередь, регулированием и распределением диапазонов (блоков)
 * номеров в глобальной телефонной сети общего пользования между компаниями,
 * также как и стандартизацией и общим контролем за услугами связи
 * занимаются соответствующие государственные и международные организации.
 *
 * В частности, существует рекомендация ITU-T под номером E.164, определяющая
 * общий международный телекоммуникационный план нумерации, используемый
 * в телефонных сетях общего пользования и некоторых других сетях.
 * Согласно E.164 номера могут иметь максимум 15 цифр и обычно записываются
 * с префиксом «+».
 *
 * https://ru.wikipedia.org/wiki/%D0%A2%D0%B5%D0%BB%D0%B5%D1%84%D0%BE%D0%BD%D0%BD%D1%8B%D0%B9_%D0%BF%D0%BB%D0%B0%D0%BD_%D0%BD%D1%83%D0%BC%D0%B5%D1%80%D0%B0%D1%86%D0%B8%D0%B8_%D0%A0%D0%BE%D1%81%D1%81%D0%B8%D0%B8
 *
 * Телефонный план нумерации России — диапазоны телефонных номеров, выделяемых
 * различным пользователям телефонной сети общего пользования в России,
 * специальные номера и другие особенности набора для совершения телефонных
 * вызовов. Все международные номера пользователей данной телефонной сети
 * имеют общее начало +7 - называемый префиксом или телефонным кодом страны.
 *
 * В сети общего пользования России применяются номера с тем же префиксом,
 * что и в Казахстане и Абхазии. Между ними разделен международный код +7,
 * разница состоит в следующих значащих цифрах префикса.
 *
 * Национальные номера абонентов (то есть без префиксов, таких как международный
 * +7 или национальный 8) состоят из 10 цифр, в которые входят зоновый код
 * (3 цифры) и номер абонента (7 цифр).
 *
 */

namespace AM.Net
{
    /// <summary>
    /// Утилиты для работы с номером телефона.
    /// </summary>
    public static class PhoneUtility
    {
        #region Public methods

        /// <summary>
        /// Очистка номера от лишних символов.
        /// Перевод кириллических символов в латиницу.
        /// </summary>
        public static string CleanupNumber
            (
                string number
            )
        {
            var builder = StringBuilderPool.Shared.Get();
            if (number.FirstChar() == '+')
            {
                builder.Append('+');
            }

            if (number.FirstChar() == '8')
            {
                builder.Append ('+');
                builder.Append ('7');
                number = number.Substring (1);
            }

            foreach (var c in number)
            {
                if (c.IsArabicDigit())
                {
                    builder.Append (c);
                }
            }

            if (builder.Length > 10 && builder[0] == '7')
            {
                builder.Insert (0, '+');
            }

            var result = builder.ToString();
            StringBuilderPool.Shared.Return(builder);

            return result;

        } // method CleanupNumber

        /// <summary>
        /// Верификация (весьма приблизительная) номера телефона.
        /// </summary>
        public static bool VerifyNumber (string number) =>
            (number.StartsWith ("8") || number.StartsWith ("+7"))
            &&  number.Length is >= 10 and <= 15;

        #endregion

    } // class PhoneUtility

} // namespace AM
