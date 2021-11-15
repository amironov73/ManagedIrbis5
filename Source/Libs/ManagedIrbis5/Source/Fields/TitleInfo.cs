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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Mapping;

#endregion

#nullable enable

namespace ManagedIrbis.Fields
{
    /// <summary>
    /// Сведения о заглавии, поле 200.
    /// </summary>
    [XmlRoot ("title")]
    public sealed class TitleInfo
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Constants

        /// <summary>
        /// Метка поля.
        /// </summary>
        public const int Tag = 200;

        /// <summary>
        /// Известные коды подполей.
        /// </summary>
        public const string KnownCodes = "abefguv";

        #endregion

        #region Properties

        /// <summary>
        /// Обозначение и номер тома. Подполе V.
        /// </summary>
        [SubField ('v')]
        [XmlAttribute ("volume")]
        [JsonPropertyName ("volume")]
        [Description ("Обозначение и номер тома")]
        [DisplayName ("Обозначение и номер тома")]
        public string? VolumeNumber { get; set; }

        /// <summary>
        /// Заглавие. Подполе A.
        /// </summary>
        [SubField ('a')]
        [XmlAttribute ("title")]
        [JsonPropertyName ("title")]
        [Description ("Заглавие")]
        [DisplayName ("Заглавие")]
        public string? Title { get; set; }

        /// <summary>
        /// Нехарактерное заглавие. Подполе U.
        /// </summary>
        [SubField ('u')]
        [XmlAttribute ("specific")]
        [JsonPropertyName ("specific")]
        [Description ("Нехарактерное заглавие")]
        [DisplayName ("Нехарактерное заглавие")]
        public string? Specific { get; set; }

        /// <summary>
        /// Общее обозначение материала. Подполе B.
        /// В связи с вводом нового ГОСТ подполе утратило смысл.
        /// </summary>
        [SubField ('b')]
        [XmlAttribute ("general")]
        [JsonPropertyName ("general")]
        [Description ("Общее обозначение материала")]
        [DisplayName ("Общее обозначение материала")]
        public string? General { get; set; }

        /// <summary>
        /// Сведения, относящиеся к заглавию. Подполе E.
        /// </summary>
        [SubField ('e')]
        [XmlAttribute ("subtitle")]
        [JsonPropertyName ("subtitle")]
        [Description ("Сведения, относящиеся к заглавию")]
        [DisplayName ("Сведения, относящиеся к заглавию")]
        public string? Subtitle { get; set; }

        /// <summary>
        /// Первые сведения об ответственности. Подполе F.
        /// </summary>
        [SubField ('f')]
        [XmlAttribute ("first")]
        [JsonPropertyName ("first")]
        [Description ("Первые сведения об ответственности")]
        [DisplayName ("Первые сведения об ответственности")]
        public string? FirstResponsibility { get; set; }

        /// <summary>
        /// Последующие сведения об ответственности. Подполе G.
        /// </summary>
        [SubField ('g')]
        [XmlAttribute ("other")]
        [JsonPropertyName ("other")]
        [Description ("Последующие сведения об ответственности")]
        [DisplayName ("Последующие сведения об ответственности")]
        public string? OtherResponsibility { get; set; }

        /// <summary>
        /// Полное заглавие.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string FullTitle
        {
            get
            {
                var builder = StringBuilderPool.Shared.Get();

                builder
                    .Append (VolumeNumber)
                    .AppendWithDelimiter (Title, ". ")
                    .AppendWithBrackets (General, " [", "]")
                    .AppendWithDelimiter (Subtitle, " : ")
                    .AppendWithDelimiter (FirstResponsibility, " / ")
                    .AppendWithDelimiter (OtherResponsibility, " ; ");

                var result = builder.ToString().EmptyToNull().ToVisibleString();
                StringBuilderPool.Shared.Return (builder);

                return result;
            }
        }

        /// <summary>
        /// Неизвестные подполя.
        /// </summary>
        [XmlElement ("unknown")]
        [JsonPropertyName ("unknown")]
        [Browsable (false)]
        public SubField[]? UnknownSubFields { get; set; }

        /// <summary>
        /// Ассоциированное поле библиографической записи.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public Field? Field { get; private set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        [Browsable (false)]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        /// <summary>
        /// Конструктор по умолчанию.
        /// </summary>
        public TitleInfo()
        {
        }

        /// <summary>
        /// Конструктор с заглавием.
        /// </summary>
        public TitleInfo
            (
                string title
            )
        {
            Title = title;
        }

