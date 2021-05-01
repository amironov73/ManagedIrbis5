// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedType.Global

/* ConfigurationUtility.cs -- some useful routines for System.Configuration
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Configuration;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using AM.Security;

using CM = System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace AM.Configuration
{
    /// <summary>
    /// Some useful routines for System.Configuration.
    /// </summary>
    public static class ConfigurationUtility
    {
        #region Private members

        private static readonly byte[] _additionalEntropy = {2, 12, 85, 0, 6};

        #endregion

        #region Public methods

        /// <summary>
        /// Application.exe.config file name with full path.
        /// </summary>
        public static string ConfigFileName =>
            string.Concat(Utility.ExecutableFileName, ".config");

        /// <summary>
        /// Декодирование конфигурационной строки, если она закодирована или зашифрована.
        /// </summary>
        /// <param name="possiblyEncrypted">Строка конфигурации.</param>
        /// <param name="password">Пароль. Если null, то используется пароль по умолчанию.</param>
        /// <returns>Расшифрованная строка конфигурации.</returns>
        public static string? DecryptString
            (
                string? possiblyEncrypted,
                string? password
            )
        {
            // Пустая строка зашифрованной быть не может.
            if (string.IsNullOrEmpty(possiblyEncrypted))
            {
                return possiblyEncrypted;
            }

            // С восклицательного знака начинается строка,
            // закодированная в банальный Base64.
            if (possiblyEncrypted[0] == '!')
            {
                var enc = possiblyEncrypted.Substring(1);
                var res = SecurityUtility.DecryptFromBase64(enc);
                return res;
            }

            // Зашифрованная строка должна начинаться со знака вопроса.
            if (possiblyEncrypted[0] != '?')
            {
                return possiblyEncrypted;
            }

            var encrypted = possiblyEncrypted.Substring(1);
            if (string.IsNullOrEmpty(password))
            {
                password = "irbis";
            }

            var result = SecurityUtility.Decrypt(encrypted, password);
            return result;
        }

        /// <summary>
        /// Получаем сеттинг из возможных кандидатов.
        /// </summary>
        public static string? FindSetting
            (
                params string[] candidates
            )
        {
            foreach (string candidate in candidates.NonEmptyLines())
            {
                var setting = CM.AppSettings[candidate];
                if (!string.IsNullOrEmpty(setting))
                {
                    return setting;
                }
            }

            return null;
        }

        /// <summary>
        /// Get boolean value from application configuration.
        /// </summary>
        public static bool GetBoolean
            (
                string key,
                bool defaultValue = default
            )
        {
            bool result = defaultValue;
            string? s = CM.AppSettings[key];

            if (!string.IsNullOrEmpty(s))
            {
                result = Utility.ToBoolean(s);
            }

            return result;
        }

        /// <summary>
        /// Get 16-bit integer value from application configuration.
        /// </summary>
        public static short GetInt16
            (
                string key,
                short defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseInt16(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get unsigned 16-bit integer.
        /// </summary>
        public static ushort GetUInt16
            (
                string key,
                ushort defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseUInt16(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get 32-bit integer value
        /// from application configuration.
        /// </summary>
        public static int GetInt32
            (
                string key,
                int defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseInt32(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get unsigned 32-bit integer value
        /// from application configuration.
        /// </summary>
        public static uint GetUInt32
            (
                string key,
                uint defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseUInt32(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get 64-bit integer value
        /// from application configuration.
        /// </summary>
        public static long GetInt64
            (
                string key,
                long defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseInt64(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get usingned 64-bit integer value
        /// from application configuration.
        /// </summary>
        public static ulong GetUInt64
            (
                string key,
                ulong defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseUInt64(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get single-precision float value from application configuration.
        /// </summary>
        public static float GetSingle
            (
                string key,
                float defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseSingle(s, out float result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get double-precision float value from application configuration.
        /// </summary>
        public static double GetDouble
            (
                string key,
                double defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseDouble(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get decimal value from application configuration.
        /// </summary>
        public static decimal GetDecimal
            (
                string key,
                decimal defaultValue = default
            )
        {
            string? s = CM.AppSettings[key];

            if (!Utility.TryParseDecimal(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Get string value from application configuration.
        /// </summary>
        public static string? GetString
            (
                string key,
                string? defaultValue = default
            )
        {
            string? result = defaultValue;
            string? s = CM.AppSettings[key];

            if (!string.IsNullOrEmpty(s))
            {
                result = s;
            }

            return result;
        }

        /// <summary>
        /// Get string value from application configuration.
        /// </summary>
        public static string? GetString
            (
                string key,
                string? defaultValue,
                string? password
            )
        {
            string? result = defaultValue;
            string? s = CM.AppSettings[key];

            if (!string.IsNullOrEmpty(s))
            {
                result = s;
            }

            result = DecryptString(result, password);

            return result;
        }

        /// <summary>
        /// Get string value from application configuration.
        /// </summary>
        public static string RequireString
            (
                string key
            )
        {
            var result = GetString(key);

            if (ReferenceEquals(result, null))
            {
                Magna.Error
                    (
                        "ConfigurationUtility::RequireString: "
                        + "key '"
                        + key
                        + "' not set"
                    );

                throw new ArgumentNullException
                    (
                        "configuration key '" + key + "' not set"
                    );
            }

            return result;
        }

        /// <summary>
        /// Whether the key is present.
        /// </summary>
        public static bool HasKey
            (
                string key
            )
        {
            var keys = CM.AppSettings.Keys;

            return keys.Cast<string>().Contains(key);
        }

        /// <summary>
        /// Throw 'key not set exception'.
        /// </summary>
        public static void ThrowKeyNotSet
            (
                string key
            )
        {
            Magna.Error
                (
                    "ConfigurationUtility::RequireKey: "
                    + "key '"
                    + key
                    + "' not set"
                );

            throw new ConfigurationErrorsException
                (
                    "configuration key '" + key + "' not set"
                );
        }

        /// <summary>
        /// Require the key must present.
        /// </summary>
        public static void RequireKey
            (
                string key
            )
        {
            if (!HasKey(key))
            {
                ThrowKeyNotSet(key);
            }
        }

        /// <summary>
        /// Get integer value from application configuration.
        /// </summary>
        public static int RequireInt32
            (
                string key
            )
        {
            RequireKey(key);

            return GetInt32(key);
        }

        /// <summary>
        /// Get date or time value from application configuration.
        /// </summary>
        public static DateTime GetDateTime
            (
                string key,
                DateTime defaultValue = default
            )
        {
            var s = CM.AppSettings[key];

            if (!Utility.TryParseDateTime(s, out var result))
            {
                result = defaultValue;
            }

            return result;
        }

        /// <summary>
        /// Добавляет указанное значение.
        /// </summary>
        public static KeyValueConfigurationCollection SetValue
            (
                this KeyValueConfigurationCollection collection,
                string key,
                string value
            )
        {
            var element = collection[key];
            if (ReferenceEquals(element, null))
            {
                collection.Add(key, value);
                element = collection[key];
            }

            element.Value = value;

            return collection;
        }

        /// <summary>
        /// Encrypt the text.
        /// </summary>
        /// <remarks>
        /// Very naive data protection against kiddy hackers.
        /// </remarks>
        public static string Protect
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Convert.ToBase64String
                    (
                        ProtectedData.Protect
                            (
                                Encoding.Unicode.GetBytes(text),
                                _additionalEntropy,
                                DataProtectionScope.LocalMachine
                            )
                    )
                : Convert.ToBase64String(Encoding.Unicode.GetBytes(text));
        }

        /// <summary>
        /// Decrypt the text.
        /// </summary>
        public static string Unprotect
            (
                string? text
            )
        {
            if (string.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows)
                ? Encoding.Unicode.GetString
                    (
                        ProtectedData.Unprotect
                                (
                                    Convert.FromBase64String(text),
                                    _additionalEntropy,
                                    DataProtectionScope.LocalMachine
                                )
                    )
                : Encoding.Unicode.GetString(Convert.FromBase64String(text));
        }

        #endregion

    } // class ConfigurationUtility

} // namespace AM.Configuration
