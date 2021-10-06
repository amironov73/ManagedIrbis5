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
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Builder;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis.Scripting
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
        }

        /// <summary>
        /// Вызывается перед форматированием всех записей.
        /// </summary>
        public virtual void BeforeAll()
        {
            // метод необходимо перегрузить в потомке
        }

        /// <summary>
        /// Получение текста поля до разделителей подполей
        /// первого повторения поля с указанной меткой.
        /// </summary>
        public string? FM (int tag) => Record?.FM (tag);

        /// <summary>
        /// Текст первого подполя с указанным тегом и кодом.
        /// </summary>
        public string? FM (int tag, char code) => Record?.FM (tag, code);

        /// <summary>
        /// Текст всех полей с указанным тегом.
        /// </summary>
        public string[] FMA (int tag) => Record?.FMA (tag) ?? Array.Empty<string>();

        /// <summary>
        /// Текст всех подполей с указанным тегом и кодом.
        /// </summary>
        public string[] FMA (int tag, char code) => Record?.FMA(tag, code) ?? Array.Empty<string>();

        /// <summary>
        /// Проверка, есть ли в записи поле с указанной меткой.
        /// </summary>
        public bool HaveField (int tag) => Record?.HaveField (tag) ?? false;

        /// <summary>
        /// Форматирование записи.
        /// </summary>
        public virtual void FormatRecord()
        {
            // метод нужно переопределить в потомке
        }

        /// <summary>
        /// Вывод текста.
        /// </summary>
        public void Write (string text) => Output.Write (text);

        /// <summary>
        /// Переход на новую строку.
        /// </summary>
        public void WriteLine() => Output.WriteLine();

        /// <summary>
        /// Вывод текста.
        /// </summary>
        public void WriteLine (string text) => Output.WriteLine (text);

        #endregion

    } // class ScriptContext

} // namespace ManagedIrbis.Scripting
