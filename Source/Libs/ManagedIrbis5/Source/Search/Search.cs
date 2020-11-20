// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* Search.cs -- построитель поисковых запросов
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Text;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Построитель поисковых запросов.
    /// </summary>
    public class Search
    {
        #region Private members

        private string _buffer = string.Empty;

        private static bool NeedWrap
            (
                string value
            )
        {
            if (string.IsNullOrEmpty(value))
            {
                return true;
            }

            var c = value[0];
            if (c == '"' || c == '\'')
            {
                return false;
            }

            return value.Contains(' ')
                || value.Contains('+')
                || value.Contains('*')
                || value.Contains('^')
                || value.Contains('#')
                || value.Contains('(')
                || value.Contains(')')
                || value.Contains('"');
        }

        private static string Wrap
            (
                string value
            )
        {
            return '"' + value + '"';
        }

        /// <summary>
        /// Оборачивает кавычками строку, если необходимо.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string WrapIfNeeded
            (
                string value
            )
        {
            return NeedWrap(value)
                ? Wrap(value)
                : value;
        }

        private Search Construct
            (
                string operatorText,
                string[] others
            )
        {
            var result = new StringBuilder("(" + _buffer);

            foreach (var other in others)
            {
                result.Append(operatorText);
                result.Append(WrapIfNeeded(other));
            }

            result.Append(")");

            return result.ToString();
        }

        private Search Construct
            (
                string operatorText,
                Search[] others
            )
        {
            var result = new StringBuilder("(" + _buffer);

            foreach (var other in others)
            {
                result.Append(operatorText);
                result.Append(WrapIfNeeded(other));
            }

            result.Append(")");

            return result.ToString();
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Запрос "все документы в базе данных".
        /// </summary>
        public static Search All() => new Search() {_buffer = "I=$"};

        /// <summary>
        /// Логическое И.
        /// </summary>
        /// <param name="others">Добавляемые выражения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search And(params string[] others)
            => Construct(" * ", others);

        /// <summary>
        /// Логическое И.
        /// </summary>
        /// <param name="others">Добавляемые выражения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search And(params Search[] others)
            => Construct(" * ", others);

        /// <summary>
        /// Поиск по совпадению с заданным значением.
        /// </summary>
        /// <param name="prefix">Префикс терминов в словаре.</param>
        /// <param name="values">Искомые значения (непустой массив).</param>
        /// <returns>Сконструированный запрос.</returns>
        public static Search EqualsTo
            (
                string prefix,
                params string[] values
            )
        {
            var result = new StringBuilder();
            var first = true;

            foreach (var value in values)
            {
                if (!first)
                {
                    result.Append(" + ");
                }

                result.Append(WrapIfNeeded(prefix + value));

                first = false;
            }

            return result.ToString();
        }

        /// <summary>
        /// Логическое НЕ.
        /// </summary>
        /// <param name="value">Правая часть оператора.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search Not
            (
                string value
            )
        {
            return _buffer = "("
                + _buffer
                + " ^ "
                + WrapIfNeeded(value)
                + ")";
        }

        /// <summary>
        /// Логическое ИЛИ.
        /// </summary>
        /// <param name="others">Добавляемые выражения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search Or(params string[] others)
            => Construct(" + ", others);

        /// <summary>
        /// Логическое ИЛИ.
        /// </summary>
        /// <param name="others">Добавляемые выражения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search Or(params Search[] others)
            => Construct(" + ", others);

        /// <summary>
        /// Оператор В ТОМ ЖЕ ПОЛЕ.
        /// </summary>
        /// <param name="others">Добавляемые выражения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search SameField(params string[] others)
            => Construct(" (G) ", others);

        /// <summary>
        /// Оператор В ТОМ ЖЕ ПОЛЕ.
        /// </summary>
        /// <param name="others">Добавляемые выражения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search SameField(params Search[] others)
            => Construct(" (G) ", others);

        /// <summary>
        /// Оператор В ТОМ ЖЕ ПОВТОРЕНИИ ПОЛЯ.
        /// </summary>
        /// <param name="others">Добавляемые выражения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search SameRepeat(params string[] others)
            => Construct(" (F) ", others);

        /// <summary>
        /// Оператор В ТОМ ЖЕ ПОВТОРЕНИИ ПОЛЯ.
        /// </summary>
        /// <param name="others">Добавляемые выражения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public Search SameRepeat(params Search[] others)
            => Construct(" (F) ", others);

        /// <summary>
        /// Поиск по ключевому слову.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Keyword(params string[] values)
            => EqualsTo("K=", values);

        /// <summary>
        /// Поиск по автору.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Author(params string[] values)
            => EqualsTo("A=", values);

        /// <summary>
        /// Поиск по заглавию.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Title(params string[] values)
            => EqualsTo("T=", values);

        /// <summary>
        /// Поиск по издательству.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Publisher(params string[] values)
            => EqualsTo("O=", values);

        /// <summary>
        /// Поиск по месту издания.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Place(params string[] values)
            => EqualsTo("MI=", values);

        /// <summary>
        /// Поиск по предметной рубрике.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Subject(params string[] values)
            => EqualsTo("S=", values);

        /// <summary>
        /// Поиск по языку текста.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Language(params string[] values)
            => EqualsTo("J=", values);

        /// <summary>
        /// Поиск по году издания.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Year(params string[] values)
            => EqualsTo("G=", values);

        /// <summary>
        /// Поиск по заглавию журнала.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Magazine(params string[] values)
            => EqualsTo("TJ=", values);

        /// <summary>
        /// Поиск по виду документа.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search DocumentKind(params string[] values)
            => EqualsTo("V=", values);

        /// <summary>
        /// Поиск по УДК.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Udc(params string[] values)
            => EqualsTo("U=", values);

        /// <summary>
        /// Поиск по ББК.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Bbk(params string[] values)
            => EqualsTo("BBK=", values);

        /// <summary>
        /// Поиск по разделу знаний.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Rzn(params string[] values)
            => EqualsTo("RZN=", values);

        /// <summary>
        /// Поиск по месту хранения.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Mhr(params string[] values)
            => EqualsTo("MHR=", values);

        /// <summary>
        /// Поиск по инвентарному номеру.
        /// </summary>
        /// <param name="values">Искомые значения.</param>
        /// <returns>Сконструированное выражение.</returns>
        public static Search Number(params string[] values)
            => EqualsTo("IN=", values);

        #endregion

        #region Operators

        public static implicit operator string(Search search) => search._buffer;

        public static implicit operator Search(string value)
            => new Search { _buffer = value };

        #endregion

        #region Object members

        /// <inheritdoc cref="Object.ToString"/>
        public override string ToString()
        {
            return _buffer;
        }

        #endregion
    }
}