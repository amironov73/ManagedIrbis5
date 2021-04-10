// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable PropertyCanBeMadeInitOnly.Local
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global
// ReSharper disable UnusedType.Global

/* BinaryResource.cs -- field 953.
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Binary resource in field 953.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Resource) + "}")]
    public sealed class BinaryResource
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Known codes.
        /// </summary>
        public const string KnownCodes = "abpt";

        /// <summary>
        /// Default tag for binary resources.
        /// </summary>
        public const int Tag = 953;

        #endregion

        #region Properties

        /// <summary>
        /// Kind of resource. Subfield a.
        /// </summary>
        /// <remarks>For example, "jpg".</remarks>
        [SubField('a')]
        [XmlElement("kind")]
        [JsonPropertyName("kind")]
        [Description("Тип двоичного ресурса")]
        [DisplayName("Тип двоичного ресурса")]
        public string? Kind { get; set; }

        /// <summary>
        /// Percent-encoded resource. Subfield b.
        /// </summary>
        [SubField('b')]
        [XmlElement("resource")]
        [JsonPropertyName("resource")]
        [Description("Двоичный ресурс (закодированный)")]
        [DisplayName("Двоичный ресурс (закодированный)")]
        public string? Resource { get; set; }

        /// <summary>
        /// Title of resource. Subfield t.
        /// </summary>
        [SubField('t')]
        [XmlElement("title")]
        [JsonPropertyName("title")]
        [Description("Название двоичного ресурса")]
        [DisplayName("Название двоичного ресурса")]
        public string? Title { get; set; }

        /// <summary>
        /// View method. Subfield p.
        /// </summary>
        /// <remarks>
        /// См. <see cref="ResourceView"/>.
        /// </remarks>
        [SubField('p')]
        [XmlElement("view")]
        [JsonPropertyName("view")]
        [Description("Характер просмотра")]
        [DisplayName("Характер просмотр")]
        public string? View { get; set; }

        /// <summary>
        /// Associated field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable(false)]
        [Description("Поле")]
        [DisplayName("Поле")]
        public Field? Field { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Description("Пользовательские данные")]
        [DisplayName("Пользовательские данные")]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public BinaryResource()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BinaryResource
            (
                string? kind,
                string? resource
            )
        {
            Kind = kind;
            Resource = resource;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public BinaryResource
            (
                string? kind,
                string? resource,
                string? title
            )
        {
            Kind = kind;
            Resource = resource;
            Title = title;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Apply to the field.
        /// </summary>
        public void ApplyToField
            (
                Field field
            )
        {
            field
                .ApplySubField('a', Kind)
                .ApplySubField('b', Resource)
                .ApplySubField('t', Title)
                .ApplySubField('p', View);
        }

        /// <summary>
        /// Decode the resource.
        /// </summary>
        public byte[] Decode()
        {
            if (string.IsNullOrEmpty(Resource))
            {
                return new byte[0];
            }

            byte[] result = IrbisUtility.DecodePercentString(Resource);

            return result;
        }

        /// <summary>
        /// Encode the resource.
        /// </summary>
        public string? Encode
            (
                byte[] array
            )
        {
            Resource = array.IsNullOrEmpty()
                ? null
                : IrbisUtility.EncodePercentString(array);

            return Resource;
        }

        /// <summary>
        /// Parse field 953.
        /// </summary>
        public static BinaryResource Parse
            (
                Field field
            )
        {
            // TODO: реализовать эффективно

            var result = new BinaryResource
            {
                Kind = field.GetFirstSubFieldValue('a').ToString(),
                Resource = field.GetFirstSubFieldValue('b').ToString(),
                Title = field.GetFirstSubFieldValue('t').ToString(),
                View = field.GetFirstSubFieldValue('p').ToString(),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse fields 953 of the <see cref="Record"/>.
        /// </summary>
        public static BinaryResource[] Parse
            (
                Record record,
                int tag = Tag
            )
        {
            var fields = record
                .Fields
                .GetField(tag);

            var result = fields
                .Select(Parse)
                .ToArray();

            return result;
        }

        /// <summary>
        /// Convert back to field.
        /// </summary>
        public Field ToField()
        {
            var result = new Field  { Tag = Tag }
                .AddNonEmptySubField('a', Kind)
                .AddNonEmptySubField('b', Resource)
                .AddNonEmptySubField('t', Title)
                .AddNonEmptySubField('p', View);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Kind = reader.ReadNullableString();
            Resource = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            View = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Kind)
                .WriteNullable(Resource)
                .WriteNullable(Title)
                .WriteNullable(View);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            Verifier<BinaryResource> verifier
                = new Verifier<BinaryResource>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Resource, "Resource");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() =>
            $"Kind: {Kind.ToVisibleString()}, Resource: {Resource.ToVisibleString()}, Title: {Title.ToVisibleString()}";

        #endregion

    } // class BinaryResource

} // namespace ManagedIrbis.Fields
