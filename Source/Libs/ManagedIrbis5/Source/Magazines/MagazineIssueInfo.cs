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

/* MagazineIssueInfo.cs -- сведения о номере журнала
 * Ars Magna project, http://arsmagna.ru
 */

#region Using directives

using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

using AM;
using AM.Collections;
using AM.IO;
using AM.Runtime;
using AM.Text;

using ManagedIrbis.Fields;

#endregion

#nullable enable

namespace ManagedIrbis.Magazines
{
    /// <summary>
    /// Сведения о номере журнала
    /// </summary>
    [XmlRoot ("issue")]
    [DebuggerDisplay ("{Year} {Number} {Supplement}")]
    public sealed class MagazineIssueInfo
        : IHandmadeSerializable,
            IVerifiable
    {
        #region Properties

        /// <summary>
        /// MFN записи.
        /// </summary>
        [XmlAttribute ("mfn")]
        [JsonPropertyName ("mfn")]
        public int Mfn { get; set; }

        /// <summary>
        /// Шифр записи.
        /// </summary>
        [XmlAttribute ("index")]
        [JsonPropertyName ("index")]
        public string? Index { get; set; }

        /// <summary>
        /// Библиографическое описание.
        /// </summary>
        [XmlAttribute ("description")]
        [JsonPropertyName ("description")]
        public string? Description { get; set; }

        /// <summary>
        /// Шифр документа в базе. Поле 903.
        /// </summary>
        [XmlAttribute ("document-code")]
        [JsonPropertyName ("document-code")]
        public string? DocumentCode { get; set; }

        /// <summary>
        /// Шифр журнала. Поле 933.
        /// </summary>
        [XmlAttribute ("magazine-code")]
        [JsonPropertyName ("magazine-code")]
        public string? MagazineCode { get; set; }

        /// <summary>
        /// Год. Поле 934.
        /// </summary>
        [XmlAttribute ("year")]
        [JsonPropertyName ("year")]
        public string? Year { get; set; }

        /// <summary>
        /// Том. Поле 935.
        /// </summary>
        [XmlAttribute ("volume")]
        [JsonPropertyName ("volume")]
        public string? Volume { get; set; }

        /// <summary>
        /// Номер, часть. Поле 936.
        /// </summary>
        [XmlAttribute ("number")]
        [JsonPropertyName ("number")]
        public string? Number { get; set; }

        /// <summary>
        /// Номер для нужд сортировки. Поля нет.
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public string? NumberForSorting
        {
            get
            {
                var result = Number;
                if (!string.IsNullOrEmpty (result))
                {
                    result = result.Trim();
                }

                return result;
            }
        }

        /// <summary>
        /// Дополнение к номеру. Поле 931^c.
        /// </summary>
        [XmlAttribute ("supplement")]
        [JsonPropertyName ("supplement")]
        public string? Supplement { get; set; }

        /// <summary>
        /// Рабочий лист. Поле 920.
        /// (чтобы отличать подшивки от выпусков журналов)
        /// </summary>
        [XmlAttribute ("worksheet")]
        [JsonPropertyName ("worksheet")]
        public string? Worksheet { get; set; }

        /// <summary>
        /// Расписанное оглавление. Поле 922.
        /// </summary>
        [XmlElement ("article")]
        [JsonPropertyName ("articles")]
        public MagazineArticleInfo[]? Articles { get; set; }

        /// <summary>
        /// Экземпляры. Поле 910.
        /// </summary>
        [XmlElement ("exemplar")]
        [JsonPropertyName ("exemplars")]
        public ExemplarInfo[]? Exemplars { get; set; }

        /// <summary>
        /// Loan count.
        /// </summary>
        [XmlElement ("loanCount")]
        [JsonPropertyName ("loanCount")]
        public int LoanCount { get; set; }

        /// <summary>
        /// Это подшивка?
        /// </summary>
        [XmlIgnore]
        [JsonIgnore]
        public bool IsBinding => Worksheet.SameString ("NJK");

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

        #region Public methods

        /// <summary>
        /// Разбор записи.
        /// </summary>
        public static MagazineIssueInfo Parse
            (
                Record record
            )
        {
            // TODO: реализовать оптимально

            var result = new MagazineIssueInfo
            {
                Mfn = record.Mfn,
                Index = record.FM (903),
                DocumentCode = record.FM (903),
                MagazineCode = record.FM (933),
                Year = record.FM (934),
                Volume = record.FM (935),
                Number = record.FM (936),
                Supplement = record.FM (931, 'c'),
                Worksheet = record.FM (920),

                Articles = record.Fields
                    .GetField (922)
                    .Select (field => MagazineArticleInfo.ParseField330 (field))
                    .ToArray(),

                Exemplars = record.Fields
                    .GetField (910)
                    .Select (field => ExemplarInfo.Parse (field))
                    .ToArray(),

                LoanCount = record.FM (999).SafeToInt32(),

                Record = record
            };

            return result;

        } // method Parse

        /// <summary>
        /// Превращение в запись.
        /// </summary>
        public Record ToRecord()
        {
            var result = new Record();
            result
                .AddNonEmptyField (903, Index)
                .AddNonEmptyField (933, MagazineCode)
                .AddNonEmptyField (934, Year)
                .AddNonEmptyField (935, Volume)
                .AddNonEmptyField (936, Number)
                .AddNonEmptyField (920, Worksheet)
                .AddNonEmptyField (999, LoanCount.ToString());

            var articles = Articles;
            if (!ReferenceEquals (articles, null))
            {
                foreach (var article in articles)
                {
                    result.Fields.Add (article.ToField());
                }
            }

            var exemplars = Exemplars;
            if (!ReferenceEquals (exemplars, null))
            {
                foreach (var exemplar in exemplars)
                {
                    result.Fields.Add (exemplar.ToField());
                }
            }

            return result;

        } // method ToRecord

        /// <summary>
        /// Should serialize the <see cref="Articles"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeArticles() => !Articles.IsNullOrEmpty();

        /// <summary>
        /// Should serialize the <see cref="LoanCount"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeLoanCount() => LoanCount != 0;

        /// <summary>
        /// Should serialize the <see cref="Mfn"/> field?
        /// </summary>
        [ExcludeFromCodeCoverage]
        [EditorBrowsable (EditorBrowsableState.Never)]
        public bool ShouldSerializeMfn() => Mfn != 0;

        /// <summary>
        /// Сравнение двух выпусков
        /// (с целью сортировки по возрастанию номеров).
        /// </summary>
        public static int CompareNumbers (MagazineIssueInfo first, MagazineIssueInfo second)
            => NumberText.Compare (first.Number, second.Number);

        #endregion

        #region IHandmadeSerializable

        /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
        public void RestoreFromStream
            (
                BinaryReader reader
            )
        {
            Mfn = reader.ReadPackedInt32();
            Index = reader.ReadNullableString();
            Description = reader.ReadNullableString();
            DocumentCode = reader.ReadNullableString();
            MagazineCode = reader.ReadNullableString();
            Year = reader.ReadNullableString();
            Volume = reader.ReadNullableString();
            Number = reader.ReadNullableString();
            Supplement = reader.ReadNullableString();
            Worksheet = reader.ReadNullableString();
            Articles = reader.ReadNullableArray<MagazineArticleInfo>();
            Exemplars = reader.ReadNullableArray<ExemplarInfo>();

        } // method RestoreFromStream

        /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
        public void SaveToStream
            (
                BinaryWriter writer
            )
        {
            writer
                .WritePackedInt32 (Mfn)
                .WriteNullable (Index)
                .WriteNullable (Description)
                .WriteNullable (DocumentCode)
                .WriteNullable (MagazineCode)
                .WriteNullable (Year)
                .WriteNullable (Volume)
                .WriteNullable (Number)
                .WriteNullable (Supplement)
                .WriteNullable (Worksheet);
            writer.WriteNullableArray (Articles);
            writer.WriteNullableArray (Exemplars);

        } // method SaveToStream

        #endregion

        #region IVerifiable members

        /// <inheritdoc cref="IVerifiable.Verify" />
        public bool Verify
            (
                bool throwOnError
            )
        {
            var verifier = new Verifier<MagazineIssueInfo> (this, throwOnError);

            verifier
                .NotNullNorEmpty (DocumentCode)
                .NotNullNorEmpty (MagazineCode)
                .NotNullNorEmpty (Number)
                .NotNullNorEmpty (Year);

            return verifier.Result;

        } // method Verify

        #endregion

        #region Object info

        /// <inheritdoc cref="object.ToString" />
        public override string ToString() => string.IsNullOrEmpty (Supplement)
            ? Number.ToVisibleString().Trim()
            : $"{Number.ToVisibleString()} ({Supplement})".Trim();

        #endregion

    } // class MagazineIssueInfo

} // namespace ManagedIrbis.Magazines