        /// <summary>
        /// Конструктор с номером тома и заглавием.
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

        #region Public methods

        /// <summary>
        /// Разбор указанного поля 200.
        /// </summary>
        public static TitleInfo ParseField200
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new TitleInfo
            {
                VolumeNumber = field.GetFirstSubFieldValue ('v'),
                Title = field.GetFirstSubFieldValue ('a'),
                Specific = field.GetFirstSubFieldValue ('u'),
                General = field.GetFirstSubFieldValue ('b'),
                Subtitle = field.GetFirstSubFieldValue ('e'),
                FirstResponsibility = field.GetFirstSubFieldValue ('f'),
                OtherResponsibility = field.GetFirstSubFieldValue ('g'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };
        }

        /// <summary>
        /// Разбор поля 330 или 922.
        /// </summary>
        public static TitleInfo ParseField330
            (
                Field field
            )
        {
            Sure.NotNull (field);

            return new TitleInfo
            {
                Title = field.GetFirstSubFieldValue ('c'),
                Subtitle = field.GetFirstSubFieldValue ('e'),
                FirstResponsibility = field.GetFirstSubFieldValue ('g'),
                UnknownSubFields = field.Subfields.GetUnknownSubFields (KnownCodes),
                Field = field
            };
        }

        /// <summary>
        /// Разбор библиографической записи.
        /// </summary>
        public static TitleInfo[] ParseRecord
            (
                Record record,
                int tag = Tag
            )
        {
            Sure.NotNull (record);
            Sure.Positive (tag);

            return record
                .EnumerateField (tag)
                .Select (field => ParseField200 (field))
                .ToArray();
        }

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="FirstResponsibility"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeFirstResponsibility()
        {
            return FirstResponsibility is not null;
        }

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="General"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeGeneral()
        {
            return General is not null;
        }

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="OtherResponsibility"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeOtherResponsibility()
        {
            return OtherResponsibility is not null;
        }

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Title"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeTitle()
        {
            return Title is not null;
        }

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Specific"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeSpecific()
        {
            return Specific is not null;
        }

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="Subtitle"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeSubtitle()
        {
            return Subtitle is not null;
        }

        /// <summary>
        /// Нужно ли сериализовать свойство <see cref="VolumeNumber"/>?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeVolumeNumber()
        {
            return VolumeNumber is not null;
        }

        /// <summary>
        /// Превращение обратно в поле 200.
        /// </summary>
        public Field ToField200()
        {
            var result = new Field (Tag)
                .AddNonEmpty ('v', VolumeNumber)
                .AddNonEmpty ('a', Title)
                .AddNonEmpty ('u', Specific)
                .AddNonEmpty ('b', General)
                .AddNonEmpty ('e', Subtitle)
                .AddNonEmpty ('f', FirstResponsibility)
                .AddNonEmpty ('g', OtherResponsibility)
                .AddRange (UnknownSubFields);

            return result;
        }

        /// <summary>
        /// Преобразование данных в поле 330/922.
        /// </summary>
        public Field ToField330
            (
                int tag
            )
        {
            var result = new Field (tag)
                .AddNonEmpty ('c', Title)
                .AddNonEmpty ('e', Subtitle)
                .AddNonEmpty ('g', FirstResponsibility)
                .AddRange (UnknownSubFields);

            return result;
        } // method ToField330

        #endregion

        #region IHandmadeSerializable members

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        void IHandmadeSerializable.RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Sure.NotNull (reader);

            VolumeNumber = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            Specific = reader.ReadNullableString();
            General = reader.ReadNullableString();
            Subtitle = reader.ReadNullableString();
            FirstResponsibility = reader.ReadNullableString();
            OtherResponsibility = reader.ReadNullableString();
            UnknownSubFields = reader.ReadNullableArray<SubField>();
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        void IHandmadeSerializable.SaveToStream
            (
                BinaryWriter writer
            )
        {
            Sure.NotNull (writer);

            writer
                .WriteNullable (VolumeNumber)
                .WriteNullable (Title)
                .WriteNullable (Specific)
                .WriteNullable (General)
                .WriteNullable (Subtitle)
                .WriteNullable (FirstResponsibility)
                .WriteNullable (OtherResponsibility)
                .WriteNullableArray (UnknownSubFields);
        }

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<TitleInfo> (this, throwOnError);

            verifier
                .NotNullNorEmpty (Title);

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return FullTitle;
        }

        #endregion

    } // class TitleInfo

} // namespace ManagedIrbis.Fields
