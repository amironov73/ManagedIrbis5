// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global

/* BibTexRecord.cs -- BibTex-запись
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

#endregion

#nullable enable

namespace ManagedIrbis.BibTex
{
    /// <summary>
    /// BibTex-запись.
    /// </summary>
    [XmlRoot ("record")]
    public sealed class BibTexRecord
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Properties

        /// <summary>
        /// Тип записи.
        /// </summary>
        [XmlAttribute ("type")]
        [JsonPropertyName ("type")]
        [Description ("Тип записи")]
        public string? Type { get; set; }

        /// <summary>
        /// Метка записи.
        /// </summary>
        [XmlAttribute ("tag")]
        [JsonPropertyName ("tag")]
        [Description ("Метка записи")]
        public string? Tag { get; set; }

        /// <summary>
        /// Поля записи.
        /// </summary>
        [XmlElement ("field")]
        [JsonPropertyName ("fields")]
        [Description ("Поля записи")]
        public List<BibTexField> Fields { get; } = new ();

        /// <summary>
        /// Произвольные пользовательские данные
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Public methods

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Fields"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeFields()
        {
            return Fields.Count != 0;
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

            Fields.Clear();
            Type = reader.ReadNullableString();
            Tag = reader.ReadNullableString();
            reader.ReadList (Fields);
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (Type)
                .WriteNullable (Tag)
                .WriteList (Fields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify"/>
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<BibTexRecord> (this, throwOnError);

            verifier
                .NotNull (Type)
                .IsOneOf (Type, RecordType.ListValues())
                .NotNullNorEmpty (Tag)
                .NotNullNorEmpty (Fields);

            foreach (var field in Fields)
            {
                verifier.VerifySubObject (field);
            }

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString"/>
        public override string ToString()
        {
            return $"{Type.ToVisibleString()}: {Tag.ToVisibleString()}";
        }

        #endregion
    }
}
