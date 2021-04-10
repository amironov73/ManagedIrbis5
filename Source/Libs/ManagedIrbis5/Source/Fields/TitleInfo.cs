// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable ConvertClosureToMethodGroup
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* TitleInfo.cs -- сведения о заглавии, поле 200
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Сведения о заглавии, поле 200.
    /// </summary>
    [DebuggerDisplay("{VolumeNumber} {Title}")]
    public sealed class TitleInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abefguv";

        /// <summary>
        /// Тег поля.
        /// </summary>
        public const int Tag = 200;

        #endregion

        #region Properties

        /// <summary>
        /// Обозначение и номер тома. Подполе v.
        /// </summary>
        [SubField('v')]
        [XmlAttribute("volume")]
        [JsonPropertyName("volume")]
        [Description("Обозначение и номер тома")]
        [DisplayName("Обозначение и номер тома")]
        public string? VolumeNumber { get; set; }

        /// <summary>
        /// Заглавие.
        /// </summary>
        [SubField('a')]
        [XmlAttribute("title")]
        [JsonPropertyName("title")]
        [Description("Заглавие")]
        [DisplayName("Заглавие")]
        public string? Title { get; set; }

        /// <summary>
        /// Нехарактерное заглавие. Подполе u.
        /// </summary>
        [SubField('u')]
        [XmlAttribute("specific")]
        [JsonPropertyName("specific")]
        [Description("Нехарактерное заглавие")]
        [DisplayName("Нехарактерное заглавие")]
        public string? Specific { get; set; }

        /// <summary>
        /// Общее обозначение материала. Подполе b.
        /// </summary>
        [SubField('b')]
        [XmlAttribute("general")]
        [JsonPropertyName("general")]
        [Description("Общее обозначение материала")]
        [DisplayName("Общее обозначение материала")]
        public string? General { get; set; }

        /// <summary>
        /// Сведения, относящиеся к заглавию. Подполе e.
        /// </summary>
        [SubField('e')]
        [XmlAttribute("subtitle")]
        [JsonPropertyName("subtitle")]
        [Description("Сведения, относящиеся к заглавию")]
        [DisplayName("Сведения, относящиеся к заглавию")]
        public string? Subtitle { get; set; }

        /// <summary>
        /// Первые сведения об ответственности. Подполе f.
        /// </summary>
        [SubField('f')]
        [XmlAttribute("first")]
        [JsonPropertyName("first")]
        [Description("Первые сведения об ответственности")]
        [DisplayName("Первые сведения об ответственности")]
        public string? FirstResponsibility { get; set; }

        /// <summary>
        /// Последующие сведения об ответственности. Подполе g.
        /// </summary>
        [SubField('g')]
        [XmlAttribute("other")]
        [JsonPropertyName("other")]
        [Description("Последующие сведения об ответственности")]
        [DisplayName("Последующие сведения об ответственности")]
        public string? OtherResponsibility { get; set; }

        /// <summary>
        /// Full title.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string FullTitle
        {
            get
            {
                var result = new StringBuilder();
                if (!string.IsNullOrEmpty(VolumeNumber))
                {
                    result.Append(VolumeNumber);
                }

                if (!string.IsNullOrEmpty(Title))
                {
                    if (result.Length != 0)
                    {
                        result.Append(". ");
                    }

                    result.Append(Title);
                }

                if (!string.IsNullOrEmpty(General))
                {
                    if (result.Length != 0)
                    {
                        result.Append(" ");
                    }

                    result.Append('[');
                    result.Append(General);
                    result.Append(']');
                }

                if (!string.IsNullOrEmpty(Subtitle))
                {
                    if (result.Length != 0)
                    {
                        result.Append(": ");
                        result.Append(Subtitle);
                    }
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Associated field.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Description("Поле")]
        [DisplayName("Поле")]
        public Field? Field { get; private set; }

        /// <summary>
        /// Arbitrary user data.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Description("Произвольные данные")]
        [DisplayName("Произвольные данные")]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Constructor.
        /// </summary>
        public TitleInfo()
        {
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TitleInfo
            (
                string title
            )
        {
            Title = title;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public TitleInfo
            (
                string volumeNumber,
                string title
            )
        {
            VolumeNumber = volumeNumber;
            Title = title;
        }

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Parse field 200.
        /// </summary>
        public static TitleInfo ParseField200
            (
                Field field
            )
        {
            // TODO: support for unknown subfields
            // TODO: реализовать эффективно

            var result = new TitleInfo
            {
                VolumeNumber = field.GetFirstSubFieldValue('v').ToString(),
                Title = field.GetFirstSubFieldValue('a').ToString(),
                Specific = field.GetFirstSubFieldValue('u').ToString(),
                General = field.GetFirstSubFieldValue('b').ToString(),
                Subtitle = field.GetFirstSubFieldValue('e').ToString(),
                FirstResponsibility = field.GetFirstSubFieldValue('f').ToString(),
                OtherResponsibility = field.GetFirstSubFieldValue('g').ToString(),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Parse field 330 or 922.
        /// </summary>
        public static TitleInfo ParseField330
            (
                Field field
            )
        {
            // TODO: support for unknown subfields
            // TODO: реализовать эффективно

            var result = new TitleInfo
            {
                Title = field.GetFirstSubFieldValue('c').ToString(),
                Subtitle = field.GetFirstSubFieldValue('e').ToString(),
                FirstResponsibility = field.GetFirstSubFieldValue('g').ToString(),
                Field = field
            };

            return result;
        }

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static TitleInfo[] Parse
            (
                Record record,
                int tag = Tag
            )
        {
            return record.Fields
                .GetField(tag)
                .Select(field => ParseField200(field))
                .ToArray();
        }

        /// <summary>
        /// Should serialize <see cref="FirstResponsibility"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeFirstResponsibility()
        {
            return !ReferenceEquals(FirstResponsibility, null);
        }

        /// <summary>
        /// Should serialize <see cref="General"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeGeneral()
        {
            return !ReferenceEquals(General, null);
        }

        /// <summary>
        /// Should serialize <see cref="OtherResponsibility"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeOtherResponsibility()
        {
            return !ReferenceEquals(OtherResponsibility, null);
        }

        /// <summary>
        /// Should serialize <see cref="Title"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeTitle()
        {
            return !ReferenceEquals(Title, null);
        }

        /// <summary>
        /// Should serialize <see cref="Specific"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSpecific()
        {
            return !ReferenceEquals(Specific, null);
        }

        /// <summary>
        /// Should serialize <see cref="Subtitle"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeSubtitle()
        {
            return !ReferenceEquals(Subtitle, null);
        }

        /// <summary>
        /// Should serialize <see cref="VolumeNumber"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public bool ShouldSerializeVolumeNumber()
        {
            return !ReferenceEquals(VolumeNumber, null);
        }

        /// <summary>
        /// Превращение обратно в поле 200.
        /// </summary>
        public Field ToField200()
        {
            var result = new Field { Tag = Tag }
                .AddNonEmptySubField('v', VolumeNumber)
                .AddNonEmptySubField('a', Title)
                .AddNonEmptySubField('u', Specific)
                .AddNonEmptySubField('b', General)
                .AddNonEmptySubField('e', Subtitle)
                .AddNonEmptySubField('f', FirstResponsibility)
                .AddNonEmptySubField('g', OtherResponsibility);

            return result;
        }

        /// <summary>
        /// Convert back to field 330/922.
        /// </summary>
        public Field ToField330
            (
                int tag
            )
        {
            var result = new Field { Tag = tag }
                .AddNonEmptySubField('c', Title)
                .AddNonEmptySubField('e', Subtitle)
                .AddNonEmptySubField('g', FirstResponsibility);

            return result;
        }

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            VolumeNumber = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Specific = reader.ReadNullableString();
            General = reader.ReadNullableString();
            Subtitle = reader.ReadNullableString();
            FirstResponsibility = reader.ReadNullableString();
            OtherResponsibility = reader.ReadNullableString();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(VolumeNumber)
                .WriteNullable(Title)
                .WriteNullable(Specific)
                .WriteNullable(General)
                .WriteNullable(Subtitle)
                .WriteNullable(FirstResponsibility)
                .WriteNullable(OtherResponsibility);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<TitleInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Title, "Title");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            if (string.IsNullOrEmpty(VolumeNumber))
            {
                return string.Format
                (
                    "Title: {0}, Subtitle: {1}",
                    Title.ToVisibleString(),
                    Subtitle.ToVisibleString()
                );
            }

            return string.Format
                (
                    "Volume: {0}, Title: {1}, Subtitle: {2}",
                    VolumeNumber.ToVisibleString(),
                    Title.ToVisibleString(),
                    Subtitle.ToVisibleString()
                );
        }

        #endregion

    } // class TitleInfo

} // namespace ManagedIrbis.Fields
