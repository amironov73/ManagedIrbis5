// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ScriptContext.cs -- контекст, в котором выполняется скриптованное форматирование
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.IO;

using ManagedIrbis.Pft.Infrastructure.Unifors;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting.Sharping
{
    /// <summary>
    /// Контекст, в котором выполняется скриптованное форматирование.
    /// </summary>
    public class ScriptContext
    {
        #region Properties

        /// <summary>
        /// Выходной поток.
        /// </summary>
        public TextWriter Output { get; }

        /// <summary>
        /// Синхронный провайдер (на всякий случай).
        /// </summary>
        public ISyncProvider Provider { get; }

        /// <summary>
        /// Запись, подлежащая форматированию.
        /// </summary>
        public Record? Record { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор.
        /// </summary>
        public ScriptContext
            (
                ISyncProvider provider,
                TextWriter output
            )
        {
            Provider = provider;
            Output = output;

        } // constructor

        #endregion

        #region Public methods

        /// <summary>
        /// Вызывается после форматирования всех записей.
        /// </summary>
        public virtual void AfterAll()
        {
            // метод необходимо перегрузить в потомке

        } // method AfterAll

        /// <summary>
        /// Вызывается перед форматированием всех записей.
        /// </summary>
        public virtual void BeforeAll()
        {
            // метод необходимо перегрузить в потомке

        } // method BeforeAll

        /// <summary>
        /// Число повторений поля с указанной меткой.
        /// </summary>
        public int Count (int tag) => Record?.Count (tag) ?? 0;

        /// <summary>
        /// Декодирование конструкции &lt;=&gt;.
        /// </summary>
        public string DecodeTitle (string? title, bool before) =>
            UniforPlusS.DecodeTitle (title, before);

        /// <summary>
        /// Заданное количество слов с начала строки.
        /// </summary>
        public string? FirstWords (string? text, int words) =>
            UniforE.GetFirstWords (text, words);

        /// <summary>
        /// Получение текста поля до разделителей подполей
        /// первого повторения поля с указанной меткой.
        /// </summary>
        public string? FM (int tag) => Record?.FM (tag);

        /// <summary>
        /// Получение текста поля до разделителей подполей
        /// указанного повторения поля.
        /// </summary>
        public string? FM (int tag, int occurrence) => Record?.GetField (tag, occurrence)?.Value;

        /// <summary>
        /// Текст первого подполя с указанным тегом и кодом.
        /// </summary>
        public string? FM (int tag, char code) => Record?.FM (tag, code);

        /// <summary>
        /// Текст первого подполя с указанным тегом и кодом у зданного повторения поля.
        /// </summary>
        public string? FM (int tag, int occurrence, char code) =>
            Record?.GetField (tag, occurrence)?.GetFirstSubFieldValue (code);

        /// <summary>
        /// Текст всех полей с указанным тегом.
        /// </summary>
        public string[] FMA (int tag) => Record?.FMA (tag) ?? Array.Empty<string>();

        /// <summary>
        /// Текст всех подполей с указанной меткой и кодом.
        /// </summary>
        public string[] FMA (int tag, char code) => Record?.FMA(tag, code) ?? Array.Empty<string>();

        /// <summary>
        /// Форматирование даты как в unifor('3')
        /// </summary>
        public string FormatDate (string format) => Unifor3.FormatDate (DateTime.Now, format);

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        public virtual void FormatRecord()
        {
            // метод нужно переопределить в потомке
        }

        /// <summary>
        /// Проверка, есть ли в записи поле с указанной меткой.
        /// </summary>
        public bool HaveField (int tag) => Record?.HaveField (tag) ?? false;

        /// <summary>
        /// Проверка, есть ли в записи подполе с указанной меткой.
        /// </summary>
        public bool HaveSubfield (int tag, char code) =>
            Record?.GetField (tag)?.HaveSubField (code) ?? false;

        /// <summary>
        /// Простейшее расформатирование на уровне "v910^b + |, |".
        /// </summary>
        public void S (string? format) => Record.SimpleFormat (format);

        /// <summary>
        /// Пропуск заданного количества слов с начала строки.
        /// </summary>
        public string? LastWords (string? text, int words) =>
            UniforF.GetLastWords (text, words);

        /// <summary>
        /// Простейшее расформатирование на уровне "v910^b + |, |".
        /// </summary>
        public void V (string? format) => Record.SimpleFormat (format);

        /// <summary>
        /// Вывод значения поля до первого разделителя
        /// или подполя с заданным кодом (с учетом повторяющихся групп).
        /// </summary>
        public bool V
            (
                int tag,
                char? code = null,
                string? prefix = null,
                string? before = null,
                string? after = null,
                string? suffix = null,
                bool skipFirst = false,
                bool skipLast = false
            )
            => RepeatingGroup.V (Output, Record, tag, code, prefix, before, after, suffix, skipFirst, skipLast);

        /// <summary>
        /// Вывод текста.
        /// </summary>
        public void Write (string? text) => Output.Write (text);

        /// <summary>
        /// Переход на новую строку.
        /// </summary>
        public void WriteLine() => Output.WriteLine();

        /// <summary>
        /// Вывод текста с последующим переходом на новую строку.
        /// </summary>
        public void WriteLine (string? text) => Output.WriteLine (text);

        #endregion

    } // class ScriptContext

} // namespace ManagedIrbis.Scripting
