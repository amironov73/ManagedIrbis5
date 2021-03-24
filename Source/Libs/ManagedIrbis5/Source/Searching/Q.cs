// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo

/* Q.cs -- search query builder.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Linq;
using System.Text;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Search query builder.
    /// </summary>
    public static class Q
    {
        #region Private members

        private static readonly char[] _specialSymbols
            = { ' ', '+', '*', '(', ')', '"' };

        #endregion

        #region Public methods

        /// <summary>
        /// Логическое "ВСЕ".
        /// </summary>
        public static string All()
        {
            return "I=$";
        }

        /// <summary>
        /// Логическое умножение.
        /// </summary>
        public static string And
            (
                this string left,
                string right
            )
        {
            return "(" + WrapIfNeeded(left) + " * " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое умножение.
        /// </summary>
        public static string And
            (
                params string[] items
            )
        {
            if (items.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            if (items.Length == 1)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    throw new SearchSyntaxException();
                }

                return WrapIfNeeded(items[0]);
            }

            var length = items.Sum(item => item.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            var first = true;
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new SearchSyntaxException();
                }

                if (!first)
                {
                    result.Append(" * ");
                }

                result.Append(WrapIfNeeded(item));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Склеивает строки.
        /// </summary>
        public static string Concat
            (
                params string[] items
            )
        {
            string result = WrapIfNeeded(string.Concat(items));

            return result;
        }

        /// <summary>
        /// Поиск вида "префикс=значение".
        /// </summary>
        public static string Equals
            (
                this string prefix,
                string value
            )
        {
            return Concat(prefix, value);
        }

        /// <summary>
        /// Поиск вида "префикс=значение".
        /// </summary>
        public static string Equals
            (
                this string prefix,
                string value1,
                string value2
            )
        {
            return Or
                (
                    Concat(prefix, value1),
                    Concat(prefix, value2)
                );
        }

        /// <summary>
        /// Поиск вида "префикс=значение1 + префикс=значение2".
        /// </summary>
        public static string Equals
            (
                this string prefix,
                params string[] values
            )
        {
            if (values.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            foreach (var value in values)
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new SearchSyntaxException();
                }
            }

            if (values.Length == 1)
            {
                return Equals(prefix, values[0]);
            }

            var length = values.Sum(item => item.Length + prefix.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            var first = true;
            foreach (string value in values)
            {
                if (!first)
                {
                    result.Append(" + ");
                }

                result.Append(Equals(prefix, value));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Нужно ли заключить данный текст в кавычки?
        /// </summary>
        public static bool NeedWrap
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                // Пустую строку всегда нужно оборачивать.
                return true;
            }

            var first = text.FirstChar();
            if (first == '"' || first == '(')
            {
                // Строка уже заключена в кавычки
                // Бывают также случаи "K=очист$"/(200,922)
                return false;
            }

            return text.ContainsAnySymbol(_specialSymbols);
        }

        /// <summary>
        /// Логическое "НЕ".
        /// </summary>
        public static string Not
            (
                this string left,
                string right
            )
        {
            return "(" + WrapIfNeeded(left) + " ^ " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое сложение.
        /// </summary>
        public static string Or
            (
                this string left,
                string right
            )
        {
            return "(" + WrapIfNeeded(left) + " + " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое сложение.
        /// </summary>
        public static string Or
            (
                params string[] items
            )
        {
            if (items.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            if (items.Length == 1)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    throw new SearchSyntaxException();
                }

                return WrapIfNeeded(items[0]);
            }

            var length = items.Sum(item => item.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            var first = true;
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new SearchSyntaxException();
                }

                if (!first)
                {
                    result.Append(" + ");
                }

                result.Append(WrapIfNeeded(item));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Логическое "В том же поле".
        /// </summary>
        public static string SameField
            (
                this string left,
                string right
            )
        {
            return "(" + WrapIfNeeded(left) + " (G) " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое "В том же поле".
        /// </summary>
        public static string SameField
            (
                params string[] items
            )
        {
            if (items.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            if (items.Length == 1)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    throw new SearchSyntaxException();
                }

                return WrapIfNeeded(items[0]);
            }

            var length = items.Sum(item => item.Length + 3);
            StringBuilder result = new StringBuilder(length);
            result.Append('(');
            var first = true;
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new SearchSyntaxException();
                }

                if (!first)
                {
                    result.Append(" (G) ");
                }

                result.Append(WrapIfNeeded(item));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Логическое "В том же повторении поля".
        /// </summary>
        public static string SameRepeat
            (
                this string left,
                string right
            )
        {
            return "(" + WrapIfNeeded(left) + " (F) " + WrapIfNeeded(right) + ")";
        }

        /// <summary>
        /// Логическое "В том же повторении поля".
        /// </summary>
        public static string SameRepeat
            (
                params string[] items
            )
        {
            if (items.Length == 0)
            {
                throw new SearchSyntaxException();
            }

            if (items.Length == 1)
            {
                if (string.IsNullOrEmpty(items[0]))
                {
                    throw new SearchSyntaxException();
                }

                return WrapIfNeeded(items[0]);
            }

            var length = items.Sum(item => item.Length + 3);
            var result = new StringBuilder(length);
            result.Append('(');
            var first = true;
            foreach (string item in items)
            {
                if (string.IsNullOrEmpty(item))
                {
                    throw new SearchSyntaxException();
                }

                if (!first)
                {
                    result.Append(" (F) ");
                }

                result.Append(WrapIfNeeded(item));
                first = false;
            }

            result.Append(')');

            return result.ToString();
        }

        /// <summary>
        /// Обёртывает кавычками текст при необходимости.
        /// </summary>
        public static string WrapIfNeeded
            (
                string? text
            )
        {
            return NeedWrap(text) ? "\"" + text + "\"" : text.ThrowIfNull();
        }

        #endregion
    }
}
