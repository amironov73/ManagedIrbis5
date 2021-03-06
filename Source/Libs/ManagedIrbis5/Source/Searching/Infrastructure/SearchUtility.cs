﻿// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CommentTypo
// ReSharper disable CheckNamespace
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* SearchUtility.cs -- разнообразные утилиты для поиска
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Text;

using AM;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Разнообразные утилиты, используемые
    /// при работе с поисковыми выражениями.
    /// </summary>
    public static class SearchUtility
    {
        #region Constants

        /// <summary>
        /// Maximal term length (bytes, not characters!).
        /// </summary>
        public const int MaxTermLength = 255;

        #endregion

        #region Properties

        /// <summary>
        /// Special symbols.
        /// </summary>
        public static char[] SpecialSymbols =
        {
            ' ', '(', ')', '+', '*', '.', '"'
        };

        /// <summary>
        /// Empty array.
        /// </summary>
        internal static readonly ISearchTree[] EmptyArray = new ISearchTree[0];

        #endregion

        #region Public methods

        /// <summary>
        /// Concatenate some terms together.
        /// </summary>
        public static string ConcatTerms
            (
                string? prefix,
                string? operation,
                IEnumerable<string> terms
            )
        {
            bool first = true;
            StringBuilder result = new StringBuilder();

            foreach (string term in terms)
            {
                if (!string.IsNullOrEmpty(term))
                {
                    if (!first)
                    {
                        result.Append(operation);
                    }

                    string wrapped = WrapTerm
                    (
                        TrimTerm(prefix + term)
                    );
                    result.Append(wrapped);

                    first = false;
                }
            }

            if (first)
            {
                Magna.Error
                    (
                        nameof(SearchUtility) + "::" + nameof(ConcatTerms)
                        + ": empty list of terms"
                    );

                throw new IrbisException("Empty list of terms");
            }

            return result.ToString();
        }

        /// <summary>
        /// Escape quotation mark for Web-IRBIS.
        /// </summary>
        public static string? EscapeQuotation
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains("\""))
            {
                return text;
            }

            string result = text.Replace
                (
                    "\"",
                    "<.>"
                );

            return result;
        }

        /// <summary>
        /// Trim the term (if exceeds <see cref="MaxTermLength"/>
        /// bytes).
        /// </summary>
        public static string TrimTerm
            (
                string term
            )
        {
            Sure.NotNull(term, nameof(term));

            int originalLength = term.Length;

            // Simple optimization
            if (originalLength < MaxTermLength / 2)
            {
                // Garanteed to fit into
                return term;
            }

            Encoding encoding = IrbisEncoding.Utf8;
            char[] charArray = term.ToCharArray();
            int currentLength = Math.Max(originalLength, MaxTermLength);

            while (currentLength > 0)
            {
                int count = encoding.GetByteCount
                    (
                        charArray,
                        0,
                        currentLength
                    );
                if (count <= MaxTermLength)
                {
                    break;
                }
                currentLength--;
            }

            string result = currentLength == originalLength
                ? term
                : term.Substring(0, currentLength);

            return result;
        }

        /// <summary>
        /// Unescape quotation mark for Web-IRBIS.
        /// </summary>
        public static string? UnescapeQuotation
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains("<.>"))
            {
                return text;
            }

            string result = text.Replace
                (
                    "<.>",
                    "\""
                );

            return result;
        }

        /// <summary>
        /// Wrap the term if needed.
        /// </summary>
        public static string WrapTerm
            (
                string term
            )
        {
            /*
            string result = term.ContainsAny(SpecialSymbols)
                ? "\"" + term + "\""
                : term;

            return result;
            */

            return term;
        }

        #endregion

    } // class SearchUtility

} // namespace ManagedIrbis.Infrastructure
