// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedParameter.Local
// ReSharper disable UnusedType.Global

/* ServerContext.cs -- серверный контекст для подключенного клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Server
{
    /// <summary>
    /// Серверный контекст, для подключенного клиента.
    /// </summary>
    [XmlRoot("context")]
    public sealed class ServerContext
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Адрес клиента (без порта!).
        /// </summary>
        [XmlElement("address")]
        [JsonPropertyName("address")]
        public string? Address { get; set; }

        /// <summary>
        /// Количество выполненных команд от данного клиента.
        /// </summary>
        [XmlElement("commandCount")]
        [JsonPropertyName("commandCount")]
        public int CommandCount { get; set; }

        /// <summary>
        /// Момент подключения клиента к данному серверу.
        /// </summary>
        [XmlElement("connected")]
        [JsonPropertyName("connected")]
        public DateTime Connected { get; set; }

        /// <summary>
        /// Уникальный идентификатор клиента.
        /// </summary>
        [XmlElement("id")]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Момент последней активности данного клиента.
        /// </summary>
        [XmlElement("lastActivity")]
        [JsonPropertyName("lastActivity")]
        public DateTime LastActivity { get; set; }

        /// <summary>
        /// Код последней выполенной команды от данного клиента.
        /// </summary>
        [XmlElement("lastCommand")]
        [JsonPropertyName("lastCommand")]
        public string? LastCommand { get;set; }

        /// <summary>
        /// Пароль пользователя.
        /// </summary>
        [XmlElement("password")]
        [JsonPropertyName("password")]
        public string? Password { get; set; }

        /// <summary>
        /// Логин пользователя.
        /// </summary>
        [XmlElement("username")]
        [JsonPropertyName("username")]
        public string? Username { get; set; }

        /// <summary>
        /// Код АРМ.
        /// </summary>
        [XmlElement("workstation")]
        [JsonPropertyName("workstation")]
        public string? Workstation { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные,
        /// сопоставленные с данным клиентом.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize the <see cref="CommandCount"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeCommandCount() => CommandCount != 0;

        /// <summary>
        /// Should serialize the <see cref="Connected"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeConnected() => Connected != DateTime.MinValue;

        /// <summary>
        /// Should serialize the <see cref="LastActivity"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeLastActivity() => LastActivity != DateTime.MinValue;

        /// <summary>
        /// Should serialize the <see cref="Workstation"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeWorkstation() => !string.IsNullOrEmpty(Workstation);

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Address = reader.ReadNullableString();
            CommandCount = reader.ReadPackedInt32();
            Connected = reader.ReadDateTime();
            Id = reader.ReadNullableString();
            LastActivity = reader.ReadDateTime();
            LastCommand = reader.ReadNullableString();
            Password = reader.ReadNullableString();
            Username = reader.ReadNullableString();
            Workstation = reader.ReadNullableString();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Address)
                .WritePackedInt32(CommandCount)
                .Write(Connected)
                .WriteNullable(Id)
                .Write(LastActivity)
                .WriteNullable(LastCommand)
                .WriteNullable(Password)
                .WriteNullable(Username)
                .WriteNullable(Workstation);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ServerContext>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Id, "Id");

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => Id.ToVisibleString();

        #endregion

    } // class ServerContext

} // namespace ManagedIrbis.Server
