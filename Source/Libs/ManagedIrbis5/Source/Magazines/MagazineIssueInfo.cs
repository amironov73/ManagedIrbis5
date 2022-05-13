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
using System.Diagnostics.Contracts;
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

namespace ManagedIrbis.Magazines;

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
    [Description ("MFN")]
    public int Mfn { get; set; }

    /// <summary>
    /// Шифр записи.
    /// </summary>
    [XmlAttribute ("index")]
    [JsonPropertyName ("index")]
    [Description ("Шифр в базе")]
    public string? Index { get; set; }

    /// <summary>
    /// Библиографическое описание.
    /// </summary>
    [XmlAttribute ("description")]
    [JsonPropertyName ("description")]
    [Description ("Библиографическое описание")]
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
    [Description ("Шифр журнала")]
    public string? MagazineCode { get; set; }

    /// <summary>
    /// Год. Поле 934.
    /// </summary>
    [XmlAttribute ("year")]
    [JsonPropertyName ("year")]
    [Description ("Год")]
    public string? Year { get; set; }

    /// <summary>
    /// Том. Поле 935.
    /// </summary>
    [XmlAttribute ("volume")]
    [JsonPropertyName ("volume")]
    [Description ("Том (если есть)")]
    public string? Volume { get; set; }

    /// <summary>
    /// Номер, часть. Поле 936.
    /// </summary>
    [XmlAttribute ("number")]
    [JsonPropertyName ("number")]
    [Description ("Номер, часть")]
    public string? Number { get; set; }

    /// <summary>
    /// Номер для нужд сортировки. Поля нет.
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
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
    [Description ("Дополнение к номеру")]
    public string? Supplement { get; set; }

    /// <summary>
    /// Рабочий лист. Поле 920.
    /// (чтобы отличать подшивки от выпусков журналов)
    /// </summary>
    [XmlAttribute ("worksheet")]
    [JsonPropertyName ("worksheet")]
    [Description ("Рабочий лист")]
    public string? Worksheet { get; set; }

    /// <summary>
    /// Расписанное оглавление. Поле 922.
    /// </summary>
    [XmlElement ("article")]
    [JsonPropertyName ("articles")]
    [Description ("Расписанное оглавление")]
    public MagazineArticleInfo[]? Articles { get; set; }

    /// <summary>
    /// Экземпляры. Поле 910.
    /// </summary>
    [XmlElement ("exemplar")]
    [JsonPropertyName ("exemplars")]
    [Description ("Экземпляры")]
    public ExemplarInfo[]? Exemplars { get; set; }

    /// <summary>
    /// Количество выдач.
    /// </summary>
    [XmlElement ("loanCount")]
    [JsonPropertyName ("loanCount")]
    [Description ("Количество выдач")]
    public int LoanCount { get; set; }

    /// <summary>
    /// Это подшивка?
    /// </summary>
    [XmlIgnore]
    [JsonIgnore]
    [Browsable (false)]
    public bool IsBinding => Worksheet.SameString ("NJK");

    /// <summary>
    /// Record.
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
    /// Формирование шифра документа по свойствам
    /// <see cref="MagazineCode"/>, <see cref="Year"/>, <see cref="Volume"/>
    /// и <see cref="Number"/>.
    /// </summary>
    [Pure]
    public string BuildIssueIndex()
    {
        return string.IsNullOrEmpty (Volume)
            ? MagazineCode + "/" + Year + "/" + Number
            : MagazineCode + "/" + Year + "/" + Volume + "/" + Number;
    }

    /// <summary>
    /// Формирование полного номера выпуска по свойствам
    /// <see cref="Year"/>, <see cref="Volume"/> и <see cref="Number"/>.
    /// </summary>
    [Pure]
    public string FullNumber()
    {
        return string.IsNullOrEmpty (Volume)
            ? Year + "/" + Number
            : Year + "/" + Volume + "/" + Number;
    }

    /// <summary>
    /// Разбор библиографической записи.
    /// </summary>
    public static MagazineIssueInfo Parse
        (
            Record record
        )
    {
        Sure.NotNull (record);

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

            Articles = record
                .EnumerateField (922)
                .Select (field => MagazineArticleInfo.ParseField330 (field))
                .ToArray(),

            Exemplars = record
                .EnumerateField (910)
                .Select (field => ExemplarInfo.ParseField (field))
                .ToArray(),

            LoanCount = record.FM (999).SafeToInt32(),

            Record = record
        };

        return result;
    }

    /// <summary>
    /// Создание библиографической записи по данным о номере журнала.
    /// </summary>
    [Pure]
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
    }

    /// <summary>
    /// Should serialize the <see cref="Articles"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    [EditorBrowsable (EditorBrowsableState.Never)]
    public bool ShouldSerializeArticles()
    {
        return !Articles.IsNullOrEmpty();
    }

    /// <summary>
    /// Should serialize the <see cref="LoanCount"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    [EditorBrowsable (EditorBrowsableState.Never)]
    public bool ShouldSerializeLoanCount()
    {
        return LoanCount != 0;
    }

    /// <summary>
    /// Should serialize the <see cref="Mfn"/> field?
    /// </summary>
    [ExcludeFromCodeCoverage]
    [EditorBrowsable (EditorBrowsableState.Never)]
    public bool ShouldSerializeMfn()
    {
        return Mfn != 0;
    }

    /// <summary>
    /// Сравнение двух выпусков
    /// (с целью сортировки по возрастанию номеров).
    /// </summary>
    public static int CompareNumbers
        (
            MagazineIssueInfo first,
            MagazineIssueInfo second
        )
    {
        Sure.NotNull (first);
        Sure.NotNull (second);

        return NumberText.Compare (first.Number, second.Number);
    }

    #endregion

    #region IHandmadeSerializable

    /// <inheritdoc cref="IHandmadeSerializable.RestoreFromStream"/>
    public void RestoreFromStream
        (
            BinaryReader reader
        )
    {
        Sure.NotNull (reader);

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
    }

    /// <inheritdoc cref="IHandmadeSerializable.SaveToStream"/>
    public void SaveToStream
        (
            BinaryWriter writer
        )
    {
        Sure.NotNull (writer);

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
    }

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
    }

    #endregion

    #region Object info

    /// <inheritdoc cref="object.ToString" />
    public override string ToString()
    {
        return string.IsNullOrEmpty (Supplement)
            ? Number.ToVisibleString().Trim()
            : $"{Number.ToVisibleString()} ({Supplement})".Trim();
    }

    #endregion
}
