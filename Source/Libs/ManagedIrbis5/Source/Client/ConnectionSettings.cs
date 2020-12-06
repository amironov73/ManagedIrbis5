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
using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Parameters;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Client
{
    /// <summary>
    /// Настройки для подключения к серверу ИРБИС64.
    /// </summary>
    [XmlRoot("connection")]
    public sealed class ConnectionSettings
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Хост по умолчанию.
        /// </summary>
        public const string DefaultHost = "127.0.0.1";

        /// <summary>
        /// База данных по умолчанию.
        /// </summary>
        public const string DefaultDatabase = "IBIS";

        /// <summary>
        /// АРМ по умолчанию.
        /// </summary>
        public const string DefaultWorkstation = "C";

        /// <summary>
        /// Порт по умолчанию.
        /// </summary>
        public const int DefaultPort = 6666;

        #endregion

        #region Properties

        /// <summary>
        /// Адрес или имя хоста сервера.
        /// </summary>
        /// <remarks>Default value is "127.0.0.1".</remarks>
        [XmlAttribute("host")]
        [JsonPropertyName("host")]
        public string? Host { get; set; }

        /// <summary>
        /// Номер порта.
        /// </summary>
        /// <remarks>Default value is 6666.</remarks>
        [XmlAttribute("port")]
        [JsonPropertyName("port")]
        public int Port { get; set; }

        /// <summary>
        /// User logon name.
        /// </summary>
        /// <remarks>Default value is <c>null</c>,
        /// so connection can't be made.</remarks>
        [XmlAttribute("username")]
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        /// <summary>
        /// User logon password.
        /// </summary>
        /// <remarks>Default value is <c>null</c>,
        /// so connection can't be made.</remarks>
        [XmlAttribute("password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Database name to connect.
        /// </summary>
        /// <remarks>Default value is "IBIS".
        /// Database with such a name can be
        /// non-existent.
        /// </remarks>
        [DefaultValue(DefaultDatabase)]
        [XmlAttribute("database")]
        [JsonPropertyName("database")]
        public string? Database { get; set; }

        /// <summary>
        /// Workstation application kind.
        /// </summary>
        /// <remarks>Default value is
        /// <see cref="ManagedIrbis.Workstation.Cataloger"/>.
        /// </remarks>
        [DefaultValue(DefaultWorkstation)]
        [XmlAttribute("workstation")]
        [JsonPropertyName("workstation")]
        public string? Workstation { get; set; }

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

        /// <summary>
        /// Saved "connected" state.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool Connected { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ConnectionSettings()
        {
            Host = DefaultHost;
            Port = DefaultPort;
            Database = DefaultDatabase;
            Username = null;
            Password = null;
            Workstation = DefaultWorkstation;
        }

        #endregion

        #region Private members

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
        /// Clone.
        /// </summary>
        public ConnectionSettings Clone()
        {
            return (ConnectionSettings)MemberwiseClone();
        }

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
        }

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
        }

        /// <summary>
        /// Encrypt the connection settings.
        /// </summary>
        public string Encrypt()
        {
            var bytes = this.SaveToMemory();
            bytes = CompressionUtility.Compress(bytes);
            var result = Convert.ToBase64String(bytes);

            return result;
        }

        /// <summary>
        /// Construct <see cref="ConnectionSettings"/>
        /// from <see cref="Connection"/>.
        /// </summary>
        public static ConnectionSettings FromConnection
            (
                Connection connection
            )
        {
            var result = new ConnectionSettings
            {
                Host = connection.Host,
                Port = connection.Port,
                Username = connection.Username,
                Password = connection.Password,
                Database = connection.Database,
                Workstation = connection.Workstation,
                Connected = connection.Connected
            };

            return result;
        }

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
        }

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
                        Port = int.Parse(value);
                        break;

                    case "user":
                    case "username":
                    case "name":
                    case "login":
                        Username = value;
                        break;

                    case "pwd":
                    case "password":
                        Password = value;
                        break;

                    case "db":
                    case "catalog":
                    case "database":
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

                    case "userdata":
                    case "data":
                        UserData = value;
                        break;

                    default:
                        Magna.Error
                            (
                                "ConnectionSettings::ParseConnectionString: "
                                + "unknown parameter: "
                                + name
                            );

                        var message = $"Unknown parameter: {name}";
                        throw new ArgumentException(message);
                }
            }

            return this;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Host = reader.ReadNullableString();
            Port = reader.ReadPackedInt32();
            Username = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            Workstation = reader.ReadNullableString();
            NetworkLogging = reader.ReadNullableString();
            SocketTypeName = reader.ReadNullableString();
            RetryLimit = reader.ReadPackedInt32();
            WebCgi = reader.ReadNullableString();
            UserData = reader.ReadNullableString();
            Connected = reader.ReadBoolean();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
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
                .WriteNullable(UserData)
                .Write(Connected);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier
                = new Verifier<ConnectionSettings>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Host, "Host")
                .Assert(Port > 0 && Port < 0x10000, "Port")
                .NotNullNorEmpty(Username, "Username")
                .NotNullNorEmpty(Password, "Password");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return Encode();
        }

        #endregion
    }
}
