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

/* ConfigurationUtility.cs -- полезные методы для System.Configuration
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Configuration;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

using AM.Linq;
using AM.Security;

using CM = System.Configuration.ConfigurationManager;

#endregion

#nullable enable

namespace AM.Configuration;

/// <summary>
/// Полезные методы для System.Configuration.
/// </summary>
public static class ConfigurationUtility
{
    #region Private members

    private static readonly byte[] _additionalEntropy = { 2, 12, 85, 0, 6 };

    #endregion

    #region Public methods

    /// <summary>
    /// Полный путь к <c>Application.exe.config</c>.
    /// </summary>
    public static string ConfigFileName =>
        string.Concat (Utility.ExecutableFileName, ".config");

    /// <summary>
    /// Декодирование конфигурационной строки, если она закодирована или зашифрована.
    /// </summary>
    /// <param name="possiblyEncrypted">Строка конфигурации.</param>
    /// <param name="password">Пароль. Если <c>null</c>, то используется пароль по умолчанию.</param>
    /// <returns>Расшифрованная строка конфигурации.</returns>
    public static string? DecryptString
        (
            string? possiblyEncrypted,
            string? password
        )
    {
        // Пустая строка зашифрованной быть не может.
        if (string.IsNullOrEmpty (possiblyEncrypted))
        {
            return possiblyEncrypted;
        }

        // С восклицательного знака начинается строка,
        // закодированная в банальный Base64.
        if (possiblyEncrypted[0] == '!')
        {
            var enc = possiblyEncrypted.Substring (1);
            var res = SecurityUtility.DecryptFromBase64 (enc);
            return res;
        }

        // Зашифрованная строка должна начинаться со знака вопроса.
        if (possiblyEncrypted[0] != '?')
        {
            return possiblyEncrypted;
        }

        var encrypted = possiblyEncrypted.Substring (1);
        if (string.IsNullOrEmpty (password))
        {
            password = "irbis";
        }

        var result = SecurityUtility.Decrypt (encrypted, password);

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
        foreach (var candidate in candidates.NonEmptyLines())
        {
            var setting = CM.AppSettings[candidate];
            if (!string.IsNullOrEmpty (setting))
            {
                return setting;
            }

            setting = Magna.Configuration[candidate];
            if (!string.IsNullOrEmpty (setting))
            {
                return setting;
            }
        }

        return null;
    }

    /// <summary>
    /// Получение логического значения из конфигурации приложения.
    /// </summary>
    public static bool GetBoolean
        (
            string key,
            bool defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var result = defaultValue;
        var s = GetString (key);
        if (!string.IsNullOrEmpty (s))
        {
            result = Utility.ToBoolean (s);
        }

        return result;
    }

    /// <summary>
    /// Получение значения 16-битного целого со знаком
    /// из конфигурации приложения.
    /// </summary>
    public static short GetInt16
        (
            string key,
            short defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseInt16 (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения беззнакового 16-битного целого
    /// из конфигурации приложения.
    /// </summary>
    public static ushort GetUInt16
        (
            string key,
            ushort defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseUInt16 (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения 32-битного целого со знаком
    /// из конфигурации приложения.
    /// </summary>
    public static int GetInt32
        (
            string key,
            int defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseInt32 (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения беззнакового 32-битного целого
    /// из конфигурации приложения.
    /// </summary>
    public static uint GetUInt32
        (
            string key,
            uint defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseUInt32 (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения 64-битного целого со знаком
    /// из конфигурации приложения.
    /// </summary>
    public static long GetInt64
        (
            string key,
            long defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseInt64 (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения беззнакового 64-битного целого
    /// из конфигурации приложения.
    /// </summary>
    public static ulong GetUInt64
        (
            string key,
            ulong defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseUInt64 (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения числа с плавающей точкой одинарной точности
    /// из конфигурации приложения.
    /// </summary>
    public static float GetSingle
        (
            string key,
            float defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseSingle (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения числа с плавающей точкой двойной точности
    /// из конфигурации приложения.
    /// </summary>
    public static double GetDouble
        (
            string key,
            double defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseDouble (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения числа с фиксированной точкой из конфигурации приложения.
    /// </summary>
    public static decimal GetDecimal
        (
            string key,
            decimal defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = GetString (key);
        if (!Utility.TryParseDecimal (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Получение значения строки из конфигурации приложения.
    /// </summary>
    public static string? GetString
        (
            string key,
            string? defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var result = defaultValue;
        var s = CM.AppSettings[key];
        if (string.IsNullOrEmpty (s))
        {
            s = Magna.Configuration[key];
        }

        if (!string.IsNullOrEmpty (s))
        {
            result = s;
        }

        return result;
    }

    /// <summary>
    /// Получение значения строки из конфигурации приложения.
    /// </summary>
    public static string? GetString
        (
            string key,
            string? defaultValue,
            string? password
        )
    {
        Sure.NotNullNorEmpty (key);

        var result = defaultValue;
        var s = GetString (key);
        if (string.IsNullOrEmpty (s))
        {
            s = Magna.Configuration[key];
        }

        if (!string.IsNullOrEmpty (s))
        {
            result = s;
        }

        result = DecryptString (result, password);

        return result;
    }

    /// <summary>
    /// Требование строкового значения из конфигурации приложения.
    /// </summary>
    public static string RequireString
        (
            string key
        )
    {
        Sure.NotNullNorEmpty (key);

        var result = GetString (key);
        if (result is null)
        {
            ThrowKeyNotSet (key);
        }

        return result;
    }

    /// <summary>
    /// Существует ли в конфигурации приложения значение с указанным именем?
    /// </summary>
    public static bool HasKey
        (
            string key
        )
    {
        Sure.NotNullNorEmpty (key);

        var keys = CM.AppSettings.Keys;

        if (keys.Cast<string>().Contains (key))
        {
            return true;
        }

        var value = Magna.Configuration[key];
        return !string.IsNullOrEmpty (value);
    }

    /// <summary>
    /// Бросает исключение "ключ не найден".
    /// </summary>
    [DoesNotReturn]
    public static void ThrowKeyNotSet
        (
            string key
        )
    {
        Sure.NotNullNorEmpty (key);

        Magna.Error
            (
                nameof (ConfigurationUtility) + "::" + nameof (RequireKey)
                + ": key '"
                + key
                + "' not set"
            );

        throw new ConfigurationErrorsException
            (
                "configuration key '" + key + "' not set"
            );
    }

    /// <summary>
    /// Требование существования значения с указанным именем.
    /// </summary>
    public static void RequireKey
        (
            string key
        )
    {
        Sure.NotNullNorEmpty (key);

        if (!HasKey (key))
        {
            ThrowKeyNotSet (key);
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
        RequireKey (key);

        return GetInt32 (key);
    }

    /// <summary>
    /// Получение даты/времени из конфигурации приложения.
    /// </summary>
    public static DateTime GetDateTime
        (
            string key,
            DateTime defaultValue = default
        )
    {
        Sure.NotNullNorEmpty (key);

        var s = CM.AppSettings[key];

        if (!Utility.TryParseDateTime (s, out var result))
        {
            result = defaultValue;
        }

        return result;
    }

    /// <summary>
    /// Добавляет (или заменяет существующее) указанное значение
    /// к конфигурации приложения.
    /// </summary>
    public static KeyValueConfigurationCollection SetValue
        (
            this KeyValueConfigurationCollection collection,
            string key,
            string value
        )
    {
        Sure.NotNull (collection);
        Sure.NotNullNorEmpty (key);

        var element = collection[key];
        if (element is null)
        {
            collection.Add (key, value);
            element = collection[key];
        }

        element.Value = value;

        return collection;
    }

    /// <summary>
    /// Простое шифрование строки.
    /// </summary>
    /// <remarks>
    /// Весьма наивная защита данных от мамкиных хакеров.
    /// </remarks>
    public static string Protect
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return string.Empty;
        }

        return RuntimeInformation.IsOSPlatform (OSPlatform.Windows)
            ? Convert.ToBase64String
                (
                    ProtectedData.Protect
                        (
                            Encoding.Unicode.GetBytes (text),
                            _additionalEntropy,
                            DataProtectionScope.LocalMachine
                        )
                )
            : Convert.ToBase64String (Encoding.Unicode.GetBytes (text));
    }

    /// <summary>
    /// Расшифровка строки.
    /// </summary>
    public static string Unprotect
        (
            string? text
        )
    {
        if (string.IsNullOrEmpty (text))
        {
            return string.Empty;
        }

        return RuntimeInformation.IsOSPlatform (OSPlatform.Windows)
            ? Encoding.Unicode.GetString
                (
                    ProtectedData.Unprotect
                        (
                            Convert.FromBase64String (text),
                            _additionalEntropy,
                            DataProtectionScope.LocalMachine
                        )
                )
            : Encoding.Unicode.GetString (Convert.FromBase64String (text));
    }

    #endregion
}
