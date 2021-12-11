// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo

/* LogClientRecord.cs -- запись о действии клиента
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.LogClientDb
{
    /// <summary>
    /// Запись о действии клиента.
    /// </summary>
    [XmlRoot ("log")]
    public sealed class LogClientRecord
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Дата/время, поле 1.
        /// </summary>
        [Field (1)]
        [XmlAttribute ("moment")]
        [JsonPropertyName ("moment")]
        [Description ("Момент действия - дата/время")]
        public string? Moment { get; set; }

        /// <summary>
        /// Логин клиента, поле 2.
        /// </summary>
        [Field (2)]
        [XmlAttribute ("login")]
        [JsonPropertyName ("login")]
        [Description ("Логин клиента")]
        public string? Login { get; set; }

        /// <summary>
        /// IP-адрес клиента, поле 3.
        /// </summary>
        [Field (3)]
        [XmlAttribute ("ip-address")]
        [JsonPropertyName ("ipAddress")]
        [Description ("IP-адрес клиента")]
        public string? IpAddress { get; set; }

        /// <summary>
        /// Имя базы данных, поле 4.
        /// </summary>
        [Field (4)]
        [XmlAttribute ("database")]
        [JsonPropertyName ("database")]
        [Description ("Имя базы данных")]
        public string? Database { get; set; }

        /// <summary>
        /// Код действия, поле 5.
        /// См. константы в <see cref="EventCode"/>.
        /// </summary>
        [Field (5)]
        [XmlAttribute ("code")]
        [JsonPropertyName ("code")]
        [Description ("Код действия")]
        public string? ActionCode { get; set; }

        /// <summary>
        /// Содержание (суть) действия, поле 6.
        /// </summary>
        [Field (6)]
        [XmlElement ("essence")]
        [JsonPropertyName ("essence")]
        [Description ("Содержание действия")]
        public string[]? ActionEssence { get; set; }

        /// <summary>
        /// Ассоциированная запись базы данных.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Record? Record { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Применение данных к записи для базы.
        /// </summary>
        public void ApplyToRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            record
                .RemoveField (1)
                .AddNonEmptyField (1, Moment)
                .RemoveField (2)
                .AddNonEmptyField (2, Login)
                .RemoveField (3)
                .AddNonEmptyField (3, IpAddress)
                .RemoveField (4)
                .AddNonEmptyField (4, Database)
                .RemoveField (5)
                .AddNonEmptyField (5, ActionCode)
                .RemoveField (6);
            if (!ActionEssence.IsNullOrEmpty())
            {
                foreach (var line in ActionEssence)
                {
                    record.AddNonEmptyField (6, line);
                }
            }
        }

        /// <summary>
        /// Разбор записи из базы данных
        /// </summary>
        public static LogClientRecord ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            return new ()
            {
                Moment = record.FM (1),
                Login = record.FM (2),
                IpAddress = record.FM (3),
                Database = record.FM (4),
                ActionCode = record.FM (5),
                ActionEssence = record.FMA (6),
                Record = record
            };
        }

        /// <summary>
        /// Преобразование данных в запись для базы.
        /// </summary>
        public Record ToRecord()
        {
            var result = new Record()
                .AddNonEmptyField (1, Moment)
                .AddNonEmptyField (2, Login)
                .AddNonEmptyField (3, IpAddress)
                .AddNonEmptyField (4, Database)
                .AddNonEmptyField (5, ActionCode);
            if (!ActionEssence.IsNullOrEmpty())
            {
                foreach (var line in ActionEssence)
                {
                    result.AddNonEmptyField (6, line);
                }
            }

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Moment = reader.ReadNullableString();
            Login = reader.ReadNullableString();
            IpAddress = reader.ReadNullableString();
            Database = reader.ReadNullableString();
            ActionCode = reader.ReadNullableString();
            ActionEssence = reader.ReadNullableStringArray();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Moment)
                .WriteNullable (Login)
                .WriteNullable (IpAddress)
                .WriteNullable (Database)
                .WriteNullable (ActionCode)
                .WriteNullableArray (ActionEssence);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<LogClientRecord> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Moment)
                .NotNullNorEmpty (Login)
                .NotNullNorEmpty (IpAddress)
                .NotNullNorEmpty (Database)
                .NotNullNorEmpty (ActionCode);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{Moment};{Login};{IpAddress};{Database};{ActionCode}";
        }

        #endregion

    }
}
