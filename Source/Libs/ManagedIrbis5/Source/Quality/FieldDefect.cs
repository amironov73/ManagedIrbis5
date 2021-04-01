// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo

/* FieldDefect.cs -- detected defect of the field/subfield
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.Quality
{
    /// <summary>
    /// Detected defect of the field/subfield.
    /// </summary>
    [XmlRoot("defect")]
    [DebuggerDisplay("Field={Field} Value={Value} Message={Message}")]
    public sealed class FieldDefect
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Поле.
        /// </summary>
        [XmlAttribute("field")]
        [JsonPropertyName("field")]
        public int Field { get; set; }

        /// <summary>
        /// Повторение поля.
        /// </summary>
        [XmlAttribute("repeat")]
        [JsonPropertyName("repeat")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Repeat { get; set; }

        /// <summary>
        /// Подполе (если есть).
        /// </summary>
        [XmlAttribute("subfield")]
        [JsonPropertyName("subfield")]
        public string? Subfield { get; set; }

        /// <summary>
        /// Значение поля/подполя.
        /// </summary>
        [JsonPropertyName("value")]
        public string? Value { get; set; }

        /// <summary>
        /// Сообщение об ошибке.
        /// </summary>
        [XmlAttribute("message")]
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        /// <summary>
        /// Урон от дефекта.
        /// </summary>
        [XmlAttribute("damage")]
        [JsonPropertyName("damage")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Damage { get; set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Should serialize <see cref="Damage"/> field?
        /// </summary>
        public bool ShouldSerializeDamage()
        {
            return Damage != 0;
        }

        /// <summary>
        /// Should serialize <see cref="Repeat"/> field?
        /// </summary>
        public bool ShouldSerializeRepeat()
        {
            return Repeat != 0;
        }

        /// <summary>
        /// Should serialize <see cref="Subfield"/> field?
        /// </summary>
        public bool ShouldSerializeSubfield()
        {
            return !string.IsNullOrEmpty(Subfield);
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Field = reader.ReadPackedInt32();
            Repeat = reader.ReadPackedInt32();
            Subfield = reader.ReadNullableString();
            Value = reader.ReadNullableString();
            Message = reader.ReadNullableString();
            Damage = reader.ReadPackedInt32();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32(Field)
                .WritePackedInt32(Repeat)
                .WriteNullable(Subfield)
                .WriteNullable(Value)
                .WriteNullable(Message)
                .WritePackedInt32(Damage);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<FieldDefect>(this, throwOnError);

            verifier
                .Positive(Field, "Field")
                .Assert(Repeat >= 0, "Repeat")
                .NotNullNorEmpty(Message, "Message")
                .Assert(Damage >= 0, "Damage");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return string.Format
                (
                    "Field: {0}, Value: {1}, Message: {2}",
                    Field,
                    Value.ToVisibleString(),
                    Message.ToVisibleString()
               );
        }

        #endregion

    } // class FieldDefect

} // namespace ManagedIrbis.Quality
