// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* ReservationInfo.cs -- данные о резервировании компьютера
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;
using ManagedIrbis.Records;

#endregion

#nullable enable

namespace ManagedIrbis.Reservations
{
    /// <summary>
    /// Данные о резервировании компьютера.
    /// База данных RESERV.
    /// </summary>
    [XmlRoot ("reservation")]
    public sealed class ReservationInfo
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Properties

        /// <summary>
        /// Читальный зал.
        /// </summary>
        [Field (10)]
        [XmlAttribute ("room")]
        [JsonPropertyName ("room")]
        [Description ("Читальный зал")]
        [DisplayName ("Читальный зал")]
        public string? Room { get; set; }

        /// <summary>
        /// Номер компьютера.
        /// </summary>
        [Field (11)]
        [XmlAttribute ("number")]
        [JsonPropertyName ("number")]
        [Description ("Номер")]
        [DisplayName ("Номер")]
        public string? Number { get; set; }

        /// <summary>
        /// Статус.
        /// </summary>
        /// <remarks>
        /// См. <see cref="ReservationStatus"/>.
        /// </remarks>
        [Field (12)]
        [XmlAttribute ("status")]
        [JsonPropertyName ("status")]
        [Description ("Статус")]
        [DisplayName ("Статус")]
        public string? Status { get; set; }

        /// <summary>
        /// Описание, например: "AutoCAD, MathCAD".
        /// </summary>
        [Field (13)]
        [XmlAttribute ("description")]
        [JsonPropertyName ("description")]
        [Description ("Описание")]
        [DisplayName ("Описание")]
        public string? Description { get; set; }

        /// <summary>
        /// Заявки на резервирование.
        /// </summary>
        [Field (20)]
        [XmlElement ("claim")]
        [JsonPropertyName ("claims")]
        [Description ("Заявки")]
        [DisplayName ("Заявки")]
        public NonNullCollection<ReservationClaim> Claims { get; private set; }

        /// <summary>
        /// История выдач.
        /// </summary>
        [Field (30)]
        [XmlElement ("history")]
        [JsonPropertyName ("history")]
        [Description ("История")]
        [DisplayName ("История")]
        public NonNullCollection<HistoryInfo> History { get; private set; }

        /// <summary>
        /// Duration.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public TimeSpan Duration
        {
            get
            {
                var result = new TimeSpan();
                if (Status == ReservationStatus.Busy)
                {
                    var entry = History.LastOrDefault();
                    if (!ReferenceEquals (entry, null)
                        && string.IsNullOrEmpty (entry.EndTimeString))
                    {
                        result = DateTime.Now - entry.BeginDate;
                    }
                }

                return result;
            }

        } // property Duration

        /// <summary>
        /// Associated record.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Record? Record { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public ReservationInfo()
        {
            Claims = new NonNullCollection<ReservationClaim>();
            History = new NonNullCollection<HistoryInfo>();
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Применение информации о резервировании к библиографической записи.
        /// </summary>
        public void ApplyToRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            record.Fields
                .ApplyFieldValue (10, Room)
                .ApplyFieldValue (11, Number)
                .ApplyFieldValue (12, Status)
                .ApplyFieldValue (13, Description);
            var claims = Claims
                .Select (item => item.ToField())
                .ToArray();
            record.ReplaceField (ReservationClaim.Tag, claims);
            var history = History
                .Select (item => item.ToField())
                .ToArray();
            record.ReplaceField (HistoryInfo.Tag, history);

        } // method ApplyToRecord

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static ReservationInfo ParseRecord
            (
                Record record
            )
        {
            Sure.NotNull (record);

            var result = new ReservationInfo
            {
                Room = record.FM (10),
                Number = record.FM (11),
                Status = record.FM (12),
                Description = record.FM (13),
                Record = record
            };
            result.Claims.AddRange
                (
                    ReservationClaim.ParseRecord (record)
                );
            result.History.AddRange
                (
                    HistoryInfo.ParseRecord (record)
                );

            return result;

        } // method ParseRecord

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Claims"/>?
        /// </summary>
        public bool ShouldSerializeClaims() => Claims.Count != 0;

        /// <summary>
        /// Нужно ли сериализовать <see cref="History"/>?
        /// </summary>
        public bool ShouldSerializeHistory() => History.Count != 0;

        /// <summary>
        /// Превращение информации в библиографическую запись.
        /// </summary>
        public Record ToRecord()
        {
            var result = new Record();
            result
                .AddNonEmptyField (10, Room)
                .AddNonEmptyField (11, Number)
                .AddNonEmptyField (12, Status)
                .AddNonEmptyField (13, Description);
            var claims = Claims
                .Select (item => item.ToField())
                .ToArray();
            result.ReplaceField (ReservationClaim.Tag, claims);
            var history = History
                .Select (item => item.ToField())
                .ToArray();
            result.ReplaceField (HistoryInfo.Tag, history);

            return result;

        } // method ToRecord

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            Room = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Status = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Claims = reader.ReadNonNullCollection<ReservationClaim>();
            History = reader.ReadNonNullCollection<HistoryInfo>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Room)
                .WriteNullable (Number)
                .WriteNullable (Status)
                .WriteNullable (Description)
                .WriteCollection (Claims)
                .WriteCollection (History);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<ReservationInfo> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Room)
                .NotNullNorEmpty (Number)
                .NotNullNorEmpty (Status);

            foreach (var claim in Claims)
            {
                verifier.VerifySubObject (claim);
            }

            foreach (var history in History)
            {
                verifier.VerifySubObject (history);
            }

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => !string.IsNullOrEmpty (Room)
                ? $"[{Room}] {Number.ToVisibleString()}: {Status.ToVisibleString()}"
                : $"{Number.ToVisibleString()}: {Status.ToVisibleString()}";

        #endregion

    } // class ReservationInfo

} // namespace ManagedIrbis.Reservations
