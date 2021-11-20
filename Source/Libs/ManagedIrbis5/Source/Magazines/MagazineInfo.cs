// This is an open source non-commercial project. Dear PVS-Studio, please check it.
// PVS-Studio Static Code Analyzer for C, C++ and C#: http://www.viva64.com

// ReSharper disable CheckNamespace
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable CommentTypo
// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming
// ReSharper disable StringLiteralTypo
// ReSharper disable UnusedParameter.Local

/* MagazineInfo.cs -- информация о журнале в целом
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.IO;
using AM.Runtime;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Информация о журнале в целом.
    /// </summary>
    [XmlRoot("magazine")]
    public sealed class MagazineInfo
        : IHandmadeSerializable,
        IVerifiable
    {
        #region Constants

        /// <summary>
        /// Code for magazine kind.
        /// </summary>
        public const string MagazineKindCode = "a";

        /// <summary>
        /// Code for newspaper kind.
        /// </summary>
        public const string NewspaperKindCode = "c";

        #endregion

        #region Properties

        /// <summary>
        /// Код документа в базе. Поле 903.
        /// </summary>
        [XmlAttribute("index")]
        [JsonPropertyName("index")]
        public string? Index { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [XmlAttribute("description")]
        [JsonPropertyName("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Заглавие. Поле 200^a.
        /// </summary>
        [XmlAttribute("title")]
        [JsonPropertyName("title")]
        public string? Title { get; set; }

        /// <summary>
        /// Подзаголовочные сведения.
        /// Поле 200^e.
        /// </summary>
        [XmlAttribute("sub-title")]
        [JsonPropertyName("sub-title")]
        public string? SubTitle { get; set; }

        /// <summary>
        /// Обозначение и выпуск серии.
        /// Поле 923^1.
        /// </summary>
        [XmlAttribute("series-number")]
        [JsonPropertyName("series-number")]
        public string? SeriesNumber { get; set; }

        /// <summary>
        /// Заголовок серии.
        /// Поле 923^i.
        /// </summary>
        [XmlAttribute("series-title")]
        [JsonPropertyName("series-title")]
        public string? SeriesTitle { get; set; }

        /// <summary>
        /// Расширенное заглавие.
        /// Включает заголовок выпуск и заголовок серии.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string ExtendedTitle
        {
            get
            {
                var result = new StringBuilder();
                result.Append(Title);
                if (!string.IsNullOrEmpty(SeriesNumber))
                {
                    result.AppendFormat(". {0}", SeriesNumber);
                }
                if (!string.IsNullOrEmpty(SeriesTitle))
                {
                    result.AppendFormat(". {0}", SeriesTitle);
                }
                if (!string.IsNullOrEmpty(SubTitle))
                {
                    result.AppendFormat(": {0}", SubTitle);
                }

                return result.ToString();
            }
        }

        /// <summary>
        /// Тип издания. Поле 110^t
        /// </summary>
        [XmlAttribute("magazine-type")]
        [JsonPropertyName("magazine-type")]
        public string? MagazineType { get; set; }

        /// <summary>
        /// Вид издания. Поле 110^b
        /// </summary>
        /// <remarks>
        /// Журнал = 'a'
        /// Газета = 'c'
        /// </remarks>
        [XmlAttribute("magazine-kind")]
        [JsonPropertyName("magazine-kind")]
        public string? MagazineKind { get; set; }

        /// <summary>
        /// Периодичность (число). Поле 110^x
        /// </summary>
        [XmlAttribute("periodicity")]
        [JsonPropertyName("periodicity")]
        public string? Periodicity { get; set; }

        /// <summary>
        /// Кумуляция. Поле 909
        /// </summary>
        [XmlElement("cumulation")]
        [JsonPropertyName("cumulation")]
        public MagazineCumulation[]? Cumulation { get; set; }

        /// <summary>
        /// Сведения о заказах (поквартальные). Поле 938.
        /// </summary>
        [XmlElement("order")]
        [JsonPropertyName("orders")]
        public QuarterlyOrderInfo[]? QuarterlyOrders { get; set; }

        /// <summary>
        /// MFN записи журнала.
        /// </summary>
        [XmlElement("mfn")]
        [JsonPropertyName("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Is newspapaper?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsNewspaper => MagazineKind.SameString(NewspaperKindCode);

        /// <summary>
        /// Record.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public Record? Record { get; set; }

        /// <summary>
        /// Произвольные пользовательские данные.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public object? UserData { get; set; }

        #endregion

        #region Construction

        #endregion

        #region Private members

        #endregion

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static MagazineInfo? Parse
            (
                Record record
            )
        {
            // TODO: реализовать оптимально

            var result = new MagazineInfo
            {
                Index = record.FM(903),
                Title = record.FM(200, 'a'),
                SubTitle = record.FM(200, 'e'),
                Cumulation = MagazineCumulation.Parse(record),
                QuarterlyOrders = QuarterlyOrderInfo.ParseRecord (record),
                SeriesNumber = record.FM(923,'h'),
                SeriesTitle = record.FM(923, 'i'),
                MagazineType = record.FM(110, 't'),
                MagazineKind = record.FM(110, 'b'),
                Periodicity = record.FM(110, 'x'),
                Record = record,
                Mfn = record.Mfn
            };

            if (string.IsNullOrEmpty(result.Title)
                || string.IsNullOrEmpty(result.Index)
                //|| string.IsNullOrEmpty(result.MagazineKind)
                //|| string.IsNullOrEmpty(result.MagazineType)
            )
            {
                return null;
            }

            return result;
        }

        /// <summary>
        /// Should serialize the <see cref="Mfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        public bool ShouldSerializeMfn()
        {
            return Mfn != 0;
        }

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream" />
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Index = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            Title = reader.ReadNullableString();
            SubTitle = reader.ReadNullableString();
            SeriesNumber = reader.ReadNullableString();
            SeriesTitle = reader.ReadNullableString();
            MagazineType = reader.ReadNullableString();
            MagazineKind = reader.ReadNullableString();
            Periodicity = reader.ReadNullableString();
            Mfn = reader.ReadPackedInt32();
            QuarterlyOrders = reader.ReadNullableArray<QuarterlyOrderInfo>();

            // TODO Handle Cumulation array
        }

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream" />
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WriteNullable(Index)
                .WriteNullable(Description)
                .WriteNullable(Title)
                .WriteNullable(SubTitle)
                .WriteNullable(SeriesNumber)
                .WriteNullable(SeriesTitle)
                .WriteNullable(MagazineType)
                .WriteNullable(MagazineKind)
                .WriteNullable(Periodicity)
                .WritePackedInt32(Mfn)
                .WriteNullableArray(QuarterlyOrders);

            // TODO Handle Cumulation array
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
                = new Verifier<MagazineInfo>(this, throwOnError);

            verifier
                .NotNullNorEmpty(Title, "Title");

            return verifier.Result;
        }

        #endregion

        #region Object members

        /// <inheritdoc cref="object.ToString" />
        public override string ToString()
        {
            return ExtendedTitle;
        }

        #endregion

    } // class MagazineInfo

} // namespace ManagedIrbis.Magazines
