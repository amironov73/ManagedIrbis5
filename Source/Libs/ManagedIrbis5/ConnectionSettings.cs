// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ConnectionSettings.cs -- настройки подключения
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
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
    /// Настройки подключения (для отдельного сохранения).
    /// </summary>
    [XmlRoot("connection")]
    public sealed class ConnectionSettings
        : IHandmadeSerializable,
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
        /// IP-address of the server.
        /// </summary>
        /// <remarks>Default value is "127.0.0.1".</remarks>
        [XmlAttribute("host")]
        [JsonPropertyName("host")]
        public string? Host { get; set; } = DefaultHost;

        /// <summary>
        /// IP-port of the server.
        /// </summary>
        /// <remarks>Default value is 6666.</remarks>
        [XmlAttribute("port")]
        [JsonPropertyName("port")]
        public int Port { get; set; } = DefaultPort;

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
        /// Arbitrary user data.
        /// </summary>
        [XmlAttribute("userdata")]
        [JsonPropertyName("userdata")]
        public string? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Clone.
        /// </summary>
        public ConnectionSettings Clone() => (ConnectionSettings)MemberwiseClone();

        /// <summary>
        /// Construct <see cref="ConnectionSettings"/>
        /// from <see cref="Connection"/>.
        /// </summary>
        public static ConnectionSettings FromConnection(Connection connection)
            => new ConnectionSettings
            {
                Host = connection.Host,
                Port = connection.Port,
                Username = connection.Username,
                Password = connection.Password,
                Database = connection.Database,
                Workstation = connection.Workstation,
            };

        /// <summary>
        /// Парсинг строки подключения.
        /// </summary>
        public ConnectionSettings ParseConnectionString
            (
                string connectionString
            )
        {
            Sure.NotNull(connectionString, nameof(connectionString));

            Parameter[] parameters = ParameterUtility.ParseString
                (
                    connectionString
                );

            foreach (Parameter parameter in parameters)
            {
                string name = parameter.Name
                    .ThrowIfNull("parameter.Name")
                    .ToLower();
                string value = parameter.Value
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
                        Workstation = value;
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

                        var message = $"Unknown parameter: {name}";
                        throw new ArgumentException(message);
                }
            }

            return this;
        } // method ParseConnectionString

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream(BinaryReader reader)
        {
            Host = reader.ReadNullableString();
            Port = reader.ReadPackedInt32();
            Username = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            Workstation = reader.ReadNullableString();
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
                .Assert(Port > 0 && Port < 0x10000, "Port")
                .NotNullNorEmpty(Username, "Username")
                .NotNullNorEmpty(Password, "Password")
                .NotNullNorEmpty(Workstation)
                .NotNullNorEmpty(Database);

            return verifier.Result;
        } // method Verify

        #endregion

    } // class ConnectionSettings

} // namespace ManagedIrbis
