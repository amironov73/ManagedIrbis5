// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertToAutoProperty
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Local
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local

/* ConnectionSettings.cs -- настройки для подключения к серверу ИРБИС64
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Parameters;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis
{
    /// <summary>
    /// Настройки для подключения к серверу ИРБИС64 (для отдельного сохранения).
    /// </summary>
    [XmlRoot("connection")]
    public sealed class ConnectionSettings
        : IHandmadeSerializable,
        IConnectionSettings,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Адрес хоста по умолчанию.
        /// </summary>
        public const string DefaultHost = "127.0.0.1";

        /// <summary>
        /// Имя базы данных по умолчанию.
        /// </summary>
        public const string DefaultDatabase = "IBIS";

        /// <summary>
        /// Код АРМ по умолчанию.
        /// </summary>
        public const string DefaultWorkstation = "C";

        /// <summary>
        /// Номер порта по умолчанию.
        /// </summary>
        public const int DefaultPort = 6666;

        #endregion

        #region Properties

        /// <summary>
        /// Адрес или имя хоста сервера ИРБИС64.
        /// </summary>
        /// <remarks>Default value is "127.0.0.1".</remarks>
        [XmlAttribute("host")]
        [JsonPropertyName("host")]
        public string? Host { get; set; } = DefaultHost;

        /// <summary>
        /// Номер порта, на котором сервер ИРБИС64 принимает клиентские подключения.
        /// </summary>
        /// <remarks>Default value is 6666.</remarks>
        [XmlAttribute("port")]
        [JsonPropertyName("port")]
        public ushort Port { get; set; } = DefaultPort;

        /// <summary>
        /// Имя (логин) пользователя системы ИРБИС64.
        /// </summary>
        /// <remarks>Default value is <c>null</c>,
        /// so connection can't be made.</remarks>
        [XmlAttribute("username")]
        [JsonPropertyName("username")]
        public string? Username { get; set; } = string.Empty;

        /// <summary>
        /// Пароль пользователя системы ИРБИС64.
        /// </summary>
        /// <remarks>Default value is <c>null</c>,
        /// so connection can't be made.</remarks>
        [XmlAttribute("password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; } = string.Empty;

        /// <summary>
        /// Имя базы данных.
        /// </summary>
        /// <remarks>Default value is "IBIS".
        /// Database with such a name can be
        /// non-existent.
        /// </remarks>
        [XmlAttribute("database")]
        [JsonPropertyName("database")]
        public string? Database { get; set; } = DefaultDatabase;

        /// <summary>
        /// Workstation application kind.
        /// </summary>
        /// <remarks>Default value is "C".
        /// </remarks>
        [XmlAttribute("workstation")]
        [JsonPropertyName("workstation")]
        public string? Workstation { get; set; } = DefaultWorkstation;

        /// <summary>
        /// Turn on network logging.
        /// </summary>
        [XmlAttribute("log")]
        [JsonPropertyName("log")]
        public string? NetworkLogging { get; set; }

        /// <summary>
        /// Type name for ClientSocket.
        /// </summary>
        [XmlAttribute("socket")]
        [JsonPropertyName("socket")]
        public string? SocketTypeName { get; set; }


        /// <summary>
        /// Флаг: включена отладка.
        /// </summary>
        public bool Debug { get; set; }

        /// <summary>
        /// Retry limit.
        /// </summary>
        [XmlAttribute("retry")]
        [JsonPropertyName("retry")]
        public int RetryLimit { get; set; }

        /// <summary>
        /// Web CGI URL.
        /// </summary>
        [XmlAttribute("web")]
        [JsonPropertyName("web")]
        public string? WebCgi { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlAttribute("userdata")]
        [JsonPropertyName("userdata")]
        public string? UserData { get; set; }

        #endregion

        #region IConnectionSettings members

        int IConnectionSettings.ClientId => 0;

        int IConnectionSettings.QueryId => 0;

        #endregion

        #region Private members

        //private int _clientId;
        //private int _queryId;

        private static void _Add
            (
                List<Parameter> list,
                string name,
                string? value
            )
        {
            if (!string.IsNullOrEmpty(value))
            {
                var parameter = new Parameter(name, value);
                list.Add(parameter);
            }
        }

        private static string _Select
            (
                string first,
                string second
            )
        {
            return string.IsNullOrEmpty(first)
                ? second
                : first;
        }

        private static int _Select
            (
                int first,
                int second
            )
        {
            return first != 0
                ? first
                : second;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение настроек к подключению.
        /// </summary>
        /// <param name="connection">Неактивное подключение.</param>
        public void Apply
            (
                IConnectionSettings connection
            )
        {
            if (connection is IIrbisProvider { Connected: true })
            {
                throw new IrbisException("Already connected");
            }

            if (!string.IsNullOrEmpty(Host))
            {
                connection.Host = Host;
            }

            connection.Port = Port;

            if (!string.IsNullOrEmpty(Username))
            {
                connection.Username = Username;
            }

            if (!string.IsNullOrEmpty(Password))
            {
                connection.Password = Password;
            }

            if (!string.IsNullOrEmpty(Workstation))
            {
                connection.Workstation = Workstation;
            }

            if (!string.IsNullOrEmpty(Database))
            {
                switch (connection)
                {
                    case IIrbisProvider provider:
                        provider.Database = Database;
                        break;

                    case ConnectionSettings settings:
                        settings.Database = Database;
                        break;
                }
            }

        } // method Apply

        /// <summary>
        /// Применение настроек к подключению.
        /// </summary>
        /// <param name="provider">Неактивное подключение.</param>
        public void Apply
            (
                IIrbisProvider provider
            )
        {
            if (provider.Connected)
            {
                throw new IrbisException("Already connected");
            }

            provider.Database = Database;

            if (provider is IConnectionSettings connection)
            {
                if (!string.IsNullOrEmpty(Host))
                {
                    connection.Host = Host;
                }

                connection.Port = Port;

                if (!string.IsNullOrEmpty(Username))
                {
                    connection.Username = Username;
                }

                if (!string.IsNullOrEmpty(Password))
                {
                    connection.Password = Password;
                }

                if (!string.IsNullOrEmpty(Workstation))
                {
                    connection.Workstation = Workstation;
                }
            }

        } // method Apply

        /// <summary>
        /// Clone.
        /// </summary>
        public ConnectionSettings Clone() => (ConnectionSettings)MemberwiseClone();

        /// <summary>
        /// Decrypt the connection settings.
        /// </summary>
        public static ConnectionSettings Decrypt
            (
                string text
            )
        {
            var bytes = Convert.FromBase64String(text);
            bytes = CompressionUtility.Decompress(bytes);
            var result = bytes.RestoreObjectFromMemory<ConnectionSettings>()
                .ThrowIfNull("RestoreObjectFromMemory");

            return result;
        } // method Decrypt

        /// <summary>
        /// Encode parameters to text representation.
        /// </summary>
        public string Encode()
        {
            var parameters = new List<Parameter>();

            _Add(parameters, "host", Host);
            _Add
                (
                    parameters,
                    "port",
                    Port == 0
                        ? null
                        : Port.ToInvariantString()
                );
            _Add(parameters, "database", Database);
            _Add(parameters, "username", Username);
            _Add(parameters, "password", Password);
            _Add
                (
                    parameters,
                    "workstation",
                    string.IsNullOrWhiteSpace(Workstation)
                        ? null
                        : Workstation
                );
            _Add(parameters, "socket", SocketTypeName);
            _Add(parameters, "log", NetworkLogging);
            _Add
                (
                    parameters,
                    "retry",
                    RetryLimit == 0
                    ? null
                    : RetryLimit.ToInvariantString()
                );
            _Add(parameters, "data", UserData);

            var result = ParameterUtility.Encode
                (
                    parameters.ToArray()
                );

            return result;
        } // method Encode

        /// <summary>
        /// Encrypt the connection settings.
        /// </summary>
        public string Encrypt()
        {
            var bytes = this.SaveToMemory();
            bytes = CompressionUtility.Compress(bytes);
            var result = Convert.ToBase64String(bytes);

            return result;
        } // method Encrypt

        /// <summary>
        /// Construct <see cref="ConnectionSettings"/>
        /// from <see cref="IConnectionSettings"/>.
        /// </summary>
        public static ConnectionSettings FromConnection
            (
                IConnectionSettings connection
            )
        {
            // TODO: устанавливать базу данных, если возможно

            return new()
            {
                Host = connection.Host,
                Port = connection.Port,
                Username = connection.Username,
                Password = connection.Password,
                // Database = connection.Database,
                Workstation = connection.Workstation
            };
        } // method FromConnection

        /// <summary>
        /// Get missing elements from the settings.
        /// </summary>
        public ConnectionElement GetMissingElements()
        {
            var result = ConnectionElement.None;

            if (string.IsNullOrEmpty(Host))
            {
                result |= ConnectionElement.Host;
            }

            if (Port == 0)
            {
                result |= ConnectionElement.Port;
            }

            if (string.IsNullOrEmpty(Username))
            {
                result |= ConnectionElement.Username;
            }

            if (string.IsNullOrEmpty(Password))
            {
                result |= ConnectionElement.Password;
            }

            if (string.IsNullOrWhiteSpace(Workstation))
            {
                result |= ConnectionElement.Workstation;
            }

            return result;
        } // method GetMissingElements

        /// <summary>
        /// Парсинг строки подключения.
        /// </summary>
        public ConnectionSettings ParseConnectionString
            (
                string connectionString
            )
        {
            var parameters = ParameterUtility.ParseString
                (
                    connectionString
                );

            foreach (var parameter in parameters)
            {
                var name = parameter.Name
                    .ThrowIfNull("parameter.Name")
                    .ToLower();
                var value = parameter.Value
                    .ThrowIfNull("parameter.Value");

                switch (name)
                {
                    case "provider":
                    case "assembly":
                    case "assemblies":
                    case "register":
                    case "type":
                        // Nothing to do
                        break;

                    case "host":
                    case "server":
                    case "address":
                        Host = value;
                        break;

                    case "port":
                        Port = ushort.Parse(value);
                        if (Port <= 0)
                        {
                            throw new ArgumentOutOfRangeException("Port=" + Port);
                        }
                        break;

                    case "user":
                    case "username":
                    case "name":
                    case "login":
                    case "account":
                        Username = value;
                        break;

                    case "password":
                    case "pwd":
                    case "secret":
                        Password = value;
                        break;

                    case "db":
                    case "database":
                    case "base":
                    case "catalog":
                        Database = value;
                        break;

                    case "arm":
                    case "workstation":
                        Workstation = value.ToUpperInvariant();
                        break;

                    case "socket":
                        SocketTypeName = value;
                        break;

                    case "log":
                        NetworkLogging = value;
                        break;

                    case "retry":
                        RetryLimit = int.Parse(value);
                        break;

                    case "web":
                    case "webcgi":
                    case "cgi":
                    case "http":
                        WebCgi = value;
                        break;

                    case "debug":
                        Debug = true;
                        break;

                    case "userdata":
                    case "data":
                        UserData = value;
                        break;

                    default:
                        Magna.Error
                            (
                                nameof(ConnectionSettings)
                                + "::" + nameof(ParseConnectionString)
                                + "unknown parameter: "
                                + name
                            );

                        throw new ArgumentException($"Unknown parameter: {name}");
                }
            }

            return this;
        } // method ParseConnectionString

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream(BinaryReader reader)
        {
            Host = reader.ReadNullableString() ?? DefaultHost;
            Port = (ushort)reader.ReadPackedInt32();
            Username = reader.ReadNullableString() ?? string.Empty;
            Password = reader.ReadNullableString() ?? string.Empty;
            Database = reader.ReadNullableString();
            Workstation = reader.ReadNullableString() ?? DefaultWorkstation;
            NetworkLogging = reader.ReadNullableString();
            SocketTypeName = reader.ReadNullableString();
            RetryLimit = reader.ReadPackedInt32();
            WebCgi = reader.ReadNullableString();
            UserData = reader.ReadNullableString();
        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream(BinaryWriter writer)
        {
            writer
                .WriteNullable(Host)
                .WritePackedInt32(Port)
                .WriteNullable(Username)
                .WriteNullable(Password)
                .WriteNullable(Database)
                .WriteNullable(Workstation)
                .WriteNullable(NetworkLogging)
                .WriteNullable(SocketTypeName)
                .WritePackedInt32(RetryLimit)
                .WriteNullable(WebCgi)
                .WriteNullable(UserData);
        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify(bool throwOnError)
        {
            var verifier = new Verifier<ConnectionSettings>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Host, "Host")
                .Assert(Port > 0, "Port")
                .NotNullNorEmpty(Username, "Username")
                .NotNullNorEmpty(Password, "Password")
                .NotNullNorEmpty(Workstation)
                .NotNullNorEmpty(Database);

            return verifier.Result;
        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Encode();

        #endregion
    } // class ConnectionSettings

} // namespace ManagedIrbis
