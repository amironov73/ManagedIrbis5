// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* ClientConfiguration.cs -- конфигурация ИРБИС-клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Json;

#endregion

#nullable enable

namespace ManagedIrbis.Client;

/// <summary>
/// Конфигурация ИРБИС-клиента.
/// </summary>
[XmlRoot ("client-configuration")]
public sealed class ClientConfiguration
{
    #region Constants

    private const string IrbisConnection = "ИРБИС64: подключение к серверу";

    #endregion

    #region Properties

    /// <summary>
    /// IP-адрес сервера.
    /// </summary>
    [JsonPropertyName ("server")]
    [XmlAttribute ("server")]
    [Category (IrbisConnection)]
    [DisplayName ("IP-адрес сервера")]
    [Description ("IP-адрес хоста, на котором запущен сервер ИРБИС64.")]
    public string Host { get; set; } = "127.0.0.1";

    /// <summary>
    /// Номер порта.
    /// </summary>
    [JsonPropertyName ("port")]
    [XmlAttribute ("port")]
    [Category (IrbisConnection)]
    [DisplayName ("Номер порта на сервере")]
    [Description ("Номер порта, на котором сервер ИРБИС64 "
                  + "ожидает подключения клиентов. Как правило, 6666.")]
    public int Port { get; set; } = 6666;

    /// <summary>
    /// Логин пользователя.
    /// </summary>
    [JsonPropertyName ("login")]
    [XmlAttribute ("login")]
    [Category (IrbisConnection)]
    [DisplayName ("Логин пользователя")]
    [Description ("Логин пользователя в системе ИРБИС64. "
                 + "Не забудьте переключить раскладку клавиатуры, "
                 + "если это необходимо!")]
    public string? Login { get; set; }

    /// <summary>
    /// Пароль.
    /// </summary>
    [JsonPropertyName ("password")]
    [XmlAttribute ("password")]
    [Category (IrbisConnection)]
    [DisplayName ("Пароль")]
    [PasswordPropertyText (true)]
    [Description ("Пароль чувствителен к регистру символов! "
                 + "Не забудьте переключить раскладку клавиатуры, "
                 + "если это необходимо!")]
    public string? Password { get; set; }

    /// <summary>
    /// База данных.
    /// </summary>
    [JsonPropertyName ("database")]
    [Category (IrbisConnection)]
    [DisplayName ("Имя базы данных")]
    [Description ("База данных в ИРБИС64. Как правило, IBIS.")]
    public string? Database { get; set; }

    #endregion

    #region Public methods

    /// <summary>
    /// Чтение конфигурации из указанного файла.
    /// </summary>
    public static ClientConfiguration LoadConfiguration
        (
            string fileName
        )
    {
        Sure.FileExists (fileName);

        var result = JsonUtility.ReadObjectFromFile<ClientConfiguration> (fileName);
        result.Login = Unprotect (result.Login);
        result.Password = Unprotect (result.Password);

        return result;
    }

    /// <summary>
    /// Запись конфигурации в указанный файл.
    /// </summary>
    public void SaveConfiguration
        (
            string fileName
        )
    {
        Sure.NotNullNorEmpty (fileName);

        var clone = (ClientConfiguration) MemberwiseClone();
        clone.Login = Protect (Login);
        clone.Password = Protect (Password);

        JsonUtility.SaveObjectToFile (clone, fileName);
    }

    /// <summary>
    /// Примитивная защита от подглядывания паролей и прочего.
    /// Работает только против совсем неопытных пользователей.
    /// </summary>
    public static string? Protect
        (
            string? value
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return value;
        }

        var bytes = Encoding.UTF8.GetBytes(value);
        var result = "!" + Convert.ToBase64String(bytes);

        return result;
    }

    /// <summary>
    /// Раскодирование (при необходимости) строкового значения,
    /// закодированного методом <see cref="Protect"/>.
    /// </summary>
    public static string? Unprotect
        (
            string? value
        )
    {
        if (string.IsNullOrEmpty (value))
        {
            return value;
        }

        if (value.FirstChar() != '!')
        {
            return value;
        }

        var bytes = Convert.FromBase64String (value.Substring(1));
        var result = Encoding.UTF8.GetString (bytes);

        return result;
    }

    #endregion
}
