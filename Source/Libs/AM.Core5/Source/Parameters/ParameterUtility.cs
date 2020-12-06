// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ParameterUtility.cs -- утилиты для работы с параметрами
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using AM.Collections;
using AM.Text;

#endregion

#nullable enable

namespace AM.Parameters
{
    /// <summary>
    /// Утилиты для работы с параметрами ИМЯ=ЗНАЧЕНИЕ.
    /// </summary>
    public static class ParameterUtility
    {
        #region Properties

        /// <summary>
        /// Escape character.
        /// </summary>
        public static char EscapeCharacter = '\\';

        /// <summary>
        /// Name separator.
        /// </summary>
        public static char NameSeparator = '=';

        /// <summary>
        /// Value separator.
        /// </summary>
        public static char ValueSeparator = ';';

        #endregion

        #region Public methods

        /// <summary>
        /// Encode parameters to sting representation.
        /// </summary>
        public static string Encode
            (
                Parameter[] parameters
            )
        {
            var result = new StringBuilder();

            char[] badNameCharacters = { NameSeparator };
            char[] badValueCharacters = { ValueSeparator };

            foreach (var parameter in parameters)
            {
                result.Append
                    (
                        Utility.Mangle
                            (
                                parameter.Name,
                                EscapeCharacter,
                                badNameCharacters
                            )
                    );
                result.Append(NameSeparator);
                result.Append
                    (
                        Utility.Mangle
                            (
                                parameter.Value,
                                EscapeCharacter,
                                badValueCharacters
                            )
                    );
                result.Append(ValueSeparator);
            }

            return result.ToString();
        }

        /// <summary>
        /// Get the parameter with specified name.
        /// </summary>
        public static string? GetParameter
            (
                this Parameter[] parameters,
                string name,
                string? defaultValue
            )
        {
            Sure.NotNullNorEmpty(name, nameof(name));

            var found = parameters
                .FirstOrDefault(p => p.Name.SameString(name));

            var result = ReferenceEquals(found, null)
                ? defaultValue
                : found.Value;

            return result;
        }

        /// <summary>
        /// Get the parameter with specified name.
        /// </summary>
#nullable disable
        public static T GetParameter<T>
            (
                this Parameter[] parameters,
                string name,
                T? defaultValue
            )
        {
            Sure.NotNullNorEmpty(name, nameof(name));

            var found = parameters
                .FirstOrDefault(p => p.Name.SameString(name));

            var result = ReferenceEquals(found, null)
                ? defaultValue
                : Utility.ConvertTo<T>(found.Value);

            return result;
        }

        /// <summary>
        /// Get the parameter with specified name.
        /// </summary>
        public static T? GetParameter<T>
            (
                this Parameter[] parameters,
                string name
            )
        {
            return GetParameter(parameters, name, default(T));
        }
#nullable restore

        /// <summary>
        /// Parse specified string.
        /// </summary>
        public static Parameter[] ParseString
            (
                string text
            )
        {
            var result = new List<Parameter>();
            var navigator = new TextNavigator(text);
            navigator.SkipWhitespace();

            while (!navigator.IsEOF)
            {
                while (true)
                {
                    var flag = false;
                    if (navigator.IsWhiteSpace())
                    {
                        flag = true;
                        navigator.SkipWhitespace();
                    }
                    if (navigator.PeekChar() == ValueSeparator)
                    {
                        flag = true;
                        navigator.SkipChar(ValueSeparator);
                    }
                    if (!flag)
                    {
                        break;
                    }
                }

                var name = navigator.ReadEscapedUntil
                    (
                        EscapeCharacter,
                        NameSeparator
                    );
                if (ReferenceEquals(name, null))
                {
                    break;
                }
                name = name.Trim();
                navigator.SkipWhitespace();

                var value = navigator.ReadEscapedUntil
                    (
                        EscapeCharacter,
                        ValueSeparator
                    );
                var parameter = new Parameter(name, value);
                result.Add(parameter);
            }

            return result.ToArray();
        }

        #endregion
    }
}
