// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* IrbisFormat.cs -- работа с форматами ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ManagedIrbis.Infrastructure;
using ManagedIrbis.Infrastructure.Sockets;

#endregion

#nullable enable

namespace ManagedIrbis.Infrastructure
{
    /// <summary>
    /// Работа с форматами ИРБИС64.
    /// </summary>
    public static class IrbisFormat
    {
        #region Constants

        /// <summary>
        /// Format ALL.
        /// </summary>
        public const string All = "&uf('+0')";

        /// <summary>
        /// BRIEF format.
        /// </summary>
        public const string Brief = "@brief";

        /// <summary>
        /// IBIS format.
        /// </summary>
        public const string Ibis = "@ibiskw_h";

        /// <summary>
        /// Informational format.
        /// </summary>
        public const string Informational = "@info_w";

        /// <summary>
        /// Optimized format.
        /// </summary>
        public const string Optimized = "@";

        #endregion

        #region Public methods

        /// <summary>
        /// Remove comments from the format.
        /// </summary>
        public static string? RemoveComments
            (
                string? text
            )
        {
            const char zero = '\0';

            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            if (!text.Contains("/*"))
            {
                return text;
            }

            int index = 0, length = text.Length;
            StringBuilder result = new StringBuilder(length);
            char state = zero;

            while (index < length)
            {
                char c = text[index];

                switch (state)
                {
                    case '\'':
                    case '"':
                    case '|':
                        if (c == state)
                        {
                            state = zero;
                        }
                        result.Append(c);
                        break;

                    default:
                        if (c == '/')
                        {
                            if (index + 1 < length && text[index + 1] == '*')
                            {
                                while (index < length)
                                {
                                    c = text[index];
                                    if (c == '\r' || c == '\n')
                                    {
                                        result.Append(c);
                                        break;
                                    }

                                    index++;
                                }
                            }
                            else
                            {
                                result.Append(c);
                            }
                        }
                        else if (c == '\'' || c == '"' || c == '|')
                        {
                            state = c;
                            result.Append(c);
                        }
                        else
                        {
                            result.Append(c);
                        }
                        break;
                }

                index++;
            } // while

            return result.ToString();
        } // method RemoveComments

        /// <summary>
        /// Prepare the dynamic format string.
        /// </summary>
        /// <remarks>Dynamic format string
        /// mustn't contains comments and
        /// string delimiters (no matter
        /// real or IRBIS).
        /// </remarks>
        public static string? PrepareFormat
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            text = RemoveComments(text);
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            int length = text.Length;
            bool flag = false;
            for (int i = 0; i < length; i++)
            {
                if (text[i] < ' ')
                {
                    flag = true;
                    break;
                }
            }

            if (!flag)
            {
                return text;
            }

            StringBuilder result = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                char c = text[i];
                if (c >= ' ')
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        } // method PrepareFormat

        /*
        /// <summary>
        /// Verify format string.
        /// </summary>
        public static bool VerifyFormat
            (
                string? text,
                bool throwOnError
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                Log.Error
                    (
                        nameof(IrbisFormat) + "::" + nameof(VerifyFormat)
                        + "text is absent"
                    );

                if (throwOnError)
                {
                    throw new VerificationException("text is absent");
                }

                return false;
            }

            foreach (char c in text)
            {
                if (c < ' ')
                {
                    Log.Error
                        (
                            nameof(IrbisFormat) + "::" + nameof(VerifyFormat)
                            + "contains forbidden symbols"
                        );
                    if (throwOnError)
                    {
                        throw new VerificationException("contains forbidden symbols");
                    }

                    return false;
                }
            } // foreach

            const char zero = '\0';
            char state = zero;
            int index = 0, length = text.Length;
            while (index < length)
            {
                char c = text[index];

                switch (state)
                {
                    case '\'':
                    case '"':
                    case '|':
                        if (c == state)
                        {
                            state = zero;
                        }
                        break;

                    default:
                        if (c == '/' && index + 1 < length && text[index + 1] == '*')
                        {
                            Log.Error
                                (
                                    nameof(IrbisFormat) + "::"
                                    + nameof(VerifyFormat)
                                    + "contains comment"
                                );
                            if (throwOnError)
                            {
                                throw new VerificationException("contains comment");
                            }

                            return false;
                        }

                        if (c == '\'' || c == '"' || c == '|')
                        {
                            state = c;
                        }
                        break;
                }

                index++;
            } // while

            if (state != zero)
            {
                Log.Error
                    (
                        nameof(IrbisFormat) + "::" + nameof(VerifyFormat)
                        + "nonclosed literal"
                    );
                if (throwOnError)
                {
                    throw new VerificationException("nonclosed literal");
                }

                return false;
            } // if

            return true;
        } // method VerifyFormat
        */

        #endregion

    } // class IrbisFormat
}